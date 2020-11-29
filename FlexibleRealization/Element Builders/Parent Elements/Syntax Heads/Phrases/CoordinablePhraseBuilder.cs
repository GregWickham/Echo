using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>The base class for PhraseBuilders that build a type of PhraseElement which CAN BE coordinated. ("Coordinable" = "Can be coordinated")</summary>
    /// <remarks>During the initial construction of the ElementBuilder from the CoreNLP constituency parse, we don't yet know whether a given
    /// phrase is going to be coordinated because its constituents are not all present.  When we reach the Configuration stage, we're able to make that
    /// decision by checking to see whether we have multiple head elements and a coordinating conjunction.</remarks>
    public abstract class CoordinablePhraseBuilder : PhraseBuilder
    {
        /// <summary>If we decide during Configure that we're NOT going to build a CoordinatedPhraseElement, then we expect to have exactly one head element</summary>
        internal IPhraseHead UnaryHead => Heads.Count() switch
        {
            0 => null,
            1 => (IPhraseHead)Heads.First(),
            _ => throw new InvalidOperationException("Unable to resolve unary head of a coordinable phrase")
        };

        /// <summary>Return the Modifiers of this that come before the head</summary>
        private protected IEnumerable<IElementTreeNode> PreModifiers => Modifiers.Where(modifier => modifier.ComesBefore(UnaryHead));

        /// <summary>Return the Modifiers of this that come after the head</summary>
        private protected IEnumerable<IElementTreeNode> PostModifiers => Modifiers.Where(modifier => modifier.ComesAfter(UnaryHead));

        /// <summary>Set <paramref name="coordinator"/> as the ONLY coordinating conjunction of the PhraseElement we're going to build.</summary>
        /// <remarks>If we already have a coordinating conjunction and try to add another one, throw an exception.</remarks>
        private protected void SetCoordinator(ConjunctionBuilder coordinator)
        {
            if (Coordinators.Count() == 0)
            {
                AddChildWithRole(coordinator, ChildRole.Coordinator);
            }
            else throw new InvalidOperationException("Can't add multiple coordinators to a coordinable phrase");
        }

        /// <summary>Return the children that have been added to this with a ChildRole of Coordinator</summary>
        private IEnumerable<IElementTreeNode> Coordinators => ChildrenWithRole(ChildRole.Coordinator);

        /// <summary>If a phrase is coordinated, it is expected to have at most one coordinator (usually a coordinating conjunction)</summary>
        private ConjunctionBuilder CoordinatorBuilder => Coordinators.Count() switch
        {
            0 => null,
            1 => (ConjunctionBuilder)Coordinators.First(),
            _ => throw new InvalidOperationException("Unable to resolve coordinator")
        };

        /// <summary>Decide whether this CoordinablePhraseBuilder is going to build a CoordinatedPhraseElement or not.</summary>
        /// <returns><list type="bullet">
        /// <item>If this is a coordinated phrase, return the appropriate CoordinatedPhraseBuilder;</item>
        /// <item>If this phrase has exactly one child, return that child;</item>
        /// <item>If this phrase does not require coordination, return this phrase.</item>
        /// </list></returns>
        public override IElementTreeNode Coordinate() => IsCoordinated switch
        {
            true => Become(this.AsCoordinatedPhrase()),
            false => Children.Count() switch
            {
                0 => throw new InvalidOperationException("Coordinable phrase has zero children during coordination"),
                1 => Children.First() switch
                {
                    PartOfSpeechBuilder => this,               // A phrase with only one child is legit if that child is a PartOfSpeechBuilder.  The phrase is there to provide features that inflect the word.
                    CompoundBuilder => this,                   // A phrase with only one child is also legit if that child is a CompoundBuilder. 
                    ParentElementBuilder peb => Become(peb),   // A phrase with only one child that's a ParentElementBuilder doesn't need to be there, so become the lone child
                    _ => throw new InvalidOperationException("Invalid lone child type")
                },
                _ => this
            }
        };

        /// <summary>Return true of this coordinable phrase actually needs to be coordinated</summary>
        internal bool IsCoordinated => (Heads.Count() > 1) && (CoordinatorBuilder != null);

        /// <summary>Convert this CoordinablePhraseBuilder to a CoordinatedPhraseBuilder, and return that CoordinatedPhraseBuilder</summary>
        /// <remarks>Can be override by subclasses that require custom coordination behavior</remarks>
        private protected virtual CoordinatedPhraseBuilder AsCoordinatedPhrase() => new CoordinatedPhraseBuilder(PhraseCategory, Heads, CoordinatorBuilder);
    }

    /// <summary>This subclass of CoordinablePhraseBuilder adds the parameterized type of the PhraseElement that this PhraseBuilder can build.</summary>
    /// <typeparam name="TPhraseSpec">The type of PhraseElement this builder will build if it is NOT coordinated.</typeparam>
    /// <remarks>If it turns out that the coordinable phrase IS IN FACT coordinated, then the builder will build a CoordinatedPhraseElement.</remarks>
    public abstract class CoordinablePhraseBuilder<TPhraseSpec> : CoordinablePhraseBuilder where TPhraseSpec : PhraseElement, new()
    {
        /// <summary>The <see cref="TPhraseSpec"/> that will be built by this if the phrase is NOT actually coordinated</summary>
        private protected TPhraseSpec Phrase = new TPhraseSpec();

        #region Phrase features

        public override phraseCategory PhraseCategory => Phrase.Category;

        public bool DiscourseFunctionSpecified
        {
            get => Phrase.discourseFunctionSpecified;
            set
            {
                Phrase.discourseFunctionSpecified = value;
                OnPropertyChanged();
            }
        }
        public discourseFunction DiscourseFunction
        {
            get => Phrase.discourseFunction;
            set
            {
                Phrase.discourseFunction = value;
                Phrase.discourseFunctionSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool AppositiveSpecified
        {
            get => Phrase.appositiveSpecified;
            set
            {
                Phrase.appositiveSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Appositive
        {
            get => Phrase.appositive;
            set
            {
                Phrase.appositive = value;
                Phrase.appositiveSpecified = true;
                OnPropertyChanged();
            }
        }

        #endregion Phrase features
    }
}

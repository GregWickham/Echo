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
        private protected CoordinablePhraseBuilder() : base() { }

        /// <summary>If we decide during Configure that we're NOT going to build a CoordinatedPhraseElement, then we expect to have exactly one head element</summary>
        internal IPhraseHead UnaryHead => Heads.Count() switch
        {
            0 => null,
            1 => (IPhraseHead)Heads.First(),
            _ => throw new InvalidOperationException("Unable to resolve unary head of a coordinable phrase")
        };

        /// <summary>Return the Modifiers of this that come before the head</summary>
        private protected IEnumerable<IElementTreeNode> PreModifiers => Modifiers
            .Where(modifier => modifier.ComesBefore(UnaryHead))
            .OrderBy(modifier => modifier);

        /// <summary>Return the Modifiers of this that come after the head</summary>
        private protected IEnumerable<IElementTreeNode> PostModifiers => Modifiers
            .Where(modifier => modifier.ComesAfter(UnaryHead))
            .OrderBy(modifier => modifier);

        /// <summary>Set <paramref name="coordinator"/> as the ONLY coordinating conjunction of the PhraseElement we're going to build.</summary>
        /// <remarks>If we already have a coordinating conjunction and try to add another one, throw an exception.</remarks>
        private protected void SetCoordinator(ConjunctionBuilder coordinator)
        {
            if (Coordinators.Count() == 0) AddChildWithRole(coordinator, ChildRole.Coordinator);
            else throw new InvalidOperationException("Can't add multiple coordinators to a coordinable phrase");
        }

        /// <summary>Return the children that have been added to this with a ChildRole of Coordinator</summary>
        private IEnumerable<IElementTreeNode> Coordinators => ChildrenWithRole(ChildRole.Coordinator);

        /// <summary>If a phrase is coordinated, it is expected to have at most one coordinator (usually a coordinating conjunction)</summary>
        private protected ConjunctionBuilder CoordinatorBuilder => Coordinators.Count() switch
        {
            0 => null,
            1 => (ConjunctionBuilder)Coordinators.First(),
            _ => throw new InvalidOperationException("Unable to resolve coordinator")
        };

        /// <summary>Decide whether this CoordinablePhraseBuilder is going to build a CoordinatedPhraseElement or not.</summary>
        /// <remarks><list type="bullet">
        /// <item>If this is a coordinated phrase, become the appropriate CoordinatedPhraseBuilder;</item>
        /// <item>If this phrase has exactly one child, become that child;</item>
        /// <item>If this phrase does not require coordination, preserve its current form.</item>
        /// </list></remarks>
        public override void Coordinate()
        {
            if (IsCoordinated) Become(this.AsCoordinatedPhrase());
            else switch (Children.Count())
            {
                case 0: 
                    Become(null);
                    break;
                case 1:
                    switch (Children.Single())
                    {
                        case CompoundBuilder: 
                            break;
                        case ParentElementBuilder peb:
                            Become(peb);
                            break;
                        //case WordElementBuilder web:
                        //    if (!web.IsPhraseHead) Become(web);
                        //    break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        /// <summary>Return true of this coordinable phrase actually needs to be coordinated</summary>
        internal bool IsCoordinated => (Heads.Count() > 1) && (CoordinatorBuilder != null);

        /// <summary>Convert this CoordinablePhraseBuilder to a CoordinatedPhraseBuilder, and return that CoordinatedPhraseBuilder</summary>
        /// <remarks>Can be override by subclasses that require custom coordination behavior</remarks>
        private protected virtual CoordinatedPhraseBuilder AsCoordinatedPhrase() => new CoordinatedPhraseBuilder(PhraseCategory, Heads, CoordinatorBuilder, ChildOrderings);

        /// <summary>From the <paramref name="candidates"/> collection, select and return the IElementTreeNode Key whose index Value is closest to <paramref name="index"/></summary>
        private protected IElementTreeNode ElementWithIndexNearest(int index, Dictionary<IElementTreeNode, int> candidates) => candidates
            .OrderBy(kvp => Math.Abs(index - kvp.Value))
            .Select(kvp => kvp.Key)
            .FirstOrDefault();
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

        public override bool DiscourseFunctionSpecified
        {
            get => Phrase.discourseFunctionSpecified;
            set
            {
                Phrase.discourseFunctionSpecified = value;
                OnPropertyChanged();
            }
        }
        public override discourseFunction DiscourseFunction
        {
            get => Phrase.discourseFunction;
            set
            {
                Phrase.discourseFunction = value;
                Phrase.discourseFunctionSpecified = true;
                OnPropertyChanged();
            }
        }

        public override bool AppositiveSpecified
        {
            get => Phrase.appositiveSpecified;
            set
            {
                Phrase.appositiveSpecified = value;
                OnPropertyChanged();
            }
        }
        public override bool Appositive
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

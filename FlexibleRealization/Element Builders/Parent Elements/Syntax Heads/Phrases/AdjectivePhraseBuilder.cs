using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG AdjPhraseSpec</summary>
    public class AdjectivePhraseBuilder : CoordinablePhraseBuilder<AdjPhraseSpec>
    {
        public AdjectivePhraseBuilder() : base() { }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Head);
            listOfRoles.Add(ChildRole.Modifier);
            listOfRoles.Add(ChildRole.Complement);
            if (CoordinatorBuilder == null || CoordinatorBuilder == child) listOfRoles.Add(ChildRole.Coordinator);
        }

        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case AdjectiveBuilder ab:
                    AddHead(ab);
                    break;
                case AdverbBuilder ab:
                    AddModifier(ab);
                    break;
                case AdjectivePhraseBuilder apb:
                    AddHead(apb);
                    break;
                case AdverbPhraseBuilder apb:
                    AddModifier(apb);
                    break;
                case PrepositionalPhraseBuilder ppb:
                    AddComplement(ppb);
                    break;
                case ConjunctionBuilder cb:
                    SetCoordinator(cb);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        #endregion Initial assignment of children

        #region Configuration

        private IEnumerable<AdverbBuilder> ComparativePreModifiers => PreModifiers
            .Where(preModifier => preModifier is AdverbBuilder)
            .Cast<AdverbBuilder>()
            .Where(adverb => adverb.Comparative);

        private IEnumerable<AdverbBuilder> NonComparativePreModifiers => PreModifiers
            .Where(preModifier => preModifier is AdverbBuilder)
            .Cast<AdverbBuilder>()
            .Where(adverb => !adverb.Comparative);

        /// <summary>Transform this AdjectivePhraseBuilder into a CoordinatedPhraseBuilder and return that CoordinatedPhraseBuilder.</summary>
        /// <remarks>The adjective phrase's Modifiers do not get incorporated into the CoordinatedPhraseElement.  Instead, each of those elements 
        /// must be applied to one of the coordinated elements.  This will change the syntactic structure of the tree, but while doing that we want to preserve the original 
        /// word order.  To accomplish that we first create a series of dictionaries that temporarily store the various elements of the original (non-coordinated) phrase,
        /// along with the original order of those elements. <para>Then we add those elements into the CoordinatedPhraseBuilder, using the dictionaries to preserve
        /// ordering.</para></remarks>
        private protected sealed override CoordinatedPhraseBuilder AsCoordinatedPhrase()
        {
            Dictionary<PartOfSpeechBuilder, int> partsOfSpeech = GetPartsOfSpeechAndIndices();
            Dictionary<IElementTreeNode, int> heads = GetHeadsAndIndices();
            Dictionary<IElementTreeNode, int> modifiers = GetModifiersAndIndices();
            CoordinatedPhraseBuilder result = base.AsCoordinatedPhrase();
            foreach (IElementTreeNode eachModifier in modifiers.Keys)
            {
                eachModifier.DetachFromParent();
                eachModifier
                    .Modify(ElementWithIndexNearest(modifiers[eachModifier], heads))
                    ?.SetChildOrdering(eachModifier, partsOfSpeech);
            };
            return result;
        }

        #endregion Configuration

        #region Editing

        private protected override HashSet<Type> ChildTypesThatCanBeAdded { get; } = new HashSet<Type>
        {
            typeof(AdjectiveBuilder),
            typeof(ConjunctionBuilder),
            typeof(AdjectivePhraseBuilder),
            typeof(AdverbBuilder),
            typeof(AdjectivePhraseBuilder),
            typeof(AdverbPhraseBuilder),
            typeof(PrepositionalPhraseBuilder)
        };

        #endregion Editing

        public override IElementTreeNode CopyLightweight() => new AdjectivePhraseBuilder { Phrase = Phrase.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

        public override NLGElement BuildElement()
        {
            Phrase.preMod = PreModifiers
                .Select(preModifier => preModifier.BuildElement())
                .ToArray();
            Phrase.head = UnaryHead.BuildWord();
            Phrase.compl = Complements
                .Select(complement => complement.BuildElement())
                .ToArray();
            return Phrase;
        }

        #region Phrase properties

        public bool ComparativeSpecified
        {
            get => Phrase.IS_COMPARATIVESpecified;
            set
            {
                Phrase.IS_COMPARATIVESpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Comparative
        {
            get => Phrase.IS_COMPARATIVE;
            set
            {
                Phrase.IS_COMPARATIVE = value;
                Phrase.IS_COMPARATIVESpecified = true;
                OnPropertyChanged();
            }
        }

        public bool SuperlativeSpecified
        {
            get => Phrase.IS_SUPERLATIVESpecified;
            set
            {
                Phrase.IS_SUPERLATIVESpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Superlative
        {
            get => Phrase.IS_SUPERLATIVE;
            set
            {
                Phrase.IS_SUPERLATIVE = value;
                Phrase.IS_SUPERLATIVESpecified = true;
                OnPropertyChanged();
            }
        }

        #endregion Phrase properties
    }
}

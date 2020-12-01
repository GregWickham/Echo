using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG AdjPhraseSpec</summary>
    public class AdjectivePhraseBuilder : CoordinablePhraseBuilder<AdjPhraseSpec>
    {
        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case AdjectiveBuilder ab:
                    AddHead(ab);
                    break;
                case AdjectivePhraseBuilder apb:
                    AddHead(apb);
                    break;
                case AdverbBuilder ab:
                    AddModifier(ab);
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


        public override IElementTreeNode CopyLightweight() => new AdjectivePhraseBuilder { Phrase = Phrase.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

        #endregion Configuration

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

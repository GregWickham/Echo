using System;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG AdvPhraseSpec</summary>
    public class AdverbPhraseBuilder : CoordinablePhraseBuilder<AdvPhraseSpec>
    {
        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case AdverbBuilder ab:
                    AddHead(ab);
                    break;
                case AdverbPhraseBuilder apb:
                    AddHead(apb);
                    break;
                case ConjunctionBuilder cb:
                    SetCoordinator(cb);
                    break;
                // Adverb phrase can't have a specifier -- this is a temporary holding place that needs to be fixed during application of dependencies
                case DeterminerBuilder db:
                    AddUnassignedChild(db);
                    break;
                default: throw new InvalidOperationException("Adverb phrase can't find a role for this element");
            }
        }

        #endregion Initial assignment of children

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

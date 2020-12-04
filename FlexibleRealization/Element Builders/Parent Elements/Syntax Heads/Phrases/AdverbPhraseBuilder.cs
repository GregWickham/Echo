using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG AdvPhraseSpec</summary>
    public class AdverbPhraseBuilder : CoordinablePhraseBuilder<AdvPhraseSpec>
    {
        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Head);
            listOfRoles.Add(ChildRole.Modifier);
            if (CoordinatorBuilder == null || CoordinatorBuilder == child) listOfRoles.Add(ChildRole.Coordinator);
        }

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
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        #endregion Initial assignment of children

        public override IElementTreeNode CopyLightweight() => new AdverbPhraseBuilder { Phrase = Phrase.CopyWithoutSpec() }
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

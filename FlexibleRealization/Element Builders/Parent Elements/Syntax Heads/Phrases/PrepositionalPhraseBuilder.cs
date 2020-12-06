using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG PPPhraseSpec</summary>
    public class PrepositionalPhraseBuilder : CoordinablePhraseBuilder<PPPhraseSpec>
    {
        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Head);
            listOfRoles.Add(ChildRole.Complement);
            if (CoordinatorBuilder == null || CoordinatorBuilder == child) listOfRoles.Add(ChildRole.Coordinator);
        }

        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case PrepositionBuilder pb:
                    AddHead(pb);
                    break;
                case PrepositionalPhraseBuilder ppb:
                    AddHead(ppb);
                    break;
                case NounPhraseBuilder npb:
                    AddComplement(npb);
                    break;
                case VerbPhraseBuilder vpb:
                    AddComplement(vpb);
                    break;
                case CoordinatedPhraseBuilder cpb:
                    AssignRoleFor(cpb);
                    break;
                case ConjunctionBuilder cb:
                    SetCoordinator(cb);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        private void AssignRoleFor(CoordinatedPhraseBuilder phrase)
        {
            switch (phrase.PhraseCategory)
            {
                case phraseCategory.PREPOSITIONAL_PHRASE:
                    AddHead(phrase);
                    break;
                case phraseCategory.NOUN_PHRASE:
                    AddComplement(phrase);
                    break;
                case phraseCategory.VERB_PHRASE:
                    AddComplement(phrase);
                    break;
                default:
                    AddUnassignedChild(phrase);
                    break;
            }
        }

        #endregion Initial assignment of children

        /// <summary>Return the CoordinatedPhraseBuilder for this prepositional phrase</summary>
        /// <remarks>The prepositional phrase's Complements do not get incorporated into the CoordinatedPhraseElement.  Instead, they must be applied to one of the
        /// coordinated elements.  When the prepositional phrase is in its non-coordinated form, the Complements are present in their expected place, so the CaseMarking
        /// syntactic relation will not change anything when applied.  Therefore we need to re-apply that syntactic relation after coordinating the phrase.</remarks>
        private protected sealed override CoordinatedPhraseBuilder AsCoordinatedPhrase()
        {
            CoordinatedPhraseBuilder result = base.AsCoordinatedPhrase();
            Complements.ToList().ForEach(complement => complement.Complete(complement.NearestOf(result.CoordinatedElements)));
            return result;
        }

        public override IElementTreeNode CopyLightweight() => new PrepositionalPhraseBuilder { Phrase = Phrase.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

        public override NLGElement BuildElement()
        {
            Phrase.head = UnaryHead.BuildWord();
            Phrase.compl = Complements
                .Select(complement => complement.BuildElement())
                .ToArray();
            return Phrase;
        }
    }
}

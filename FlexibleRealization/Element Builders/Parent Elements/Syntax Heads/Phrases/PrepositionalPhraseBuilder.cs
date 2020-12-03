using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG PPPhraseSpec</summary>
    public class PrepositionalPhraseBuilder : CoordinablePhraseBuilder<PPPhraseSpec>
    {
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
                case ConjunctionBuilder cb:
                    SetCoordinator(cb);
                    break;
                default:
                    AddUnassignedChild(child);
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
            Complements.ToList().ForEach(complement => complement.Complete(complement.Nearest(result.CoordinatedElements)));
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

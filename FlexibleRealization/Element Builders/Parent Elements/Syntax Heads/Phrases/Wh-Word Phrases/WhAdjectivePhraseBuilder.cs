using System;
using System.Linq;

namespace FlexibleRealization
{
    public class WhAdjectivePhraseBuilder : AdjectivePhraseBuilder
    {
        public WhAdjectivePhraseBuilder() : base() { }

        internal WordElementBuilder WhWord { get; private set; }

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case PronounBuilder pb:
                    SetWhWord(pb);
                    break;
                case AdjectiveBuilder adjb:
                    AddHead(adjb);
                    break;
                case AdverbBuilder advb:
                    AddModifier(advb);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        private void SetWhWord(WordElementBuilder whWordBuilder)
        {
            if (WhWord != null) throw new InvalidOperationException("Can't add multiple Wh words to a WhAdjectivePhraseBuilder");
            else WhWord = whWordBuilder;
        }

        public override void Consolidate()
        {
            if (WhWord != null)
            {
                if (Parent is SubordinateClauseBuilder subordinateClause)
                {
                    subordinateClause.SetComplementizer(WhWord);
                    WhWord = null;
                }
            }
            if (Children.Count() == 0) Become(null);
            else BecomeAdjectivePhrase();
        }

        private void BecomeAdjectivePhrase()
        {
            AdjectivePhraseBuilder adjectivePhrase = new AdjectivePhraseBuilder();
            Parent?.ReplaceChild(this, adjectivePhrase);
            MoveChildrenTo(adjectivePhrase);
        }

        public override IElementTreeNode CopyLightweight() => new WhAdjectivePhraseBuilder { WhWord = (WordElementBuilder)WhWord.CopyLightweight() }
            .LightweightCopyChildrenFrom(this);
    }
}

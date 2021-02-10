using System;
using System.Linq;

namespace FlexibleRealization
{
    public class WhNounPhraseBuilder : NounPhraseBuilder
    {
        public WhNounPhraseBuilder() : base() { }

        internal WordElementBuilder WhWord { get; private set; }

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case WhDeterminerBuilder wdb:
                    SetWhWord(wdb);
                    break;
                case WhPronounBuilder wpb:
                    SetWhWord(wpb);
                    break;
                default:
                    base.AssignRoleFor(child);
                    break;
            }
        }

        private void SetWhWord(WordElementBuilder whWordBuilder)
        {
            if (WhWord != null) throw new InvalidOperationException("Can't add multiple Wh words to a WhNounPhraseBuilder");
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
            else BecomeNounPhrase();
        }

        private void BecomeNounPhrase()
        {
            NounPhraseBuilder nounPhrase = new NounPhraseBuilder();
            Parent?.ReplaceChild(this, nounPhrase);
            MoveChildrenTo(nounPhrase);
        }

        public override IElementTreeNode CopyLightweight() => new WhNounPhraseBuilder { WhWord = (WordElementBuilder)WhWord.CopyLightweight() }
            .LightweightCopyChildrenFrom(this);
    }
}

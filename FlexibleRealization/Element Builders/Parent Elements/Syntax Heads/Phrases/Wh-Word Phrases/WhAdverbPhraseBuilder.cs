using System;
using System.Linq;

namespace FlexibleRealization
{
    public class WhAdverbPhraseBuilder : AdverbPhraseBuilder
    {
        public WhAdverbPhraseBuilder() : base() { }

        internal WordElementBuilder WhWord { get; private set; }

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case WhAdverbBuilder wab:
                    SetWhWord(wab);
                    break;
                case AdverbBuilder ab:
                    SetWhWord(ab);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        private void SetWhWord(WordElementBuilder whWordBuilder)
        {
            if (WhWord != null) throw new InvalidOperationException("Can't add multiple Wh words to a WhAdverbPhraseBuilder");
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
            else BecomeAdverbPhrase();
        }

        private void BecomeAdverbPhrase()
        {
            AdverbPhraseBuilder adverbPhrase = new AdverbPhraseBuilder();
            Parent?.ReplaceChild(this, adverbPhrase);
            MoveChildrenTo(adverbPhrase);
        }

        public override IElementTreeNode CopyLightweight() => new WhAdverbPhraseBuilder { WhWord = (WordElementBuilder)WhWord.CopyLightweight() }
            .LightweightCopyChildrenFrom(this);
    }
}

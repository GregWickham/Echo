using SimpleNLG;

namespace FlexibleRealization
{
    public class ModalBuilder : WordElementBuilder
    {
        public ModalBuilder(ParseToken token) : base(lexicalCategory.MODAL, token) { }

        public override IElementTreeNode CopyLightweight() => new ModalBuilder(Token.Copy());
    }
}

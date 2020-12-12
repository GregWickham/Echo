using SimpleNLG;

namespace FlexibleRealization
{
    public class ModalBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public ModalBuilder(ParseToken token) : base(lexicalCategory.MODAL, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private ModalBuilder(ParseToken token, string word) : base(lexicalCategory.MODAL, token, word) { }

        public override IElementTreeNode CopyLightweight() => new ModalBuilder(Token.Copy(), WordSource.GetWord());
    }
}

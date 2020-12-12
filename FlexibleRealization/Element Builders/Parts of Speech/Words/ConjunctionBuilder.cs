using SimpleNLG;

namespace FlexibleRealization
{
    public class ConjunctionBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public ConjunctionBuilder(ParseToken token) : base(lexicalCategory.CONJUNCTION, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private ConjunctionBuilder(ParseToken token, string word) : base(lexicalCategory.CONJUNCTION, token, word) { }

        public override IElementTreeNode CopyLightweight() => new ConjunctionBuilder(Token.Copy(), WordSource.GetWord());
    }
}

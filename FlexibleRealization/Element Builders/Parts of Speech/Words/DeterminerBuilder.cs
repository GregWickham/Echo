using SimpleNLG;

namespace FlexibleRealization
{
    public class DeterminerBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public DeterminerBuilder(ParseToken token) : base(lexicalCategory.DETERMINER, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private protected DeterminerBuilder(ParseToken token, string word) : base(lexicalCategory.DETERMINER, token, word) { }

        public override IElementTreeNode CopyLightweight() => new DeterminerBuilder(Token.Copy(), WordSource.GetWord());
    }
}

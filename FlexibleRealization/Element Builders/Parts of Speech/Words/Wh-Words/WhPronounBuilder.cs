using SimpleNLG;

namespace FlexibleRealization
{
    public class WhPronounBuilder : PronounBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WhPronounBuilder(ParseToken token) : base(token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private WhPronounBuilder(ParseToken token, string word) : base(token, word) { }

        public override IElementTreeNode CopyLightweight() => new WhPronounBuilder(Token.Copy(), WordSource.GetWord());
    }
}

namespace FlexibleRealization
{
    public class WhAdverbBuilder : AdverbBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WhAdverbBuilder(ParseToken token) : base(token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private WhAdverbBuilder(ParseToken token, string word) : base(token, word) { }

        public override IElementTreeNode CopyLightweight() => new WhAdverbBuilder(Token.Copy(), WordSource.GetWord());
    }
}

namespace FlexibleRealization
{
    public class WhDeterminerBuilder : DeterminerBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WhDeterminerBuilder(ParseToken token) : base(token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private WhDeterminerBuilder(ParseToken token, string word) : base(token, word) { }

        public override IElementTreeNode CopyLightweight() => new WhDeterminerBuilder(Token.Copy(), WordSource.GetWord());
    }
}

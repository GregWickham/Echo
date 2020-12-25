namespace FlexibleRealization
{
    public class WhAdverbBuilder : AdverbBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WhAdverbBuilder(ParseToken token) : base(token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private WhAdverbBuilder(int index, string word) : base(index, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public WhAdverbBuilder() : base() { }

        public override IElementTreeNode CopyLightweight() => new WhAdverbBuilder(Index, WordSource.GetWord());
    }
}

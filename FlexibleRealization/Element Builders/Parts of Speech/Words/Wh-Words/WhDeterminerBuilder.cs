namespace FlexibleRealization
{
    public class WhDeterminerBuilder : DeterminerBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WhDeterminerBuilder(ParseToken token) : base(token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private WhDeterminerBuilder(int index, string word) : base(index, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public WhDeterminerBuilder() : base() { }

        public override IElementTreeNode CopyLightweight() => new WhDeterminerBuilder(Index, WordSource.GetWord());
    }
}

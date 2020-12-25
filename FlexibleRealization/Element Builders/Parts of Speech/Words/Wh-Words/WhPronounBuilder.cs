using SimpleNLG;

namespace FlexibleRealization
{
    public class WhPronounBuilder : PronounBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WhPronounBuilder(ParseToken token) : base(token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private WhPronounBuilder(int index, string word) : base(index, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public WhPronounBuilder() : base() { }

        public override IElementTreeNode CopyLightweight() => new WhPronounBuilder(Index, WordSource.GetWord());
    }
}

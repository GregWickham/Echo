using SimpleNLG;

namespace FlexibleRealization
{
    public class CardinalNumberBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public CardinalNumberBuilder(ParseToken token) : base(lexicalCategory.ADJECTIVE, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private CardinalNumberBuilder(string word) : base(lexicalCategory.ADJECTIVE, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public CardinalNumberBuilder() : base(lexicalCategory.ADJECTIVE) { }

        public override IElementTreeNode CopyLightweight() => new CardinalNumberBuilder(WordSource.GetWord());
    }
}

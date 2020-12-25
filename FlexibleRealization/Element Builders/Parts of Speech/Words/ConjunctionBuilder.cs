using SimpleNLG;

namespace FlexibleRealization
{
    public class ConjunctionBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public ConjunctionBuilder(ParseToken token) : base(lexicalCategory.CONJUNCTION, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private ConjunctionBuilder(int index, string word) : base(lexicalCategory.CONJUNCTION, index, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public ConjunctionBuilder() : base(lexicalCategory.CONJUNCTION) { }

        public override IElementTreeNode CopyLightweight() => new ConjunctionBuilder(Index, WordSource.GetWord());
    }
}

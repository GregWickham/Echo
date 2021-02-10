using SimpleNLG;

namespace FlexibleRealization
{
    public class InfinitivalToBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public InfinitivalToBuilder(ParseToken token) : base(lexicalCategory.VERB, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private InfinitivalToBuilder(string word) : base(lexicalCategory.ADVERB, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public InfinitivalToBuilder() : base(lexicalCategory.ADVERB) { }

        public override IElementTreeNode CopyLightweight() => new InfinitivalToBuilder(WordSource.GetWord());
    }
}

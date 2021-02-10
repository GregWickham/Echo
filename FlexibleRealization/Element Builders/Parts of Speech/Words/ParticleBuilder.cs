using SimpleNLG;

namespace FlexibleRealization
{
    public class ParticleBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public ParticleBuilder(ParseToken token) : base(lexicalCategory.ADVERB, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private ParticleBuilder(string word) : base(lexicalCategory.ADVERB, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public ParticleBuilder() : base(lexicalCategory.ADVERB) { }

        public override IElementTreeNode CopyLightweight() => new ParticleBuilder(WordSource.GetWord());
    }
}

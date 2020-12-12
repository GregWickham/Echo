using SimpleNLG;

namespace FlexibleRealization
{
    public class ParticleBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public ParticleBuilder(ParseToken token) : base(lexicalCategory.ADVERB, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private ParticleBuilder(ParseToken token, string word) : base(lexicalCategory.ADVERB, token, word) { }

        public override IElementTreeNode CopyLightweight() => new ParticleBuilder(Token.Copy(), WordSource.GetWord());
    }
}

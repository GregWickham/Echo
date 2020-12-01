using SimpleNLG;

namespace FlexibleRealization
{
    public class ParticleBuilder : WordElementBuilder
    {
        public ParticleBuilder(ParseToken token) : base(lexicalCategory.ADVERB, token) { }

        public override IElementTreeNode CopyLightweight() => new ParticleBuilder(Token.Copy());
    }
}

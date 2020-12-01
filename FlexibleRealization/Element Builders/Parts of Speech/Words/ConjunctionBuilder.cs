using SimpleNLG;

namespace FlexibleRealization
{
    public class ConjunctionBuilder : WordElementBuilder
    {
        public ConjunctionBuilder(ParseToken token) : base(lexicalCategory.CONJUNCTION, token) { }

        public override IElementTreeNode CopyLightweight() => new ConjunctionBuilder(Token.Copy());
    }
}

using SimpleNLG;

namespace FlexibleRealization
{
    public class DeterminerBuilder : WordElementBuilder
    {
        public DeterminerBuilder(ParseToken token) : base(lexicalCategory.DETERMINER, token) { }
    }
}

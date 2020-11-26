using SimpleNLG;
using System;

namespace FlexibleRealization
{
    public class PunctuationBuilder : PartOfSpeechBuilder
    {
        public PunctuationBuilder(ParseToken token) : base(token) { }

        public override NLGElement BuildElement() => throw new NotImplementedException();
    }
}

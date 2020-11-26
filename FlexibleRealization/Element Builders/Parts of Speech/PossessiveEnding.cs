using SimpleNLG;
using System;

namespace FlexibleRealization
{
    public class PossessiveEnding : PartOfSpeechBuilder
    {
        public PossessiveEnding(ParseToken token) : base(token) { }

        public override NLGElement BuildElement() => throw new InvalidOperationException();
    }
}

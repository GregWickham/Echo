using SimpleNLG;
using System;

namespace FlexibleRealization
{
    public class PossessiveEnding : PartOfSpeechBuilder
    {
        public PossessiveEnding(ParseToken token) : base(token) { }

        public override NLGElement BuildElement() => throw new InvalidOperationException();

        public override IElementTreeNode CopyLightweight() => new PossessiveEnding(Token.Copy());
    }
}

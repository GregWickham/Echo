using System;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    public abstract class WhWordPhraseBuilder : ParentElementBuilder
    {
        internal WordElementBuilder HeadWord { get; set; }

        public override NLGElement BuildElement() => HeadWord.BuildWord();
    }
}

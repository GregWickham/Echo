using System.Collections.Generic;
using System.Diagnostics;

namespace FlexibleRealization
{
    [DebuggerDisplay("({Index}) {PartOfSpeech}: {Word}")]
    public class ParseToken
    {
        public int Index { get; set; }
        public string Word { get; set; }
        public string Lemma { get; set; }
        public string PartOfSpeech { get; set; }
        public string NamedEntityClass { get; set; }

        internal ParseToken Copy() => (ParseToken)MemberwiseClone();
    }
}

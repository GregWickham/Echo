using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class AdvPhraseSpec
    {
        public AdvPhraseSpec() => Category = phraseCategory.ADVERB_PHRASE;

        public AdvPhraseSpec Copy() => (AdvPhraseSpec)MemberwiseClone();

        public AdvPhraseSpec CopyWithoutSpec()
        {
            AdvPhraseSpec result = Copy();
            result.NullOutSpecElements();
            return result;
        }

        [XmlIgnore]
        public bool Comparative
        {
            set
            {
                IS_COMPARATIVE = value;
                IS_COMPARATIVESpecified = true;
            }
        }

        [XmlIgnore]
        public bool Superlative
        {
            set
            {
                IS_SUPERLATIVE = value;
                IS_SUPERLATIVESpecified = true;
            }
        }
    }
}

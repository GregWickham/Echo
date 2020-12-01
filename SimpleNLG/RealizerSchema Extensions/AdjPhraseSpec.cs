using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class AdjPhraseSpec
    {
        public AdjPhraseSpec() => Category = phraseCategory.ADJECTIVE_PHRASE;

        public AdjPhraseSpec Copy() => (AdjPhraseSpec)MemberwiseClone();

        public AdjPhraseSpec CopyWithoutSpec()
        {
            AdjPhraseSpec result = Copy();
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

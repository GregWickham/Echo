using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class AdvPhraseSpec
    {
        public AdvPhraseSpec() => Category = phraseCategory.ADVERB_PHRASE;

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

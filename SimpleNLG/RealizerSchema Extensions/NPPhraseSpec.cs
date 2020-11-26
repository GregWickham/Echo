using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class NPPhraseSpec
    {
        public NPPhraseSpec() => Category = phraseCategory.NOUN_PHRASE;

        #region Phrase properties

        [XmlIgnore]
        public NLGElement Specifier
        {
            set => spec = value;
        }

        [XmlIgnore]
        public bool AdjectiveOrdering
        {
            set
            {
                ADJECTIVE_ORDERING = value;
                ADJECTIVE_ORDERINGSpecified = true;
            }
        }

        [XmlIgnore]
        public bool Elided
        {
            set
            {
                ELIDED = value;
                ELIDEDSpecified = true;
            }
        }

        [XmlIgnore]
        public numberAgreement Number
        {
            set
            {
                NUMBER = value;
                NUMBERSpecified = true;
            }
        }

        [XmlIgnore]
        public gender Gender
        {
            set
            {
                GENDER = value;
                GENDERSpecified = true;
            }
        }

        [XmlIgnore]
        public person Person
        {
            set
            {
                PERSON = value;
                PERSONSpecified = true;
            }
        }

        [XmlIgnore]
        public bool Possessive
        {
            set
            {
                POSSESSIVE = value;
                POSSESSIVESpecified = true;
            }
        }

        [XmlIgnore]
        public bool Pronominal
        {
            set
            {
                PRONOMINAL = value;
                PRONOMINALSpecified = true;
            }
        }

        #endregion Phrase properties

        public NPPhraseSpec SetSpecifier(NLGElement element)
        {
            Specifier = element;
            return this;
        }
    }
}

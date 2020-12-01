using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class CoordinatedPhraseElement
    {
        public CoordinatedPhraseElement(phraseCategory category) => Category = category;

        public CoordinatedPhraseElement Copy() => (CoordinatedPhraseElement)MemberwiseClone();

        public CoordinatedPhraseElement CopyWithoutSpec()
        {
            CoordinatedPhraseElement result = Copy();
            result.Coordinated = null;
            return result;
        }

        [XmlIgnore]
        public NLGElement[] Coordinated
        {
            set
            {
                coord = value;
            }
        }

        [XmlIgnore]
        public string Conjunction
        {
            set
            {
                conj = value;
            }
        }

        [XmlIgnore]
        public phraseCategory Category
        {
            get => cat;
            set
            {
                cat = value;
                catSpecified = true;
            }
        }

        [XmlIgnore]
        public bool Appositive
        {
            set
            {
                APPOSITIVE = value;
                APPOSITIVESpecified = true;
            }
        }

        [XmlIgnore]
        public string ConjunctionType
        {
            set
            {
                CONJUNCTION_TYPE = value;
            }
        }

        [XmlIgnore]
        public string Modal
        {
            set
            {
                MODAL = value;
            }
        }

        [XmlIgnore]
        public bool Negated
        {
            set
            {
                NEGATED = value;
                NEGATEDSpecified = true;
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
        public bool Progressive
        {
            set
            {
                PROGRESSIVE = value;
                PROGRESSIVESpecified = true;
            }
        }

        [XmlIgnore]
        public bool RaiseSpecifier
        {
            set
            {
                RAISE_SPECIFIER = value;
                RAISE_SPECIFIERSpecified = true;
            }
        }

        [XmlIgnore]
        public bool SuppressedComplementizer
        {
            set
            {
                SUPRESSED_COMPLEMENTISER = value;
                SUPRESSED_COMPLEMENTISERSpecified = true;
            }
        }

        [XmlIgnore]
        public tense Tense
        {
            set
            {
                TENSE = value;
                TENSESpecified = true;
            }
        }
    }
}

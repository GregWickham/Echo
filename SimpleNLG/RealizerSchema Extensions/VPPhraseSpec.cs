using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class VPPhraseSpec
    {
        public VPPhraseSpec() => Category = phraseCategory.VERB_PHRASE;

        [XmlIgnore]
        public bool AggregateAuxiliary
        {
            set
            {
                AGGREGATE_AUXILIARY = value;
                AGGREGATE_AUXILIARYSpecified = true;
            }
        }

        [XmlIgnore]
        public form Form
        {
            set
            {
                FORM = value;
                FORMSpecified = true;
            }
        }

        [XmlIgnore]
        public string Modal
        {
            set => MODAL = value;
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
        public bool Passive
        {
            set
            {
                PASSIVE = value;
                PASSIVESpecified = true;
            }
        }

        [XmlIgnore]
        public bool Perfect
        {
            set
            {
                PERFECT = value;
                PERFECTSpecified = true;
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
        public bool Progressive
        {
            set
            {
                PROGRESSIVE = value;
                PROGRESSIVESpecified = true;
            }
        }

        [XmlIgnore]
        public bool SuppressGenitiveInGerund
        {
            set
            {
                SUPPRESS_GENITIVE_IN_GERUND = value;
                SUPPRESS_GENITIVE_IN_GERUNDSpecified = true;
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

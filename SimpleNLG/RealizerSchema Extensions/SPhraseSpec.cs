using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class SPhraseSpec
    {
        public SPhraseSpec() => Category = phraseCategory.CLAUSE;

        public SPhraseSpec Copy() => (SPhraseSpec)MemberwiseClone();

        public SPhraseSpec CopyWithoutSpec()
        {
            SPhraseSpec result = Copy();
            result.CuePhrase = null;
            result.Subjects = null;
            result.Predicate = null;
            return result;
        }

        [XmlIgnore]
        public NLGElement CuePhrase
        {
            set => cuePhrase = value;
        }

        [XmlIgnore]
        public NLGElement[] Subjects
        {
            set => subj = value;
        }

        [XmlIgnore]
        public NLGElement Predicate
        {
            set => vp = value;
        }

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
        public clauseStatus ClauseStatus
        {
            set
            {
                CLAUSE_STATUS = value;
                CLAUSE_STATUSSpecified = true;
            }
        }

        [XmlIgnore]
        public string Complementiser
        {
            get => COMPLEMENTISER;
            set => COMPLEMENTISER = value;
        }

        [XmlIgnore]
        public form Form
        {
            get => FORM;
            set
            {
                FORM = value;
                FORMSpecified = true;
            }
        }

        [XmlIgnore]
        public interrogativeType InterrogativeType
        {
            set
            {
                INTERROGATIVE_TYPE = value;
                INTERROGATIVE_TYPESpecified = true;
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
        public bool SuppressedComplementiser
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

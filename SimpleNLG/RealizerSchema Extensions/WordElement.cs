using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class WordElement
    {
        [XmlIgnore]
        public string Base
        {
            get => @base;
            set => @base = value;
        }

        [XmlIgnore]
        public lexicalCategory PartOfSpeech
        {
            set
            {
                cat = value;
                catSpecified = true;
            }
        }

        [XmlIgnore]
        public string ID
        {
            set => id = value;
        }

        [XmlIgnore]
        public bool ExpletiveSubject
        {
            set 
            {
                EXPLETIVE_SUBJECT = value;
                EXPLETIVE_SUBJECTSpecified = true;
            }
        }

        [XmlIgnore]
        public bool Proper
        {
            set
            {
                PROPER = value;
                PROPERSpecified = true;
            }
        }

        [XmlIgnore]
        public inflection Inflection
        {
            set
            {
                var = value;
                varSpecified = true;
            }
        }

        [XmlIgnore]
        public bool Canned
        {
            set
            {
                canned = value;
                cannedSpecified = true;
            }
        }

        public WordElement SetProper(bool proper)
        {
            Proper = proper;
            return this;
        }

    }
}

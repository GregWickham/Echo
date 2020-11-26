using System.Xml.Serialization;

namespace SimpleNLG
{
    public partial class PhraseElement
    {
        [XmlIgnore]
        public NLGElement[] FrontModifiers
        {
            set => frontMod = value;
        }

        [XmlIgnore]
        public NLGElement[] PreModifiers
        {
            set => preMod = value;
        }

        [XmlIgnore]
        public NLGElement[] Complements
        {
            set => compl = value;
        }

        [XmlIgnore]
        public NLGElement[] PostModifiers
        {
            set => postMod = value;
        }

        [XmlIgnore]
        public WordElement Head
        {
            set => head = value;
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

        //[XmlIgnore]
        //public discourseFunction DiscourseFunction
        //{
        //    set
        //    {
        //        discourseFunction = value;
        //        discourseFunctionSpecified = true;
        //    }
        //}

        //[XmlIgnore]
        //public bool Appositive
        //{
        //    set
        //    {
        //        appositive = value;
        //        appositiveSpecified = true;
        //    }
        //}
    }
}

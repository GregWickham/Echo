using SimpleNLG;
using System;

namespace FlexibleRealization
{
    public enum PronounCase { Nominative, Objective, Possessive, Reflexive }

    public class PronounBuilder : WordElementBuilder, IPhraseHead
    {
        /// <summary>This constructor is using during parsing</summary>
        public PronounBuilder(ParseToken token) : base(lexicalCategory.PRONOUN, token)
        {
            switch (token.PartOfSpeech)
            {
                case "PRP$":
                    AnnotatePossessive();
                    break;
                case "PRP":
                    AnnotatePersonal();
                    break;
            }
        }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private protected PronounBuilder(string word) : base(lexicalCategory.PRONOUN, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public PronounBuilder() : base(lexicalCategory.PRONOUN) { }

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsNounPhrase();

        #region Features

        private PronounCase _case;
        public PronounCase Case 
        {
            get => _case; 
            set
            {
                _case = value;
                CaseSpecified = true;
            }
        }
        public bool CaseSpecified { get; set; } = false;

        private person _person;
        public person Person 
        {
            get => _person; 
            set
            {
                _person = value;
                PersonSpecified = true;
            }
        }
        public bool PersonSpecified { get; set; } = false;

        private gender _gender;
        public gender Gender 
        {
            get => _gender;
            set
            {
                _gender = value;
            }
        }
        public bool GenderSpecified { get; set; } = false;

        private numberAgreement _number;
        public numberAgreement Number 
        {
            get => _number;
            set
            {
                _number = value;
                NumberSpecified = true;
            }
        }
        public bool NumberSpecified { get; set; } = false;

        private void AnnotatePersonal()
        {
            switch (Token.Lemma.ToLower())
            {
                case "i":
                    Case = PronounCase.Nominative;
                    Person = person.FIRST;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "me":
                    Case = PronounCase.Objective;
                    Person = person.FIRST;
                    Number = numberAgreement.PLURAL;
                    break;
                case "you":
                    Person = person.SECOND;
                    break;
                case "he":
                    Case = PronounCase.Nominative;
                    Person = person.THIRD;
                    Gender = gender.MASCULINE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "she":
                    Case = PronounCase.Nominative;
                    Person = person.THIRD;
                    Gender = gender.FEMININE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "it":
                    Person = person.THIRD;
                    Gender = gender.NEUTER;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "him":
                    Case = PronounCase.Objective;
                    Person = person.THIRD;
                    Gender = gender.MASCULINE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "her":
                    Case = PronounCase.Objective;
                    Person = person.THIRD;
                    Gender = gender.FEMININE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "we":
                    Case = PronounCase.Nominative;
                    Person = person.FIRST;
                    Number = numberAgreement.PLURAL;
                    break;
                case "us":
                    Case = PronounCase.Objective;
                    Person = person.FIRST;
                    Number = numberAgreement.PLURAL;
                    break;
                case "they":
                    Case = PronounCase.Nominative;
                    Person = person.THIRD;
                    Number = numberAgreement.PLURAL;
                    break;
                case "them":
                    Case = PronounCase.Objective;
                    Person = person.THIRD;
                    Number = numberAgreement.PLURAL;
                    break;
                case "myself":
                    Case = PronounCase.Reflexive;
                    Person = person.FIRST;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "yourself":
                    Case = PronounCase.Reflexive;
                    Person = person.SECOND;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "yourselves":
                    Case = PronounCase.Reflexive;
                    Person = person.SECOND;
                    Number = numberAgreement.PLURAL;
                    break;
                case "himself":
                    Case = PronounCase.Reflexive;
                    Person = person.THIRD;
                    Gender = gender.MASCULINE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "herself":
                    Case = PronounCase.Reflexive;
                    Person = person.THIRD;
                    Gender = gender.FEMININE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "itself":
                    Case = PronounCase.Reflexive;
                    Person = person.THIRD;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "ourselves":
                    Case = PronounCase.Reflexive;
                    Person = person.FIRST;
                    Number = numberAgreement.PLURAL;
                    break;
                case "themselves":
                    Case = PronounCase.Reflexive;
                    Person = person.THIRD;
                    Number = numberAgreement.PLURAL;
                    break;
                case "oneself":
                    Case = PronounCase.Reflexive;
                    Person = person.FIRST;
                    Number = numberAgreement.SINGULAR;
                    break;
                default: throw new ArgumentException("Unrecognized personal pronoun");
            }
        }

        private void AnnotatePossessive()
        {
            Case = PronounCase.Possessive;
            switch (Token.Lemma.ToLower())
            {
                case "my":
                    Person = person.FIRST;
                    Gender = gender.NEUTER;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "we":
                    Person = person.FIRST;
                    Gender = gender.NEUTER;
                    Number = numberAgreement.PLURAL;
                    break;
                case "you":
                    Person = person.SECOND;
                    Gender = gender.NEUTER;
                    Number = numberAgreement.BOTH;
                    break;
                case "she":
                    Person = person.THIRD;
                    Gender = gender.FEMININE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "he":
                    Person = person.THIRD;
                    Gender = gender.MASCULINE;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "its":
                    Person = person.THIRD;
                    Gender = gender.NEUTER;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "they":
                    Person = person.THIRD;
                    Gender = gender.NEUTER;
                    Number = numberAgreement.PLURAL;
                    break;
                default: throw new ArgumentException("Unrecognized possessive pronoun");
            }
        }

        #endregion Features

        internal NounPhraseBuilder AsNounPhrase()
        {
            NounPhraseBuilder result = new NounPhraseBuilder()
            {
                Pronominal = true,
                Person = Person,
                Number = Number,
                Gender = Gender
            };
            switch (Case)
            {
                case PronounCase.Possessive:
                    result.Possessive = true;
                    break;
            }
            result.AddHead(this);
            return result;
        }

        public override IElementTreeNode CopyLightweight() => new PronounBuilder(WordSource.GetWord())
        {
            Case = Case,
            Person = Person,
            Gender = Gender,
            Number = Number
        };
    }
}

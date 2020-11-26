using SimpleNLG;
using System;

namespace FlexibleRealization
{
    public enum PronounCase { Subjective, Objective, Possessive, Reflexive }

    public class PronounBuilder : WordElementBuilder, IPhraseHead
    {
        public PronounBuilder(ParseToken token) : base(lexicalCategory.PRONOUN, token)
        {
            switch (token.PartOfSpeech)
            {
                case "PRP$":
                    AnnotatePossessive();
                    break;
            }
        }

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsNounPhrase();

        internal PronounCase Case { get; set; }
        internal person Person { get; set; }
        internal gender Gender { get; set; }
        internal numberAgreement Number { get; set; }

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
                default: throw new NotImplementedException();
            }
        }

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

    }
}

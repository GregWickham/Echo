﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.NounPhrases.SpecifierTypes
{
    [TestClass]
    public class Article
    {
        [TestClass]
        public class Indefinite
        {
            [TestMethod]
            public void AHand() => Assert.AreEqual("A hand.",
                Client.Realize(Phrase.Noun("hand").SetSpecifier(Word.Determiner("a"))));

            [TestClass]
            public class Inflected
            {
                [TestMethod]
                public void AnOracle() => Assert.AreEqual("An oracle.",
                    Client.Realize(Phrase.Noun("oracle").SetSpecifier(Word.Determiner("a"))));

                [TestMethod]
                public void AnImbecile() => Assert.AreEqual("An imbecile.",
                    Client.Realize(Phrase.Noun("imbecile").SetSpecifier(Word.Determiner("a"))));
            }
        }

        [TestClass]
        public class Definite
        {
            [TestMethod]
            public void TheBicycle() => Assert.AreEqual("The bicycle.",
                Client.Realize(Phrase.Noun("bicycle").SetSpecifier(Word.Determiner("the"))));
        }
    }

    [TestClass]
    public class PossessivePronoun
    {
        [TestClass]
        public class FirstPerson
        {
            [TestMethod]
            public void Singular() => Assert.AreEqual("My hand.", Client.Realize(new NPPhraseSpec
            {
                head = Word.Noun("hand"),
                spec = new NPPhraseSpec
                {
                    head = Word.Pronoun("it"),
                    Possessive = true,
                    Person = person.FIRST,
                    Number = numberAgreement.SINGULAR
                }
            }));

            [TestMethod]
            public void Plural() => Assert.AreEqual("Our flag.", Client.Realize(new NPPhraseSpec
            {
                head = Word.Noun("flag"),
                spec = new NPPhraseSpec
                {
                    head = Word.Pronoun("it"),
                    Possessive = true,
                    Person = person.FIRST,
                    Number = numberAgreement.PLURAL
                }
            }));
        }

        [TestClass]
        public class SecondPerson
        {
            [TestMethod]
            public void Singular() => Assert.AreEqual("Your hand.", Client.Realize(new NPPhraseSpec
            {
                head = Word.Noun("hand"),
                spec = new NPPhraseSpec
                {
                    head = Word.Pronoun("it"),
                    Possessive = true,
                    Person = person.SECOND,
                    Number = numberAgreement.SINGULAR
                }
            }));

            [TestMethod]
            public void Plural() => Assert.AreEqual("Your flag.", Client.Realize(new NPPhraseSpec
            {
                head = Word.Noun("flag"),
                spec = new NPPhraseSpec
                {
                    head = Word.Pronoun("it"),
                    Possessive = true,
                    Person = person.SECOND,
                    Number = numberAgreement.PLURAL
                }
            }));
        }

        [TestClass]
        public class ThirdPerson
        {
            [TestClass]
            public class Singular
            {
                [TestMethod]
                public void Feminine() => Assert.AreEqual("Her blouse.", Client.Realize(new NPPhraseSpec
                {
                    head = Word.Noun("blouse"),
                    spec = new NPPhraseSpec
                    {
                        head = Word.Pronoun("it"),
                        Possessive = true,
                        Person = person.THIRD,
                        Number = numberAgreement.SINGULAR,
                        Gender = gender.FEMININE
                    }
                }));

                [TestMethod]
                public void Masculine() => Assert.AreEqual("His truck.", Client.Realize(new NPPhraseSpec
                {
                    head = Word.Noun("truck"),
                    spec = new NPPhraseSpec
                    {
                        head = Word.Pronoun("it"),
                        Possessive = true,
                        Person = person.THIRD,
                        Number = numberAgreement.SINGULAR,
                        Gender = gender.MASCULINE
                    }
                }));

                [TestMethod]
                public void Neuter() => Assert.AreEqual("Its engine.", Client.Realize(new NPPhraseSpec
                {
                    head = Word.Noun("engine"),
                    spec = new NPPhraseSpec
                    {
                        head = Word.Pronoun("it"),
                        Possessive = true,
                        Person = person.THIRD,
                        Number = numberAgreement.SINGULAR,
                        Gender = gender.NEUTER
                    }
                }));
            }

            [TestMethod]
            public void Plural() => Assert.AreEqual("Their country.", Client.Realize(new NPPhraseSpec
            {
                head = Word.Noun("country"),
                spec = new NPPhraseSpec
                {
                    head = Word.Pronoun("it"),
                    Possessive = true,
                    Person = person.THIRD,
                    Number = numberAgreement.PLURAL
                }
            }));
        }
    }
}

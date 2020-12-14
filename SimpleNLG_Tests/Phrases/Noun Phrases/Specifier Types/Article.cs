using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.NounPhrases.SpecifierTypes
{
    [TestClass]
    public class Article
    {
        [TestClass]
        public class Definite
        {
            [TestMethod]
            public void TheBicycle() => Assert.AreEqual("The bicycle.",
                Client.Realize(Phrase.Noun("bicycle").SetSpecifier(Word.Determiner("the"))));
        }

        [TestClass]
        public class Indefinite
        {
            [TestMethod]
            public void AHand() => Assert.AreEqual("A hand.",
                Client.Realize(Phrase.Noun("hand").SetSpecifier(Word.Determiner("a"))));

            [TestClass]
            public class Agreement
            {
                [TestClass]
                public class Correct
                {
                    /// <summary>Handled correctly by SimpleNLG</summary>
                    [TestMethod]
                    public void AnOracle() => Assert.AreEqual("An oracle.",
                        Client.Realize(Phrase.Noun("oracle").SetSpecifier(Word.Determiner("a"))));

                    /// <summary>Handled correctly by SimpleNLG</summary>
                    [TestMethod]
                    public void AnImbecile() => Assert.AreEqual("An imbecile.",
                        Client.Realize(Phrase.Noun("imbecile").SetSpecifier(Word.Determiner("a"))));

                    /// <summary>Handled correctly by SimpleNLG</summary>
                    [TestMethod]
                    public void AnAwfulSituation() => Assert.AreEqual("An awful situation.", Client.Realize(new NPPhraseSpec
                    {
                        Head = Word.Noun("situation"),
                        Specifier = Word.Determiner("a"),
                        PreModifiers = new NLGElement[]
                        {
                            Word.Adjective("awful")
                        }
                    }));
                }

                [TestClass]
                public class Incorrect
                {
                    /// <summary>NOT handled correctly by SimpleNLG</summary>
                    [TestMethod]
                    public void AnExtremelyBadSituation() => Assert.AreEqual("A extremely bad situation.", Client.Realize(new NPPhraseSpec
                    {
                        Head = Word.Noun("situation"),
                        Specifier = Word.Determiner("a"),
                        PreModifiers = new NLGElement[]
                        {
                            new AdjPhraseSpec
                            {
                                Head = Word.Adjective("bad"),
                                PreModifiers = new NLGElement[]
                                {
                                    Word.Adverb("extremely")
                                }
                            }
                        }
                    }));
                }
            }
        }
    }
}

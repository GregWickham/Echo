using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.NounPhrases.ModifierTypes
{
    [TestClass]
    public class Adjective
    {
        [TestMethod]
        public void HeadOnly() => Assert.AreEqual("Tall man.",
            Client.Realize(new NPPhraseSpec
            {
                head = Word.Noun("man"),
                PreModifiers = new NLGElement[]
                {
                    Word.Adjective("tall")
                }
            }));

        [TestClass]
        public class Comparative
        {
            [TestMethod]
            public void AMorePefectUnion() => Assert.AreEqual("A more perfect union.",
                Client.Realize(new NPPhraseSpec
                {
                    head = Word.Noun("union"),
                    spec = Word.Determiner("a"),
                    PreModifiers = new NLGElement[]
                    {
                        new AdjPhraseSpec
                        {
                            head = Word.Adjective("perfect"),
                            preMod = new NLGElement[]
                            {
                                new AdvPhraseSpec
                                {
                                    head = Word.Adverb("more")
                                }
                            }
                        }
                    }
                }));

            [TestClass]
            public class Intensifier
            {
                [TestClass]
                public class Adverb
                {
                    [TestMethod]
                    public void AMuchTallerBuilding() => Assert.AreEqual("A much taller building.",
                        Client.Realize(new NPPhraseSpec
                        {
                            head = Word.Noun("building"),
                            spec = Word.Determiner("a"),
                            PreModifiers = new NLGElement[]
                            {
                                new AdjPhraseSpec
                                {
                                    head = Word.Adjective("tall"),
                                    Comparative = true,
                                    preMod = new NLGElement[]
                                    {
                                        Word.Adverb("much")
                                    }
                                }
                            }
                        }));

                    [TestMethod]
                    public void AMuchMoreAmusingJoke() => Assert.AreEqual("A much more amusing joke.",
                        Client.Realize(new NPPhraseSpec
                        {
                            head = Word.Noun("joke"),
                            spec = Word.Determiner("a"),
                            PreModifiers = new NLGElement[]
                            {
                                new AdjPhraseSpec
                                {
                                    head = Word.Adjective("amusing"),
                                    preMod = new NLGElement[]
                                    {
                                        new AdvPhraseSpec
                                        {
                                            head = Word.Adverb("more"),
                                            preMod = new NLGElement[]
                                            {
                                                Word.Adverb("much")
                                            }
                                        }
                                    }
                                }
                            }
                        }));
                }

                [TestClass]
                public class Coordinated
                {
                    [TestMethod]
                    public void AMuchMoreImportantAndUltimatelyDecisiveFactor() => Assert.AreEqual("A much more important and ultimately decisive factor.",
                        Client.Realize(new NPPhraseSpec
                        {
                            head = Word.Noun("factor"),
                            spec = Word.Determiner("a"),
                            PreModifiers = new NLGElement[]
                            {
                                new CoordinatedPhraseElement
                                {
                                    conj = "and",
                                    coord = new NLGElement[]
                                    {
                                        new AdjPhraseSpec
                                        {
                                            head = Word.Adjective("important"),
                                            preMod = new NLGElement[]
                                            {
                                                new AdvPhraseSpec
                                                {
                                                    head = Word.Adverb("more"),
                                                    preMod = new NLGElement[]
                                                    {
                                                        Word.Adverb("much")
                                                    }
                                                }
                                            }
                                        },
                                        new AdjPhraseSpec
                                        {
                                            head = Word.Adjective("decisive"),
                                            preMod = new NLGElement[]
                                            {
                                                Word.Adverb("ultimately")
                                            }
                                        }
                                    }
                                },
                            }
                        }));
                }
            }
        }
    }
}

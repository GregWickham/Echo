using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.AdjectivePhrases
{
    [TestClass]
    public class HeadOnly
    {
        [TestMethod]
        public void Tall()
        {
            Assert.AreEqual(Client.Realize(Phrase.Adjective("tall")), "Tall.");
        }

        [TestMethod]
        public void Beautiful()
        {
            Assert.AreEqual(Client.Realize(Phrase.Adjective("beautiful")), "Beautiful.");
        }
    }

    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void BeautifulAndTalented()
        {
            string realized = Client.Realize(new CoordinatedPhraseElement
            {
                conj = "and",
                coord = new NLGElement[]
                {
                    Word.Adjective("beautiful"),
                    Word.Adjective("talented")
                }
            });
            Assert.AreEqual("beautiful and talented", realized);
        }

        [TestClass]
        public class Comparative
        {
            [TestMethod]
            public void MoreBeautifulAndTalented()
            {
                string realized = Client.Realize(new CoordinatedPhraseElement
                {
                    Category = phraseCategory.ADJECTIVE_PHRASE,
                    conj = "and",
                    coord = new NLGElement[]
                    {
                        new AdjPhraseSpec
                        {
                            Head = Word.Adjective("beautiful"),
                            preMod = new NLGElement[]
                            {
                                Word.Adverb("more")
                            }
                        },
                        Word.Adjective("talented")
                    }
                });
                Assert.AreEqual("More beautiful and talented.", realized);
            }
        }

        [TestClass]
        public class Superlative
        {
            [TestMethod]
            public void MostIncompetentAndCorrupt()
            {
                string realized = Client.Realize(new CoordinatedPhraseElement
                {
                    Category = phraseCategory.ADJECTIVE_PHRASE,
                    conj = "and",
                    coord = new NLGElement[]
                    {
                        new AdjPhraseSpec
                        {
                            Head = Word.Adjective("incompetent"),
                            preMod = new NLGElement[]
                            {
                                Word.Adverb("most")
                            }
                        },
                        Word.Adjective("corrupt")
                    }
                });
                Assert.AreEqual("Most incompetent and corrupt.", realized);
            }
        }
    }

    public class Superlative
    {
        private static string SuperlativeFormOf(string adjective) => Client.Realize(new AdjPhraseSpec
        {
            head = Word.Adjective(adjective),
            Superlative = true
        });

        [TestClass]
        public class OneSyllable
        {
            [TestMethod]
            public void Tall() => Assert.AreEqual("Tallest.", SuperlativeFormOf("tall"));

            [TestMethod]
            public void Fat() => Assert.AreEqual("Fattest.", SuperlativeFormOf("fat"));

            [TestMethod]
            public void Big() => Assert.AreEqual("Biggest.", SuperlativeFormOf("big"));

            [TestMethod]
            public void Sad() => Assert.AreEqual("Saddest.", SuperlativeFormOf("sad"));
        }

        [TestClass]
        public class TwoSyllables
        {
            [TestMethod]
            public void Happy() => Assert.AreEqual("Happiest.", SuperlativeFormOf("happy"));

            [TestMethod]
            public void Simple() => Assert.AreEqual("Simplest.", SuperlativeFormOf("simple"));

            [TestMethod]
            public void Busy() => Assert.AreEqual("Busiest.", SuperlativeFormOf("busy"));

            [TestMethod]
            public void Rounded()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("rounded"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("most")
                    }
                });
                Assert.AreEqual("Most rounded.", realized);
            }

            [TestMethod]
            public void Tangled()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("tangled"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("most")
                    }
                });
                Assert.AreEqual("Most tangled.", realized);
            }

            [TestMethod]
            public void Sudden()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("sudden"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("most")
                    }
                });
                Assert.AreEqual("Most sudden.", realized);
            }

            [TestMethod]
            public void Lovely() => Assert.AreEqual("Loveliest.", SuperlativeFormOf("lovely"));

        }

        [TestClass]
        public class ThreeOrMoreSyllables
        {
            [TestMethod]
            public void Important()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("important"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("most")
                    }
                });
                Assert.AreEqual("Most important.", realized);
            }

            [TestMethod]
            public void Expensive()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("expensive"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("most")
                    }
                });
                Assert.AreEqual("Most expensive.", realized);
            }
        }

        [TestClass]
        public class Irregular
        {
            [TestMethod]
            public void Good() => Assert.AreEqual("Best.", SuperlativeFormOf("good"));

            [TestMethod]
            public void Bad() => Assert.AreEqual("Worst.", SuperlativeFormOf("bad"));

            [TestMethod]
            public void Far() => Assert.AreEqual("Farthest.", SuperlativeFormOf("far"));
        }
    }

    [TestClass]
    public class ModifierTypes
    {
        [TestClass]
        public class Adverb
        {
            [TestMethod]
            public void UtterlyAtrocious()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("atrocious"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("utterly")
                    }
                });
                Assert.AreEqual("Utterly atrocious.", realized);
            }

            [TestClass]
            public class Coordinated
            {
                [TestMethod]
                public void CompletelyAndTotallyIncompetent()
                {
                    string realized = Client.Realize(new AdjPhraseSpec
                    {
                        head = Word.Adjective("incompetent"),
                        PreModifiers = new NLGElement[]
                        {
                            new CoordinatedPhraseElement
                            {
                                Conjunction = "and",
                                coord = new NLGElement[]
                                {
                                    Word.Adverb("completely"),
                                    Word.Adverb("totally")
                                }
                            }
                        }
                    });
                    Assert.AreEqual("Completely and totally incompetent.", realized);
                }
            }
        }
    }
}

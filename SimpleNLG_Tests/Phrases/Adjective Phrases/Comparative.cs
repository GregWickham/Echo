using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.AdjectivePhrases
{
    public class Comparative
    {
        private static string ComparativeFormOf(string adjective) => Client.Realize(new AdjPhraseSpec
        {
            head = Word.Adjective(adjective),
            Comparative = true
        });

        [TestClass]
        public class OneSyllable
        {
            [TestMethod]
            public void Tall() => Assert.AreEqual("Taller.", ComparativeFormOf("tall"));

            [TestMethod]
            public void Fat() => Assert.AreEqual("Fatter.", ComparativeFormOf("fat"));

            [TestMethod]
            public void Big() => Assert.AreEqual("Bigger.", ComparativeFormOf("big"));

            [TestMethod]
            public void Sad() => Assert.AreEqual("Sadder.", ComparativeFormOf("sad"));
        }

        [TestClass]
        public class TwoSyllables
        {
            [TestMethod]
            public void Happy() => Assert.AreEqual("Happier.", ComparativeFormOf("happy"));

            [TestMethod]
            public void Simple() => Assert.AreEqual("Simpler.", ComparativeFormOf("simple"));

            [TestMethod]
            public void Busy() => Assert.AreEqual("Busier.", ComparativeFormOf("busy"));

            [TestMethod]
            public void Rounded()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("rounded"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("more")
                    }
                });
                Assert.AreEqual("More rounded.", realized);
            }

            [TestMethod]
            public void Tangled()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("tangled"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("more")
                    }
                });
                Assert.AreEqual("More tangled.", realized);
            }

            [TestMethod]
            public void Sudden()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("sudden"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("more")
                    }
                });
                Assert.AreEqual("More sudden.", realized);
            }

            [TestMethod]
            public void Lovely() => Assert.AreEqual("Lovelier.", ComparativeFormOf("lovely"));
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
                        Phrase.Adverb("more")
                    }
                });
                Assert.AreEqual("More important.", realized);
            }

            [TestMethod]
            public void Expensive()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("expensive"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("more")
                    }
                });
                Assert.AreEqual("More expensive.", realized);
            }

            [TestMethod]
            public void Beautiful()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("important"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("more")
                    }
                });
                Assert.AreEqual("More important.", realized);
            }

            [TestClass]
            public class WithIntensifier
            {
                [TestMethod]
                public void MuchBetter()
                {
                    string realized = Client.Realize(new AdjPhraseSpec
                    {
                        head = Word.Adjective("better"),
                        PreModifiers = new NLGElement[]
                        {
                            Word.Adverb("much")
                        }
                    });
                    Assert.AreEqual("Much better.", realized);
                }

                [TestMethod]
                public void MuchMoreImportant()
                {
                    string realized = Client.Realize(new AdjPhraseSpec
                    {
                        head = Word.Adjective("important"),
                        PreModifiers = new NLGElement[]
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
                    });
                    Assert.AreEqual("Much more important.", realized);
                }

                [TestMethod]
                public void FarMoreImportant()
                {
                    string realized = Client.Realize(new AdjPhraseSpec
                    {
                        head = Word.Adjective("important"),
                        PreModifiers = new NLGElement[]
                        {
                            new AdvPhraseSpec
                            {
                                head = Word.Adverb("more"),
                                preMod = new NLGElement[]
                                {
                                    Word.Adverb("far")
                                }
                            }
                        }
                    });
                    Assert.AreEqual("Far more important.", realized);
                }

                [TestMethod]
                public void WayMoreImportant()
                {
                    string realized = Client.Realize(new AdjPhraseSpec
                    {
                        head = Word.Adjective("important"),
                        PreModifiers = new NLGElement[]
                        {
                            new AdvPhraseSpec
                            {
                                head = Word.Adverb("more"),
                                preMod = new NLGElement[]
                                {
                                    Word.Adverb("way")
                                }
                            }
                        }
                    });
                    Assert.AreEqual("Way more important.", realized);
                }
            }
        }

        [TestClass]
        public class Irregular
        {
            [TestMethod]
            public void Good() => Assert.AreEqual("Better.", ComparativeFormOf("good"));

            [TestMethod]
            public void Bad() => Assert.AreEqual("Worse.", ComparativeFormOf("bad"));

            [TestMethod]
            public void Far() => Assert.AreEqual("Farther.", ComparativeFormOf("far"));

            [TestMethod]
            public void Real()
            {
                string realized = Client.Realize(new AdjPhraseSpec
                {
                    head = Word.Adjective("real"),
                    PreModifiers = new NLGElement[]
                    {
                        Phrase.Adverb("more")
                    }
                });
                Assert.AreEqual("More real.", realized);
            }
        }
    }

}

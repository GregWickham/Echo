using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.PrepositionalPhrases
{
    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void InAndAroundTheLake()
        {
            string realized = Client.Realize(new CoordinatedPhraseElement
            {
                Category = phraseCategory.PREPOSITIONAL_PHRASE,
                conj = "and",
                coord = new NLGElement[]
                {
                    Word.Preposition("in"),
                    new PPPhraseSpec
                    {
                        Head = Word.Preposition("around"),
                        Complements = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("lake")
                            }
                        }
                    }
                }
            });
            Assert.AreEqual("In and around the lake.", realized);
        }

        [TestClass]
        public class Phrases
        {
            [TestMethod]
            public void OverTheRiverAndThroughTheWood()
            {
                string realized = Client.Realize(new CoordinatedPhraseElement
                {
                    Category = phraseCategory.PREPOSITIONAL_PHRASE,
                    conj = "and",
                    coord = new NLGElement[]
                    {
                    new PPPhraseSpec
                    {
                        Head = Word.Preposition("over"),
                        Complements = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("river")
                            }
                        }
                    },
                    new PPPhraseSpec
                    {
                        Head = Word.Preposition("through"),
                        Complements = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("wood")
                            }
                        }
                    }
                    }
                });
                Assert.AreEqual("Over the river and through the wood.", realized);
            }
        }
    }
}


using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses.Independent.PredicateFeatures.ComplementTypes
{
    [TestClass]
    public class PredicativeAdjectivePhrase
    {
        [TestMethod]
        public void CitizensAreAngryWithTheHighPrices() => Assert.AreEqual("Citizens are angry with the high prices.", Client.Realize(new SPhraseSpec
        {
            subj = new NLGElement[]
            {
                new NPPhraseSpec
                {
                    Head = Word.Noun("citizen"),
                    Number = numberAgreement.PLURAL
                }
            },
            vp = new VPPhraseSpec
            {
                Head = Word.Verb("are"),
                compl = new NLGElement[]
                {
                    new AdjPhraseSpec
                    {
                        Head = Word.Adjective("angry"),
                        Complements = new NLGElement[]
                        {
                            new PPPhraseSpec
                            {
                                Head = Word.Preposition("with"),
                                Complements = new NLGElement[]
                                {
                                    new NPPhraseSpec
                                    {
                                        Specifier = Word.Determiner("the"),
                                        Head = Word.Noun("prices"),
                                        PreModifiers = new NLGElement[]
                                        {
                                            Word.Adjective("high")
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
            }
        }));
    }

}


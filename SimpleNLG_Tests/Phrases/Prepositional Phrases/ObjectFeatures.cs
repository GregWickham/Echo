using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.PrepositionalPhrases.ObjectFeatures
{
    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void OnTheSeasAndOceans()
        {
            string realized = Client.Realize(new PPPhraseSpec
            {
                Head = Word.Preposition("on"),
                Complements = new NLGElement[]
                {
                    new CoordinatedPhraseElement
                    {
                        Conjunction = "and",
                        Coordinated = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("sea"),
                                Number = numberAgreement.PLURAL
                            },
                            new NPPhraseSpec
                            {
                                Head = Word.Noun("ocean"),
                                Number = numberAgreement.PLURAL
                            }
                        }
                    }
                }
            });
            Assert.AreEqual("On the seas and oceans.", realized);
        }

        [TestClass]
        public class MixedNounAndGerund
        {
            [TestMethod]
            public void CauseOfPainAndSuffering()
            {
                string realized = Client.Realize(new NPPhraseSpec
                {
                    Head = Word.Noun("cause"),
                    Complements = new NLGElement[]
                    {
                        new PPPhraseSpec
                        {
                            Head = Word.Preposition("of"),
                            Complements = new NLGElement[]
                            {
                                new CoordinatedPhraseElement
                                {
                                    Conjunction = "and",
                                    Coordinated = new NLGElement[]
                                    {
                                        new NPPhraseSpec
                                        {
                                            Head = Word.Noun("pain")
                                        },
                                        new VPPhraseSpec
                                        {
                                            Form = form.GERUND,
                                            Head = Word.Noun("suffer")
                                        }
                                    }
                                }
                            }
                        }
                    }
                });                    
                Assert.AreEqual("Cause of pain and suffering.", realized);
            }
        }
    }
}


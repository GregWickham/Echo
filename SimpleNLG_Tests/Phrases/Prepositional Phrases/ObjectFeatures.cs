using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.PrepositionalPhrases.ObjectFeatures
{
    [TestClass]
    public class CoordinatedObject
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
    }
}


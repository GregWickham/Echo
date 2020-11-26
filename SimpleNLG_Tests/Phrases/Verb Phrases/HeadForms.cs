using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.VerbPhrases.HeadForms
{
    [TestClass]
    public class PresentParticiple
    {
        [TestMethod]
        public void Marching()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Form = form.PRESENT_PARTICIPLE,
                Head = Word.Verb("march")
            });
            Assert.AreEqual("Marching.", realized);
        }
    }

    [TestClass]
    public class PastParticiple
    {
        [TestMethod]
        public void Visited()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Form = form.PAST_PARTICIPLE,
                Head = Word.Verb("visit")
            });
            Assert.AreEqual("Visited.", realized);
        }

        [TestClass]
        public class Passive
        {
            [TestMethod]
            public void WasDestroyed()
            {
                string realized = Client.Realize(new VPPhraseSpec
                {
                    Tense = tense.PAST,
                    Passive = true,
                    Head = Word.Verb("destroy")
                });
                Assert.AreEqual("Was destroyed.", realized);
            }
        }
    }


}

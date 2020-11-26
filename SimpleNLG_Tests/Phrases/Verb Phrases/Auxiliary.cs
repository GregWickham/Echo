using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.VerbPhrases.Auxiliary
{
    [TestClass]
    public class PastParticiple
    {
        [TestMethod]
        public void WasRunning()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Tense = tense.PAST,
                Head = Word.Verb("be"),
                Complements = new NLGElement[]
                {
                    new VPPhraseSpec
                    {
                        Form = form.GERUND,
                        Head = Word.Verb("run")
                    }
                }
            });
            Assert.AreEqual("Was running.", realized);
        }
    }

}

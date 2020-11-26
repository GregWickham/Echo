using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.VerbPhrases.Coordinated
{
    [TestClass]
    public class PresentParticiple
    {
        [TestMethod]
        public void DrivingAndCrying()
        {
            string realized = Client.Realize(new CoordinatedPhraseElement
            {
                Conjunction = "and",
                coord = new NLGElement[]
                {
                    new VPPhraseSpec
                    {
                        Form = form.PRESENT_PARTICIPLE,
                        Head = Word.Verb("drive")
                    },
                    new VPPhraseSpec
                    {
                        Form = form.PRESENT_PARTICIPLE,
                        Head = Word.Verb("cry")
                    },
                }
            });
            Assert.AreEqual("driving and crying", realized);
        }
    }
           
}

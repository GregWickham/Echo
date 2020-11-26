using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.NounPhrases.HeadFeatures
{
    [TestClass]
    public class Coordinated
    {
        [TestClass]
        public class Gerunds
        {
            [TestMethod]
            public void LootingAndPillaging() => Assert.AreEqual("looting and pillaging",
                Client.Realize(new CoordinatedPhraseElement
                {
                    Conjunction = "and",
                    coord = new NLGElement[]
                    {
                        new VPPhraseSpec
                        {
                            Head = Word.Verb("loot"),
                            Form = form.GERUND
                        },
                        new VPPhraseSpec
                        {
                            Head = Word.Verb("pillage"),
                            Form = form.GERUND
                        }
                    }
                }));
        }

    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.AdjectivePhrases.CoordinatedElements
{
    [TestClass]
    public class Head
    {
        [TestMethod]
        public void LoathsomeAndSupremelyDetestable() => Assert.AreEqual(
            "loathsome and supremely detestable",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("loathsome and supremely detestable")));

        [TestClass]
        public class WithModifiers
        {
            [TestMethod]
            public void MuchMoreImportantAndUltimatelyDecisive() => Assert.AreEqual(
                "much more important and ultimately decisive",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("much more important and ultimately decisive")));
        }
    }
}

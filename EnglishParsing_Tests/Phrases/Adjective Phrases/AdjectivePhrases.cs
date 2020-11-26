using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.AdjectivePhrases
{
    [TestClass]
    public class HeadOnly
    {
        [TestMethod]
        public void Beautiful() => Assert.AreEqual(
           "Beautiful.",
           SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("beautiful")));
    }

    [TestClass]
    public class Comparative
    {
        [TestMethod]
        public void MoreImportant() => Assert.AreEqual(
            "More important.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("more important")));

        [TestMethod]
        public void LessImportant() => Assert.AreEqual(
            "Less important.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("less important")));

        [TestClass]
        public class WithIntensifier
        {
            [TestMethod]
            public void FarMoreImportant() => Assert.AreEqual(
                "Far more important.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("far more important")));
        }
    }

    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void LoathsomeAndSupremelyDetestable() => Assert.AreEqual(
            "loathsome and supremely detestable",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("loathsome and supremely detestable")));
    }
}

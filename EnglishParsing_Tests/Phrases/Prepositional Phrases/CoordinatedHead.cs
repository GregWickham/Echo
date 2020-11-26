using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.PrepositionalPhrases
{
    [TestClass]
    public class CoordinatedHead
    {
        [TestMethod]
        public void OverTheRiverAndThroughTheWood() => Assert.AreEqual(
            "over the river and through the wood",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("over the river and through the wood")));

        [TestMethod]
        public void InAndAroundTheLake() => Assert.AreEqual(
            "in and around the lake",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("in and around the lake")));
    }
}

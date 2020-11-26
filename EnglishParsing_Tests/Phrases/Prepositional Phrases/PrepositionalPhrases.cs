using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.PrepositionalPhrases
{
    [TestClass]
    public class Simple
    {
        [TestMethod]
        public void OnABicycle() => Assert.AreEqual(
            "On a bicycle.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("on a bicycle")));
    }

    [TestClass]
    public class Nested
    {
        [TestMethod]
        public void InTheDeepestLakeInTheWorld() => Assert.AreEqual(
            "In the deepest lake in the world.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("in the deepest lake in the world")));
    }

}

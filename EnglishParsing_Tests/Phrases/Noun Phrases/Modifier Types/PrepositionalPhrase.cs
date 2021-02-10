using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ModifierTypes
{
    [TestClass]
    public class PrepositionalPhrase
    {
        [TestMethod]
        public void TheCatInTheHat() => Assert.AreEqual(
            "The cat in the hat.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the cat in the hat")));

        [TestMethod]
        public void AFlyOnTheWall() => Assert.AreEqual(
            "A fly on the wall.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a fly on the wall")));

        [TestMethod]
        public void ThePlantInTheWindow() => Assert.AreEqual(
            "The plant in the window.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the plant in the window")));

        [TestMethod]
        public void AFigureOfEvidentlyPictorialIntent() => Assert.AreEqual(
            "A figure of evidently pictorial intent.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a figure of evidently pictorial intent")));
    }
}
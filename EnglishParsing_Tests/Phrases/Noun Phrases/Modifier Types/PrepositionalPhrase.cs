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
    }
}
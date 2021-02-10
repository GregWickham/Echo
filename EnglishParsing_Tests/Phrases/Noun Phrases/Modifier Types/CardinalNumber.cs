using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ModifierTypes
{
    [TestClass]
    public class CardinalNumber
    {
        [TestMethod]
        public void SevenHorses() => Assert.AreEqual("Seven horses.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("seven horses")));

        [TestMethod]
        public void IHaveThreeSisters() => Assert.AreEqual("I have three sisters.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("I have three sisters")));
    }
}
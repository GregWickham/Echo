using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.HeadFeatures.Tenses
{
    [TestClass]
    public class Past
    {
        [TestMethod]
        public void Tried() => Assert.AreEqual("Tried.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("tried")));

        [TestMethod]
        public void Followed() => Assert.AreEqual("Followed.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("followed")));
    }
}


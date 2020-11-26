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

    [TestClass]
    public class Present
    {
        [TestClass]
        public class ThirdPersonSingular
        {
            [TestMethod]
            public void Tries() => Assert.AreEqual("Tries.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("tries")));

            [TestMethod]
            public void Perseveres() => Assert.AreEqual("Perseveres.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("perseveres")));
        }
    }

    [TestClass]
    public class Future
    {
        [TestMethod]
        public void WillPrevail() => Assert.AreEqual("Will prevail.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("will prevail")));
    }
}


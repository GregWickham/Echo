using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.HeadFeatures.Tenses
{
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
}


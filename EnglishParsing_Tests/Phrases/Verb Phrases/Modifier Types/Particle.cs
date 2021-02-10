using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ModifierTypes
{
    [TestClass]
    public class Particle
    {
        [TestMethod]
        public void LooksUp() => Assert.AreEqual("Looks up.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("looks up")));

        [TestMethod]
        public void RanAway() => Assert.AreEqual("Ran away.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("ran away")));
    }
}

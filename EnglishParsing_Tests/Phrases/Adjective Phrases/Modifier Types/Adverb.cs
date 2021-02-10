using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.AdjectivePhrases.ModifierTypes
{
    [TestClass]
    public class Adverb
    {
        [TestMethod]
        public void StrikinglyBold() => Assert.AreEqual(
            "Strikingly bold.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("strikingly bold")));

        [TestMethod]
        public void DisgustinglyMendacious() => Assert.AreEqual(
            "Disgustingly mendacious.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("disgustingly mendacious")));

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
        public class Superlative
        {
            [TestMethod]
            public void MostImportant() => Assert.AreEqual(
                "Most important.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("most important")));

            [TestMethod]
            public void LeastImportant() => Assert.AreEqual(
                "Least important.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("least important")));
        }
    }
}

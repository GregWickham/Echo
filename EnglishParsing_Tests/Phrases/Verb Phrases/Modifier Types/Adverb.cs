using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ModifierTypes
{
    [TestClass]
    public class Adverb
    {
        [TestMethod]
        public void WorksHard() => Assert.AreEqual("Works hard.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("works hard")));

        [TestMethod]
        public void ThePresidentInadvertentlySpokeTheTruth() => Assert.AreEqual("The president inadvertently spoke the truth.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the president inadvertently spoke the truth")));

        [TestClass]
        public class Intensified
        {
            [TestMethod]
            public void RanVeryFast() => Assert.AreEqual("Ran very fast.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("ran very fast"))); 

            [TestMethod]
            public void WorksTooHard() => Assert.AreEqual("Works too hard.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("works too hard")));
        }
    }
}

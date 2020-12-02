using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.PredicateFeatures
{
    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void MaryJillAndAnitaAllHaveRedHairAndMeticulouslyCareForIt() => Assert.AreEqual(
            "Mary, Jane and Anita all have red hair and meticulously care for it.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("Mary, Jane and Anita all have red hair and meticulously care for it")));

        [TestClass]
        public class WithModalAndPostModifier
        {
            [TestMethod]
            public void MaryJillAndAnitaAllHaveRedHairAndWillMeticulouslyCareForItForever() => Assert.AreEqual(
                "Mary, Jane and Anita all have red hair and will meticulously care for it forever.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("Mary, Jane and Anita all have red hair and will meticulously care for it forever")));
        }
    }
}

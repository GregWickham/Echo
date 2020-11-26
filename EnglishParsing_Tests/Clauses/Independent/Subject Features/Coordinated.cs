using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.SubjectFeatures
{
    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void MaryJillAndAnitaHaveRedHair() => Assert.AreEqual(
            "Mary, Jane and Anita have red hair.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("Mary, Jane and Anita have red hair")));

        [TestMethod]
        public void MaryJillAndAnitaAllHaveRedHair() => Assert.AreEqual(
            "Mary, Jane and Anita all have red hair.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("Mary, Jane and Anita all have red hair")));
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.PredicateFeatures
{
    [TestClass]
    public class Tense
    {
        [TestClass]
        public class Past
        {
            [TestMethod]
            public void TheFirstHalfOfThePrincipalManuscriptToldAVeryPeculiarTale() => Assert.AreEqual(
                "The first half of the principal manuscript told a very peculiar tale.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the first half of the principal manuscript told a very peculiar tale")));
        }
    }
}

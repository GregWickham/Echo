using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.PredicateFeatures.ComplementTypes
{
    [TestClass]
    public class SubordinateClause
    {
        [TestMethod]
        public void ItCanRingLikeFireWhenYouLoseYourWay() => Assert.AreEqual(
            "It can ring like fire when you lose your way.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("it can ring like fire when you lose your way")));
    }
}

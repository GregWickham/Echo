using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.PredicateFeatures.ModifierTypes
{
    [TestClass]
    public class AdverbPhrase
    {
        [TestMethod]
        public void WeShallNeverSurrender() => Assert.AreEqual(
            "We shall never surrender.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall never surrender")));
    }
}

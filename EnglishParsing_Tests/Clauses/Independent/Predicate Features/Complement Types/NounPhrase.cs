using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.PredicateFeatures.ComplementTypes
{
    [TestClass]
    public class NounPhrase
    {
        [TestMethod]
        public void WeShallDefendOurIsland() => Assert.AreEqual(
            "We shall defend our island.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall defend our island")));

        [TestClass]
        public class WithNestedComplements
        {
            [TestMethod]
            public void AdamSandlerIsAnEgregiousTollOnTheNervesOfAllPeopleWithGoodTasteAndDecency() => Assert.AreEqual(
                "Adam Sandler is an egregious toll on the nerves of all people with good taste and decency.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("Adam Sandler is an egregious toll on the nerves of all people with good taste and decency")));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ComplementTypes
{
    [TestClass]
    public class PrepositionalPhrase
    {
        [TestMethod]
        public void TheCatInTheHat() => Assert.AreEqual(
            "The cat in the hat.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the cat in the hat")));
    }

    [TestClass]
    public class SubordinateClause
    {
        [TestMethod]
        public void TheFlyThatLandedOnHisHead() => Assert.AreEqual(
            "The fly that landed on his head.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the fly that landed on his head")));

        [TestMethod]
        public void TheLyingLiarWhoLies() => Assert.AreEqual(
            "The lying liar who lies.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the lying liar who lies")));

        [TestMethod]
        public void TheYoungPeopleWhoAttendedTheProtest() => Assert.AreEqual(
            "The young people who attended the protest.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the young people who attended the protest")));
    }
}

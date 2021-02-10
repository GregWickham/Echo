using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ModifierTypes
{
    [TestClass]
    public class TemporalNounPhrase
    {
        [TestMethod]
        public void IReadTheNewsToday() => Assert.AreEqual("I read the news today.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("I read the news today")));

        [TestMethod]
        public void WeHadCoffeeThisMorning() => Assert.AreEqual("We had coffee this morning.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we had coffee this morning")));

        [TestMethod]
        public void WeMayDieTomorrow() => Assert.AreEqual("We may die tomorrow.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we may die tomorrow")));

        [TestMethod]
        public void WeWillMeetWednesday() => Assert.AreEqual("We will meet Wednesday.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we will meet Wednesday")));
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ComplementTypes
{
    [TestClass]
    public class SubordinateNounClause
    {
        [TestMethod]
        public void TheQuestionIsWhetherWeCanFinishOnTime() => Assert.AreEqual("The question is whether we can finish on time.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the question is whether we can finish on time")));

        [TestMethod]
        public void WeShallSeeIfItWorks() => Assert.AreEqual("We shall see if it works.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall see if it works")));

        [TestMethod]
        public void WeBothKnowWhatHeDidWithTheMoney() => Assert.AreEqual("We both know what he did with the money.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we both know what he did with the money")));

        [TestMethod]
        public void ThePerpetratorKnowsWhyItHappened() => Assert.AreEqual("The perpetrator knows why it happened.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the perpetrator knows why it happened")));

        [TestMethod]
        public void TheDetectiveWantsToKnowWhereTheMurderTookPlace() => Assert.AreEqual("The detective wants to know where the murder took place.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the detective wants to know where the murder took place")));

        [TestMethod]
        public void OurTimelineExplainsWhenTheMurderHappened() => Assert.AreEqual("Our timeline explains when the murder happened.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("our timeline explains when the murder happened")));
    }
}

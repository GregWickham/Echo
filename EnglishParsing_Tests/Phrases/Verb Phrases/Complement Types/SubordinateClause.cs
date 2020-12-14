using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ComplementTypes
{
    [TestClass]
    public class SubordinateClause
    {
        [TestMethod]
        public void TheQuestionIsWhetherWeCanFinishOnTime() => Assert.AreEqual("The question is whether we can finish on time.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the question is whether we can finish on time")));
    }
}

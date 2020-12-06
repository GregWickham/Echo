using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.PrepositionalPhrases.ObjectFeatures
{
    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void CauseOfPainAndSuffering() => Assert.AreEqual(
            "Cause of pain and suffering.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("cause of pain and suffering")));
    }
}

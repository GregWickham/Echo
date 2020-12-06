using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.PrepositionalPhrases.ObjectTypes
{
    [TestClass]
    public class VerbPhrase
    {
        [TestMethod]
        public void ACauseOfSufferingToHisParents() => Assert.AreEqual(
            "A cause of suffering to his parents.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a cause of suffering to his parents")));
    }
}

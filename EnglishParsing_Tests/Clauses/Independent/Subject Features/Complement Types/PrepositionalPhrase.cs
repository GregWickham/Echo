using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.SubjectFeatures.ComplementTypes
{
    [TestClass]
    public class PrepositionalPhrase
    {
        [TestMethod]
        public void HugeThrongsOfVotersWaitedInLine() => Assert.AreEqual(
            "Huge throngs of voters waited in line.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("huge throngs of voters waited in line")));
    }
}

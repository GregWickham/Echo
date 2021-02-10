using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.HeadFeatures.Tenses
{
    [TestClass]
    public class Future
    {
        [TestMethod]
        public void WillPrevail() => Assert.AreEqual("Will prevail.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("will prevail")));
    }
}


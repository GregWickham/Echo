using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.PhraseFeatures
{
    [TestClass]
    public class Possessive
    {
        [TestMethod]
        public void MyHeadache() => Assert.AreEqual("My headache.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("my headache")));

        [TestMethod]
        public void KarensComplaint() => Assert.AreEqual("Karen's complaint.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("Karen's complaint")));
    }
}

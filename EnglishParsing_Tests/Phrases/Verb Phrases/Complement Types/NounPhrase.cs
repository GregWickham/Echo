using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ComplementTypes
{
    [TestClass]
    public class NounPhrase
    {
        [TestMethod]
        public void TheAuditorExaminedTheBooksCarefully() => Assert.AreEqual("The auditor examined the books carefully.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the auditor examined the books carefully")));
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ComplementTypes
{
    [TestClass]
    public class NounPhraseAndPrepositionalPhrase
    {
        [TestMethod]
        public void TheProsecutorWillUseTheBooksAsEvidence() => Assert.AreEqual("The prosecutor will use the books as evidence.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the prosecutor will use the books as evidence")));
    }
}
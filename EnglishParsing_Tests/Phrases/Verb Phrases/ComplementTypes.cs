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

    [TestClass]
    public class PrepositionalPhrase
    {
        [TestMethod]
        public void StaringIntoTheSun() => Assert.AreEqual("Staring into the sun.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("staring into the sun")));


        [TestMethod]
        public void RunningAgainstTheDevil() => Assert.AreEqual("Running against the devil.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("running against the devil")));
    }

    [TestClass]
    public class NounPhraseAndPrepositionalPhrase
    {
        [TestMethod]
        public void TheProsecutorWillUseTheBooksAsEvidence() => Assert.AreEqual("The prosecutor will use the books as evidence.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the prosecutor will use the books as evidence")));
    }


    [TestClass]
    public class AdjectivePhrase
    {
        [TestMethod]
        public void TheChildIsHappy() => Assert.AreEqual("The child is happy.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the child is happy")));

        [TestClass]
        public class WithPrepositionalPhraseComplement
        {
            [TestMethod]
            public void TheShopperIsAngryWithTheHighPrices() => Assert.AreEqual("The shopper is angry with the high prices.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("The shopper is angry with the high prices")));
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ComplementTypes
{
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

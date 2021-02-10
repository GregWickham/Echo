using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.AdjectivePhrases.ComplementTypes
{
    [TestClass]
    public class PrepositionalPhrase
    {
        [TestMethod]
        public void IAmGreenWithEnvy() => Assert.AreEqual(
            "I am green with envy.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("I am green with envy")));
    }
}

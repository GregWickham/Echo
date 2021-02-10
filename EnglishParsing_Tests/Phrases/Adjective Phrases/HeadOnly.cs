using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.AdjectivePhrases
{
    [TestClass]
    public class HeadOnly
    {
        [TestMethod]
        public void Beautiful() => Assert.AreEqual(
           "Beautiful.",
           SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("beautiful")));
    }

}

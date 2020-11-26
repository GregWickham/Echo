using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.HeadFeatures
{
    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void DrivingAndCrying() => Assert.AreEqual("driving and crying",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("driving and crying")));
    }
}


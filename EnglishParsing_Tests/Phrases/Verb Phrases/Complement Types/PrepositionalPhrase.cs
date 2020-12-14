using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ComplementTypes
{
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
}
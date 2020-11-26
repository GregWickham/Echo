using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.HeadFeatures.Forms
{


    [TestClass]
    public class PastParticiple
    {
        [TestMethod]
        public void ComprisedOfElites() => Assert.AreEqual("Comprised of elites.", 
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("comprised of elites")));
    }
}


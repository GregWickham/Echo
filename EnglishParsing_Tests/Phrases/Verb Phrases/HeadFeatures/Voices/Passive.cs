using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.HeadFeatures.Voices.Passive
{
    [TestClass]
    public class Tenses
    {
        [TestClass]
        public class Past
        {
            [TestClass]
            public class PastParticiple
            {
                [TestMethod]
                public void WasDestroyed() => Assert.AreEqual("Was destroyed.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("was destroyed")));
            }
        }

    }

}


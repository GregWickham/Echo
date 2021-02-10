using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ModifierTypes
{
    [TestClass]
    public class AdjectivePhrase
    {
        [TestMethod]
        public void TheEccentricSculptorResponsibleForThisApparentDisturbanceOfAnOldMansPeaceOfMind() => Assert.AreEqual("The eccentric sculptor responsible for this apparent disturbance of an old man's peace of mind.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the eccentric sculptor responsible for this apparent disturbance of an old man's peace of mind")));

        [TestClass]
        public class WithOxfordCommaAdded
        {
            [TestMethod]
            public void AThinDarkYoungManOfNeuroticAndExcitedAspect() => Assert.AreEqual("A thin, dark, young man of neurotic and excited aspect.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a thin, dark young man of neurotic and excited aspect")));
        }
    }
}
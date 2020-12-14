using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ModifierTypes
{
    [TestClass]
    public class NominalModifier
    {
        [TestMethod]
        public void AnAirForceBomberSquadron() => Assert.AreEqual("An Air Force bomber squadron.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("an Air Force bomber squadron")));

        [TestMethod]
        public void TheUnitedStatesAirForce() => Assert.AreEqual("The United States Air Force.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the United States Air Force")));

        [TestClass]
        public class CompoundHead
        {
            [TestMethod]
            public void StunninglyVividGeometricHallucinations() => Assert.AreEqual("Stunningly vivid geometric hallucinations.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("stunningly vivid geometric hallucinations")));
        }
    }
}

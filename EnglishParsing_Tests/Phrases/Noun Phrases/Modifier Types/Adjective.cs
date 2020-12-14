using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ModifierTypes
{
    [TestClass]
    public class Adjective
    {
        [TestMethod]
        public void SlenderMan() => Assert.AreEqual("Slender man.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("slender man")));

        [TestClass]
        public class AdverbIntensifier
        {
            [TestMethod]
            public void VeryImportantPerson() => Assert.AreEqual("Very important person.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("very important person")));

            [TestMethod]
            public void AShockinglyBrazenLiar() => Assert.AreEqual("A shockingly brazen liar.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a shockingly brazen liar")));
        }

        [TestClass]
        public class Comparative
        {
            [TestMethod]
            public void AMoreInterestingPresentation() => Assert.AreEqual("A more interesting presentation.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a more interesting presentation")));

            [TestClass]
            public class AdverbIntensifier
            {
                [TestMethod]
                public void AMuchMoreColorfulOutfit() => Assert.AreEqual("A much more colorful outfit.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a much more colorful outfit")));

                [TestMethod]
                public void FarMoreVividHallucinations() => Assert.AreEqual("Far more vivid hallucinations.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("far more vivid hallucinations")));
            }
        }

        [TestClass]
        public class Superlative
        {
            [TestMethod]
            public void TheDumbestSOB() => Assert.AreEqual("The dumbest son of a bitch.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the dumbest son of a bitch")));

            [TestMethod]
            public void TheWorstPresidentInTheHistoryOfTheCountry() => Assert.AreEqual("The worst president in the history of the country.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the worst president in the history of the country")));
        }
    }
}
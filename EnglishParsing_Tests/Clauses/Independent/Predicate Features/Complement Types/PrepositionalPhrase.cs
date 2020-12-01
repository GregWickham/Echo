using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.PredicateFeatures.ComplementTypes
{
    [TestClass]
    public class PrepositionalPhrase
    {
        [TestMethod]
        public void IFightDarkForcesInTheClearMoonlight() => Assert.AreEqual(
            "I fight dark forces in the clear moonlight.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("I fight dark forces in the clear moonlight")));

        [TestMethod]
        public void WeShallFightInFrance() => Assert.AreEqual(
            "We shall fight in France.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight in France")));

        [TestMethod]
        public void WeShallFightOnTheBeaches() => Assert.AreEqual(
            "We shall fight on the beaches.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight on the beaches")));

        [TestMethod]
        public void WeShallFightInTheHills() => Assert.AreEqual(
            "We shall fight in the hills.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight in the hills")));

        [TestClass]
        public class CompoundNounObject
        {
            [TestMethod]
            public void WeShallFightOnTheLandingGrounds() => Assert.AreEqual(
                "We shall fight on the landing grounds.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight on the landing grounds")));
        }

        [TestClass]
        public class CoordinatedObject
        {
            [TestMethod]
            public void WeShallFightOnTheSeasAndOceans() => Assert.AreEqual(
                "We shall fight on the seas and oceans.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight on the seas and oceans")));

            [TestMethod]
            public void WeShallFightInTheFieldsAndInTheStreets() => Assert.AreEqual(
                "We shall fight in the fields and in the streets.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight in the fields and in the streets")));

            [TestMethod]
            public void WeShallFightWithGrowingConfidenceAndGrowingStrength() => Assert.AreEqual(
                "We shall fight with growing confidence and growing strength.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight with growing confidence and growing strength")));


            [TestMethod]
            public void WeShallFightWithGrowingConfidenceAndGrowingStrengthInTheAir() => Assert.AreEqual(
                "We shall fight with growing confidence and growing strength in the air.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we shall fight with growing confidence and growing strength in the air")));

            [TestMethod]
            public void RosePunchedHerOpponentInTheFaceAndBody() => Assert.AreEqual(
                "Rose punched her opponent in the face and body.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("Rose punched her opponent in the face and body")));
        }
    }

}

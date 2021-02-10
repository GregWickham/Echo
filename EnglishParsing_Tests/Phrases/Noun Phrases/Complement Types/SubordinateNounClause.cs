using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ComplementTypes
{
    [TestClass]
    public class SubordinateNounClause
    {
        [TestClass]
        public class Complementizers
        {
            [TestClass]
            public class That
            {
                [TestMethod]
                public void TheClaimThatTheEarthIsFlat() => Assert.AreEqual("The claim that the Earth is flat.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the claim that the Earth is flat")));

                [TestMethod]
                public void TheFactThatYouBrushYourTeethBeforeBed() => Assert.AreEqual("The fact that you brush your teeth before bed.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the fact that you brush your teeth before bed")));

                [TestMethod]
                public void OurHopeThatNoChildWillEverGoHungry() => Assert.AreEqual("Our hope that no child will ever go hungry.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("our hope that no child will ever go hungry")));
            }
        }

        [TestClass]
        public class WhWords
        {
            [TestClass]
            public class WhPronoun
            {
                [TestClass]
                public class Who
                {
                    [TestMethod]
                    public void AnEnemyWhoLurksInTheShadows() => Assert.AreEqual("An enemy who lurks in the shadows.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("an enemy who lurks in the shadows")));
                }

                [TestClass]
                public class Whom
                {
                    [TestMethod]
                    public void TheManWhomISawInTheCrosswalk() => Assert.AreEqual("The man whom I saw in the crosswalk.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the man whom I saw in the crosswalk")));
                }

                [TestClass]
                public class Where
                {
                    [TestMethod]
                    public void SituationsWhereCalmPrevails() => Assert.AreEqual("Situations where calm prevails.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("situations where calm prevails")));

                    [TestMethod]
                    public void ASituationWhereCalmerHeadsPrevailed() => Assert.AreEqual("A situation where calmer heads prevailed.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a situation where calmer heads prevailed")));
                }

                [TestClass]
                public class When
                {
                    [TestMethod]
                    public void ATimeWhenTheWorldWasYoung() => Assert.AreEqual("A time when the world was young.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a time when the world was young")));
                }


                [TestClass]
                public class How
                {
                    [TestMethod]
                    public void WeAllSawHowYouTreatedYourWife() => Assert.AreEqual("We all saw how you treated your wife.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we all saw how you treated your wife")));
                }

                [TestClass]
                public class Why
                {
                    [TestMethod]
                    public void TheReasonWhyTheDesignIsSoComplex() => Assert.AreEqual("The reason why the design is so complex.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the reason why the design is so complex")));
                }

                [TestClass]
                public class Which
                {
                    [TestMethod]
                    public void AnIdeaWhichCouldHaveMerit() => Assert.AreEqual("An idea which could have merit.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("an idea which could have merit")));
                }

                [TestClass]
                public class Possessive
                {
                    [TestMethod]
                    public void ADebutanteWhoseDressIsLovely() => Assert.AreEqual("A debutante whose dress is lovely.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a debutante whose dress is lovely")));

                    [TestMethod]
                    public void AWitnessWhoseHonestyAndIntegrityAreUnimpeachable() => Assert.AreEqual("A witness whose honesty and integrity are unimpeachable.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a witness whose honesty and integrity are unimpeachable")));
                }
            }
        }

    }
}

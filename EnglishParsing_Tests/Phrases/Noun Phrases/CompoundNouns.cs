using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases
{
    public partial class HeadOnly
    {
        [TestClass]
        public class CompoundNouns
        {
            [TestClass]
            public class Open
            {
                [TestMethod]
                public void PeanutButter() => Assert.AreEqual(
                    "Peanut butter.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("peanut butter")));

                [TestMethod]
                public void IceCream() => Assert.AreEqual(
                    "Ice cream.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("ice cream")));

                [TestMethod]
                public void LibraryBook() => Assert.AreEqual(
                    "Library book.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("library book")));

                [TestMethod]
                public void PostOffice() => Assert.AreEqual(
                    "Post office.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("post office")));

                [TestClass]
                public class ThreeComponentWords
                {
                    [TestMethod]
                    public void TheUnitedStatesNavy() => Assert.AreEqual(
                        "The United States Navy.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the United States Navy")));
                }

                [TestClass]
                public class ParsedAsSpecifier
                {
                    [TestMethod]
                    public void NoOne() => Assert.AreEqual(
                        "No one.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("no one")));
                }

                [TestClass]
                public class ParsedAsModifier
                {
                    [TestMethod]
                    public void RealEstate() => Assert.AreEqual(
                        "Real estate.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("real estate")));

                    [TestMethod]
                    public void HighSchool() => Assert.AreEqual(
                        "High school.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("high school")));

                    [TestMethod]
                    public void SweetTooth() => Assert.AreEqual(
                        "Sweet tooth.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("sweet tooth")));

                    [TestMethod]
                    public void HotDog() => Assert.AreEqual(
                        "Hot dog.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("hot dog")));

                    [TestMethod]
                    public void GrandJury() => Assert.AreEqual(
                        "Grand jury.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("grand jury")));

                    [TestMethod]
                    public void FullMoon() => Assert.AreEqual(
                        "Full moon.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("full moon")));

                    [TestMethod]
                    public void HalfSister() => Assert.AreEqual(
                        "Half sister.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("half sister")));

                    [TestClass]
                    public class PresentParticiple
                    {
                        [TestMethod]
                        public void LivingRoom() => Assert.AreEqual(
                            "Living room.",
                            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("living room")));

                        [TestMethod]
                        public void FeedingTime() => Assert.AreEqual(
                            "Feeding time.",
                            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("feeding time")));

                        [TestMethod]
                        public void WitchingHour() => Assert.AreEqual(
                            "Witching hour.",
                            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("witching hour")));

                        [TestMethod]
                        public void SwimmingPool() => Assert.AreEqual(
                            "Swimming pool.",
                            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("swimming pool")));

                        //[TestMethod]
                        //public void FightingWords() => Assert.AreEqual(
                        //    "Fighting words.",
                        //    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("fighting words")));
                    }
                }
            }
        }
    }
}

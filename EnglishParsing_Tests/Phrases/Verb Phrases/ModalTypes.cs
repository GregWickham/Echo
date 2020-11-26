using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ModalTypes
{
    [TestClass]
    public class Can
    {
        [TestMethod]
        public void CanPrevail() => Assert.AreEqual("Can prevail.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("can prevail")));

        [TestMethod]
        public void ICanTellTheQueenOfDiamonds() => Assert.AreEqual("I can tell the queen of diamonds.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("i can tell the queen of diamonds")));
    }

    [TestClass]
    public class Could
    {

        [TestMethod]
        public void CouldEnlist() => Assert.AreEqual("Could enlist.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("could enlist")));

        [TestMethod]
        public void ICouldPayYouBack() => Assert.AreEqual("I could pay you back.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("i could pay you back")));
    }

    [TestClass]
    public class May
    {
        [TestMethod]
        public void YouMayBeRight() => Assert.AreEqual("You may be right.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("you may be right")));

        [TestMethod]
        public void IMayBeCrazy() => Assert.AreEqual("I may be crazy.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("i may be crazy")));
    }

    [TestClass]
    public class Might
    {
        [TestMethod]
        public void MightAttend() => Assert.AreEqual("Might attend.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("might attend")));

        [TestMethod]
        public void MightBe() => Assert.AreEqual("Might be.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("might be")));
    }

    [TestClass]
    public class Must
    {
        [TestMethod]
        public void MustFight() => Assert.AreEqual("Must fight.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("must fight")));

        [TestMethod]
        public void MustRetreat() => Assert.AreEqual("Must retreat.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("must retreat")));
    }

    [TestClass]
    public class Shall
    {
        [TestMethod]
        public void ShallOvercome() => Assert.AreEqual("Shall overcome.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("shall overcome")));
    }

    [TestClass]
    public class Should
    {
        [TestMethod]
        public void ShouldConcede() => Assert.AreEqual("Should concede.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("should concede")));
    }

    [TestClass]
    public class Will
    {
        [TestMethod]
        public void WillChange() => Assert.AreEqual("Will change.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("will change")));
    }

    [TestClass]
    public class Would
    {
        [TestMethod]
        public void WouldBe() => Assert.AreEqual("Would be.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("would be")));

        [TestMethod]
        public void WouldConsider() => Assert.AreEqual("Would consider.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("would consider")));
    }
}

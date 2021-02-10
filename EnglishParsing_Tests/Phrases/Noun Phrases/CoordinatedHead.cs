using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.CoordinatedElements
{
    [TestClass]
    public class Head
    {
        [TestMethod]
        public void ABoyAndHisDog() => Assert.AreEqual(
            "a boy and his dog",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a boy and his dog")));

        [TestMethod]
        public void YourExquisiteWifeAndMother() => Assert.AreEqual(
            "your exquisite wife and mother",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("your exquisite wife and mother")));

        [TestMethod]
        public void ASupremelyIncompetentBuffoonAndALoathsomeWretch() => Assert.AreEqual(
            "a supremely incompetent buffoon and a loathsome wretch",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a supremely incompetent buffoon and a loathsome wretch")));
    }
}

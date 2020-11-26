using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases
{
    [TestClass]
    public partial class HeadOnly
    {
        [TestMethod]
        public void Person() => Assert.AreEqual(
            "Person.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("person")));

        [TestMethod]
        public void Woman() => Assert.AreEqual(
            "Woman.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("woman")));

        // Man gets parsed as an interjection :(

        [TestMethod]
        public void Camera() => Assert.AreEqual(
            "Camera.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("camera")));

        [TestMethod]
        public void TV() => Assert.AreEqual(
            "TV.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("TV")));
    }
}



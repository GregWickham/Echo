using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ComplementTypes
{
    [TestClass]
    public class VerbPhrase
    {
        [TestMethod]
        public void SheHasProblemsFindingHerShoes() => Assert.AreEqual("She has problems finding her shoes.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("she has problems finding her shoes")));

        [TestMethod]
        public void HisTroubleFinishingHisDegree() => Assert.AreEqual("His trouble finishing his degree.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("his trouble finishing his degree")));
    }
}

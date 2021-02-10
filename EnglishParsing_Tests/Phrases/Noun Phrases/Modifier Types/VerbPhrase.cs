using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ModifierTypes
{
    [TestClass]
    public class VerbPhrase
    {
        [TestClass]
        public class Participle
        {
            [TestClass]
            public class Past
            {
                [TestMethod]
                public void TheArtistFormerlyKnownAsPrince() => Assert.AreEqual(
                    "The artist formerly known as Prince.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the artist formerly known as Prince")));
            }
        }
    }
}
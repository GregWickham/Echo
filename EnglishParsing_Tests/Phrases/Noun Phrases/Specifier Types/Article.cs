using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.SpecifierTypes
{
    [TestClass]
    public class Article
    {
        [TestClass]
        public class Indefinite
        {
            [TestMethod]
            public void AHand() => Assert.AreEqual("A hand.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a hand")));

            /// <summary>SimpleNLG does indefinite article agreement when the article immediately precedes a noun or adjective.  But not when it precedes an adverb.</summary>
            [TestClass]
            public class Agreement
            {
                /// <summary>This case would be handled correctly by SimpleNLG</summary>
                [TestMethod]
                public void AnOracle() => Assert.AreEqual("An oracle.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a oracle")));

                /// <summary>This case would be handled correctly by SimpleNLG</summary>
                [TestMethod]
                public void ABadCold() => Assert.AreEqual("A bad cold.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a bad cold")));

                /// <summary>This case would be handled correctly by SimpleNLG</summary>
                [TestMethod]
                public void AnAwfulSituation() => Assert.AreEqual("An awful situation.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a awful situation")));

                /// <summary>This case would NOT be handled correctly by SimpleNLG, but it's handled by the Echo DeterminerBuilder</summary>
                [TestMethod]
                public void AnExtremelyBadSituation() => Assert.AreEqual("An extremely bad situation.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("a extremely bad situation")));
            }
        }

        [TestClass]
        public class Definite
        {
            [TestMethod]
            public void TheBicycle() => Assert.AreEqual("The bicycle.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the bicycle")));
        }
    }
}

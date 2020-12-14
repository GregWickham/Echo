using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.SpecifierTypes
{
    [TestClass]
    public class PossessivePronoun
    {
        [TestClass]
        public class FirstPerson
        {
            [TestClass]
            public class Singular
            {
                [TestMethod]
                public void MyHand() => Assert.AreEqual("My hand.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("my hand")));
            }

            [TestClass]
            public class Plural
            {
                [TestMethod]
                public void OurFlag() => Assert.AreEqual("Our flag.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("our flag")));
            }
        }

        [TestClass]
        public class SecondPerson
        {
            [TestClass]
            public class Singular
            {
                [TestMethod]
                public void YourHand() => Assert.AreEqual("Your hand.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("your hand")));
            }

            [TestClass]
            public class Plural
            {
                [TestMethod]
                public void YourCountry() => Assert.AreEqual("Your country.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("your country")));
            }
        }

        [TestClass]
        public class ThirdPerson
        {
            [TestClass]
            public class Singular
            {
                [TestClass]
                public class Feminine
                {
                    [TestMethod]
                    public void HerBlouse() => Assert.AreEqual("Her blouse.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("her blouse")));
                }

                [TestClass]
                public class Masculine
                {
                    [TestMethod]
                    public void HisTruck() => Assert.AreEqual("His truck.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("his truck")));
                }

                [TestClass]
                public class Neuter
                {
                    [TestMethod]
                    public void ItsEngine() => Assert.AreEqual("Its engine.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("its engine")));
                }
            }

            [TestClass]
            public class Plural
            {
                [TestMethod]
                public void TheirSupplier() => Assert.AreEqual("Their supplier.",
                        SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("their supplier")));
            }
        }
    }
}
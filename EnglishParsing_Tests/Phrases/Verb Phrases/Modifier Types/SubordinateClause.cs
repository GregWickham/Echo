using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ModifierTypes
{
    [TestClass]
    public class SubordinateClause
    {
        [TestClass]
        public class Complementizers
        {
            [TestClass]
            public class Because
            {
                [TestMethod]
                public void IRanBecauseIWasAfraid() => Assert.AreEqual("I ran because I was afraid.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("I ran because I was afraid")));
            }

            [TestClass]
            public class But
            {
                [TestMethod]
                public void IRanButTheyCaughtMe() => Assert.AreEqual("I ran but they caught me.",
                    SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("I ran but they caught me")));
            }
        }
    }
}

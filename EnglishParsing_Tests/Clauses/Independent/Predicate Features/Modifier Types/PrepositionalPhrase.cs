using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.Clauses.Independent.PredicateFeatures.ModifierTypes
{
    [TestClass]
    public class PrepositionalPhrase
    {
        [TestMethod]
        public void TheBeginningOfWardsMadness() => Assert.AreEqual(
            "The beginning of Ward's madness is a matter of dispute among alienists.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the beginning of Ward's madness is a matter of dispute among alienists")));

        
           [TestClass]
        public class Multiple
        {
            [TestMethod]
            public void WeLiveOnAPlacidIslandOfIgnorance() => Assert.AreEqual(
                "We live on a placid island of ignorance in the midst of black seas of infinity.",
                SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("we live on a placid island of ignorance in the midst of black seas of infinity")));
        }
    }
}

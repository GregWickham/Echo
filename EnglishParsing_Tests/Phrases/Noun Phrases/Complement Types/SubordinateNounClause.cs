using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.NounPhrases.ComplementTypes
{
    [TestClass]
    public class SubordinateNounClause
    {
        [TestMethod]
        public void TheClaimThatTheEarthIsFlat() => Assert.AreEqual("The claim that the Earth is flat.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the claim that the Earth is flat")));

        [TestMethod]
        public void TheFactThatYouBrushYourTeethBeforeBed() => Assert.AreEqual("The fact that you brush your teeth before bed.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("the fact that you brush your teeth before bed")));

        [TestMethod]
        public void OurHopeThatNoChildWillEverGoHungry() => Assert.AreEqual("Our hope that no child will ever go hungry.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("our hope that no child will ever go hungry")));
    }
}

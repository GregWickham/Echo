using Microsoft.VisualStudio.TestTools.UnitTesting;
using FlexibleRealization;

namespace EnglishParsing.Tests.VerbPhrases.ComplementTypes
{
    [TestClass]
    public class Infinitive
    {
        [TestMethod]
        public void IWantToGoSkydiving() => Assert.AreEqual("I want to go skydiving.",
            SimpleNLG.Client.Realize(FlexibleRealizerFactory.SpecFrom("I want to go skydiving")));
    }
}
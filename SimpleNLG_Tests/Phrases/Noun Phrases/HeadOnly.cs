using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.NounPhrases
{
    [TestClass]
    public class HeadOnly
    {
        [TestMethod]
        public void Hand() => Assert.AreEqual("Hand.", 
            Client.Realize(Phrase.Noun("hand")));

        [TestClass]
        public class Compound
        {
            [TestMethod]
            public void GroceryStore() => Assert.AreEqual("Grocery store.", 
                Client.Realize(Phrase.Noun("grocery store")));
        }
    }
}


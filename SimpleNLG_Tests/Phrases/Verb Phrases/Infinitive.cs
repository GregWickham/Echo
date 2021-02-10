using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.VerbPhrases.HeadForms
{
    [TestClass]
    public class Infinitive
    {
        [TestMethod]
        public void ToGo()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Form = form.INFINITIVE,
                Head = Word.Verb("go")
            });
            Assert.AreEqual("To go.", realized);
        }
    }

}

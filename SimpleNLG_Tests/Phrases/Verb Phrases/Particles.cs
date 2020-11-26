using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.VerbPhrases.Particles
{
    [TestClass]
    public class AfterHeadWord
    {
        [TestMethod]
        public void LookedUp()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Tense = tense.PAST,
                Head = Word.Verb("look"),
                PostModifiers = new NLGElement[]
                {
                    Word.Adverb("up")                    
                }
            });
            Assert.AreEqual("Looked up.", realized);
        }

        [TestMethod]
        public void DroveAway()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Tense = tense.PAST,
                Head = Word.Verb("drive"),
                PostModifiers = new NLGElement[]
                {
                    Word.Adverb("away")
                }
            });
            Assert.AreEqual("Drove away.", realized);
        }

        [TestMethod]
        public void StartsOut()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Tense = tense.PRESENT,
                Head = Word.Verb("start"),
                PostModifiers = new NLGElement[]
                {
                    Word.Adverb("out")
                }
            });
            Assert.AreEqual("Starts out.", realized);
        }

        [TestMethod]
        public void TakingOff()
        {
            string realized = Client.Realize(new VPPhraseSpec
            {
                Tense = tense.PRESENT,
                Form = form.PRESENT_PARTICIPLE,
                Head = Word.Verb("take"),
                PostModifiers = new NLGElement[]
                {
                    Word.Adverb("off")
                }
            });
            Assert.AreEqual("Taking off.", realized);
        }
    }
}


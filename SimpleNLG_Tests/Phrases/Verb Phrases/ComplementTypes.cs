using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.VerbPhrases.ComplementTypes
{
    [TestClass]
    public class OpenClausal
    {
        [TestMethod]
        public void EvolutionGoneHorriblyWrong()
        {
            string realized = Client.Realize(new NPPhraseSpec
            {
                Head = Word.Noun("evolution"),
                PostModifiers = new NLGElement[]
                {
                    new VPPhraseSpec
                    {
                        Form = form.PAST_PARTICIPLE,
                        Head = Word.Verb("go"),
                        PostModifiers = new NLGElement[]
                        {
                            new AdjPhraseSpec
                            {
                                PreModifiers = new NLGElement[]
                                {
                                    Word.Adverb("horribly")
                                },
                                Head = Word.Adjective("wrong")
                            }
                        }
                    }
                }
            });
            Assert.AreEqual("Evolution gone horribly wrong.", realized);
        }

    }
}


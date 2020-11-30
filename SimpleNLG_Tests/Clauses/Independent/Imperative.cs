using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses.Independent
{
    [TestClass]
    public class Imperative
    {
        [TestMethod]
        public void PutThePianoAndTheDrumIntoTheTruck() => Assert.AreEqual("Put the piano and the drum into the truck.",
            Client.Realize(new SPhraseSpec
            {
                FORM = form.IMPERATIVE,
                FORMSpecified = true,
                vp = new VPPhraseSpec
                {
                    compl = new NLGElement[]
                    {
                        new CoordinatedPhraseElement
                        {
                            conj = "and",
                            coord = new NLGElement[]
                            {
                                new NPPhraseSpec
                                {
                                    head = Word.Noun("piano"),
                                    spec = Word.Determiner("the")
                                },
                                new NPPhraseSpec
                                {
                                    head = Word.Noun("drum"),
                                    spec = Word.Determiner("the")
                                }
                            }
                        },
                        new PPPhraseSpec()
                        {
                            compl = new NLGElement[]
                            {
                                new NPPhraseSpec
                                {
                                    head = Word.Noun("truck"),
                                    spec = Word.Determiner("the")
                                }
                            },
                            head = Word.Preposition("into")
                        }
                    },
                    head = Word.Verb("put")
                }
            }));

    }
}

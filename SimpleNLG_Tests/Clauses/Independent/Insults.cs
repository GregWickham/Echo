using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses
{
    [TestClass]
    public class Insult
    {
        [TestMethod]
        public void GregIsBad() => Assert.AreEqual("Greg is bad.", Client.Realize(new SPhraseSpec
        {
            Form = form.NORMAL,
            Tense = tense.PRESENT,
            subj = new NLGElement[]
            {
                new NPPhraseSpec
                {
                    head = Word.Noun("greg").SetProper(true)
                }
            },
            vp = new VPPhraseSpec
            {
                head = Word.Verb("be"),
                compl = new NLGElement[]
                {
                    new AdjPhraseSpec
                    {
                        head = Word.Adjective("bad")
                    }
                }
            }
        }));

        [TestMethod]
        public void CoordinatedComplement() => Assert.AreEqual("Greg is an incompetent fool, an imbecile and a wildly mendacious narcissist.", Client.Realize(new SPhraseSpec
        {
            Form = form.NORMAL,
            Tense = tense.PRESENT,
            subj = new NLGElement[]
            {
                Word.Noun("greg").SetProper(true)
            },
            vp = new VPPhraseSpec
            {
                head = Word.Verb("be"),
                compl = new NLGElement[]
                {
                    new CoordinatedPhraseElement
                    {
                        conj = "and",
                        coord = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                preMod = new NLGElement[]
                                {
                                    Word.Adjective("incompetent")
                                },
                                head = Word.Noun("fool"),
                                spec = Word.Determiner("a")
                            },
                            new NPPhraseSpec
                            {
                                head = Word.Noun("imbecile"),
                                spec = Word.Determiner("a")
                            },
                            new NPPhraseSpec
                            {
                                preMod = new NLGElement[]
                                {
                                    new AdjPhraseSpec
                                    {
                                        head = Word.Adjective("mendacious"),
                                        preMod = new NLGElement[]
                                        {
                                            Word.Adverb("wildly")
                                        }
                                    }
                                },
                                head = Word.Noun("narcissist"),
                                spec = Word.Determiner("a")
                            }
                        }
                    }
                }
            }
        }));

    }
}

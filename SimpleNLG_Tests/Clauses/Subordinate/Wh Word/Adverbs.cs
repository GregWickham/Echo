using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses.Subordinate.WhWord.Adverbs
{
    [TestClass]
    public class When
    {
        [TestMethod]
        public void WhenYouLoseYourWay()
        {
            Assert.AreEqual("When you lose your way.",
            Client.Realize(new SPhraseSpec
            {
                ClauseStatus = clauseStatus.SUBORDINATE,
                Complementiser = "when",
                Subjects = new NLGElement[]
                {
                    Word.Pronoun("you"),
                },
                Predicate = new VPPhraseSpec
                {
                    Form = form.BARE_INFINITIVE,
                    Head = Word.Verb("lose"),
                    Complements = new NLGElement[]
                    {
                        new NPPhraseSpec
                        {
                            Specifier = new NPPhraseSpec
                            {
                                Head = Word.Pronoun("you"),
                                Possessive = true
                            },
                            Head = Word.Noun("way"),
                        }
                    }
                }
            }));
        }
    }

}

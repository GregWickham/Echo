using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses.Subordinate.WhWord.PossessivePronoun
{
    [TestClass]
    public class Whose
    {
        [TestMethod]
        public void WhoseReputationPrecedesHim()
        {
            Assert.AreEqual("Whose reputation precedes him.",
            Client.Realize(new SPhraseSpec
            {
                ClauseStatus = clauseStatus.SUBORDINATE,
                Complementiser = "whose",
                Subjects = new NLGElement[]
                {
                    Word.Noun("reputation")
                },
                Predicate = new VPPhraseSpec
                {    
                    Head = Word.Verb("precede"),
                    Complements = new NLGElement[]
                    {
                        Word.Noun("him")
                    }
                }
            }));
        }
    }

}


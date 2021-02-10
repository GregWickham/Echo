using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses.Independent.Coordinated
{
    [TestClass]
    public class AsSubordinateComplement
    {
        [TestMethod]
        public void TheChildrenRanButTheMonsterCaughtThem()
        {
            Assert.AreEqual("The children ran but the monster caught them.",
            Client.Realize(new SPhraseSpec
            {
                Subjects = new NLGElement[]
                {
                    new NPPhraseSpec
                    {
                        Specifier = Word.Determiner("the"),
                        Head = Word.Noun("child"),
                        Number = numberAgreement.PLURAL
                    }
                },
                Predicate = new VPPhraseSpec
                {
                    Tense = tense.PAST,
                    Head = Word.Verb("run"),
                    Complements = new NLGElement[]
                    {
                        new SPhraseSpec
                        {
                            ClauseStatus = clauseStatus.SUBORDINATE,
                            Complementiser = "but",
                            Subjects = new NLGElement[]
                            {
                                new NPPhraseSpec
                                {
                                    Specifier = Word.Determiner("the"),
                                    Head = Word.Noun("monster")
                                }
                            },
                            Predicate = new VPPhraseSpec
                            {
                                Tense = tense.PAST,
                                Head = Word.Verb("catch"),
                                Complements = new NLGElement[]
                                {
                                    Word.Pronoun("them")
                                }
                            }
                        }
                    }
                }
            }));
        }
    }

    [TestClass]
    public class AsPeerIndependentClauses
    {
        [TestMethod]
        public void TheChildrenRanButTheMonsterCaughtThem()
        {
            Assert.AreEqual("the children ran but the monster caught them",
            Client.Realize(new CoordinatedPhraseElement
            {
                Conjunction = "but",
                Coordinated = new NLGElement[]
                {
                    new SPhraseSpec
                    {
                        Subjects = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("child"),
                                Number = numberAgreement.PLURAL
                            }
                        },
                        Predicate = new VPPhraseSpec
                        {
                            Tense = tense.PAST,
                            Head = Word.Verb("run"),
                        }
                    },
                    new SPhraseSpec
                    {
                        Subjects = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("monster")
                            }
                        },
                        Predicate = new VPPhraseSpec
                        {
                            Tense = tense.PAST,
                            Head = Word.Verb("catch"),
                            Complements = new NLGElement[]
                            {
                                Word.Pronoun("them")
                            }
                        }
                    }
                }
            }));
        }

        [TestMethod]
        public void TheChildrenRanBecauseTheMonsterWasChasingThem()
        {
            Assert.AreEqual("the children ran because the monster was chasing them",
            Client.Realize(new CoordinatedPhraseElement
            {
                Conjunction = "because",
                Coordinated = new NLGElement[]
                {
                    new SPhraseSpec
                    {
                        Subjects = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("child"),
                                Number = numberAgreement.PLURAL
                            }
                        },
                        Predicate = new VPPhraseSpec
                        {
                            Tense = tense.PAST,
                            Head = Word.Verb("run"),
                        }
                    },
                    new SPhraseSpec
                    {
                        Subjects = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = Word.Determiner("the"),
                                Head = Word.Noun("monster")
                            }
                        },
                        Predicate = new VPPhraseSpec
                        {
                            Tense = tense.PAST,
                            Progressive = true,
                            Head = Word.Verb("chase"),
                            Complements = new NLGElement[]
                            {
                                Word.Pronoun("them")
                            }
                        }
                    }
                }
            }));
        }

    }
}


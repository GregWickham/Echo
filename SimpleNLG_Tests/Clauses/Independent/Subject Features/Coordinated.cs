using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses.Independent.SubjectFeatures
{
    [TestClass]
    public class Coordinated
    {
        [TestMethod]
        public void MaryJillAndAnitaHaveRedHair() => Assert.AreEqual("Mary, Jill and Anita have red hair.", Client.Realize(new SPhraseSpec
        {
            subj = new NLGElement[]
            {
                new CoordinatedPhraseElement
                {
                    conj = "and",
                    coord = new NLGElement[]
                    {
                        Word.Noun("Mary").SetProper(true),
                        Word.Noun("Jill").SetProper(true),
                        Word.Noun("Anita").SetProper(true)
                    }
                }
            },
            vp = new VPPhraseSpec
            {
                Head = Word.Verb("have"),
                Tense = tense.PRESENT,
                compl = new NLGElement[]
                {
                    new NPPhraseSpec
                    {
                        Head = Word.Noun("hair"),
                        PreModifiers = new NLGElement[]
                        {
                            Word.Adjective("red")
                        }
                    }
                }
            }
        }));

        [TestMethod]
        public void LootingAndPillagingArePopularActivitiesAmongMyPeople() => Assert.AreEqual("Looting and pillaging are popular activities among my people.", Client.Realize(new SPhraseSpec
        {
            subj = new NLGElement[]
            {
                new CoordinatedPhraseElement
                {
                    Conjunction = "and",
                    coord = new NLGElement[]
                    {
                        new VPPhraseSpec
                        {
                            Head = Word.Verb("loot"),
                            Form = form.GERUND
                        },
                        new VPPhraseSpec
                        {
                            Head = Word.Verb("pillage"),
                            Form = form.GERUND
                        }
                    }
                }
            },
            vp = new VPPhraseSpec
            {
                Head = Word.Verb("are"),
                Tense = tense.PRESENT,
                compl = new NLGElement[]
                {
                    new NPPhraseSpec
                    {
                        Head = Word.Noun("activities"),
                        PreModifiers = new NLGElement[]
                        {
                            Word.Adjective("popular")
                        }
                    },
                    new PPPhraseSpec
                    {
                        Head = Word.Preposition("among"),
                        Complements = new NLGElement[]
                        {
                            new NPPhraseSpec
                            {
                                Specifier = new NPPhraseSpec
                                {
                                    Head = Word.Pronoun("me"),
                                    Possessive = true
                                },
                                Head = Word.Noun("people")
                            }
                        }
                    }
                }
            }
        }));


        [TestClass]
        public class WithDeterminer
        {
            [TestMethod]
            public void MaryJillAndAnitaAllHaveRedHair() => Assert.AreEqual("Mary, Jill and Anita all have red hair.", Client.Realize(new SPhraseSpec
            {
                subj = new NLGElement[]
                {
                    new CoordinatedPhraseElement
                    {
                        conj = "and",
                        coord = new NLGElement[]
                        {
                            Word.Noun("Mary").SetProper(true),
                            Word.Noun("Jill").SetProper(true),
                            Word.Noun("Anita").SetProper(true)
                        }
                    },
                    new NPPhraseSpec
                    {
                        Specifier = Word.Determiner("all")
                    }
                },
                vp = new VPPhraseSpec
                {
                    Head = Word.Verb("have"),
                    Tense = tense.PRESENT,
                    compl = new NLGElement[]
                    {
                        new NPPhraseSpec
                        {
                            Head = Word.Noun("hair"),
                            PreModifiers = new NLGElement[]
                            {
                                Word.Adjective("red")
                            }
                        }
                    }
                }
            }));
        }
    }

}


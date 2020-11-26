using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.NounPhrases.ComplementTypes
{
    [TestClass]
    public class SubordinateClause
    {
        [TestMethod]
        public void LiesAndTheLyingLiarWhoTellsThem() => Assert.AreEqual("lies and the lying liar who tells them",
            Client.Realize(new CoordinatedPhraseElement
            {
                Conjunction = "and",
                Coordinated = new NLGElement[]
                {
                    Word.Noun("lies"),
                    new NPPhraseSpec
                    {
                        Specifier = Word.Determiner("the"),
                        head = Word.Noun("liar"),
                        PreModifiers = new NLGElement[]
                        {
                            new VPPhraseSpec
                            {
                                Head = Word.Verb("lie"),
                                Form = form.PRESENT_PARTICIPLE
                            }
                        },
                        Complements = new NLGElement[]
                        {
                            new SPhraseSpec
                            {
                                ClauseStatus = clauseStatus.SUBORDINATE,                                
                                Complementiser = "who",
                                Predicate = new VPPhraseSpec
                                {
                                    Head = new WordElement
                                    {
                                        Base = "tell"
                                    }
                                },                                
                                Complements = new NLGElement[]
                                {
                                    Word.Pronoun("them")
                                }
                            }
                        }
                    }
                }
            }));

        //[TestClass]
        //public class Plural
        //{
        //    [TestMethod]
        //    public void LiesAndTheLyingLiarsWhoTellThem() => Assert.AreEqual("lies and the lying liars who tell them",
        //        Client.Realize(new CoordinatedPhraseElement
        //        {
        //            Conjunction = "and",
        //            Coordinated = new NLGElement[]
        //            {
        //                Word.Noun("lies"),
        //                new NPPhraseSpec
        //                {
        //                    Number = numberAgreement.PLURAL,
        //                    Specifier = Word.Determiner("the"),
        //                    head = Word.Noun("liar"),
        //                    PreModifiers = new NLGElement[]
        //                    {
        //                        new VPPhraseSpec
        //                        {
        //                            Head = Word.Verb("lie"),
        //                            Form = form.PRESENT_PARTICIPLE
        //                        }
        //                    },
        //                    Complements = new NLGElement[]
        //                    {
        //                        new SPhraseSpec
        //                        {
        //                            ClauseStatus = clauseStatus.SUBORDINATE,
        //                            Complementiser = "who",
        //                            Predicate = new VPPhraseSpec
        //                            {
        //                                Head = new WordElement
        //                                {
        //                                    Inflection = inflection.UNCOUNT,
        //                                    Base = "tell",
        //                                    canned = true,
        //                                    cannedSpecified = true
        //                                }
        //                            },
        //                            Complements = new NLGElement[]
        //                            {
        //                                Word.Pronoun("them")
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }));
        //}
    }
}
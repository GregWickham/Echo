using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimpleNLG.Tests.Clauses.Independent.PredicateFeatures.ComplementTypes
{
    [TestClass]
    public class PrepositionalPhrase
    {
        [TestClass]
        public class Multiple
        {
            [TestMethod]
            public void WeShallFightWithGrowingConfidenceAndGrowingStrengthInTheAir() => Assert.AreEqual("We shall fight with growing confidence and growing strength in the air.", Client.Realize(new SPhraseSpec
            {
                subj = new NLGElement[]
                {
                    new NPPhraseSpec
                    {
                        Head = Word.Pronoun("I"),
                        Number = numberAgreement.PLURAL
                    }
                },
                vp = new VPPhraseSpec
                {
                    Modal = "shall",
                    Head = Word.Verb("fight"),
                    compl = new NLGElement[]
                    {
                        new PPPhraseSpec
                        {
                            Head = Word.Preposition("with"),
                            Complements = new NLGElement[]
                            {
                                new CoordinatedPhraseElement
                                {
                                    Category = phraseCategory.PREPOSITIONAL_PHRASE,
                                    Conjunction = "and",
                                    Coordinated = new NLGElement[]
                                    {
                                        new NPPhraseSpec
                                        {
                                            Head = Word.Noun("confidence"),
                                            PreModifiers = new NLGElement[]
                                            {
                                                new VPPhraseSpec
                                                {
                                                    Form = form.PRESENT_PARTICIPLE,
                                                    Head = Word.Verb("grow")
                                                }
                                            }
                                        },
                                        new NPPhraseSpec
                                        {
                                            Head = Word.Noun("strength"),
                                            PreModifiers = new NLGElement[]
                                            {
                                                new VPPhraseSpec
                                                {
                                                    Form = form.PRESENT_PARTICIPLE,
                                                    Head = Word.Verb("grow")
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        },
                        new PPPhraseSpec
                        {
                            Head = Word.Preposition("in"),
                            Complements = new NLGElement[]
                            {
                                new NPPhraseSpec
                                {
                                    Specifier = Word.Determiner("the"),
                                    Head = Word.Noun("air")
                                }
                            }
                        }
                    },
                }
            }));
        }
    }

}


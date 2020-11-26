namespace SimpleNLG
{
    public static class Phrase
    {
        public static NPPhraseSpec Noun(string headWord) => new NPPhraseSpec { head = Word.Noun(headWord) };
        public static NPPhraseSpec Noun(WordElement headWord) => new NPPhraseSpec { head = headWord };


        public static VPPhraseSpec Verb(string headWord) => new VPPhraseSpec { head = Word.Verb(headWord) };
        public static VPPhraseSpec Verb(WordElement headWord) => new VPPhraseSpec { head = headWord };


        public static AdjPhraseSpec Adjective(string headWord) => new AdjPhraseSpec { head = Word.Adjective(headWord) };
        public static AdjPhraseSpec Adjective(WordElement headWord) => new AdjPhraseSpec { head = headWord };


        public static AdvPhraseSpec Adverb(string headWord) => new AdvPhraseSpec { head = Word.Adverb(headWord) };
        public static AdvPhraseSpec Adverb(WordElement headWord) => new AdvPhraseSpec { head = headWord };


        public static PPPhraseSpec Prepositional(string headWord) => new PPPhraseSpec { head = Word.Preposition(headWord) };
        public static PPPhraseSpec Prepositional(WordElement headWord) => new PPPhraseSpec { head = headWord };


        public static NPPhraseSpec Pronoun(string headWord) => new NPPhraseSpec { head = Word.Pronoun(headWord) };
        public static NPPhraseSpec Pronoun(WordElement headWord) => new NPPhraseSpec { head = headWord };
    }
}

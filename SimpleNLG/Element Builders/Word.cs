namespace SimpleNLG
{
    public static class Word
    {
        public static WordElement Noun(string noun) => new WordElement
        {
            @base = noun,
            PartOfSpeech = lexicalCategory.NOUN
        };

        public static WordElement Verb(string verb) => new WordElement
        {
            @base = verb,
            PartOfSpeech = lexicalCategory.VERB
        };

        public static WordElement Adjective(string adjective) => new WordElement
        {
            @base = adjective,
            PartOfSpeech = lexicalCategory.ADJECTIVE
        };

        public static WordElement Adverb(string adverb) => new WordElement
        {
            @base = adverb,
            PartOfSpeech = lexicalCategory.ADVERB
        };

        public static WordElement Determiner(string determiner) => new WordElement
        {
            @base = determiner,
            PartOfSpeech = lexicalCategory.DETERMINER
        };

        public static WordElement Pronoun(string pronoun) => new WordElement
        {
            @base = pronoun,
            PartOfSpeech = lexicalCategory.PRONOUN
        };

        public static WordElement Conjunction(string conjunction) => new WordElement
        {
            @base = conjunction,
            PartOfSpeech = lexicalCategory.CONJUNCTION
        };

        public static WordElement Preposition(string preposition) => new WordElement
        {
            @base = preposition,
            PartOfSpeech = lexicalCategory.PREPOSITION
        };

        public static WordElement Complementizer(string complementizer) => new WordElement
        {
            @base = complementizer,
            PartOfSpeech = lexicalCategory.COMPLEMENTISER
        };

        public static WordElement Modal(string modal) => new WordElement
        {
            @base = modal,
            PartOfSpeech = lexicalCategory.MODAL
        };

        public static WordElement Auxiliary(string auxiliary) => new WordElement
        {
            @base = auxiliary,
            PartOfSpeech = lexicalCategory.AUXILIARY
        };
    }
}

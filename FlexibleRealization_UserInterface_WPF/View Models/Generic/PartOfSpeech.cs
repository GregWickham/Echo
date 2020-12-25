using System;
using System.Collections.Generic;
using System.Linq;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>Provides utility methods for UI-friendly rendering of <see cref="PartOfSpeechBuilder"/> features</summary>
    internal static class PartOfSpeech
    {

        //internal static string DescriptionFor(PartOfSpeechBuilder element) => TreebankTag.Strings[element.Token.PartOfSpeech];

        internal static class TreebankTag
        {
            //internal static string LabelFor(PartOfSpeechBuilder element) => element switch
            //{
            //    WhAdverbBuilder => "Wh-Adv",
            //    WhDeterminerBuilder => "Wh-Det",
            //    WhPronounBuilder => "Wh-Prn",

            //    NounBuilder => "N",
            //    VerbBuilder => "V",
            //    AdjectiveBuilder => "Adj",
            //    AdverbBuilder => "Adv",
            //    ConjunctionBuilder => "Conj",
            //    DeterminerBuilder => "Det",
            //    ModalBuilder => "Md",
            //    ParticleBuilder => "Prt",
            //    PrepositionBuilder => "Prp",
            //    PronounBuilder => "Prn",

            //    _ => throw new InvalidOperationException("Can't find a label for this part of speech")
            //};

            //internal static string DescriptionFor(PartOfSpeechBuilder element) => element switch
            //{
            //    WhAdverbBuilder => "Wh-Adverb",
            //    WhDeterminerBuilder => "Wh-Determiner",
            //    WhPronounBuilder => "Wh-Pronoun",

            //    NounBuilder => "Noun",
            //    VerbBuilder => "Verb",
            //    AdjectiveBuilder => "Adjective",
            //    AdverbBuilder => "Adverb",
            //    ConjunctionBuilder => "Conjunction",
            //    DeterminerBuilder => "Determiner",
            //    ModalBuilder => "Modal",
            //    ParticleBuilder => "Particle",
            //    PrepositionBuilder => "Preposition",
            //    PronounBuilder => "Pronoun",

            //    _ => throw new InvalidOperationException("Can't find a description for this part of speech")
            //};

            ///// <summary>Return a new WordElementBuilder of the type specified by <paramref name="partOfSpeechDescription"/></summary>
            //internal static WordElementBuilder FromDescription(string partOfSpeechDescription) => partOfSpeechDescription switch
            //{
            //    "Wh-Adverb" => new WhAdverbBuilder(),
            //    "Wh-Determiner" => new WhDeterminerBuilder(),
            //    "Wh-Pronoun" => new WhPronounBuilder(),

            //    "Noun" => new NounBuilder(),
            //    "Verb" => new VerbBuilder(),
            //    "Adjective" => new AdjectiveBuilder(),
            //    "Adverb" => new AdverbBuilder(),
            //    "Conjunction" => new ConjunctionBuilder(),
            //    "Determiner" => new DeterminerBuilder(),
            //    "Modal" => new ModalBuilder(),
            //    "Particle" => new ParticleBuilder(),
            //    "Preposition" => new PrepositionBuilder(),
            //    "Pronoun" => new PronounBuilder(),

            //    _ => throw new InvalidOperationException("Can't make a WordElementBuilder for this description type")
            //};

            //internal static string FromDescription(string description) => Strings.Single(kvp => kvp.Value.Equals(description)).Key;

            //internal static Dictionary<string, string> Strings { get; } = new Dictionary<string, string>
            //{
            //    { "CC", "Any" },
            //    { "CD", "Cardinal number" },
            //    { "DT", "Determiner" },
            //    { "EX", "Existential there" },
            //    { "FW", "Foreign word" },
            //    { "IN", "Preposition or subordinating conjunction" },
            //    { "JJ", "Adjective" },
            //    { "JJR", "Adjective, comparative" },
            //    { "JJS", "Adjective, superlative" },
            //    { "LS", "List item marker" },
            //    { "MD", "Modal" },
            //    { "NN", "Noun, singular or mass" },
            //    { "NNS", "Noun, plural" },
            //    { "NNP", "Proper noun, singular" },
            //    { "NNPS", "Proper noun, plural" },
            //    { "POS", "Possessive ending" },
            //    { "PRP", "Personal pronoun" },
            //    { "PRP$", "Possessive pronoun" },
            //    { "RB", "Adverb" },
            //    { "RBR", "Adverb, comparative" },
            //    { "RBS", "Adverb, superlative" },
            //    { "RP", "Particle" },
            //    { "SYM", "Symbol" },
            //    { "TO", "to" },
            //    { "UH", "Interjection" },
            //    { "VB", "Verb, base form" },
            //    { "VBD", "Verb, past tense" },
            //    { "VBG", "Verb, gerund or present participle" },
            //    { "VBN", "Verb, past participle" },
            //    { "VBP", "Verb, non-3rd person singular present" },
            //    { "VBZ", "Verb, 3rd person singular present" },
            //    { "WDT", "Wh- determiner" },
            //    { "WP", "Wh- pronoun" },
            //    { "WP$", "Possessive wh- pronoun" },
            //    { "WRB", "Wh- adverb" },

            //    //Punctuation
            //    { "#", "Pound sign" },
            //    { "$", "Dollar sign" },
            //    { ".", "Period" },
            //    { ",", "Comma" },
            //    { ":", "Colon or semicolon" },
            //    { "(", "Open parenthesis" },
            //    { ")", "Close parenthesis" },
            //    { "\"", "Backslash" },
            //    { "‘", "Open single quote" },
            //    { "“", "Open double quote" },
            //    { "’", "Close single quote" },
            //    { "”", "Close double quote" },            
            //};
        }
    }
}

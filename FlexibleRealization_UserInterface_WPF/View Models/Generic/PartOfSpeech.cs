using System.Collections.Generic;
using System.Linq;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>Provides utility methods for UI-friendly rendering of <see cref="PartOfSpeechBuilder"/> features</summary>
    internal static class PartOfSpeech
    {
        internal static string DescriptionFor(PartOfSpeechBuilder element) => Tag.Strings[element.Token.PartOfSpeech];

        internal static class Tag
        {
            internal static string FromDescription(string description) => Strings.Single(kvp => kvp.Value.Equals(description)).Key;

            internal static Dictionary<string, string> Strings { get; } = new Dictionary<string, string>
            {
                { "CC", "Any" },
                { "CD", "Cardinal number" },
                { "DT", "Determiner" },
                { "EX", "Existential there" },
                { "FW", "Foreign word" },
                { "IN", "Preposition or subordinating conjunction" },
                { "JJ", "Adjective" },
                { "JJR", "Adjective, comparative" },
                { "JJS", "Adjective, superlative" },
                { "LS", "List item marker" },
                { "MD", "Modal" },
                { "NN", "Noun, singular or mass" },
                { "NNS", "Noun, plural" },
                { "NNP", "Proper noun, singular" },
                { "NNPS", "Proper noun, plural" },
                { "POS", "Possessive ending" },
                { "PRP", "Personal pronoun" },
                { "PRP$", "Possessive pronoun" },
                { "RB", "Adverb" },
                { "RBR", "Adverb, comparative" },
                { "RBS", "Adverb, superlative" },
                { "RP", "Particle" },
                { "SYM", "Symbol" },
                { "TO", "to" },
                { "UH", "Interjection" },
                { "VB", "Verb, base form" },
                { "VBD", "Verb, past tense" },
                { "VBG", "Verb, gerund or present participle" },
                { "VBN", "Verb, past participle" },
                { "VBP", "Verb, non-3rd person singular present" },
                { "VBZ", "Verb, 3rd person singular present" },
                { "WDT", "Wh- determiner" },
                { "WP", "Wh- pronoun" },
                { "WP$", "Possessive wh- pronoun" },
                { "WRB", "Wh- adverb" },

                //Punctuation
                { "#", "Pound sign" },
                { "$", "Dollar sign" },
                { ".", "Period" },
                { ",", "Comma" },
                { ":", "Colon or semicolon" },
                { "(", "Open parenthesis" },
                { ")", "Close parenthesis" },
                { "\"", "Backslash" },
                { "‘", "Open single quote" },
                { "“", "Open double quote" },
                { "’", "Close single quote" },
                { "”", "Close double quote" },            
            };
        }
    }
}

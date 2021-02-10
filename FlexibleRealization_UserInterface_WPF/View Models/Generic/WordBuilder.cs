using System;
using System.Collections.Generic;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>Provides utility methods for UI-friendly rendering of WordElementBuilder features</summary>
    internal static class WordBuilder
    {
        internal static readonly IEnumerable<string> PartOfSpeechDescriptions = new List<string>
        {
            "Wh-Adverb",
            "Wh-Determiner",
            "Wh-Pronoun",
            "Noun",
            "Verb",
            "Adjective",
            "Adverb",
            "Conjunction",
            "Determiner",
            "Modal",
            "Particle",
            "Preposition",
            "Pronoun"
        };

        internal static string LabelFor(WordElementBuilder element) => element switch
        {
            WhAdverbBuilder => "Wh-Adv",
            WhDeterminerBuilder => "Wh-Det",
            WhPronounBuilder => "Wh-Prn",

            NounBuilder => "N",
            VerbBuilder => "V",
            AdjectiveBuilder => "Adj",
            AdverbBuilder => "Adv",
            ConjunctionBuilder => "Conj",
            DeterminerBuilder => "Det",
            ModalBuilder => "Md",
            ParticleBuilder => "Prt",
            PrepositionBuilder => "Prp",
            PronounBuilder => "Prn",

            CardinalNumberBuilder => "Num",

            _ => throw new InvalidOperationException("Can't find a label for this part of speech")
        };

        internal static string DescriptionFor(WordElementBuilder element) => element switch
        {
            WhAdverbBuilder => "Wh-Adverb",
            WhDeterminerBuilder => "Wh-Determiner",
            WhPronounBuilder => "Wh-Pronoun",

            NounBuilder => "Noun",
            VerbBuilder => "Verb",
            AdjectiveBuilder => "Adjective",
            AdverbBuilder => "Adverb",
            ConjunctionBuilder => "Conjunction",
            DeterminerBuilder => "Determiner",
            ModalBuilder => "Modal",
            ParticleBuilder => "Particle",
            PrepositionBuilder => "Preposition",
            PronounBuilder => "Pronoun",

            CardinalNumberBuilder => "Number",

            _ => throw new InvalidOperationException("Can't find a description for this part of speech")
        };

        /// <summary>Return a new WordElementBuilder of the type specified by <paramref name="wordDescription"/></summary>
        internal static WordElementBuilder FromDescription(string wordDescription) => wordDescription switch
        {
            "Wh-Adverb" => new WhAdverbBuilder(),
            "Wh-Determiner" => new WhDeterminerBuilder(),
            "Wh-Pronoun" => new WhPronounBuilder(),

            "Noun" => new NounBuilder(),
            "Verb" => new VerbBuilder(),
            "Adjective" => new AdjectiveBuilder(),
            "Adverb" => new AdverbBuilder(),
            "Conjunction" => new ConjunctionBuilder(),
            "Determiner" => new DeterminerBuilder(),
            "Modal" => new ModalBuilder(),
            "Particle" => new ParticleBuilder(),
            "Preposition" => new PrepositionBuilder(),
            "Pronoun" => new PronounBuilder(),

            "Number" => new CardinalNumberBuilder(),

            _ => throw new InvalidOperationException("Can't make a WordElementBuilder for this description type")
        };
    }
}

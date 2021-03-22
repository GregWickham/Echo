using System;
using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>Provides utility methods for UI-friendly rendering of <see cref="NLGElement"/> features</summary>
    internal static class NLGElementFeature
    {
        internal static class LexicalCategory
        {
            internal static Dictionary<lexicalCategory, string> Strings { get; } = new Dictionary<lexicalCategory, string>
            {
                { lexicalCategory.ANY, "Any" },
                { lexicalCategory.SYMBOL, "Symbol" },
                { lexicalCategory.NOUN, "Noun" },
                { lexicalCategory.ADJECTIVE, "Adjective" },
                { lexicalCategory.ADVERB, "Adverb" },
                { lexicalCategory.VERB, "Verb" },
                { lexicalCategory.DETERMINER, "Determiner" },
                { lexicalCategory.PRONOUN, "Pronoun" },
                { lexicalCategory.CONJUNCTION, "Conjunction" },
                { lexicalCategory.PREPOSITION, "Preposition" },
                { lexicalCategory.COMPLEMENTISER, "Complementiser" },
                { lexicalCategory.MODAL, "Model" },
                { lexicalCategory.AUXILIARY, "Auxiliary" }
            };
        }

        internal static class Inflection
        {
            internal static Dictionary<inflection, string> Strings { get; } = new Dictionary<inflection, string>
            {
                { inflection.GRECO_LATIN_REGULAR, "Greco-Latin Regular" },
                { inflection.IRREGULAR, "Irregular" },
                { inflection.REGULAR, "Regular" },
                { inflection.REGULAR_DOUBLE, "Regular Double" },
                { inflection.UNCOUNT, "Uncount" },
                { inflection.INVARIANT, "Invariant" }
            };
        }

        internal static class PhraseCategory
        {
            internal static Dictionary<phraseCategory, string> Strings { get; } = new Dictionary<phraseCategory, string>
            {
                { phraseCategory.CLAUSE, "Clause" },
                { phraseCategory.ADJECTIVE_PHRASE, "Adjective Phrase" },
                { phraseCategory.ADVERB_PHRASE, "Adverb Phrase" },
                { phraseCategory.NOUN_PHRASE, "Noun Phrase" },
                { phraseCategory.PREPOSITIONAL_PHRASE, "Prepositional Phrase" },
                { phraseCategory.VERB_PHRASE, "Verb Phrase" },
                { phraseCategory.CANNED_TEXT, "Canned Text" }
            };
        }

        internal static class DiscourseFunction
        {
            internal static Dictionary<discourseFunction, string> Strings { get; } = new Dictionary<discourseFunction, string>
            {
                { discourseFunction.AUXILIARY, "Auxiliary" },
                { discourseFunction.COMPLEMENT, "Complement" },
                { discourseFunction.CONJUNCTION, "Conjunction" },
                { discourseFunction.CUE_PHRASE, "Cue Phrase" },
                { discourseFunction.FRONT_MODIFIER, "Front Modifier" },
                { discourseFunction.HEAD, "Head" },
                { discourseFunction.INDIRECT_OBJECT, "Indirect Object" },
                { discourseFunction.OBJECT, "Object" },
                { discourseFunction.PRE_MODIFIER, "Pre-Modifier" },
                { discourseFunction.POST_MODIFIER, "Post-Modifier" },
                { discourseFunction.SPECIFIER, "Specifier" },
                { discourseFunction.SUBJECT, "Subject" },
                { discourseFunction.VERB_PHRASE, "Verb Phrase" },
            };
        }

        internal static class Number
        {
            internal static Dictionary<numberAgreement, string> Strings { get; } = new Dictionary<numberAgreement, string>
            {
                { numberAgreement.BOTH, "Both" },
                { numberAgreement.PLURAL, "Plural" },
                { numberAgreement.SINGULAR, "Singular" }
            };
        }

        internal static class Gender
        {
            internal static Dictionary<gender, string> Strings { get; } = new Dictionary<gender, string>
            {
                { gender.MASCULINE, "Masculine" },
                { gender.FEMININE, "Feminine" },
                { gender.NEUTER, "Neuter" }
            };
        }

        internal static class Person
        {
            internal static Dictionary<person, string> Strings { get; } = new Dictionary<person, string>
            {
                { person.FIRST, "First" },
                { person.SECOND, "Second" },
                { person.THIRD, "Third" }
            };
        }

        internal static class Form
        {
            internal static Dictionary<form, string> Strings { get; } = new Dictionary<form, string>
            {
                { form.BARE_INFINITIVE, "Bare Infinitive" },
                { form.GERUND, "Gerund" },
                { form.IMPERATIVE, "Imperative" },
                { form.INFINITIVE, "Infinitive" },
                { form.NORMAL, "Normal" },
                { form.PAST_PARTICIPLE, "Past Participle" },
                { form.PRESENT_PARTICIPLE, "Present Participle" }
            };
        }

        internal static class Tense
        {
            internal static Dictionary<tense, string> Strings { get; } = new Dictionary<tense, string>
            {
                { tense.FUTURE, "Future" },
                { tense.PAST, "Past" },
                { tense.PRESENT, "Present" }
            };
        }

        internal static class ClauseStatus
        {
            internal static Dictionary<clauseStatus, string> Strings { get; } = new Dictionary<clauseStatus, string>
            {
                { clauseStatus.MATRIX, "Matrix" },
                { clauseStatus.SUBORDINATE, "Subordinate" }
            };
        }

        internal static class InterrogativeType
        {
            internal static Dictionary<interrogativeType, string> Strings { get; } = new Dictionary<interrogativeType, string>
            {
                { interrogativeType.HOW, "How" },
                { interrogativeType.WHAT_OBJECT, "What - Object" },
                { interrogativeType.WHAT_SUBJECT, "What - Subject" },
                { interrogativeType.WHERE, "Where" },
                { interrogativeType.WHO_INDIRECT_OBJECT, "Who - Indirect Object" },
                { interrogativeType.WHO_OBJECT, "Who - Object" },
                { interrogativeType.WHO_SUBJECT, "Who - Subject" },
                { interrogativeType.WHY, "Why" },
                { interrogativeType.YES_NO, "Yes / No" }
            };
        }

        internal static class DocumentCategory
        {
            internal static Dictionary<documentCategory, string> Strings { get; } = new Dictionary<documentCategory, string>
            {
                { documentCategory.DOCUMENT, "Document" },
                { documentCategory.SECTION, "Section" },
                { documentCategory.PARAGRAPH, "Paragraph" },
                { documentCategory.SENTENCE, "Sentence" },
                { documentCategory.LIST, "List" },
                { documentCategory.LIST_ITEM, "List Item" }
            };
        }

    }
}

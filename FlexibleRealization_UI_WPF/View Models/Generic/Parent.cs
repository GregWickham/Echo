using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>Provides utility methods for UI-friendly rendering of <see cref="ParentElementBuilder"/> features</summary>
    internal static class Parent
    {
        /// <summary>Return a short string describing the <see cref="ParentElementBuilder"/> <paramref name="element"/></summary>
        internal static string LabelFor(ParentElementBuilder element) => element switch
        {
            FragmentBuilder => "Frag",
            IndependentClauseBuilder => "S",
            SubordinateClauseBuilder => "SBAR",
            TemporalNounPhraseBuilder => "NP-TMP",
            NounPhraseBuilder => "NP",
            NominalModifierBuilder => "NML",
            VerbPhraseBuilder => "VP",
            AdjectivePhraseBuilder => "AdjP",
            AdverbPhraseBuilder => "AdvP",
            PrepositionalPhraseBuilder => "PP",
            CoordinatedPhraseBuilder cpb => cpb.PhraseCategory switch
            {
                phraseCategory.NOUN_PHRASE => "C(NP)",
                phraseCategory.VERB_PHRASE => "C(VP)",
                phraseCategory.ADJECTIVE_PHRASE => "C(AdjP)",
                phraseCategory.ADVERB_PHRASE => "C(AdvP)",
                phraseCategory.PREPOSITIONAL_PHRASE => "C(PP)",
                _ => throw new InvalidOperationException("Can't find a label for this coordinated phrase type")
            },
            CompoundNounBuilder => "CompN",
            UnknownParentBuilder => "X",

            _ => throw new InvalidOperationException("Can't find a label for this builder type")
        };

        /// <summary>Return a long string describing the <see cref="ParentElementBuilder"/> <paramref name="element"/></summary>
        internal static string DescriptionFor(ParentElementBuilder element) => element switch
        {
            FragmentBuilder => "Fragment",
            IndependentClauseBuilder => "Independent Clause",
            SubordinateClauseBuilder => "Subordinate Clause",
            TemporalNounPhraseBuilder => "Temporal Noun Phrase",
            NounPhraseBuilder => "Noun Phrase",
            NominalModifierBuilder => "Nominal Modifier",
            VerbPhraseBuilder => "Verb Phrase",
            AdjectivePhraseBuilder => "Adjective Phrase",
            AdverbPhraseBuilder => "Adverb Phrase",
            PrepositionalPhraseBuilder => "Prepositional Phrase",
            CoordinatedPhraseBuilder cpb => "Coordinated " + cpb.PhraseCategory switch
            {
                phraseCategory.NOUN_PHRASE => "Noun Phrase",
                phraseCategory.VERB_PHRASE => "Verb Phrase",
                phraseCategory.ADJECTIVE_PHRASE => "Adjective Phrase",
                phraseCategory.ADVERB_PHRASE => "Adverb Phrase",
                phraseCategory.PREPOSITIONAL_PHRASE => "Prepositional Phrase",
                _ => throw new InvalidOperationException("Can't find a description for this coordinated phrase type")
            },
            CompoundNounBuilder => "Compound Noun",
            UnknownParentBuilder => "Unknown",

            _ => throw new InvalidOperationException("Can't find a description for this builder type")
        };

        internal static IEnumerable<string> SpecifiedFeaturesFor(ParentElementBuilder element)
        {
            List<string> properties = new List<string>();
            switch (element)
            {
                case ClauseBuilder cb:
                    if (cb.DiscourseFunctionSpecified) properties.Add(NLGElementFeature.DiscourseFunction.Strings[cb.DiscourseFunction]);
                    if (cb.PersonSpecified) properties.Add($"{NLGElementFeature.Person.Strings[cb.Person]} Person");
                    if (cb.TenseSpecified) properties.Add($"{NLGElementFeature.Tense.Strings[cb.Tense]} Tense");
                    if (cb.FormSpecified) properties.Add(NLGElementFeature.Form.Strings[cb.Form]);
                    if (cb.InterrogativeTypeSpecified) properties.Add($"Interrogative: {NLGElementFeature.InterrogativeType.Strings[cb.InterrogativeType]}");
                    if (cb.AggregateAuxiliarySpecified && cb.AggregateAuxiliary) properties.Add("Aggregate Auxiliary");
                    if (cb.AppositiveSpecified && cb.Appositive) properties.Add("Appositive");
                    if (cb.ComplementiserSpecified) properties.Add($"Complementiser: {cb.Complementiser}");
                    if (cb.NegatedSpecified && cb.Negated) properties.Add("Negated");
                    if (cb.PassiveSpecified) properties.Add(cb.Passive ? "Passive" : "Active");
                    if (cb.PerfectSpecified && cb.Perfect) properties.Add("Perfect");
                    if (cb.ProgressiveSpecified && cb.Progressive) properties.Add("Progressive");
                    if (cb.SuppressGenitiveInGerundSpecified && cb.SuppressGenitiveInGerund) properties.Add("Suppress Genitive in Gerund");
                    if (cb.SuppressedComplementiserSpecified && cb.SuppressedComplementiser) properties.Add("Suppressed Complementiser");
                    break;
                case CoordinatedPhraseBuilder cpb:
                    if (cpb.PersonSpecified) properties.Add($"{NLGElementFeature.Person.Strings[cpb.Person]} Person");
                    if (cpb.TenseSpecified) properties.Add($"{NLGElementFeature.Tense.Strings[cpb.Tense]} Tense");
                    if (cpb.NumberSpecified) properties.Add(NLGElementFeature.Number.Strings[cpb.Number]);
                    if (cpb.AppositiveSpecified && cpb.Appositive) properties.Add("Appositive");
                    if (cpb.ConjunctionTypeSpecified) properties.Add($"Conjunction Type: {cpb.ConjunctionType}");
                    if (cpb.PossessiveSpecified && cpb.Possessive) properties.Add("Possessive");
                    if (cpb.ProgressiveSpecified && cpb.Progressive) properties.Add("Progressive");
                    if (cpb.RaiseSpecifierSpecified && cpb.RaiseSpecifier) properties.Add("Raise Specifier");
                    if (cpb.SuppressedComplementiserSpecified && cpb.SuppressedComplementiser) properties.Add("Suppressed Complementiser");
                    break;
                case NounPhraseBuilder npb:
                    if (npb.DiscourseFunctionSpecified) properties.Add(NLGElementFeature.DiscourseFunction.Strings[npb.DiscourseFunction]);
                    if (npb.PersonSpecified) properties.Add($"{NLGElementFeature.Person.Strings[npb.Person]} Person");
                    if (npb.GenderSpecified) properties.Add(NLGElementFeature.Gender.Strings[npb.Gender]);
                    if (npb.NumberSpecified) properties.Add(NLGElementFeature.Number.Strings[npb.Number]);
                    if (npb.AdjectiveOrderingSpecified && npb.AdjectiveOrdering) properties.Add("Adjective Ordering");
                    if (npb.AppositiveSpecified && npb.Appositive) properties.Add("Appositive");
                    if (npb.ElidedSpecified && npb.Elided) properties.Add("Elided");
                    if (npb.PossessiveSpecified && npb.Possessive) properties.Add("Possessive");
                    if (npb.PronominalSpecified && npb.Pronominal) properties.Add("Pronominal");
                    break;
                case VerbPhraseBuilder vpb:
                    if (vpb.DiscourseFunctionSpecified) properties.Add(NLGElementFeature.DiscourseFunction.Strings[vpb.DiscourseFunction]);
                    if (vpb.PersonSpecified) properties.Add($"{NLGElementFeature.Person.Strings[vpb.Person]} Person");
                    if (vpb.TenseSpecified) properties.Add($"{NLGElementFeature.Tense.Strings[vpb.Tense]} Tense");
                    if (vpb.FormSpecified) properties.Add(NLGElementFeature.Form.Strings[vpb.Form]);
                    if (vpb.AggregateAuxiliarySpecified && vpb.AggregateAuxiliary) properties.Add("Aggregate Auxiliary");
                    if (vpb.AppositiveSpecified && vpb.Appositive) properties.Add("Appositive");
                    if (vpb.NegatedSpecified && vpb.Negated) properties.Add("Negated");
                    if (vpb.PassiveSpecified) properties.Add(vpb.Passive ? "Passive" : "Active");
                    if (vpb.PerfectSpecified && vpb.Perfect) properties.Add("Perfect");
                    if (vpb.ProgressiveSpecified && vpb.Progressive) properties.Add("Progressive");
                    if (vpb.SuppressGenitiveInGerundSpecified && vpb.SuppressGenitiveInGerund) properties.Add("Suppress Genitive in Gerund");
                    if (vpb.SuppressedComplementiserSpecified && vpb.SuppressedComplementiser) properties.Add("Suppressed Complementiser");
                    break;
                case AdjectivePhraseBuilder apb:
                    if (apb.DiscourseFunctionSpecified) properties.Add(NLGElementFeature.DiscourseFunction.Strings[apb.DiscourseFunction]);
                    if (apb.AppositiveSpecified && apb.Appositive) properties.Add("Appositive");
                    if (apb.ComparativeSpecified && apb.Comparative) properties.Add("Comparative");
                    if (apb.SuperlativeSpecified && apb.Superlative) properties.Add("Superlative");
                    break;
                case AdverbPhraseBuilder apb:
                    if (apb.DiscourseFunctionSpecified) properties.Add(NLGElementFeature.DiscourseFunction.Strings[apb.DiscourseFunction]);
                    if (apb.AppositiveSpecified && apb.Appositive) properties.Add("Appositive");
                    if (apb.ComparativeSpecified && apb.Comparative) properties.Add("Comparative");
                    if (apb.SuperlativeSpecified && apb.Superlative) properties.Add("Superlative");
                    break;
                case PrepositionalPhraseBuilder ppb:
                    if (ppb.DiscourseFunctionSpecified) properties.Add(NLGElementFeature.DiscourseFunction.Strings[ppb.DiscourseFunction]);
                    if (ppb.AppositiveSpecified && ppb.Appositive) properties.Add("Appositive");
                    break;
            }
            return properties;
        }

        internal static class ChildRole
        {
            internal static string DescriptionFrom(ParentElementBuilder.ChildRole role) => Strings[role];

            internal static IEnumerable<string> StringFormsOf(IEnumerable<ParentElementBuilder.ChildRole> roles) => roles.Select(role => Strings[role]);

            internal static ParentElementBuilder.ChildRole FromDescription(string description) => Strings.Single(kvp => kvp.Value.Equals(description)).Key;

            /// <summary>Strings describing the <see cref="ParentElementBuilder.ChildRole"/> <paramref name="role"/>s</summary>
            internal static Dictionary<ParentElementBuilder.ChildRole, string> Strings { get; } = new Dictionary<ParentElementBuilder.ChildRole, string>
            {
                { ParentElementBuilder.ChildRole.NoParent, "(No Parent)" },
                { ParentElementBuilder.ChildRole.Unassigned, "Unassigned" },
                { ParentElementBuilder.ChildRole.Subject, "Subject" },
                { ParentElementBuilder.ChildRole.Predicate, "Predicate" },
                { ParentElementBuilder.ChildRole.Head, "Head" },
                { ParentElementBuilder.ChildRole.Modifier, "Modifier" },
                { ParentElementBuilder.ChildRole.Complement, "Complement" },
                { ParentElementBuilder.ChildRole.Specifier, "Specifier" },
                { ParentElementBuilder.ChildRole.Modal, "Modal" },
                { ParentElementBuilder.ChildRole.Coordinator, "Coordinator" },
                { ParentElementBuilder.ChildRole.Coordinated, "Coordinated Element" },
                { ParentElementBuilder.ChildRole.Complementizer, "Complementizer" },
                { ParentElementBuilder.ChildRole.Component, "Component Word" },
            };

            /// <summary>Labels describing the <see cref="ParentElementBuilder.ChildRole"/> <paramref name="role"/>s</summary>
            internal static Dictionary<ParentElementBuilder.ChildRole, string> Labels { get; } = new Dictionary<ParentElementBuilder.ChildRole, string>
            {
                { ParentElementBuilder.ChildRole.NoParent, "(No Parent)" },
                { ParentElementBuilder.ChildRole.Unassigned, "?" },
                { ParentElementBuilder.ChildRole.Subject, "S" },
                { ParentElementBuilder.ChildRole.Predicate, "P" },
                { ParentElementBuilder.ChildRole.Head, "H" },
                { ParentElementBuilder.ChildRole.Modifier, "M" },
                { ParentElementBuilder.ChildRole.Complement, "C" },
                { ParentElementBuilder.ChildRole.Specifier, "S" },
                { ParentElementBuilder.ChildRole.Modal, "MD" },
                { ParentElementBuilder.ChildRole.Coordinator, "CD" },
                { ParentElementBuilder.ChildRole.Coordinated, "CE" },
                { ParentElementBuilder.ChildRole.Complementizer, "CM" },
                { ParentElementBuilder.ChildRole.Component, "W" },
            };
        }
    }
}

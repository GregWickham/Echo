using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FlexibleRealization.UserInterface.ViewModels
{
    public abstract class ParentProperties : ElementProperties
    {
        internal static ParentProperties For(ParentElementBuilder builder) => builder switch
        {
            IndependentClauseBuilder icb => new IndependentClauseProperties(icb),
            SubordinateClauseBuilder scb => new SubordinateClauseProperties(scb),
            NounPhraseBuilder npb => new NounPhraseProperties(npb),
            VerbPhraseBuilder vpb => new VerbPhraseProperties(vpb),
            AdjectivePhraseBuilder apb => new AdjectivePhraseProperties(apb),
            AdverbPhraseBuilder apb => new AdverbPhraseProperties(apb),
            PrepositionalPhraseBuilder ppb => new PrepositionalPhraseProperties(ppb),
            NominalModifierBuilder nmb => new NominalModifierProperties(nmb),
            CompoundBuilder cwb => new CompoundWordProperties(cwb),
            UnknownParentBuilder upb => new UnknownParentProperties(upb),

            _ => throw new ArgumentException("No properties class defined for this ParentElementBuilder type")
        };

        private protected ParentProperties(ParentElementBuilder peb) { Model = peb; }

        private ParentElementBuilder Model;

        [Browsable(false)]
        public override string Description => Parent.DescriptionFor(Model);

        [Browsable(false)]
        public IEnumerable<string> LexicalCategoryValues => NLGElementFeature.LexicalCategory.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> InflectionValues => NLGElementFeature.Inflection.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> PhraseCategoryValues => NLGElementFeature.PhraseCategory.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> DiscourseFunctionValues => NLGElementFeature.DiscourseFunction.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> NumberAgreementValues => NLGElementFeature.Number.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> GenderValues => NLGElementFeature.Gender.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> PersonValues => NLGElementFeature.Person.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> FormValues => NLGElementFeature.Form.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> TenseValues => NLGElementFeature.Tense.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> ClauseStatusValues => NLGElementFeature.ClauseStatus.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> InterrogativeTypeValues => NLGElementFeature.InterrogativeType.Strings.Values;

        [Browsable(false)]
        public IEnumerable<string> DocumentCategoryValues => NLGElementFeature.DocumentCategory.Strings.Values;
    }
}
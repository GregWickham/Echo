using System.Collections.Generic;
using System.Linq;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="NounPhraseBuilder"/> in a PropertyGrid</summary>
    public class NounPhraseProperties : ParentProperties
    {
        internal NounPhraseProperties(NounPhraseBuilder npb) : base(npb) { Model = npb; }

        private NounPhraseBuilder Model;

        #region Syntax

        [Browsable(false)]
        public IEnumerable<string> RoleValues => Parent.ChildRole.StringFormsOf(Model.ValidRolesInCurrentParent);

        [Category("Syntax|")]
        [DisplayName("Role")]
        [ItemsSourceProperty("RoleValues")]
        public string Role
        {
            get => Parent.ChildRole.DescriptionFrom(Model.AssignedRole);
            set => Model.AssignedRole = Parent.ChildRole.FromDescription(value);
        }

        [Category("Syntax|")]
        [DisplayName("Start Index")]
        public int MinimumIndex => Model.MinimumIndex;

        [Category("Syntax|")]
        [DisplayName("End Index")]
        public int MaximumIndex => Model.MaximumIndex;

        #endregion Syntax

        #region Features

        [Category("Features|Adjective Ordering?")]
        [DisplayName("Specified")]
        public bool AdjectiveOrderingSpecified
        {
            get => Model.AdjectiveOrderingSpecified;
            set => Model.AdjectiveOrderingSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("AdjectiveOrderingSpecified")]
        public bool AdjectiveOrdering
        {
            get => Model.AdjectiveOrdering;
            set => Model.AdjectiveOrdering = value;
        }

        [Category("Features|Appositive?")]
        [DisplayName("Specified")]
        public bool AppositiveSpecified
        {
            get => Model.AppositiveSpecified;
            set => Model.AppositiveSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("AppositiveSpecified")]
        public bool Appositive
        {
            get => Model.Appositive;
            set => Model.Appositive = value;
        }

        [Category("Features|Discourse Function")]
        [DisplayName("Specified")]
        public bool DiscourseFunctionSpecified
        {
            get => Model.DiscourseFunctionSpecified;
            set => Model.DiscourseFunctionSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("DiscourseFunctionSpecified")]
        [ItemsSourceProperty("DiscourseFunctionValues")]
        public string DiscourseFunction
        {
            get => NLGElementFeature.DiscourseFunction.Strings[Model.DiscourseFunction];
            set => Model.DiscourseFunction = NLGElementFeature.DiscourseFunction.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Elided?")]
        [DisplayName("Specified")]
        public bool ElidedSpecified
        {
            get => Model.ElidedSpecified;
            set => Model.ElidedSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("ElidedSpecified")]
        public bool Elided
        {
            get => Model.Elided;
            set => Model.Elided = value;
        }

        [Category("Features|Number")]
        [DisplayName("Specified")]
        public bool NumberSpecified
        {
            get => Model.NumberSpecified;
            set => Model.NumberSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("NumberSpecified")]
        [ItemsSourceProperty("NumberAgreementValues")]
        public string Number
        {
            get => NLGElementFeature.Number.Strings[Model.Number];
            set => Model.Number = NLGElementFeature.Number.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Gender")]
        [DisplayName("Specified")]
        public bool GenderSpecified
        {
            get => Model.GenderSpecified;
            set => Model.GenderSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("GenderSpecified")]
        [ItemsSourceProperty("GenderValues")]
        public string Gender
        {
            get => NLGElementFeature.Gender.Strings[Model.Gender];
            set => Model.Gender = NLGElementFeature.Gender.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Person")]
        [DisplayName("Specified")]
        public bool PersonSpecified
        {
            get => Model.PersonSpecified;
            set => Model.PersonSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("PersonSpecified")]
        [ItemsSourceProperty("PersonValues")]
        public string Person
        {
            get => NLGElementFeature.Person.Strings[Model.Person];
            set => Model.Person = NLGElementFeature.Person.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Possessive?")]
        [DisplayName("Specified")]
        public bool PossessiveSpecified
        {
            get => Model.PossessiveSpecified;
            set => Model.PossessiveSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("PossessiveSpecified")]
        public bool Possessive
        {
            get => Model.Possessive;
            set => Model.Possessive = value;
        }

        [Category("Features|Pronominal?")]
        [DisplayName("Specified")]
        public bool PronominalSpecified
        {
            get => Model.PronominalSpecified;
            set => Model.PronominalSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("PronominalSpecified")]
        public bool Pronominal
        {
            get => Model.Pronominal;
            set => Model.Pronominal = value;
        }

        #endregion Features
    }
}
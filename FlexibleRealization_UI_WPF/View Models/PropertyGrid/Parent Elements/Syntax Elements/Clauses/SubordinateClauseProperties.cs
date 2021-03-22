using System.Collections.Generic;
using System.Linq;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="SubordinateClauseBuilder"/> in a PropertyGrid</summary>
    public class SubordinateClauseProperties : ParentProperties
    {
        internal SubordinateClauseProperties(SubordinateClauseBuilder scb) : base(scb) { Model = scb; }

        private SubordinateClauseBuilder Model;

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

        [Category("Features|Aggregate Auxiliary?")]
        [DisplayName("Specified")]
        public bool AggregateAuxiliarySpecified
        {
            get => Model.AggregateAuxiliarySpecified;
            set => Model.AggregateAuxiliarySpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("AggregateAuxiliarySpecified")]
        public bool AggregateAuxiliary
        {
            get => Model.AggregateAuxiliary;
            set => Model.AggregateAuxiliary = value;
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

        [Category("Features|Clause Status")]
        [DisplayName("Specified")]
        public bool ClauseStatusSpecified
        {
            get => Model.ClauseStatusSpecified;
            set => Model.ClauseStatusSpecified = value;
        }

        [Description("Value")]
        [VisibleBy("ClauseStatusSpecified")]
        [ItemsSourceProperty("ClauseStatusValues")]
        public string ClauseStatus
        {
            get => NLGElementFeature.ClauseStatus.Strings[Model.ClauseStatus];
            set => Model.ClauseStatus = NLGElementFeature.ClauseStatus.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Complementiser")]
        [DisplayName("Word")]
        public string Complementiser
        {
            get => Model.Complementiser;
            set => Model.Complementiser = value;
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

        [Category("Features|Predicate Form")]
        [DisplayName("Specified")]
        public bool FormSpecified
        {
            get => Model.FormSpecified;
            set => Model.FormSpecified = value;
        }

        [Description("Value")]
        [VisibleBy("FormSpecified")]
        [ItemsSourceProperty("FormValues")]
        public string Form
        {
            get => NLGElementFeature.Form.Strings[Model.Form];
            set => Model.Form = NLGElementFeature.Form.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Interrogative Type")]
        [DisplayName("Specified")]
        public bool InterrogativeTypeSpecified
        {
            get => Model.InterrogativeTypeSpecified;
            set => Model.InterrogativeTypeSpecified = value;
        }

        [Description("Value")]
        [VisibleBy("InterrogativeTypeSpecified")]
        [ItemsSourceProperty("InterrogativeTypeValues")]
        public string InterrogativeType
        {
            get => NLGElementFeature.InterrogativeType.Strings[Model.InterrogativeType];
            set => Model.InterrogativeType = NLGElementFeature.InterrogativeType.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        [Category("Features|Modal")]
        [DisplayName("Word")]
        public string Modal
        {
            get => Model.Modal;
            set => Model.Modal = value;
        }

        [Category("Features|Negated?")]
        [DisplayName("Specified")]
        public bool NegatedSpecified
        {
            get => Model.NegatedSpecified;
            set => Model.NegatedSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("NegatedSpecified")]
        public bool Negated
        {
            get => Model.Negated;
            set => Model.Negated = value;
        }

        [Category("Features|Passive?")]
        [DisplayName("Specified")]
        public bool PassiveSpecified
        {
            get => Model.PassiveSpecified;
            set => Model.PassiveSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("PassiveSpecified")]
        public bool Passive
        {
            get => Model.Passive;
            set => Model.Passive = value;
        }

        [Category("Features|Perfect?")]
        [DisplayName("Specified")]
        public bool PerfectSpecified
        {
            get => Model.PerfectSpecified;
            set => Model.PerfectSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("PerfectSpecified")]
        public bool Perfect
        {
            get => Model.Perfect;
            set => Model.Perfect = value;
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

        [Category("Features|Progressive?")]
        [DisplayName("Specified")]
        public bool ProgressiveSpecified
        {
            get => Model.ProgressiveSpecified;
            set => Model.ProgressiveSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("ProgressiveSpecified")]
        public bool Progressive
        {
            get => Model.Progressive;
            set => Model.Progressive = value;
        }

        [Category("Features|Suppress Genitive in Gerund?")]
        [DisplayName("Specified")]
        public bool SuppressGenitiveInGerundSpecified
        {
            get => Model.SuppressGenitiveInGerundSpecified;
            set => Model.SuppressGenitiveInGerundSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("SuppressGenitiveInGerund")]
        public bool SuppressGenitiveInGerund
        {
            get => Model.SuppressGenitiveInGerund;
            set => Model.SuppressGenitiveInGerund = value;
        }


        [Category("Features|Suppressed Complementiser?")]
        [DisplayName("Specified")]
        public bool SuppressedComplementiserSpecified
        {
            get => Model.SuppressedComplementiserSpecified;
            set => Model.SuppressedComplementiserSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("SuppressedComplementiserSpecified")]
        public bool SuppressedComplementiser
        {
            get => Model.SuppressedComplementiser;
            set => Model.SuppressedComplementiser = value;
        }

        [Category("Features|Tense")]
        [DisplayName("Specified")]
        public bool TenseSpecified
        {
            get => Model.TenseSpecified;
            set => Model.TenseSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("TenseSpecified")]
        [ItemsSourceProperty("TenseValues")]
        public string Tense
        {
            get => NLGElementFeature.Tense.Strings[Model.Tense];
            set => Model.Tense = NLGElementFeature.Tense.Strings.Single(kvp => kvp.Value.Equals(value)).Key;
        }

        #endregion Features
    }
}
using System.Collections.Generic;
using System.Linq;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting an <see cref="AdjectivePhraseBuilder"/> in a PropertyGrid</summary>
    public class AdjectivePhraseProperties : ParentProperties
    {
        internal AdjectivePhraseProperties(AdjectivePhraseBuilder apb) : base(apb) { Model = apb; }

        private AdjectivePhraseBuilder Model;

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

        [Category("Features|Comparative?")]
        [DisplayName("Specified")]
        public bool ComparativeSpecified
        {
            get => Model.ComparativeSpecified;
            set => Model.ComparativeSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("ComparativeSpecified")]
        public bool Comparative
        {
            get => Model.Comparative;
            set => Model.Comparative = value;
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

        [Category("Features|Superlative?")]
        [DisplayName("Specified")]
        public bool SuperlativeSpecified
        {
            get => Model.SuperlativeSpecified;
            set => Model.SuperlativeSpecified = value;
        }

        [DisplayName("Value")]
        [VisibleBy("SuperlativeSpecified")]
        public bool Superlative
        {
            get => Model.Superlative;
            set => Model.Superlative = value;
        }

        #endregion Features
    }
}
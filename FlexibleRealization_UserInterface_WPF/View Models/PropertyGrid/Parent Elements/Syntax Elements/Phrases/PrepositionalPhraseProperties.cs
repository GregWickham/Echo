using System.Linq;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="PrepositionalPhraseBuilder"/> in a PropertyGrid</summary>
    public class PrepositionalPhraseProperties : ParentProperties
    {
        internal PrepositionalPhraseProperties(PrepositionalPhraseBuilder ppb) : base(ppb) { Model = ppb; }

        private PrepositionalPhraseBuilder Model;

        #region Syntax

        [Category("Syntax|")]
        [DisplayName("Role")]
        public string Role => Parent.DescriptionFor(Model.AssignedRole);

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

        #endregion Features

        [Category("XML|")]
        [FillTab]
        [HeaderPlacement(HeaderPlacement.Collapsed)]
        public string Serialized => Model.XML;
    }
}
using System.Collections.Generic;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="PartOfSpeechBuilder"/> in a PropertyGrid</summary>
    public class PartOfSpeechProperties : ElementProperties
    {
        internal static PartOfSpeechProperties For(PartOfSpeechBuilder builder) => new PartOfSpeechProperties(builder);

        private protected PartOfSpeechProperties(PartOfSpeechBuilder posb) { Model = posb; }

        private PartOfSpeechBuilder Model;

        public override string Description => PartOfSpeech.DescriptionFor(Model);

        #region Syntax

        [Browsable(false)]
        public IEnumerable<string> RoleValues => Parent.ChildRole.StringFormsOf(Model.ValidRolesInCurrentParent);

        [Category("Syntax|")]
        [DisplayName("Role")]
        [ItemsSourceProperty("RoleValues")]
        public string Role
        {
            get => Parent.ChildRole.StringFormOf(Model.AssignedRole);
            set => Model.AssignedRole = Parent.ChildRole.FromString(value);
        }

        #endregion Syntax

        #region Features

        [Category("Features|")]
        [DisplayName("POS Tag")]
        public string Tag => Model.Token.PartOfSpeech;

        [Category("Features|")]
        [DisplayName("Index")]
        public int Index => Model.Token.Index;

        [Category("Features|")]
        [DisplayName("Word")]
        public string Word => Model.Token.Word;

        [Category("Features|")]
        [DisplayName("Lemma")]
        public string Lemma => Model.Token.Lemma;

        #endregion Features
    }
}
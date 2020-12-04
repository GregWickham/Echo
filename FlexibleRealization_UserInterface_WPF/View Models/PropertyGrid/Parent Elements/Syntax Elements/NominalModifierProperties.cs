using System.Collections.Generic;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="NominalModifierBuilder"/> in a PropertyGrid</summary>
    public class NominalModifierProperties : ParentProperties
    {
        internal NominalModifierProperties(NominalModifierBuilder nmb) : base(nmb) { Model = nmb; }

        private NominalModifierBuilder Model;

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

        #endregion Syntax
    }
}
using System.Collections.Generic;
using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="NominalModifierBuilder"/> in a PropertyGrid</summary>
    public class UnknownParentProperties : ParentProperties
    {
        internal UnknownParentProperties(UnknownParentBuilder upb) : base(upb) { Model = upb; }

        private UnknownParentBuilder Model;

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
    }
}
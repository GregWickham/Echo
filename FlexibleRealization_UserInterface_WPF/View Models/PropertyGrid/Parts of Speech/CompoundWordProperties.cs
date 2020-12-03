using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="CompoundBuilder"/> in a PropertyGrid</summary>
    public class CompoundWordProperties : ParentProperties
    {
        internal CompoundWordProperties(CompoundBuilder cwb) : base (cwb) { Model = cwb; }

        private CompoundBuilder Model;

        [Browsable(false)]
        public override string Description => Parent.DescriptionFor(Model);

        [Category("Syntax|")]
        [DisplayName("Role")]
        public string Role => Parent.ChildRole.Strings[Model.AssignedRole];
    }
}
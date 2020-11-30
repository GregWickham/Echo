using PropertyTools.DataAnnotations;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="NominalModifierBuilder"/> in a PropertyGrid</summary>
    public class NominalModifierProperties : ParentProperties
    {
        internal NominalModifierProperties(NominalModifierBuilder nmb) : base(nmb) { Model = nmb; }

        private NominalModifierBuilder Model;

        [Browsable(false)]
        public override string Description => Parent.DescriptionFor(Model);

        [Category("Syntax|")]
        [DisplayName("Role")]
        public string Role => Parent.DescriptionFor(Model.AssignedRole);
    }
}
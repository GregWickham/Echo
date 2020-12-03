using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>This parent type can be returned by the constituency parse, but we eliminate it during Configuration</summary>
    public class ParticleParent : ParentElementBuilder
    {
        private WordElement Particle = new WordElement();

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case ParticleBuilder pb:
                    AddChildWithRole(pb, ChildRole.Head);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Head);
        }

        public override NLGElement BuildElement() => BuildWord();

        public WordElement BuildWord() => Particle;

        public override IElementTreeNode CopyLightweight() => new ParticleParent { Particle = Particle.CopyWithoutSpec() };
    }
}

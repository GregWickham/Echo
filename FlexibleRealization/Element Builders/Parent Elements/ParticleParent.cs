using System;
using System.Linq;
using System.Text;
using SimpleNLG;

namespace FlexibleRealization
{
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
                default: throw new InvalidOperationException("Particle parent can't find a role for this child type");
            }
        }

        public override NLGElement BuildElement() => BuildWord();

        public WordElement BuildWord()
        {

            return Particle;
        }
    }
}

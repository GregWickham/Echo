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

        public override NLGElement BuildElement() => BuildWord();

        public WordElement BuildWord() => Particle;
    }
}

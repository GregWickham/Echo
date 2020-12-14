namespace FlexibleRealization.Dependencies
{
    /// <summary>expletive dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/mark.html</remarks>
    public class Marker : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case PrepositionBuilder prepositionDependent:
                    if (prepositionDependent.Parent is SubordinateClauseBuilder)    // It's a subordinating conjunction, not a preposition
                        prepositionDependent.Parent.SetRoleOfChild(prepositionDependent, ParentElementBuilder.ChildRole.Complementizer);
                    break;
                default: break;
            }
        }
    }
}

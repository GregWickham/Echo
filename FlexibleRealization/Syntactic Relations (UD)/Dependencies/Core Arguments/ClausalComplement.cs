namespace FlexibleRealization.Dependencies
{
    /// <summary>ccomp dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/ccomp.html</remarks>
    public class ClausalComplement : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case VerbBuilder verbDependent:
                    switch (Governor)
                    {
                        case VerbBuilder verbGovernor:
                            SubordinateClauseBuilder clauseContainingDependent = Dependent.LowestAncestorOfType<SubordinateClauseBuilder>();
                            if (clauseContainingDependent != null)
                                clauseContainingDependent.Complete(verbGovernor);
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }
    }
}

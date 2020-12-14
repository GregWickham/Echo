namespace FlexibleRealization.Dependencies
{
    /// <summary>advcl dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/advcl.html</remarks>
    public class AdverbialClauseModifier : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case VerbBuilder verbDependent:
                    break;
                default: break;
            }
        }
    }
}

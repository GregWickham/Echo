namespace FlexibleRealization.Dependencies
{
    /// <summary>aux:pass dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/aux_.html</remarks>
    public class AuxiliaryPassive : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case VerbBuilder verbDependent:
                    {
                        switch (Governor)
                        {
                            case VerbBuilder verbGovernor:
                                if (verbGovernor.IsPhraseHead)
                                {
                                    verbDependent.DetachFromParent();
                                    verbGovernor.ParentVerbPhrase.Passive = true;
                                }
                                break;
                        }
                        break;
                    }
                default: break;
            }
        }
    }
}

namespace FlexibleRealization.Dependencies
{
    /// <summary>obl:tmod dependency</summary>
    /// <remarks>https://universaldependencies.org/en/dep/obl-tmod.html</remarks>
    public class TemporalModifier : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case NounBuilder nounDependent:
                    switch (Governor)
                    {
                        case VerbBuilder verbGovernor:
                            if (nounDependent.IsPhraseHead && nounDependent.Parent is TemporalNounPhraseBuilder temporalNounPhrase)
                            temporalNounPhrase.Modify(verbGovernor);
                            break;
                    }
                    break;
            }
        }
    }
}

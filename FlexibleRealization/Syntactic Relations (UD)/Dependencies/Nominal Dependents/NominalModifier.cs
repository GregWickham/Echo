namespace FlexibleRealization.Dependencies
{
    /// <summary>nmod dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/nmod.html</remarks>
    public class NominalModifier : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case NounBuilder nounDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            if (nounDependent.IsObjectOfPreposition)
                            {
                                PrepositionBuilder prepositionGoverningDependent = nounDependent.GoverningPreposition;
                                PrepositionalPhraseBuilder phraseOfGoverningPreposition = prepositionGoverningDependent.ParentPrepositionalPhrase;
                                phraseOfGoverningPreposition.Modify(nounGovernor);
                            }
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }
    }
}

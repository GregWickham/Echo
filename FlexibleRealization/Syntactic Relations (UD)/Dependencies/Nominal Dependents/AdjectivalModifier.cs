using System;
using SimpleNLG;

namespace FlexibleRealization.Dependencies
{
    /// <summary>amod dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/amod.html</remarks>
    public class AdjectivalModifier : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case AdjectiveBuilder adjectiveDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            adjectiveDependent.Modify(nounGovernor);
                            break;
                        default: break;
                    }
                    break;
                case VerbBuilder verbDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            if (verbDependent.IsGerundOrPresentParticiple)
                            {
                                if (verbDependent.IsPhraseHead)
                                {
                                    verbDependent.ParentVerbPhrase.Form = form.PRESENT_PARTICIPLE;
                                    verbDependent.ParentVerbPhrase.Modify(nounGovernor);
                                }
                                else verbDependent.AsVerbPhrase(form.PRESENT_PARTICIPLE).Modify(nounGovernor);
                            }
                            else throw new InvalidOperationException("Verb is acting as an adjective but it's not a present participle");
                            break;
                        default: throw new InvalidOperationException("Verb is acting as an adjective but it can't modify this type of governor");
                    }
                    break;
                default: break;
            }

        }
    }
}

using SimpleNLG;

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
                                    if (verbGovernor.ParentVerbPhrase.Form == form.PAST_PARTICIPLE)
                                    {
                                        // The constituency parse identified the verb as a past participle, but it's a passive past tense so switch it back to normal form
                                        verbGovernor.ParentVerbPhrase.Form = form.NORMAL;
                                    }
                                }
                                break;
                        }
                        break;
                    }
            }
        }
    }
}

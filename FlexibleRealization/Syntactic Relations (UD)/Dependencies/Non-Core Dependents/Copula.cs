using System;

namespace FlexibleRealization.Dependencies
{
    /// <summary>cop dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/cop.html</remarks>
    public class Copula : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case VerbBuilder verbDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            if (!nounGovernor.Completes(verbDependent))
                                throw new InvalidOperationException();
                            break;
                        case AdjectiveBuilder adjectiveGovernor:
                            if (!adjectiveGovernor.Completes(verbDependent))
                                throw new InvalidOperationException();
                            break;
                    }
                    break;
            }
        }
    }
}

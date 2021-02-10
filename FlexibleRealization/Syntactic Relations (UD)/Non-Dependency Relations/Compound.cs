using System;

namespace FlexibleRealization.Dependencies
{
    /// <summary>compound relation</summary>
    /// <remarks>https://universaldependencies.org/u/dep/compound.html</remarks>
    public class Compound : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case NounBuilder nounDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            nounDependent.FormCompoundWith(nounGovernor);
                            break;
                        default: throw new InvalidOperationException();
                    }
                    break;
                case AdjectiveBuilder adjectiveDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            adjectiveDependent.FormCompoundWith(nounGovernor);  
                            break;
                        default: throw new InvalidOperationException();
                    }
                    break;
            }
        }
    }


    /// <summary>compound relation for particle verb</summary>
    /// <remarks>https://universaldependencies.org/u/dep/compound.html</remarks>
    public class CompoundParticleVerb : SyntacticRelation
    {
        public override void Apply()
        {
        }
    }
}

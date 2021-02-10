using System;

namespace FlexibleRealization.Dependencies
{
    /// <summary>det dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/det.html</remarks>
    public class Determiner : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case DeterminerBuilder determinerDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            determinerDependent.Specify(nounGovernor);
                            break;
                    }
                    break;
            }
        }
    }
}

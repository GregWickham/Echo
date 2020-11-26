using System.Collections.Generic;
using System.Linq;

namespace FlexibleRealization.Dependencies
{
    /// <summary>case dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/case.html</remarks>
    public class CaseMarking : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case PrepositionBuilder prepositionDependent:
                    switch (Governor)
                    {
                        case NounBuilder nounGovernor:
                            nounGovernor.Complete(prepositionDependent);
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }
    }
}

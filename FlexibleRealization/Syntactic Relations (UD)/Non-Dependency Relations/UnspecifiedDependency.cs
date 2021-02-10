using System;

namespace FlexibleRealization.Dependencies
{
    /// <summary>dep relation</summary>
    /// <remarks>https://universaldependencies.org/u/dep/dep.html</remarks>
    public class UnspecifiedDependency : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Governor)
            {
                case AdjectiveBuilder adjectiveGovernor:
                    switch (adjectiveGovernor.Parent)
                    {
                        case AdjectivePhraseBuilder adjectivePhraseParent:
                            switch (Dependent)
                            {
                                case AdverbBuilder adverbDependent:
                                    if (adverbDependent.Parent is AdverbPhraseBuilder && adverbDependent.AssignedRole == ParentElementBuilder.ChildRole.Head)
                                        adverbDependent.Parent.MoveTo(adjectivePhraseParent, ParentElementBuilder.ChildRole.Modifier);
                                    else
                                        adverbDependent.MoveTo(adjectivePhraseParent, ParentElementBuilder.ChildRole.Modifier);                                   
                                    break;
                                default: throw new InvalidOperationException();
                            }
                            break;
                        default: throw new InvalidOperationException();
                    }
                    break;
                case NounBuilder nounGovernor:
                    break;
            }
        }
    }
}

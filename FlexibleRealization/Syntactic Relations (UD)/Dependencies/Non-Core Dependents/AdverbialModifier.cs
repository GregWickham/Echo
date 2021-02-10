using System;
using System.Linq;

namespace FlexibleRealization.Dependencies
{
    /// <summary>advmod dependency</summary>
    /// <remarks>https://universaldependencies.org/u/dep/advmod.html</remarks>
    public class AdverbialModifier : SyntacticRelation
    {
        public override void Apply()
        {
            switch (Dependent)
            {
                case WhAdverbBuilder whAdverbDependent:
                    break;
                case AdverbBuilder adverbDependent:
                    switch (Governor)
                    {
                        // The AdjectiveBuilder case checks for a condition where the CoreNLP dependency indicates that Dependent modifies an adjective, but that adjective is the
                        // head of a phrase and is modified by a comparative adverb.  For SimpleNLG we represent this comparative adverb as a phrase that's modified by Dependent.
                        case AdjectiveBuilder adjectiveGovernor:
                            AdverbBuilder interveningComparative = InterveningComparativeAdverb;
                            if (interveningComparative != null)
                                adverbDependent.Modify(interveningComparative);
                            else
                                adverbDependent.Modify(adjectiveGovernor);
                            break;
                        case AdverbBuilder adverbGovernor:
                            adverbDependent.Modify(adverbGovernor);
                            break;
                        case VerbBuilder verbGovernor:
                            adverbDependent.Modify(verbGovernor);
                            break;
                        case NounBuilder nounGovernor:
                            AdjectiveBuilder interveningAdjective = InterveningAdjective; 
                            if (interveningAdjective != null)
                                adverbDependent.Modify(interveningAdjective);
                            break;
                        default: break;
                    }
                    break;
                default: break;
            }
        }

        private AdverbBuilder InterveningComparativeAdverb => Dependent.LowestCommonAncestor<PhraseBuilder>(Governor).PartsOfSpeechInSubtreeBetween(Dependent, Governor)
            .Where(posb => posb is AdverbBuilder)
            .Cast<AdverbBuilder>()
            .Where(advb => advb.Comparative)
            .FirstOrDefault();

        private AdjectiveBuilder InterveningAdjective => Dependent.LowestCommonAncestor<PhraseBuilder>(Governor).PartsOfSpeechInSubtreeBetween(Dependent, Governor)
            .Where(posb => posb is AdjectiveBuilder)
            .Cast<AdjectiveBuilder>()
            .FirstOrDefault();

    }
}

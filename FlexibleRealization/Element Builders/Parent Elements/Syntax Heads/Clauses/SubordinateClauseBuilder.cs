using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    public class SubordinateClauseBuilder : ClauseBuilder
    {
        public SubordinateClauseBuilder() : base(clauseStatus.SUBORDINATE) { }

        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case NounPhraseBuilder npb:
                    AddSubject(npb);
                    break;
                case VerbPhraseBuilder vpb:
                    SetPredicate(vpb);
                    break;
                case CoordinatedPhraseBuilder cpb:
                    AssignRoleFor(cpb);
                    break;
                case IndependentClauseBuilder icb:
                    Assimilate(icb);
                    break;
                case WhAdverbPhraseBuilder wapb:
                    SetComplementizer(wapb.HeadWord);
                    break;
                case WhNounPhraseBuilder wnpb:
                    SetComplementizer(wnpb.HeadWord);
                    break;
                case AdverbPhraseBuilder apb:
                    AddUnassignedChild(apb);
                    break;
                case PrepositionBuilder pb:
                    AddUnassignedChild(pb);
                    break;
                default: throw new InvalidOperationException("Subordinate clause can't find a role for this element");
            }
        }

        #endregion Initial assignment of children

        /// <summary>Set <paramref name="complementizer"/> as the ONLY complementizer of the subordinate clause we're going to build.</summary>
        /// <remarks>If we already have a complementizer and try to add another one, throw an exception.</remarks>
        private void SetComplementizer(WordElementBuilder complementizer)
        {
            if (Complementizers.Count() == 0)
            {
                AddChildWithRole(complementizer, ChildRole.Complementizer);
            }
            else throw new InvalidOperationException("Can't add multiple coordinators to a subordinate clause");
        }

        /// <summary>Return the children that have been added to this with a ChildRole of Coordinator</summary>
        private IEnumerable<WordElementBuilder> Complementizers => ChildrenWithRole(ChildRole.Complementizer).Cast<WordElementBuilder>();

        /// <summary>If a phrase is coordinated, it is expected to have at most one coordinator (usually a coordinating conjunction)</summary>
        private WordElementBuilder ComplementizerBuilder => Complementizers.Count() switch
        {
            0 => null,
            1 => Complementizers.First(),
            _ => throw new InvalidOperationException("Unable to resolve complementizer")
        };

        public override NLGElement BuildElement()
        {
            // The CoreNLP constituency parser may return a clause with no predicate, or no subjects.
            // If that happened, it should have been taken care of during the Configure() process.
            // Once we get to this point, assume we have a predicate and at least one subject.
            if (ComplementizerBuilder != null) Clause.Complementiser = ComplementizerBuilder.BuildWord().Base;
            Clause.subj = Subjects.Select(subject => subject.BuildElement()).ToArray();
            Clause.vp = PredicateBuilder.BuildElement();
            return Clause;
        }
    }
}

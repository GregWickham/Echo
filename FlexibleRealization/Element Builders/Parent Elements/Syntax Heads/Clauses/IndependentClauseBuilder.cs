using System;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    public class IndependentClauseBuilder : ClauseBuilder
    {
        public IndependentClauseBuilder() : base(clauseStatus.MATRIX) { }

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
                case AdverbPhraseBuilder apb:
                    AddUnassignedChild(apb);
                    break;
                case PrepositionBuilder pb:
                    AddUnassignedChild(pb);
                    break;
                default: throw new InvalidOperationException("Independent clause can't find a role for this element");
            }
        }

        #endregion Initial assignment of children

        #region Configuration

        public override IElementTreeNode Consolidate()
        {
            if (PredicateBuilder == null)
            {
                if (Subjects.Count() == 1) return Become(Subjects.First());
                else throw new InvalidOperationException("Clause has no predicate, and multiple subjects that are not coordinated");
            }
            else if (Subjects.Count() == 0) return Become(PredicateBuilder);
            else return this;
        }

        public override IElementTreeNode CopyLightweight() => new IndependentClauseBuilder { Clause = Clause.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

        #endregion Configuration

        public override NLGElement BuildElement()
        {
            // The CoreNLP constituency parser may return a clause with no predicate, or no subjects.
            // If that happened, it should have been taken care of during the Configure() process.
            // Once we get to this point, assume we have a predicate and at least one subject.
            Clause.subj = Subjects.Select(subject => subject.BuildElement()).ToArray();
            Clause.vp = PredicateBuilder.BuildElement();
            return Clause;
        }

    }
}

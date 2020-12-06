using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    public class IndependentClauseBuilder : ClauseBuilder
    {
        public IndependentClauseBuilder() : base(clauseStatus.MATRIX) { }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Subject);
            if (PredicateBuilder == null) listOfRoles.Add(ChildRole.Predicate);
        }

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
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        #endregion Initial assignment of children

        #region Configuration

        public override void Consolidate()
        {
            if (Children.Count() == 0) Become(null);
            else
            {
                if (PredicateBuilder == null)
                {
                    if (Subjects.Count() == 1) Become(Subjects.Single());
                }
                else if (Subjects.Count() == 0) Become(PredicateBuilder);
            }
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

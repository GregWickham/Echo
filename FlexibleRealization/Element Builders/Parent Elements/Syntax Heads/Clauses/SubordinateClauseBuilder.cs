using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    public class SubordinateClauseBuilder : ClauseBuilder
    {
        public SubordinateClauseBuilder() : base(clauseStatus.SUBORDINATE) { }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Subject);
            if (PredicateBuilder == null) listOfRoles.Add(ChildRole.Predicate);
            if (ComplementizerBuilder == null) listOfRoles.Add(ChildRole.Complementizer);
        }

        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case NounPhraseBuilder npb:  // Might be a WhNounPhraseBuilder
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
                case ConjunctionBuilder cb:
                    SetComplementizer(cb);
                    break;
                case AdverbPhraseBuilder apb:  // Might be a WhAdverbPhraseBuilder
                    AddUnassignedChild(apb);
                    break;
                case PrepositionBuilder pb:
                    AddUnassignedChild(pb);
                    break;
                case AdjectivePhraseBuilder whadjpb:  // Might be a WhAdjectivePhraseBuilder
                    AddUnassignedChild(whadjpb);
                    break;
                default: throw new InvalidOperationException("Subordinate clause can't find a role for this element");
            }
        }

        #endregion Initial assignment of children

        /// <summary>Set <paramref name="complementizer"/> as the ONLY complementizer of the subordinate clause we're going to build.</summary>
        /// <remarks>If we already have a complementizer and try to add another one, throw an exception.  Visibility is not private because the
        /// Complementizer can be set by a Wh word phrase during its Configuration.</remarks>
        internal void SetComplementizer(WordElementBuilder complementizer)
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

        #region Features

        //public bool ComplementiserSpecified => Clause.Complementiser != null;
        //public string Complementiser
        //{
        //    get => Clause.COMPLEMENTISER;
        //    set
        //    {
        //        Clause.COMPLEMENTISER = value == null || value.Length == 0 ? null : value;
        //        OnPropertyChanged();
        //    }
        //}

        //public bool SuppressedComplementiserSpecified
        //{
        //    get => Clause.SUPRESSED_COMPLEMENTISERSpecified;
        //    set
        //    {
        //        Clause.SUPRESSED_COMPLEMENTISERSpecified = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public bool SuppressedComplementiser
        //{
        //    get => Clause.SUPRESSED_COMPLEMENTISER;
        //    set
        //    {
        //        Clause.SUPRESSED_COMPLEMENTISER = value;
        //        Clause.SUPRESSED_COMPLEMENTISERSpecified = true;
        //        OnPropertyChanged();
        //    }
        //}

        #endregion Features

        public override IElementTreeNode CopyLightweight() => new SubordinateClauseBuilder { Clause = Clause.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

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

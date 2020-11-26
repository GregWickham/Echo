using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG SPhraseSpec</summary>
    public abstract class ClauseBuilder : SyntaxHeadBuilder
    {
        private protected ClauseBuilder(clauseStatus type) => ClauseStatus = type;

        #region Initial assignment of children

        private protected void AddSubject(IElementTreeNode head) => AddChildWithRole(head, ChildRole.Subject);

        private protected IEnumerable<IElementTreeNode> Subjects => ChildrenWithRole(ChildRole.Subject);

        private protected void SetPredicate(VerbPhraseBuilder predicate)
        {
            if (Predicates.Count() == 0)
            {
                AddChildWithRole(predicate, ChildRole.Predicate);
            }
            else throw new InvalidOperationException("Can't add multiple predicates to a clause");
        }

        private protected IEnumerable<IElementTreeNode> Predicates => ChildrenWithRole(ChildRole.Predicate);

        private protected VerbPhraseBuilder PredicateBuilder => Predicates.Count() switch
        {
            0 => null,
            1 => Predicates.First() as VerbPhraseBuilder,
            _ => throw new InvalidOperationException("Unable to resolve clause predicate")
        };

        /// <summary>Assimilate <paramref name="clause"/> into this clause</summary>
        private protected void Assimilate(ClauseBuilder clause)
        {
            clause.DetachFromParent();
            foreach (IElementTreeNode eachSubject in clause.Subjects.ToList())
            {
                eachSubject.DetachFromParent();
                AddSubject(eachSubject);
            }
            VerbPhraseBuilder predicate = clause.PredicateBuilder;
            if (predicate != null)
            {
                predicate.DetachFromParent();
                SetPredicate(predicate);
            }
        }

        #endregion Initial assignment of children

        public SPhraseSpec Clause = new SPhraseSpec();

        #region Clause features

        public bool DiscourseFunctionSpecified
        {
            get => Clause.discourseFunctionSpecified;
            set
            {
                Clause.discourseFunctionSpecified = value;
                OnPropertyChanged();
            }
        }
        public discourseFunction DiscourseFunction
        {
            get => Clause.discourseFunction;
            set
            {
                Clause.discourseFunction = value;
                Clause.discourseFunctionSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool AppositiveSpecified
        {
            get => Clause.appositiveSpecified;
            set
            {
                Clause.appositiveSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Appositive
        {
            get => Clause.appositive;
            set
            {
                Clause.appositive = value;
                Clause.appositiveSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool AggregateAuxiliarySpecified
        {
            get => Clause.AGGREGATE_AUXILIARYSpecified;
            set
            {
                Clause.AGGREGATE_AUXILIARYSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool AggregateAuxiliary
        {
            get => Clause.AGGREGATE_AUXILIARY;
            set
            {
                Clause.AGGREGATE_AUXILIARY = value;
                Clause.AGGREGATE_AUXILIARYSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool ClauseStatusSpecified
        {
            get => Clause.CLAUSE_STATUSSpecified;
            set
            {
                Clause.CLAUSE_STATUSSpecified = value;
                OnPropertyChanged();
            }
        }
        public clauseStatus ClauseStatus
        {
            get => Clause.CLAUSE_STATUS;
            set
            {
                Clause.CLAUSE_STATUS = value;
                Clause.CLAUSE_STATUSSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool ComplementiserSpecified => Clause.Complementiser != null;
        public string Complementiser
        {
            get => Clause.COMPLEMENTISER;
            set
            {
                Clause.COMPLEMENTISER = value.Length == 0 ? null : value;
                OnPropertyChanged();
            }
        }

        public bool FormSpecified
        {
            get => Clause.FORMSpecified;
            set
            {
                Clause.FORMSpecified = value;
                OnPropertyChanged();
            }
        }
        public form Form
        {
            get => Clause.FORM;
            set
            {
                Clause.FORM = value;
                Clause.FORMSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool InterrogativeTypeSpecified
        {
            get => Clause.INTERROGATIVE_TYPESpecified;
            set
            {
                Clause.INTERROGATIVE_TYPESpecified = value;
                OnPropertyChanged();
            }
        }
        public interrogativeType InterrogativeType
        {
            get => Clause.INTERROGATIVE_TYPE;
            set 
            {
                Clause.INTERROGATIVE_TYPE = value;
                Clause.INTERROGATIVE_TYPESpecified = true;
                OnPropertyChanged();
            }
        }

        public bool ModalSpecified => Clause.MODAL != null;
        public string Modal
        {
            get => Clause.MODAL;
            set
            {
                Clause.MODAL = value.Length == 0 ? null : value;
                OnPropertyChanged();
            }
        }

        public bool NegatedSpecified
        {
            get => Clause.NEGATEDSpecified;
            set
            {
                Clause.NEGATEDSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Negated
        {
            get => Clause.NEGATED;
            set
            {
                Clause.NEGATED = value;
                Clause.NEGATEDSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool PassiveSpecified
        {
            get => Clause.PASSIVESpecified;
            set
            {
                Clause.PASSIVESpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Passive
        {
            get => Clause.PASSIVE;
            set
            {
                Clause.PASSIVE = value;
                Clause.PASSIVESpecified = true;
                OnPropertyChanged();
            }
        }

        public bool PerfectSpecified
        {
            get => Clause.PERFECTSpecified;
            set
            {
                Clause.PERFECTSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Perfect
        {
            get => Clause.PERFECT;
            set
            {
                Clause.PERFECT = value;
                Clause.PERFECTSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool PersonSpecified
        {
            get => Clause.PERSONSpecified;
            set
            {
                Clause.PERSONSpecified = value;
                OnPropertyChanged();
            }
        }
        public person Person
        {
            get => Clause.PERSON;
            set
            {
                Clause.PERSON = value;
                Clause.PERSONSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool ProgressiveSpecified
        {
            get => Clause.PROGRESSIVESpecified;
            set
            {
                Clause.PROGRESSIVESpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Progressive
        {
            get => Clause.PROGRESSIVE;
            set
            {
                Clause.PROGRESSIVE = value;
                Clause.PROGRESSIVESpecified = true;
                OnPropertyChanged();
            }
        }

        public bool SuppressGenitiveInGerundSpecified
        {
            get => Clause.SUPPRESS_GENITIVE_IN_GERUNDSpecified;
            set
            {
                Clause.SUPPRESS_GENITIVE_IN_GERUNDSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool SuppressGenitiveInGerund
        {
            get => Clause.SUPPRESS_GENITIVE_IN_GERUND;
            set
            {
                Clause.SUPPRESS_GENITIVE_IN_GERUND = value;
                Clause.SUPPRESS_GENITIVE_IN_GERUNDSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool SuppressedComplementiserSpecified
        {
            get => Clause.SUPRESSED_COMPLEMENTISERSpecified;
            set
            {
                Clause.SUPRESSED_COMPLEMENTISERSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool SuppressedComplementiser
        {
            get => Clause.SUPRESSED_COMPLEMENTISER;
            set
            {
                Clause.SUPRESSED_COMPLEMENTISER = value;
                Clause.SUPRESSED_COMPLEMENTISERSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool TenseSpecified
        {
            get => Clause.TENSESpecified;
            set
            {
                Clause.TENSESpecified = value;
                OnPropertyChanged();
            }
        }
        public tense Tense
        {
            get => Clause.TENSE;
            set
            {
                Clause.TENSE = value;
                Clause.TENSESpecified = true;
                OnPropertyChanged();
            }
        }

        #endregion Clause features

    }
}

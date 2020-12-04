using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    public class CoordinatedPhraseBuilder : SyntaxHeadBuilder
    {
        private CoordinatedPhraseBuilder(phraseCategory category)
        {
            Phrase = new CoordinatedPhraseElement(category);
        }

        internal CoordinatedPhraseBuilder(phraseCategory category, IEnumerable<IElementTreeNode> coordinated, ConjunctionBuilder coordinator)
        {
            Phrase = new CoordinatedPhraseElement(category);
            SetCoordinatedElements(coordinated);
            SetCoordinator(coordinator);
        }

        /// <summary>The CoordinatedPhraseElement that will be built by this</summary>
        private CoordinatedPhraseElement Phrase;

        public phraseCategory PhraseCategory => Phrase.Category;

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Coordinated);
            if (CoordinatorBuilder == null || CoordinatorBuilder == child) listOfRoles.Add(ChildRole.Coordinator);
        }

        /// <summary>This method should never be called because CoordinatedPhraseBuilder is not created from a constituency parse.</summary>
        /// <remarks>A CoordinatedPhraseBuilder is only created by a CoordinablePhraseBuilder and given its complete
        /// set of children at the time of its creation.  A standard set of children are assigned by the constructor; others
        /// may be assigned during the Configuration process.</remarks>
        private protected override void AssignRoleFor(IElementTreeNode child) => throw new NotImplementedException();

        /// <summary>Return the IElementBuilders coordinated by this CoordinatedPhraseBuilder</summary>
        internal IEnumerable<IElementTreeNode> CoordinatedElements => ChildrenWithRole(ChildRole.Coordinated).OrderBy(element => element.MinTokenIndex);

        /// <summary>Assign the IElementBuilders to be <paramref name="coordinated"/> by this CoordinatedPhraseBuilder</summary>
        private void SetCoordinatedElements(IEnumerable<IElementTreeNode> coordinated) => AddChildrenWithRole(coordinated, ChildRole.Coordinated);

        /// <summary>Set <paramref name="coordinator"/> as the ONLY coordinating conjunction of the CoordinatedPhraseElement we're going to build.</summary>
        /// <remarks>If we already have a coordinating conjunction and try to add another one, throw an exception.</remarks>
        private void SetCoordinator(ConjunctionBuilder coordinator)
        {
            if (Coordinators.Count() == 0) AddChildWithRole(coordinator, ChildRole.Coordinator);
            else throw new InvalidOperationException("Can't add multiple coordinators to a coordinated phrase");
        }

        /// <summary>Return the children that have been added to this with a ChildRole of Coordinator</summary>
        private IEnumerable<ConjunctionBuilder> Coordinators => ChildrenWithRole(ChildRole.Coordinator).Cast<ConjunctionBuilder>();

        /// <summary>If a phrase is coordinated, it is expected to have at most one coordinator (usually a coordinating conjunction)</summary>
        private ConjunctionBuilder CoordinatorBuilder => Coordinators.Count() switch
        {
            0 => null,
            1 => Coordinators.First(),
            _ => throw new InvalidOperationException("Unable to resolve Coordinator")
        };

        public override IElementTreeNode CopyLightweight() => new CoordinatedPhraseBuilder(PhraseCategory)
            .LightweightCopyChildrenFrom(this);

        public override NLGElement BuildElement()
        {
            CoordinatedPhraseElement phrase = new CoordinatedPhraseElement();
            phrase.conj = CoordinatorBuilder.BuildWord().Base;
            phrase.coord = CoordinatedElements.Select(coordinated => coordinated.BuildElement()).ToArray();
            return phrase;
        }

        #region Phrase features

        public bool ConjunctionSpecified => Phrase.conj != null;
        public string Conjunction
        {
            get => Phrase.conj;
            set => Phrase.conj = value.Length == 0 ? null : value;
        }

        public bool AppositiveSpecified
        {
            get => Phrase.APPOSITIVESpecified;
            set => Phrase.APPOSITIVESpecified = value;
        }
        public bool Appositive
        {
            get => Phrase.APPOSITIVE;
            set
            {
                Phrase.APPOSITIVE = value;
                Phrase.APPOSITIVESpecified = true;
            }
        }

        public bool ConjunctionTypeSpecified => Phrase.CONJUNCTION_TYPE != null;
        public string ConjunctionType
        {
            get => Phrase.CONJUNCTION_TYPE;
            set => Phrase.CONJUNCTION_TYPE = value.Length == 0 ? null : value;
        }

        public bool ModalSpecified => Phrase.MODAL != null;
        public string Modal
        {
            get => Phrase.MODAL;
            set => Phrase.MODAL = value.Length == 0 ? null : value;
        }

        public bool NegatedSpecified
        {
            get => Phrase.NEGATEDSpecified;
            set => Phrase.NEGATEDSpecified = value;
        }
        public bool Negated
        {
            get => Phrase.NEGATED;
            set
            {
                Phrase.NEGATED = value;
                Phrase.NEGATEDSpecified = true;
            }
        }

        public bool NumberSpecified
        {
            get => Phrase.NUMBERSpecified;
            set => Phrase.NUMBERSpecified = value;
        }
        public numberAgreement Number
        {
            get => Phrase.NUMBER;
            set
            {
                Phrase.NUMBER = value;
                Phrase.NUMBERSpecified = true;
            }
        }

        public bool PersonSpecified
        {
            get => Phrase.PERSONSpecified;
            set => Phrase.PERSONSpecified = value;
        }
        public person Person
        {
            get => Phrase.PERSON;
            set
            {
                Phrase.PERSON = value;
                Phrase.PERSONSpecified = true;
            }
        }

        public bool PossessiveSpecified
        {
            get => Phrase.POSSESSIVESpecified;
            set => Phrase.POSSESSIVESpecified = value;
        }
        public bool Possessive
        {
            get => Phrase.POSSESSIVE;
            set
            {
                Phrase.POSSESSIVE = value;
                Phrase.POSSESSIVESpecified = true;
            }
        }

        public bool ProgressiveSpecified
        {
            get => Phrase.PROGRESSIVESpecified;
            set => Phrase.PROGRESSIVESpecified = value;
        }
        public bool Progressive
        {
            get => Phrase.PROGRESSIVE;
            set
            {
                Phrase.PROGRESSIVE = value;
                Phrase.PROGRESSIVESpecified = true;
            }
        }

        public bool RaiseSpecifierSpecified
        {
            get => Phrase.RAISE_SPECIFIERSpecified;
            set => Phrase.RAISE_SPECIFIERSpecified = value;
        }
        public bool RaiseSpecifier
        {
            get => Phrase.RAISE_SPECIFIER;
            set
            {
                Phrase.RAISE_SPECIFIER = value;
                Phrase.RAISE_SPECIFIER = true;
            }
        }

        public bool SuppressedComplementiserSpecified
        {
            get => Phrase.SUPRESSED_COMPLEMENTISERSpecified;
            set => Phrase.SUPRESSED_COMPLEMENTISERSpecified = value;
        }
        public bool SuppressedComplementiser
        {
            get => Phrase.SUPRESSED_COMPLEMENTISER;
            set
            {
                Phrase.SUPRESSED_COMPLEMENTISER = value;
                Phrase.SUPRESSED_COMPLEMENTISERSpecified = true;
            }
        }

        public bool TenseSpecified
        {
            get => Phrase.TENSESpecified;
            set => Phrase.TENSESpecified = value;
        }
        public tense Tense
        {
            get => Phrase.TENSE;
            set
            {
                Phrase.TENSE = value;
                Phrase.TENSESpecified = true;
            }
        }

        #endregion Phrase features
    }
}

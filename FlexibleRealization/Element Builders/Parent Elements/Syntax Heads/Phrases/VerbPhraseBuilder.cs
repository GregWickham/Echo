using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG VPPhraseSpec</summary>
    public class VerbPhraseBuilder : CoordinablePhraseBuilder<VPPhraseSpec>
    {
        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case VerbBuilder vb:
                    AssignRoleFor(vb);
                    break;
                case ModalBuilder mb:
                    SetModal(mb);
                    break;
                case NounPhraseBuilder npb:
                    AddComplement(npb);
                    break;
                case VerbPhraseBuilder vpb:
                    AddHead(vpb);
                    break;
                case AdjectivePhraseBuilder apb:
                    AddComplement(apb);
                    break;
                case AdverbPhraseBuilder apb:
                    AddModifier(apb);
                    break;
                case PrepositionalPhraseBuilder ppb:
                    AddModifier(ppb);
                    break;
                case ConjunctionBuilder cb:
                    SetCoordinator(cb);
                    break;
                case CoordinatedPhraseBuilder cpb:
                    AssignRoleFor(cpb);
                    break;
                case ParticleParent pp:
                    AddModifier(pp);
                    break;
                case SubordinateClauseBuilder scb:
                    AddModifier(scb);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        private void AssignRoleFor(VerbBuilder verb)
        {
            AddHead(verb.Token.PartOfSpeech switch
            {
                "VBD" => verb.AsVerbPhrase(tense.PAST),
                "VBN" => verb.AsVerbPhrase(tense.PAST),
                "VBP" => verb.AsVerbPhrase(tense.PRESENT),
                "VBZ" => verb.AsVerbPhrase(tense.PRESENT),
                "VBG" => verb.AsVerbPhrase(form.GERUND),
                _ => verb
            });
        }

        private void AssignRoleFor(CoordinatedPhraseBuilder phrase)
        {
            switch (phrase.PhraseCategory)
            {
                case phraseCategory.NOUN_PHRASE:
                    AddComplement(phrase);
                    break;
                default: throw new InvalidOperationException("Verb phrase builder can't assign this type of coordinated phrase");
            }
        }

        /// <summary>Set <paramref name="modal"/> as the ONLY modal for this verb phrase</summary>
        private void SetModal(ModalBuilder modal)
        {
            if (Modals.Count() == 0)
            {
                AddChildWithRole(modal, ChildRole.Modal);
            }
            else throw new InvalidOperationException("Can't add multiple modals to a verb phrase");
        }

        /// <summary>Return the children that have been added to this with a ChildRole of Modal</summary>
        private IEnumerable<IElementTreeNode> Modals => ChildrenWithRole(ChildRole.Modal);

        /// <summary>Return the modal for this verb phrase, or null if it has no modal</summary>
        private ModalBuilder ModalBuilder => Modals.Count() switch
        {
            0 => null,
            1 => (ModalBuilder)Modals.First(),
            _ => throw new InvalidOperationException("Unable to resolve modal")
        };

        #endregion Initial assignment of children

        #region Configuration

        public override IElementTreeNode Configure()
        {
            base.Configure();
            // The CoreNLP constituency parse can have a noun phrase that contains another noun phrase as its head.  
            // For SimpleNLG realization we need to flatten this configuration into a single noun phrase.
            if (Heads.Count() == 1 && Heads.First() is VerbPhraseBuilder loneHeadPhrase)
            {
                RemoveChild(loneHeadPhrase);
                Assimilate(loneHeadPhrase);
            }
            return this;
        }

        /// <summary>Merge <paramref name="phraseToAssimilate"/> into this verb phrase</summary>
        private void Assimilate(VerbPhraseBuilder phraseToAssimilate)
        {
            AddHeads(phraseToAssimilate.Heads);
            if (phraseToAssimilate.ModalBuilder != null)
            {
                if (ModalBuilder == null) SetModal(phraseToAssimilate.ModalBuilder);
                else throw new InvalidOperationException("Modals collided when trying to assimilate a verb phrase");
            }
            AddModifiers(phraseToAssimilate.Modifiers);
            AddComplements(phraseToAssimilate.Complements);

            if (phraseToAssimilate.FormSpecified) Form = phraseToAssimilate.Form;
            if (phraseToAssimilate.TenseSpecified) Tense = phraseToAssimilate.Tense;
            if (phraseToAssimilate.PassiveSpecified) Passive = phraseToAssimilate.Passive;
        }

        #endregion Configuration

        #region Phrase features

        public bool AggregateAuxiliarySpecified
        {
            get => Phrase.AGGREGATE_AUXILIARYSpecified;
            set
            {
                Phrase.AGGREGATE_AUXILIARYSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool AggregateAuxiliary
        {
            get => Phrase.AGGREGATE_AUXILIARY;
            set
            {
                Phrase.AGGREGATE_AUXILIARY = value;
                Phrase.AGGREGATE_AUXILIARYSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool FormSpecified
        {
            get => Phrase.FORMSpecified;
            set
            {
                Phrase.FORMSpecified = value;
                OnPropertyChanged();
            }
        }
        public form Form
        {
            get => Phrase.FORM;
            set
            {
                Phrase.FORM = value;
                Phrase.FORMSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool ModalSpecified => Phrase.MODAL != null;
        public string Modal
        {
            get => Phrase.MODAL;
            set
            {
                Phrase.MODAL = value.Length == 0 ? null : value;
                OnPropertyChanged();
            }
        }

        public bool NegatedSpecified
        {
            get => Phrase.NEGATEDSpecified;
            set
            {
                Phrase.NEGATEDSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Negated
        {
            get => Phrase.NEGATED;
            set
            {
                Phrase.NEGATED = value;
                Phrase.NEGATEDSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool PassiveSpecified
        {
            get => Phrase.PASSIVESpecified;
            set
            {
                Phrase.PASSIVESpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Passive
        {
            get => Phrase.PASSIVE;
            set
            {
                Phrase.PASSIVE = value;
                Phrase.PASSIVESpecified = true;
                OnPropertyChanged();
            }
        }

        public bool PerfectSpecified
        {
            get => Phrase.PERFECTSpecified;
            set
            {
                Phrase.PERFECTSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Perfect
        {
            get => Phrase.PERFECT;
            set
            {
                Phrase.PERFECT = value;
                Phrase.PERFECTSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool PersonSpecified
        {
            get => Phrase.PERSONSpecified;
            set
            {
                Phrase.PERSONSpecified = value;
                OnPropertyChanged();
            }
        }
        public person Person
        {
            get => Phrase.PERSON;
            set
            {
                Phrase.PERSON = value;
                Phrase.PERSONSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool ProgressiveSpecified
        {
            get => Phrase.PROGRESSIVESpecified;
            set
            {
                Phrase.PROGRESSIVESpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Progressive
        {
            get => Phrase.PROGRESSIVE;
            set
            {
                Phrase.PROGRESSIVE = value;
                Phrase.PROGRESSIVESpecified = true;
                OnPropertyChanged();
            }
        }

        public bool SuppressGenitiveInGerundSpecified
        {
            get => Phrase.SUPPRESS_GENITIVE_IN_GERUNDSpecified;
            set
            {
                Phrase.SUPPRESS_GENITIVE_IN_GERUNDSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool SuppressGenitiveInGerund
        {
            get => Phrase.SUPPRESS_GENITIVE_IN_GERUND;
            set
            {
                Phrase.SUPPRESS_GENITIVE_IN_GERUND = value;
                Phrase.SUPPRESS_GENITIVE_IN_GERUNDSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool SuppressedComplementiserSpecified
        {
            get => Phrase.SUPRESSED_COMPLEMENTISERSpecified;
            set
            {
                Phrase.SUPRESSED_COMPLEMENTISERSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool SuppressedComplementiser
        {
            get => Phrase.SUPRESSED_COMPLEMENTISER;
            set
            {
                Phrase.SUPRESSED_COMPLEMENTISER = value;
                Phrase.SUPRESSED_COMPLEMENTISERSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool TenseSpecified
        {
            get => Phrase.TENSESpecified;
            set
            {
                Phrase.TENSESpecified = value;
                OnPropertyChanged();
            }
        }
        public tense Tense
        {
            get => Phrase.TENSE;
            set
            {
                Phrase.TENSE = value;
                Phrase.TENSESpecified = true;
                OnPropertyChanged();
            }
        }

        #endregion Phrase features

        public override NLGElement BuildElement()
        {
            if (ModalBuilder != null) Modal = ModalBuilder.BuildWord().Base;
            Phrase.preMod = PreModifiers
                .Select(preModifier => preModifier.BuildElement())
                .ToArray();
            Phrase.head = UnaryHead.BuildWord();
            Phrase.compl = Complements
                .Select(complement => complement.BuildElement())
                .ToArray();
            Phrase.postMod = PostModifiers
                .Select(postModifier => postModifier.BuildElement())
                .ToArray();
            return Phrase;
        }
    }
}

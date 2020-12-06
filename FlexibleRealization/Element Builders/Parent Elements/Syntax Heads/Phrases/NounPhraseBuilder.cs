using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG NPPhraseSpec</summary>
    public class NounPhraseBuilder : CoordinablePhraseBuilder<NPPhraseSpec>
    {
        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Head);
            listOfRoles.Add(ChildRole.Modifier);
            listOfRoles.Add(ChildRole.Complement);
            if (SpecifierBuilder == null || SpecifierBuilder == child) listOfRoles.Add(ChildRole.Specifier);
            if (CoordinatorBuilder == null || CoordinatorBuilder == child) listOfRoles.Add(ChildRole.Coordinator);
        }

        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case NounBuilder nb:
                    AddHead(nb);
                    break;
                case DeterminerBuilder db:
                    SetSpecifier(db);
                    break;
                case AdjectiveBuilder ab:
                    AddModifier(ab);
                    break;
                case AdverbBuilder ab:
                    AddModifier(ab);
                    break;
                case PronounBuilder pb:
                    AssignRoleFor(pb);
                    break;
                case VerbBuilder vb:
                    AssignRoleFor(vb);
                    break;
                case NounPhraseBuilder npb:
                    AssignRoleFor(npb);
                    break;
                case AdjectivePhraseBuilder apb:
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
                case PunctuationBuilder pb:
                    // Leave it up to SimpleNLG to add punctuation
                    break;
                case PossessiveEnding pe:
                    Possessive = true;
                    break;
                case NominalModifierBuilder nmb:
                    AddModifier(nmb);
                    break;
                case SubordinateClauseBuilder scb:
                    AddComplement(scb);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        private void AssignRoleFor(PronounBuilder pronoun)
        {
            switch (pronoun.Case)
            {
                case PronounCase.Subjective:
                    AddHead(pronoun);
                    break;
                case PronounCase.Possessive:
                    SetSpecifier(pronoun.AsNounPhrase());
                    break;
                default:
                    AddUnassignedChild(pronoun);
                    break;
            }
        }

        private void AssignRoleFor(VerbBuilder verb)
        {
            if (verb.IsGerundOrPresentParticiple) 
                AddUnassignedChild(verb);    // Later on, while applying dependency relations, we'll have to decide whether it's a gerund acting as a noun, or a present participle acting as an adjective
            else
                AddUnassignedChild(verb);
        }

        private void AssignRoleFor(NounPhraseBuilder phrase)
        {
            if (phrase.PossessiveSpecified && phrase.Possessive) SetSpecifier(phrase);
            else AddHead(phrase);
        }

        private void AssignRoleFor(CoordinatedPhraseBuilder phrase)
        {
            switch (phrase.PhraseCategory)
            {
                case phraseCategory.ADJECTIVE_PHRASE:
                    AddModifier(phrase);
                    break;
                default:
                    AddUnassignedChild(phrase);
                    break;
            }
        }

        #endregion Initial assignment of children

        #region Configuration

        public sealed override void Configure()
        {
            base.Configure();
            // The CoreNLP constituency parse can have a noun phrase that contains another noun phrase as its head.  
            // For SimpleNLG realization we need to flatten this configuration into a single noun phrase.
            if (Heads.Count() == 1 && Heads.Single() is NounPhraseBuilder loneHeadPhrase)
            {
                RemoveChild(loneHeadPhrase);
                Assimilate(loneHeadPhrase);
            }
        }

        /// <summary>Set <paramref name="specifier"/> as the ONLY specifier for this noun phrase</summary>
        private void SetSpecifier(IElementTreeNode specifier)
        {
            if (Specifiers.Count() == 0) AddChildWithRole(specifier, ChildRole.Specifier);
            else throw new InvalidOperationException("Can't add multiple specifiers to a noun phrase");
        }

        /// <summary>Return the children that have been added to this with a ChildRole of Specifier</summary>
        private IEnumerable<IElementTreeNode> Specifiers => ChildrenWithRole(ChildRole.Specifier);

        /// <summary>Return the specifier for this noun phrase, or null if it has no specifier</summary>
        private IElementTreeNode SpecifierBuilder => Specifiers.Count() switch
        {
            0 => null,
            1 => Specifiers.First(),
            _ => throw new InvalidOperationException("Unable to resolve Specifier")
        };

        /// <summary>Merge <paramref name="phraseToAssimilate"/> into this NounPhraseBuilder</summary>
        private void Assimilate(NounPhraseBuilder phraseToAssimilate)
        {
            AddHeads(phraseToAssimilate.Heads);
            if (phraseToAssimilate.CoordinatorBuilder != null)
            {
                if (CoordinatorBuilder == null) SetCoordinator(phraseToAssimilate.CoordinatorBuilder);
                else throw new InvalidOperationException("Coordinators collided when trying to assimilate a noun phrase");
            }
            if (phraseToAssimilate.SpecifierBuilder != null)
            {
                if (SpecifierBuilder == null) SetSpecifier(phraseToAssimilate.SpecifierBuilder);
                else throw new InvalidOperationException("Specifiers collided when trying to assimilate a noun phrase");
            }
            AddModifiers(phraseToAssimilate.Modifiers);
            AddComplements(phraseToAssimilate.Complements);
        }

        /// <summary>Return the CoordinatedPhraseBuilder for this noun phrase</summary>
        /// <remarks>The noun phrase's Specifier does not get incorporated into the CoordinatedPhraseElement.  Instead, it must be applied to one of the
        /// coordinated elements.  When the noun phrase is in its non-coordinated form, the Specifier is present in its expected place, so the Determiner
        /// syntactic relation will not change anything when applied.  Therefore we need to re-apply that syntactic relation after coordinating the phrase.</remarks>
        private protected sealed override CoordinatedPhraseBuilder AsCoordinatedPhrase()
        {
            CoordinatedPhraseBuilder result = base.AsCoordinatedPhrase();
            SpecifierBuilder?.Specify(SpecifierBuilder.NearestOf(result.CoordinatedElements));
            Modifiers.ToList().ForEach(modifier => modifier.Modify(modifier.NearestOf(result.CoordinatedElements)));
            Complements.ToList().ForEach(complement => complement.Complete(complement.NearestOf(result.CoordinatedElements)));
            return result;
        }

        public override IElementTreeNode CopyLightweight() => new NounPhraseBuilder { Phrase = Phrase.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

        #endregion Configuration

        public override NLGElement BuildElement()
        {
            if (SpecifierBuilder != null) Phrase.spec = SpecifierBuilder.BuildElement();
            Phrase.preMod = PreModifiers
                .Select(preModifier => preModifier.BuildElement())
                .ToArray();
            if (UnaryHead != null) Phrase.head = UnaryHead.BuildWord();
            Phrase.compl = Complements
                .Select(complement => complement.BuildElement())
                .ToArray();
            Phrase.postMod = PostModifiers
                .Select(postModifier => postModifier.BuildElement())
                .ToArray();
            return Phrase;
        }

        #region Phrase features

        public bool AdjectiveOrderingSpecified
        {
            get => Phrase.ADJECTIVE_ORDERINGSpecified;
            set
            {
                Phrase.ADJECTIVE_ORDERINGSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool AdjectiveOrdering
        {
            get => Phrase.ADJECTIVE_ORDERING;
            set
            {
                Phrase.ADJECTIVE_ORDERING = value;
                Phrase.ADJECTIVE_ORDERINGSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool ElidedSpecified
        {
            get => Phrase.ELIDEDSpecified;
            set
            {
                Phrase.ELIDEDSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Elided
        {
            get => Phrase.ELIDED;
            set
            {
                Phrase.ELIDED = value;
                Phrase.ELIDEDSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool NumberSpecified
        {
            get => Phrase.NUMBERSpecified;
            set
            {
                Phrase.NUMBERSpecified = value;
                OnPropertyChanged();
            }
        }
        public numberAgreement Number
        {
            get => Phrase.NUMBER;
            set
            {
                Phrase.NUMBER = value;
                Phrase.NUMBERSpecified = true;
                OnPropertyChanged();
            }
        }

        public bool GenderSpecified
        {
            get => Phrase.GENDERSpecified;
            set
            {
                Phrase.GENDERSpecified = value;
                OnPropertyChanged();
            }
        }
        public gender Gender
        {
            get => Phrase.GENDER;
            set
            {
                Phrase.GENDER = value;
                Phrase.GENDERSpecified = true;
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

        public bool PossessiveSpecified
        {
            get => Phrase.POSSESSIVESpecified;
            set
            {
                Phrase.POSSESSIVESpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Possessive
        {
            get => Phrase.POSSESSIVE;
            set
            {
                Phrase.POSSESSIVE = value;
                Phrase.POSSESSIVESpecified = true;
                OnPropertyChanged();
            }
        }

        public bool PronominalSpecified
        {
            get => Phrase.PRONOMINALSpecified;
            set
            {
                Phrase.PRONOMINALSpecified = value;
                OnPropertyChanged();
            }
        }
        public bool Pronominal
        {
            get => Phrase.PRONOMINAL;
            set
            {
                Phrase.PRONOMINAL = value;
                Phrase.PRONOMINALSpecified = true;
                OnPropertyChanged();
            }
        }

        #endregion Phrase features
    }
}

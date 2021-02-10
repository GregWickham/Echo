using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG NPPhraseSpec</summary>
    public class NounPhraseBuilder : CoordinablePhraseBuilder<NPPhraseSpec>
    {
        public NounPhraseBuilder() : base() { }

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
                    AssignRoleFor(nb);
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
                case VerbPhraseBuilder vpb:
                    AddModifier(vpb);
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
                case CardinalNumberBuilder cnb:
                    AddModifier(cnb);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        private void AssignRoleFor(NounBuilder noun)
        {
            AddHead(noun);
            //if (noun.NumberSpecified) Number = noun.Number;
        }

        private void AssignRoleFor(PronounBuilder pronoun)
        {
            switch (pronoun.Case)
            {
                case PronounCase.Nominative:
                    AddHead(pronoun);
                    if (pronoun.PersonSpecified) Person = pronoun.Person;
                    if (pronoun.GenderSpecified) Gender = pronoun.Gender;
                    if (pronoun.NumberSpecified) Number = pronoun.Number;
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

        public override void Configure()
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

        private Dictionary<IElementTreeNode, int> GetSpecifiersAndIndices()
        {
            SortedList<IElementTreeNode, object> sortedChildren = new SortedList<IElementTreeNode, object>();
            Children.ToList().ForEach(child => sortedChildren.Add(child, null));
            Dictionary<IElementTreeNode, int> result = new Dictionary<IElementTreeNode, int>();
            Specifiers.ToList().ForEach(specifier => result.Add(specifier, sortedChildren.IndexOfKey(specifier)));
            return result;
        }

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

        /// <summary>Transform this NounPhraseBuilder into a CoordinatedPhraseBuilder and return that CoordinatedPhraseBuilder.</summary>
        /// <remarks>The noun phrase's Specifier, Modifiers, and Complements do not get incorporated into the CoordinatedPhraseElement.  Instead, each of those elements 
        /// must be applied to one of the coordinated elements.  This will change the syntactic structure of the tree, but while doing that we want to preserve the original 
        /// word order.  To accomplish that we first create a series of dictionaries that temporarily store the various elements of the original (non-coordinated) phrase,
        /// along with the original order of those elements. <para>Then we add those elements into the CoordinatedPhraseBuilder, using the dictionaries to preserve
        /// ordering.</para></remarks>
        private protected sealed override CoordinatedPhraseBuilder AsCoordinatedPhrase()
        {
            Dictionary<PartOfSpeechBuilder, int> partsOfSpeech = GetPartsOfSpeechAndIndices();
            Dictionary<IElementTreeNode, int> heads = GetHeadsAndIndices();
            Dictionary<IElementTreeNode, int> specifiers = GetSpecifiersAndIndices();
            Dictionary<IElementTreeNode, int> modifiers = GetModifiersAndIndices();
            Dictionary<IElementTreeNode, int> complements = GetComplementsAndIndices();
            CoordinatedPhraseBuilder result = base.AsCoordinatedPhrase();
            foreach (IElementTreeNode eachSpecifier in specifiers.Keys)
            {
                eachSpecifier.DetachFromParent();
                eachSpecifier
                    .Specify(ElementWithIndexNearest(specifiers[eachSpecifier], heads))
                    ?.SetChildOrdering(eachSpecifier, partsOfSpeech);
            };
            foreach (IElementTreeNode eachModifier in modifiers.Keys)
            {
                eachModifier.DetachFromParent();
                eachModifier
                    .Modify(ElementWithIndexNearest(modifiers[eachModifier], heads))
                    ?.SetChildOrdering(eachModifier, partsOfSpeech);
            };
            foreach (IElementTreeNode eachComplement in complements.Keys)
            {
                eachComplement.DetachFromParent();
                eachComplement
                    .Complete(ElementWithIndexNearest(complements[eachComplement], heads))
                    ?.SetChildOrdering(eachComplement, partsOfSpeech);
            };
            return result;
        }


        #endregion Configuration

        #region Editing

        private protected override HashSet<Type> ChildTypesThatCanBeAdded { get; } = new HashSet<Type>
        {
            typeof(NounBuilder),
            typeof(ConjunctionBuilder),
            typeof(DeterminerBuilder),
            typeof(AdjectiveBuilder),
            typeof(AdverbBuilder),
            typeof(PronounBuilder),
            typeof(VerbBuilder),
            typeof(NounPhraseBuilder),
            typeof(VerbPhraseBuilder),
            typeof(AdjectivePhraseBuilder),
            typeof(PrepositionalPhraseBuilder),
            typeof(SubordinateClauseBuilder),
            typeof(CardinalNumberBuilder)
        };

        #endregion Editing

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

        public override IElementTreeNode CopyLightweight() => new NounPhraseBuilder { Phrase = Phrase.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

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
    }
}

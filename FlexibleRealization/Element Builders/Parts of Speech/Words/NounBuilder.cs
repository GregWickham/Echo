using System;
using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization
{
    public class NounBuilder : WordElementBuilder, IPhraseHead
    {
        /// <summary>This constructor is using during parsing</summary>
        public NounBuilder(ParseToken token) : base(lexicalCategory.NOUN, token) 
        {
            switch (token.PartOfSpeech)
            {
                case "NN":
                    Number = numberAgreement.SINGULAR;
                    break;
                case "NNS":
                    Number = numberAgreement.PLURAL;
                    break;
                case "NNP":
                    Proper = true;
                    Number = numberAgreement.SINGULAR;
                    break;
                case "NNPS":
                    Proper = true;
                    Number = numberAgreement.PLURAL;
                    break;
            }
        }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private NounBuilder(string word) : base(lexicalCategory.NOUN, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public NounBuilder() : base(lexicalCategory.NOUN) { }

        /// <summary>Return true if this NounBuilder is a head of a NounPhraseBuilder</summary>
        public override bool IsPhraseHead => Parent is NounPhraseBuilder && AssignedRole == ParentElementBuilder.ChildRole.Head;

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsNounPhrase();

        /// <summary>If the parent of this is a NounPhraseBuilder return that parent, else return null</summary>
        internal NounPhraseBuilder ParentNounPhrase => Parent as NounPhraseBuilder;

        /// <summary>Return true if this NounBuilder is the object of a preposition</summary>
        internal bool IsObjectOfPreposition
        {
            get
            {
                PrepositionalPhraseBuilder containingPrepositionalPhrase = AncestorOfWhichThisIsDirectlyOrIndirectlyA(ParentElementBuilder.ChildRole.Complement) as PrepositionalPhraseBuilder;
                return containingPrepositionalPhrase != null;
            }
        }

        /// <summary>Search for a PrepositionBuilder of which this NounBuilder is an object</summary>
        /// <returns>The PrepositionBuilder if found, or null if not found</returns>
        internal PrepositionBuilder GoverningPreposition
        {
            get
            {
                PrepositionalPhraseBuilder containingPrepositionalPhrase = AncestorOfWhichThisIsDirectlyOrIndirectlyA(ParentElementBuilder.ChildRole.Complement) as PrepositionalPhraseBuilder;
                return containingPrepositionalPhrase?.UnaryHead as PrepositionBuilder;
            }
        }

        /// <summary>Return true if this NounBuilder is part of a compound noun.</summary>
        internal bool IsPartOfACompoundNoun => Parent is CompoundNounBuilder && AssignedRole == ParentElementBuilder.ChildRole.Component;

        /// <summary>Return true if this noun is allowed to form a compound noun with <paramref name="anotherNoun"/></summary>
        private bool CanBeCompoundedWith(NounBuilder anotherNoun) => !IsPartOfANominalModifier;

        /// <summary>Return true if this is part of a compound syntax formation with <paramref name="anotherElementBuilder"/></summary>
        internal override bool IsCompoundedWith(ElementBuilder anotherElementBuilder)
        {
            return IsDirectlyCompoundedWith(anotherElementBuilder)
                || IsPartOfNominalModifierWith(anotherElementBuilder);
        }

        /// <summary>If necessary, reconfigure the appropriate things so this and <paramref name="anotherNoun"/> become components of a compound word</summary>
        internal void FormCompoundWith(NounBuilder anotherNoun)
        {
            if (!IsCompoundedWith(anotherNoun) && CanBeCompoundedWith(anotherNoun))
            {
                if (anotherNoun.IsPartOfACompoundNoun)
                    MoveTo(anotherNoun.Parent, ParentElementBuilder.ChildRole.Component);
                else 
                    MoveTo(anotherNoun.AsCompoundNoun(), ParentElementBuilder.ChildRole.Component);
            }
        }

        /// <summary>Return true if this NounBuilder and <paramref name="anotherElementModifier"/> are part of a nominal modifier.</summary>
        private bool IsPartOfNominalModifierWith(ElementBuilder anotherElementModifier)
        {
            NominalModifierBuilder commonAncestorNominalModifier = LowestCommonAncestor<NominalModifierBuilder>(anotherElementModifier);
            if (commonAncestorNominalModifier == null) return false;
            else return ActsWithRoleInAncestor(ParentElementBuilder.ChildRole.Modifier, commonAncestorNominalModifier) 
                    && anotherElementModifier.ActsWithRoleInAncestor(ParentElementBuilder.ChildRole.Modifier, commonAncestorNominalModifier);
        }

        /// <summary>Transform this NounBuilder into a NounPhraseBuilder with this as its head</summary>
        internal NounPhraseBuilder AsNounPhrase()
        {
            NounPhraseBuilder result = new NounPhraseBuilder();
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        /// <summary>Transform this NounBuilder into a CompoundNounBuilder with this as a component</summary>
        internal CompoundNounBuilder AsCompoundNoun()
        {
            CompoundNounBuilder result = new CompoundNounBuilder();
            Parent?.ReplaceChild(this, result);
            result.AddComponent(this);
            return result;
        }

        #region Features

        private numberAgreement _number;
        public numberAgreement Number
        {
            get => _number;
            set
            {
                _number = value;
                NumberSpecified = true;
            }
        }
        public bool NumberSpecified { get; set; } = false;

        #endregion Features

        #region Editing

        private protected override HashSet<Type> TypesThatCanBeAdded { get; } = new HashSet<Type>
        {
            typeof(AdjectiveBuilder)
        };

        /// <summary>Add <paramref name="node"/> to the tree in which this exists</summary>
        public override IParent Add(IElementTreeNode node)
        {
            IParent modifiedParent;
            switch (node)
            {
                case AdjectiveBuilder advb:
                    modifiedParent = this.AsNounPhrase();
                    modifiedParent.AddChild(advb);
                    return modifiedParent;
                default: return null;
            }
        }

        #endregion Editing

        public override IElementTreeNode CopyLightweight() => new NounBuilder(WordSource.GetWord());
    }
}

using System;
using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization
{
    public class VerbBuilder : WordElementBuilder, IPhraseHead
    {
        /// <summary>This constructor is using during parsing</summary>
        public VerbBuilder(ParseToken token) : base(lexicalCategory.VERB, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private VerbBuilder(string word) : base(lexicalCategory.VERB, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public VerbBuilder() : base(lexicalCategory.VERB) { }

        internal bool IsGerundOrPresentParticiple => Token.PartOfSpeech == "VBG";

        /// <summary>Return true if this VerbBuilder is a head of a VerbPhraseBuilder</summary>
        public override bool IsPhraseHead => Parent is VerbPhraseBuilder && AssignedRole == ParentElementBuilder.ChildRole.Head;

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsVerbPhrase();

        /// <summary>If the parent of this is a VerbPhraseBuilder return that parent, else return null</summary>
        internal VerbPhraseBuilder ParentVerbPhrase => Parent as VerbPhraseBuilder;

        /// <summary>Transform this VerbBuilder into a VerbPhraseBuilder with this as its head</summary>
        internal VerbPhraseBuilder AsVerbPhrase()
        {
            VerbPhraseBuilder result = new VerbPhraseBuilder(this);
            Parent?.ReplaceChild(this, result);
            //result.AddHead(this);
            return result;
        }

        /// <summary>Transform this VerbBuilder into a VerbPhraseBuilder with form <paramref name="phraseForm"/> and this as its head</summary>
        internal VerbPhraseBuilder AsVerbPhrase(form phraseForm)
        {
            VerbPhraseBuilder result = new VerbPhraseBuilder() { Form = phraseForm };
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        /// <summary>Transform this VerbBuilder into a VerbPhraseBuilder with tense <paramref name="phraseTense"/> and this as its head</summary>
        internal VerbPhraseBuilder AsVerbPhrase(tense phraseTense)
        {
            VerbPhraseBuilder result = new VerbPhraseBuilder() { Tense = phraseTense };
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        #region Editing

        private protected override HashSet<Type> TypesThatCanBeAdded { get; } = new HashSet<Type>
        {
            typeof(AdverbBuilder)
        };

        /// <summary>Add <paramref name="node"/> to the tree in which this exists</summary>
        public override IParent Add(IElementTreeNode node)
        {
            IParent modifiedParent;
            switch (node)
            {
                case AdverbBuilder advb:
                    modifiedParent = IsPhraseHead
                        ? Parent
                        : this.AsVerbPhrase();
                    modifiedParent.AddChild(advb);
                    return modifiedParent;
                default: return null;
            }
        }

        #endregion Editing

        public override IElementTreeNode CopyLightweight() => new VerbBuilder(WordSource.GetWord());
    }
}

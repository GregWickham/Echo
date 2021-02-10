using System;
using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization
{
    public class AdverbBuilder : WordElementBuilder, IPhraseHead
    {
        /// <summary>This constructor is using during parsing</summary>
        public AdverbBuilder(ParseToken token) : base(lexicalCategory.ADVERB, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private protected AdverbBuilder(string word) : base(lexicalCategory.ADVERB, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph.</summary>
        public AdverbBuilder() : base(lexicalCategory.ADVERB) { }

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsAdverbPhrase();

        internal bool Comparative => Token.PartOfSpeech.Equals("RBR");
        internal bool Superlative => Token.PartOfSpeech.Equals("RBS");

        /// <summary>Transform this AdverbBuilder into an AdverbPhraseBuilder with this as its head.</summary>
        internal AdverbPhraseBuilder AsAdverbPhrase()
        {
            AdverbPhraseBuilder result = new AdverbPhraseBuilder();
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        #region Editing

        private protected override HashSet<Type> TypesThatCanBeAdded { get; } = new HashSet<Type>
        {
            typeof(AdverbBuilder)
        };

        /// <summary>Add <paramref name="node"/> to the tree in which this exists.</summary>
        public override IParent Add(IElementTreeNode node)
        {
            IParent modifiedParent;
            switch (node)
            {
                case AdverbBuilder advb:
                    modifiedParent = this.AsAdverbPhrase();
                    modifiedParent.AddChild(advb);
                    return modifiedParent;
                default: return null;
            }
        }

        #endregion Editing

        public override IElementTreeNode CopyLightweight() => new AdverbBuilder(WordSource.GetWord());
    }
}

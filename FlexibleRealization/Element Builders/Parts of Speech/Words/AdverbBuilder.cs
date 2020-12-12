using SimpleNLG;

namespace FlexibleRealization
{
    public class AdverbBuilder : WordElementBuilder, IPhraseHead
    {
        /// <summary>This constructor is using during parsing</summary>
        public AdverbBuilder(ParseToken token) : base(lexicalCategory.ADVERB, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private protected AdverbBuilder(ParseToken token, string word) : base(lexicalCategory.ADVERB, token, word) { }

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsAdverbPhrase();

        internal bool Comparative => Token.PartOfSpeech.Equals("RBR");
        internal bool Superlative => Token.PartOfSpeech.Equals("RBS");

        /// <summary>Transform this <see cref="AdverbBuilder"/> into an <see cref="AdverbPhraseBuilder"/> with this as its head</summary>
        internal AdverbPhraseBuilder AsAdverbPhrase()
        {
            AdverbPhraseBuilder result = new AdverbPhraseBuilder();
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        public override IElementTreeNode CopyLightweight() => new AdverbBuilder(Token.Copy(), WordSource.GetWord());
    }
}

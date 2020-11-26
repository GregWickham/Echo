using SimpleNLG;

namespace FlexibleRealization
{
    public class AdverbBuilder : WordElementBuilder, IPhraseHead
    {
        public AdverbBuilder(ParseToken token) : base(lexicalCategory.ADVERB, token) { }

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
    }
}

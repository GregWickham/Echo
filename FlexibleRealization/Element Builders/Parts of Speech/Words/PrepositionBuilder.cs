using SimpleNLG;

namespace FlexibleRealization
{
    public class PrepositionBuilder : WordElementBuilder, IPhraseHead
    {
        /// <summary>This constructor is using during parsing</summary>
        public PrepositionBuilder(ParseToken token) : base(lexicalCategory.PREPOSITION, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private PrepositionBuilder(ParseToken token, string word) : base(lexicalCategory.PREPOSITION, token, word) { }

        /// <summary>Return true if this PrepositionBuilder is a head of a PrepositionalPhraseBuilder</summary>
        public override bool IsPhraseHead => Parent is PrepositionalPhraseBuilder && AssignedRole == ParentElementBuilder.ChildRole.Head;

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsPrepositionalPhrase();

        /// <summary>If the parent of this is a PrepositionalPhraseBuilder return that parent, else return null</summary>
        internal PrepositionalPhraseBuilder ParentPrepositionalPhrase => Parent as PrepositionalPhraseBuilder;

        /// <summary>Transform this PrepositionBuilder into a PrepositionalPhraseBuilder with this as its head</summary>
        internal PrepositionalPhraseBuilder AsPrepositionalPhrase()
        {
            PrepositionalPhraseBuilder result = new PrepositionalPhraseBuilder();
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        public override IElementTreeNode CopyLightweight() => new PrepositionBuilder(Token.Copy(), WordSource.GetWord());
    }
}

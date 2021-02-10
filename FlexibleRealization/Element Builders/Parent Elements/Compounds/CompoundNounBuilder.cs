using SimpleNLG;

namespace FlexibleRealization
{
    public class CompoundNounBuilder : CompoundBuilder, IPhraseHead
    {
        public CompoundNounBuilder() : base(lexicalCategory.NOUN) { }

        /// <summary>Implementation of IPhraseHead : AsPhrase()</summary>
        public override PhraseBuilder AsPhrase() => AsNounPhrase();

        /// <summary>Transform this CompoundNounBuilder into a NounPhraseBuilder with this as its head</summary>
        internal NounPhraseBuilder AsNounPhrase()
        {
            NounPhraseBuilder result = new NounPhraseBuilder();
            Parent.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        public override IElementTreeNode CopyLightweight() => new CompoundNounBuilder().LightweightCopyChildrenFrom(this);
    }
}

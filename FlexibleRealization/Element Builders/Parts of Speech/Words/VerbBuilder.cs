using SimpleNLG;

namespace FlexibleRealization
{
    public class VerbBuilder : WordElementBuilder, IPhraseHead
    {
        public VerbBuilder(ParseToken token) : base(lexicalCategory.VERB, token) { }

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
            VerbPhraseBuilder result = new VerbPhraseBuilder();
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
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

        /// <summary>Transform this VerbBuilder into a VerbPhraseBuilder with form <paramref name="phraseForm"/> and this as its head</summary>
        internal VerbPhraseBuilder AsVerbPhrase(tense phraseTense)
        {
            VerbPhraseBuilder result = new VerbPhraseBuilder() { Tense = phraseTense };
            Parent?.ReplaceChild(this, result);
            result.AddHead(this);
            return result;
        }

        public override IElementTreeNode CopyLightweight() => new VerbBuilder(Token.Copy());
    }
}

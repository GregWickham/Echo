using SimpleNLG;

namespace FlexibleRealization
{
    public class ModalBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public ModalBuilder(ParseToken token) : base(lexicalCategory.MODAL, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private ModalBuilder(int index, string word) : base(lexicalCategory.MODAL, index, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public ModalBuilder() : base(lexicalCategory.MODAL) { }

        public override IElementTreeNode CopyLightweight() => new ModalBuilder(Index, WordSource.GetWord());
    }
}

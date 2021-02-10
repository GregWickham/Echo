using SimpleNLG;

namespace FlexibleRealization
{
    public class WhPronounBuilder : PronounBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WhPronounBuilder(ParseToken token) : base(token) 
        {
            switch (token.PartOfSpeech)
            {
                case "WP$":
                    Case = PronounCase.Possessive;
                    break;
            }
        }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private WhPronounBuilder(string word) : base(word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public WhPronounBuilder() : base() { }

        public override IElementTreeNode CopyLightweight() => new WhPronounBuilder(WordSource.GetWord());
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleNLG;

namespace FlexibleRealization
{
    [DebuggerDisplay("{WordSource.GetWord()}")]
    public abstract class WordElementBuilder : PartOfSpeechBuilder
    {
        public WordElementBuilder(lexicalCategory category, ParseToken token) : base(token)
        {
            Word.PartOfSpeech = category;
            WordSource = new SingleWordSource(category switch
            {
                lexicalCategory.VERB => token.Lemma,
                _ => token.Word
            });
        }

        private WordElement Word = new WordElement();

        public IWordSource WordSource { get; set; }


        #region Word properties

        public lexicalCategory LexicalCategory
        {
            get => Word.cat;
        }

        public bool IsProper
        {
            set
            {
                Word.PROPER = value;
                Word.PROPERSpecified = true;
            }
        }

        public inflection Inflection
        {
            set 
            {
                Word.var = value;
                Word.varSpecified = true;
            }
        }

        public bool IsCanned
        {
            set
            {
                Word.canned = value;
                Word.cannedSpecified = true;
            }
        }

        #endregion Word properties

        public override NLGElement BuildElement() => BuildWord();

        public WordElement BuildWord()
        {
            Word.Base = WordSource.GetWord();
            return Word;
        }

        public override IEnumerable<IElementTreeNode> GetRealizableVariations() => new Variations(this);

        public class Variations : IEnumerable<WordElementBuilder>
        {
            internal Variations(WordElementBuilder word) => Builder = word;

            private WordElementBuilder Builder;

            public IEnumerator<WordElementBuilder> GetEnumerator() => new Flexor(Builder);
            IEnumerator IEnumerable.GetEnumerator() => new Flexor(Builder);
        }

        public class Flexor : IEnumerator<WordElementBuilder>
        {
            internal Flexor(WordElementBuilder word) => Builder = word;

            private WordElementBuilder Builder;

            public WordElementBuilder Current => Builder;
            object IEnumerator.Current => Builder;

            public void Dispose() { }

            public bool MoveNext() => Builder.WordSource.MoveNext();

            public void Reset() => Builder.WordSource.Reset();
        }
    }
}

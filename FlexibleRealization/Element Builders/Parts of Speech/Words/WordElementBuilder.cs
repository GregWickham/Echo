using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    [DebuggerDisplay("{WordSource.GetWord()}")]
    public abstract class WordElementBuilder : PartOfSpeechBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public WordElementBuilder(lexicalCategory category, ParseToken token) : base(token)
        {
            Word.PartOfSpeech = category;
            WordSource = new SingleWordSource(category switch
            {
                lexicalCategory.VERB => token.Lemma,
                _ => token.Word
            });
        }

        /// <summary>This constructor is used during LightweightCopy().  WordSource is constrained to a specific word.</summary>
        private protected WordElementBuilder(lexicalCategory category, ParseToken token, string word) : base(token)
        {
            Word.PartOfSpeech = category;
            WordSource = new SingleWordSource(word);
        }

        /// <summary>The WordElement that this will build</summary>
        private WordElement Word = new WordElement();

        /// <summary>A IWordSource that will supply the word we use during building</summary>
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
            Word.Base = SelectWord();
            return Word;
        }

        /// <summary>Return the word to be used during element building</summary>
        internal virtual string SelectWord() => WordSource.GetWord();

        ///Return an IEnumerator for the variations of this
        public override IEnumerator<IElementTreeNode> GetVariationsEnumerator() => new Variations.Enumerator(this);

        /// <summary>Return the realizable variations of this</summary>
        public override IEnumerable<IElementTreeNode> GetRealizableVariations() => new Variations(this).Select(variation => variation.AsRealizableTree());

        public class Variations : IEnumerable<IElementTreeNode>
        {
            internal Variations(WordElementBuilder word) => Builder = word;

            private WordElementBuilder Builder;

            public IEnumerator<IElementTreeNode> GetEnumerator() => new Enumerator(Builder);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Builder);

            public class Enumerator : IEnumerator<IElementTreeNode>
            {
                internal Enumerator(WordElementBuilder word)
                {
                    Builder = word;
                    WordVariations = Builder.WordSource.GetVariationsEnumerator();
                }

                private WordElementBuilder Builder;

                private IEnumerator<string> WordVariations;

                public IElementTreeNode Current => Builder;
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (WordVariations.MoveNext()) return true;
                    else
                    {
                        Reset();
                        return false;
                    }
                }

                public void Reset() => WordVariations.Reset();
            }
        }
    }
}

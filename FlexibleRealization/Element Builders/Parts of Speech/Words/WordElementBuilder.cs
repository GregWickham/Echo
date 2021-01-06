using System;
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
        private protected WordElementBuilder(lexicalCategory category, int index, string word) : base(index)
        {
            Word.PartOfSpeech = category;
            WordSource = new SingleWordSource(word);
        }

        /// <summary>This constructor is used by the UI when the user is changing a part-of-speech to a different part-of-speech. WordSource will be added by the UI later in the process.</summary>
        private protected WordElementBuilder(lexicalCategory category) => Word.PartOfSpeech = category;

        /// <summary>The WordElement that this will build</summary>
        private WordElement Word = new WordElement();

        /// <summary>An IWordSource that will supply the word we use during building</summary>
        public IWordSource WordSource { get; set; }

        /// <summary>Return true if this WordElementBuilder knows how to "add" <paramref name="node"/> to the tree in which it exists</summary>
        // TODO: Make this method abstract by overriding in all subclasses
        public virtual bool CanAdd(IElementTreeNode node) => false;

        /// <summary>Add <paramref name="node"/> to the tree in which this exists</summary>
        // TODO: Make this method abstract by overriding in all subclasses
        public virtual bool Add(IElementTreeNode node) => false;

        internal void InsertBefore(ElementBuilder insertPoint)
        {
            Root.InsertBefore(insertPoint, this);
        }

        /// <summary>Replace this with <paramref name="replacement"/> in the ElementBuilder tree</summary>
        /// <remarks>The constructor just created a bare-bones version of the <paramref name="replacement"/>, so we need to fill in whatever content the <paramref name="replacement"/> will need.</remarks>
        public void ReplaceWith(WordElementBuilder replacement)
        {
            replacement.Index = Index;
            replacement.WordSource = WordSource;
            Become(replacement);
            replacement.OnTreeStructureChanged();
        }

        /// <summary>Return a new WordElementBuilder of the lexical category specified by <paramref name="category"/></summary>
        public static WordElementBuilder OfCategory(lexicalCategory category) => category switch
        {
            lexicalCategory.ADJECTIVE => new AdjectiveBuilder(),
            lexicalCategory.ADVERB => new AdverbBuilder(),
            lexicalCategory.CONJUNCTION => new ConjunctionBuilder(),
            lexicalCategory.DETERMINER => new DeterminerBuilder(),
            lexicalCategory.MODAL => new ModalBuilder(),
            lexicalCategory.NOUN => new NounBuilder(),
            lexicalCategory.PREPOSITION => new PrepositionBuilder(),
            lexicalCategory.PRONOUN => new PronounBuilder(),
            lexicalCategory.VERB => new VerbBuilder(),

            _ => throw new InvalidOperationException($"Can't make a WordElementBuilder for category {category}")
        };

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
                    WordVariations = Builder.WordSource.GetStringVariationsEnumerator();
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

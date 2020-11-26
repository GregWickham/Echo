using System;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleNLG;

namespace FlexibleRealization
{
    [DebuggerDisplay("{Word.Base}")]
    public abstract class WordElementBuilder : PartOfSpeechBuilder
    {
        public WordElementBuilder(lexicalCategory category, ParseToken token) : base(token)
        {
            Word.cat = category;
            Word.catSpecified = true;
            Word.@base = category switch
            {   
                lexicalCategory.VERB => token.Lemma,
                _ => token.Word
            };
        }

        private WordElement Word = new WordElement();

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

        public WordElement BuildWord() => Word;
    }
}

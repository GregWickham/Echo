using System.Collections;
using System.Collections.Generic;

namespace FlexibleRealization
{
    public class SingleWordSource : IWordSource
    {
        internal SingleWordSource(string anchor) { Word = anchor; }

        public object Current => Word;

        private string Word { get; set; }

        public string GetWord() => Word;

        public string DefaultWord => Word;

        IEnumerator<string> IWordSource.GetStringVariationsEnumerator() => new Variations.Enumerator(this);

        public class Variations : IEnumerable<string>
        {
            internal Variations(SingleWordSource source) => Source = source;

            private SingleWordSource Source;

            public IEnumerator<string> GetEnumerator() => new Enumerator(Source);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Source);


            public class Enumerator : IEnumerator<string>
            {
                internal Enumerator(SingleWordSource source)
                {
                    Source = source;
                    Reset();
                }

                private SingleWordSource Source;

                private bool HasBeenAdvanced = false;

                public string Current => Source.GetWord();
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext()
                {
                    if (HasBeenAdvanced) return false;
                    else
                    {
                        HasBeenAdvanced = true;
                        return true;
                    }
                }

                public void Reset() => HasBeenAdvanced = false;
            }
        }
    }
}

namespace FlexibleRealization
{
    public class SingleWordSource : IWordSource
    {
        internal SingleWordSource(string anchor) { Word = anchor; }

        public object Current => Word;

        private string Word { get; set; }

        public string GetWord() => Word;

        public bool MoveNext() => false;

        public void Reset() { }
    }
}

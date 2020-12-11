namespace FlexibleRealization
{
    public class SingleWordSource : IWordSource
    {
        internal SingleWordSource(string anchor) { Word = anchor; }

        private string Word { get; set; }

        public string GetWord() => Word;
    }
}

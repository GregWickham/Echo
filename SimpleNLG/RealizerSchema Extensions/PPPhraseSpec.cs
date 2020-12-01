namespace SimpleNLG
{
    public partial class PPPhraseSpec
    {
        public PPPhraseSpec() => Category = phraseCategory.PREPOSITIONAL_PHRASE;

        public PPPhraseSpec Copy() => (PPPhraseSpec)MemberwiseClone();

        public PPPhraseSpec CopyWithoutSpec()
        {
            PPPhraseSpec result = Copy();
            result.NullOutSpecElements();
            return result;
        }

    }
}

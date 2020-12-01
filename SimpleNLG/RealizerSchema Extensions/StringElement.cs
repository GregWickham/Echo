namespace SimpleNLG
{
    public partial class StringElement
    {
        public StringElement Copy() => (StringElement)MemberwiseClone();

        public StringElement CopyWithoutSpec()
        {
            StringElement result = Copy();
            result.val = null;
            return result;
        }
    }
}

namespace FlexibleRealization
{
    public class WhNounPhraseBuilder : WhWordPhraseBuilder
    {
        public WhNounPhraseBuilder() : base() { }

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case WhDeterminerBuilder wdb:
                    HeadWord = wdb;
                    break;
                case WhPronounBuilder wpb:
                    HeadWord = wpb;
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

    }
}

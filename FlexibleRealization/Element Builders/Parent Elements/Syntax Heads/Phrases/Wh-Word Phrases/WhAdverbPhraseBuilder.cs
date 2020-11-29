using System;

namespace FlexibleRealization
{
    public class WhAdverbPhraseBuilder : WhWordPhraseBuilder
    {
        public WhAdverbPhraseBuilder() : base() { }

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case WhAdverbBuilder wab:
                    HeadWord = wab;
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

    }
}

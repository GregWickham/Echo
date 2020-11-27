﻿using System;

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
                default: throw new InvalidOperationException("Wh-adverb phrase can't find a role for this element");
            }
        }

    }
}
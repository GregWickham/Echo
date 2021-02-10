using System;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>An object that can build the head word of a phrase</summary>
    internal interface IPhraseHead : IElementBuilder, IElementTreeNode
    {
        WordElement BuildWord();
    }
}

using System.Collections.Generic;

namespace FlexibleRealization
{
    /// <summary>An object that spans a range of part-of-speech indices</summary>
    public interface IIndexRange
    {
        bool ComesBefore(IIndexRange theOtherElement);

        bool ComesAfter(IIndexRange theOtherElement);

        int MinTokenIndex { get; }

        int MaxTokenIndex { get; }

        int DistanceFrom(IIndexRange anotherElement);

        IElementTreeNode NearestOf(IEnumerable<IElementTreeNode> elements);
    }
}

using System.Collections.Generic;

namespace FlexibleRealization
{
    /// <summary>Delegate for an operation that can be applied to an IElementTreeNode</summary>
    /// <param name="target">The IElementTreeNode to which the operation will be applied</param>
    /// <returns>The IElementTreeNode that results from applying the operation</returns>
    public delegate IElementTreeNode ElementTreeNodeOperation(IElementTreeNode target);


    /// <summary>A node in a tree of elements</summary>
    public interface IElementTreeNode : IIndexRange, IElementBuilder, ISyntaxComponent
    {
        ParentElementBuilder Parent { get; set; }

        int Depth { get; }

        ParentElementBuilder Root { get; }

        bool IsRoot { get; }

        List<IElementTreeNode> Ancestors { get; }

        void DetachFromParent();

        bool IsChildOf(ParentElementBuilder prospectiveParent);

        IEnumerable<IElementTreeNode> WithAllDescendentBuilders { get; }

        IEnumerable<PartOfSpeechBuilder> PartsOfSpeechInSubtree { get; }

        IElementTreeNode CopyLightweight();

        IElementTreeNode AttachDependencies(List<(string Relation, string Specifier, int GovernorIndex, int DependentIndex)> dependencies);

        IElementTreeNode ApplyDependencies();

        IElementTreeNode Propagate(ElementTreeNodeOperation operateOn);

        IElementTreeNode Configure();

        IElementTreeNode Coordinate();

        IElementTreeNode Consolidate();
    }
}

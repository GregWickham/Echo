using System.Collections.Generic;
using FlexibleRealization.Dependencies;

namespace FlexibleRealization
{
    /// <summary>Delegate for an operation that can be applied to an IElementTreeNode</summary>
    /// <param name="target">The IElementTreeNode to which the operation will be applied</param>
    /// <returns>The IElementTreeNode that results from applying the operation</returns>
    public delegate void ElementTreeNodeOperation(IElementTreeNode target);


    /// <summary>A node in a tree of elements</summary>
    public interface IElementTreeNode : IIndexRange, IElementBuilder, ISyntaxComponent
    {
        IParent Parent { get; set; }

        int Depth { get; }

        RootNode Root { get; }

        List<IElementTreeNode> Ancestors { get; }

        IElementTreeNode DetachFromParent();

        bool IsChildOf(ParentElementBuilder prospectiveParent);

        IEnumerable<IElementTreeNode> WithAllDescendentBuilders { get; }

        IEnumerable<PartOfSpeechBuilder> PartsOfSpeechInSubtree { get; }

        PartOfSpeechBuilder PartOfSpeechInSubtreeWithIndex(int index);

        IEnumerable<SyntacticRelation> SyntacticRelationsWithAtLeastOneEndpointInSubtree { get; }

        void MoveTo(IParent newParent);

        IElementTreeNode AsRealizableTree();

        IElementTreeNode CopyLightweight();

        void Propagate(ElementTreeNodeOperation operateOn);

        void Configure();

        void Coordinate();

        void Consolidate();
    }
}

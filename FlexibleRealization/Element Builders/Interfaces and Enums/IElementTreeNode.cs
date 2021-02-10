using System;
using System.Collections.Generic;
using FlexibleRealization.Dependencies;

namespace FlexibleRealization
{
    /// <summary>Delegate for an operation that can be applied to an IElementTreeNode</summary>
    /// <param name="target">The IElementTreeNode to which the operation will be applied</param>
    /// <returns>The IElementTreeNode that results from applying the operation</returns>
    public delegate void ElementTreeNodeOperation(IElementTreeNode target);


    /// <summary>A node in a tree of elements</summary>
    public interface IElementTreeNode : IElementBuilder, ISyntaxComponent, IComparable<IElementTreeNode>
    {
        int FlexDB_ID { get; }

        IParent Parent { get; set; }

        int Depth { get; }

        RootNode Root { get; }

        IElementTreeNode Stem { get; }

        List<IElementTreeNode> Ancestors { get; }

        IElementTreeNode DetachFromParent();

        bool IsChildOf(ParentElementBuilder prospectiveParent);

        IEnumerable<IElementTreeNode> WithAllDescendentBuilders { get; }

        IEnumerable<TElement> GetElementsOfTypeInSubtree<TElement>() where TElement : ElementBuilder;

        bool IsInSubtreeOf(IElementTreeNode node);

        TElementBuilder LowestAncestorOfType<TElementBuilder>() where TElementBuilder : ElementBuilder;

        IEnumerable<SyntacticRelation> SyntacticRelationsWithAtLeastOneEndpointInSubtree { get; }

        int MinimumIndex { get; }

        int MaximumIndex { get; }

        bool ComesBefore(IElementTreeNode theOtherElement);

        bool ComesAfter(IElementTreeNode theOtherElement);

        void SetOrderingRelativeTo(IElementTreeNode insertPoint, NodeRelation relation);

        int RelativeIndexOf(PartOfSpeechBuilder partOfSpeech);

        bool MoveTo(IParent newParent);

        bool MoveTo(IParent newParent, ParentElementBuilder.ChildRole role);

        void Remove();

        RealizationResult Realize();

        IElementTreeNode AsRealizableTree();

        IElementTreeNode CopyLightweight();

        IEnumerator<IElementTreeNode> GetVariationsEnumerator();

        IEnumerable<IElementTreeNode> GetRealizableVariations();

        void Propagate(ElementTreeNodeOperation operateOn);

        void Configure();

        void Coordinate();

        void Consolidate();

        void CreateChildOrderingsFromIndices();
    }
}

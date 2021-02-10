using System.Collections.Generic;

namespace FlexibleRealization
{
    /// <summary>An object that can be the parent of an <see cref="IElementTreeNode"/></summary>
    public interface IParent
    {
        int Depth { get; }

        RootNode Root { get; }

        IElementTreeNode Stem { get; }

        List<ParentElementBuilder.ChildRole> ValidRolesForChild(ElementBuilder child);

        void AddChild(IElementTreeNode child);

        void AddChildWithRole(IElementTreeNode child, ParentElementBuilder.ChildRole role);

        ParentElementBuilder.ChildRole RoleFor(IElementTreeNode child);

        void SetRoleOfChild(IElementTreeNode child, ParentElementBuilder.ChildRole newRole);

        void SetChildOrdering(IElementTreeNode childToOrder, IElementTreeNode childToOrderRelativeTo, NodeRelation relation);

        void SetChildOrdering(IElementTreeNode childToOrder, Dictionary<PartOfSpeechBuilder, int> partsOfSpeech);

        void RemoveChild(IElementTreeNode child);

        void ReplaceChild(IElementTreeNode existingChild, IElementTreeNode newChild);

        bool MoveTo(IParent newParent, ParentElementBuilder.ChildRole role);

        bool ActsAsHeadOf(PhraseBuilder phrase);

        bool ActsWithRoleInAncestor(ParentElementBuilder.ChildRole role, ParentElementBuilder ancestor);

        IParent AncestorOfWhichThisIsDirectlyOrIndirectlyA(ParentElementBuilder.ChildRole role);
    }
}

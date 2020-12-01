using System.Linq;
using System.Collections.Generic;

namespace FlexibleRealization
{
    /// <summary>The base class of all ParentElementBuilders</summary>
    public abstract class ParentElementBuilder : ElementBuilder
    {
        public virtual bool CanAddChild(ElementBuilder potentialChild) => true;

        /// <summary>This method is used during initial construction of an ElementBuilder tree from a constituency parse.  It can also be used during the Configuration
        /// process when a ParentElementBuilder needs another chance to define the proper role for a child.</summary>
        public void AddChild(IElementTreeNode newChild) => AssignRoleFor(newChild);

        /// <summary>AssignRoleFor must be overridden by all subclasses, to define the roles that are appropriate to various child types relative to each parent type</summary>
        private protected abstract void AssignRoleFor(IElementTreeNode child);

        /// <summary>Add <paramref name="child"/> as a child of this, with ChildRole <paramref name="role"/></summary>
        internal void AddChildWithRole(IElementTreeNode child, ChildRole role)
        {
            ChildrenAndRoles.Add(child, role);
            child.Parent = this;
        }

        /// <summary>Add all the ElementBuilders in <paramref name="children"/> as children of this, with ChildRole <paramref name="role"/></summary>
        private protected void AddChildrenWithRole(IEnumerable<IElementTreeNode> children, ChildRole role)
        {
            foreach (ElementBuilder eachChildToAdd in children)
            {
                AddChildWithRole(eachChildToAdd, role);
            }
        }

        /// <summary>Add <paramref name="newChild"/> as a child of this, with ChildRole Unassigned</summary>
        private protected void AddUnassignedChild(IElementTreeNode newChild) => AddChildWithRole(newChild, ChildRole.Unassigned);

        #region Tree structure

        /// <summary>The possible roles that a child IElementBuilder can have relative to a ParentElementBuilder</summary>
        public enum ChildRole
        {
            NoParent,       // the element is the root of its graph
            Unassigned,
            Subject,        // of a clause
            Predicate,      // of a clause
            Head,
            Modifier,
            Complement,
            Specifier,      // of a noun phrase
            Modal,
            Coordinator,    // of a coordinated phrase 
            Coordinated,
            Complementizer, // of a subordinate clause
            Component       // of a compound word
        }

        /// <summary>The central collection holding all the ElementBuilder children of a ParentElementBuilder and the roles of those children</summary>
        /// <remarks>Many properties and methods operate upon this collection</remarks>
        private Dictionary<IElementTreeNode, ChildRole> ChildrenAndRoles = new Dictionary<IElementTreeNode, ChildRole>();

        /// <summary>Return the immediate children of this</summary>
        public override IEnumerable<IElementTreeNode> Children => ChildrenAndRoles.Select(kvp => kvp.Key);

        /// <summary>Return the ChildRole assigned for <paramref name="child"/></summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public ChildRole RoleFor(IElementTreeNode child) => ChildrenAndRoles[child];

        /// <summary>Return the immediate children of this having the supplied <paramref name="role"/></summary>
        private protected IEnumerable<IElementTreeNode> ChildrenWithRole(ChildRole role) => ChildrenAndRoles
            .Where(kvp => kvp.Value == role)
            .Select(kvp => kvp.Key);

        /// <summary>Return the children of this that have ChildRole Unassigned</summary>
        private protected IEnumerable<IElementTreeNode> UnassignedChildren => Children
            .Where(child => RoleFor(child) == ChildRole.Unassigned);

        /// <summary>Return all the descendants of this, (does not include <see cref="ParseToken"/>s)</summary>
        public override IEnumerable<IElementTreeNode> DescendentBuilders
        {
            get
            {
                List<IElementTreeNode> result = new List<IElementTreeNode>();
                AddDescendantsTo(result);
                return result;
            }
        }

        /// <summary>Return all the descendants of this, plus this (does not include <see cref="ParseToken"/>s)</summary>
        public override IEnumerable<IElementTreeNode> WithAllDescendentBuilders
        {
            get
            {
                List<IElementTreeNode> result = new List<IElementTreeNode> { this };
                AddDescendantsTo(result);
                return result;
            }
        }

        /// <summary>Add all descendants of this to <paramref name="list"/></summary>
        private protected void AddDescendantsTo(List<IElementTreeNode> list)
        {
            foreach (IElementTreeNode child in Children) list.AddRange(child.WithAllDescendentBuilders);
        }

        /// <summary>Return all the PartOfSpeechBuilders descended from this that have indexes between <paramref name="start"/> and <paramref name="end"/>, non-inclusive</summary>
        internal IEnumerable<PartOfSpeechBuilder> PartsOfSpeechInSubtreeBetween(PartOfSpeechBuilder start, PartOfSpeechBuilder end) => PartsOfSpeechInSubtree
            .Where(posb => posb.Token.Index > start.Token.Index && posb.Token.Index < end.Token.Index);

        #endregion Tree structure

        #region Configuration

        /// <summary>Override of Configure for ParentElementBuilders.  If a subclass overrides this implementation, it should call this base form after its own custom manipulations.</summary>
        public override IElementTreeNode Configure()
        {
            foreach (ElementBuilder eachUnassignedChild in UnassignedChildren.ToList())
            {
                RemoveChild(eachUnassignedChild);
                AssignRoleFor(eachUnassignedChild);
            }
            return this;
        }

        /// <summary>Default override of Consolidate for ParentElementBuilders</summary>
        public override IElementTreeNode Consolidate() => Children.Count() switch
        {
            0 => Become(null),
            1 => Become(Children.First()),
            _ => this
        };

        /// <summary>Change the ChildRole of <paramref name="child"/> to <paramref name="newRole"/></summary>
        internal void ChangeRoleOfChild(IElementTreeNode child, ChildRole newRole)
        {
            RemoveChild(child);
            AddChildWithRole(child, newRole);
        }

        /// <summary>Find all children with assigned role <paramref name="originalRole"/>, and change their role to <paramref name="newRole"/></summary>
        private protected void ChangeChildRoles(ChildRole originalRole, ChildRole newRole)
        {
            IEnumerable<IElementTreeNode> childrenToChange = ChildrenAndRoles
                .Where(kvp => kvp.Value == originalRole)
                .Select(kvp => kvp.Key)
                .ToList();
            foreach (IElementTreeNode childToChange in childrenToChange)
            {
                ChildrenAndRoles[childToChange] = newRole;
            }
        }

        /// <summary>Sever the parent-child link between this and <paramref name="childToRemove"/></summary>
        internal void RemoveChild(IElementTreeNode childToRemove)
        {
            ChildrenAndRoles.Remove(childToRemove);
            childToRemove.Parent = null;
        }

        /// <summary>Replace <paramref name="existingChild"/> with <paramref name="newChild"/> using the same role.</summary>
        internal void ReplaceChild(IElementTreeNode existingChild, IElementTreeNode newChild)
        {
            ChildRole role = RoleFor(existingChild);
            RemoveChild(existingChild);
            AddChildWithRole(newChild, role);
        }

        internal ParentElementBuilder LightweightCopyChildrenFrom(ParentElementBuilder anotherParent)
        {
            foreach (KeyValuePair<IElementTreeNode, ChildRole> eachChildKVP in anotherParent.ChildrenAndRoles)
            {
                AddChildWithRole(eachChildKVP.Key.CopyLightweight(), eachChildKVP.Value);
            }
            return this;
        }

        #endregion Configuration
    }
}

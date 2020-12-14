using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlexibleRealization
{
    /// <summary>The base class of all ParentElementBuilders</summary>
    public abstract class ParentElementBuilder : ElementBuilder, IParent
    {
        public virtual bool CanAddChild(ElementBuilder potentialChild) => true;

        /// <summary>This method is used during initial construction of an ElementBuilder tree from a constituency parse.  It can also be used during the Configuration
        /// process when a ParentElementBuilder needs another chance to define the proper role for a child.</summary>
        public void AddChild(IElementTreeNode newChild) => AssignRoleFor(newChild);

        /// <summary>AssignRoleFor must be overridden by all subclasses, to define the roles that are appropriate to various child types relative to each parent type</summary>
        private protected abstract void AssignRoleFor(IElementTreeNode child);

        /// <summary>Add <paramref name="child"/> as a child of this, with ChildRole <paramref name="role"/></summary>
        public void AddChildWithRole(IElementTreeNode child, ChildRole role)
        {
            ChildrenAndRoles.Add(child, role);
            child.Parent = this;
        }

        /// <summary>Add all the ElementBuilders in <paramref name="children"/> as children of this, with ChildRole <paramref name="role"/></summary>
        private protected void AddChildrenWithRole(IEnumerable<IElementTreeNode> newChildren, ChildRole role) => newChildren.ToList().ForEach(newChild => AddChildWithRole(newChild, role));

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

        /// <summary>Return a list of the valid ChildRoles for <paramref name="child"/> as a child of this</summary>
        public List<ChildRole> ValidRolesForChild(ElementBuilder child)
        {
            List<ChildRole> result = new List<ChildRole>();
            AddValidRolesForChildTo(result, child);
            return result;
        }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected abstract void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child);

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
        private protected void AddDescendantsTo(List<IElementTreeNode> list) => Children.ToList().ForEach(child => list.AddRange(child.WithAllDescendentBuilders));

        /// <summary>Return all the PartOfSpeechBuilders descended from this that have indexes between <paramref name="start"/> and <paramref name="end"/>, non-inclusive</summary>
        internal IEnumerable<PartOfSpeechBuilder> PartsOfSpeechInSubtreeBetween(PartOfSpeechBuilder start, PartOfSpeechBuilder end) => GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
            .Where(posb => posb.Token.Index > start.Token.Index && posb.Token.Index < end.Token.Index);

        #endregion Tree structure

        #region Configuration

        public override void Propagate(ElementTreeNodeOperation operateOn)
        {
            Children.ToList().ForEach(child => child.Propagate(operateOn));
            operateOn(this);
        }

        /// <summary>Override of Configure for ParentElementBuilders.  If a subclass overrides this implementation, it should call this base form after its own custom manipulations.</summary>
        public override void Configure() => UnassignedChildren.ToList().ForEach(unassignedChild =>
        {
            RemoveChild(unassignedChild);
            AssignRoleFor(unassignedChild);
        });

        /// <summary>Default override of Consolidate for ParentElementBuilders</summary>
        public override void Consolidate() 
        {
            switch (Children.Count())
            {
                case 0:
                    Become(null);
                    break;
                case 1:
                    Become(Children.Single());
                    break;
                default: break;
            }
        }

        /// <summary>Change the role of an existing child <paramref name="child"/> to <paramref name="newRole"/></summary>
        public void SetRoleOfChild(IElementTreeNode child, ChildRole newRole) => ChildrenAndRoles[child] = newRole;

        /// <summary>Find all children with assigned role <paramref name="originalRole"/>, and change their role to <paramref name="newRole"/></summary>
        private protected void ChangeChildRoles(ChildRole originalRole, ChildRole newRole) => ChildrenWithRole(originalRole).ToList()
            .ForEach(childToChange => SetRoleOfChild(childToChange, newRole));

        /// <summary>Sever the parent-child link between this and <paramref name="childToRemove"/></summary>
        public void RemoveChild(IElementTreeNode childToRemove)
        {
            ChildrenAndRoles.Remove(childToRemove);
            childToRemove.Parent = null;
        }

        /// <summary>Replace <paramref name="existingChild"/> with <paramref name="newChild"/> using the same role.</summary>
        public void ReplaceChild(IElementTreeNode existingChild, IElementTreeNode newChild)
        {
            ChildRole existingRole = RoleFor(existingChild);
            RemoveChild(existingChild);
            if (newChild != null) AddChildWithRole(newChild, existingRole);
        }

        internal ParentElementBuilder LightweightCopyChildrenFrom(ParentElementBuilder anotherParent)
        {
            anotherParent.ChildrenAndRoles.ToList().ForEach(kvp => AddChildWithRole(kvp.Key.CopyLightweight(), kvp.Value));
            return this;
        }

        #endregion Configuration

        ///Return an IEnumerator for the variations of this
        public override IEnumerator<IElementTreeNode> GetVariationsEnumerator() => new Variations.Enumerator(this);

        /// <summary>Return the realizable variations of this</summary>
        public override IEnumerable<IElementTreeNode> GetRealizableVariations() => new Variations(this).Select(variation => variation.AsRealizableTree());

        public class Variations : IEnumerable<IElementTreeNode>
        {
            internal Variations(ParentElementBuilder parent) => Builder = parent;

            private ParentElementBuilder Builder;

            public IEnumerator<IElementTreeNode> GetEnumerator() => new Enumerator(Builder);
            IEnumerator IEnumerable.GetEnumerator() => new Enumerator(Builder);

            /// <summary>For a ParentElementBuilder, enumerating variations involves enumerating all combinations of the variations for each child.</summary>
            /// <remarks><para>To accomplish this, we get an IEnumerator for each child, and chain those enumerators together in a mechanism that works like the
            /// counter chain mechanism in a mechanical clock.</para>
            /// <para>Pulses of MoveNext() come into the mechanism from this Enumerator's consumer.  Those pulses are fed into the IEnumerator for the first
            /// child.  When that first child has cycled through all its variations, its IEnumerator is Reset() and the next IEnumerator in the chain is
            /// pulsed.  And so on.  The process terminates when the IEnumerator at the end of the chain can no longer be pulsed.  At this point we have
            /// enumerated all combinations of child variations.</para></remarks>
            public class Enumerator : IEnumerator<IElementTreeNode>
            {
                internal Enumerator(ParentElementBuilder parent)
                {
                    Builder = parent;
                    Components = Builder.Children.Select(child => child.GetVariationsEnumerator()).ToList();
                    // The external consumer will send the required MoveNext() to the IEnumerator at the beginning of the chain; but it will not do that for the other IEnumerators in the chain.
                    // Therefore we need to initialize the chain by sending MoveNext() to each of them, so their Current value becomes defined and we can begin enumerating combinations. 
                    Components
                        .Where(component => component != Components[0]) // All the IEnumerators except the first one
                        .ToList()
                        .ForEach(childOtherThanTheFirstOne => childOtherThanTheFirstOne.MoveNext());
                }

                private ParentElementBuilder Builder;

                private List<IEnumerator<IElementTreeNode>> Components;

                public IElementTreeNode Current => Builder;
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext() => MoveNext(0);

                public void Reset() => Components.ForEach(child => child.Reset());

                /// <summary>Try to pulse the <paramref name="componentIndex"/>'th IEnumerator in the chain</summary>
                private bool MoveNext(int componentIndex)
                {
                    if (!Components[componentIndex].MoveNext())
                    {
                        // If we're trying to pulse the last IEnumerator and we can't do it, then we're all done
                        if (componentIndex == Components.Count - 1) return false;
                        else
                        {
                            Components[componentIndex].Reset();
                            Components[componentIndex].MoveNext();
                            return MoveNext(componentIndex + 1);
                        }
                    }
                    else return true;
                }
            }
        }

    }
}

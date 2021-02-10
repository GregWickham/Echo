using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FlexibleRealization
{
    /// <summary>The base class of all ParentElementBuilders</summary>
    public abstract class ParentElementBuilder : ElementBuilder, IParent
    {
        private protected ParentElementBuilder() { }

        /// <summary>This method is used during initial construction of an ElementBuilder tree from a constituency parse.  It can also be used during the Configuration
        /// process when a ParentElementBuilder needs another chance to define the proper role for a child.</summary>
        public void AddChild(IElementTreeNode newChild) => AssignRoleFor(newChild);

        /// <summary>AssignRoleFor must be overridden by all subclasses, to define the roles that are appropriate to various child types relative to each parent type.</summary>
        private protected abstract void AssignRoleFor(IElementTreeNode child);

        /// <summary>Add <paramref name="child"/> as a child of this, with ChildRole <paramref name="role"/></summary>
        public void AddChildWithRole(IElementTreeNode child, ChildRole role)
        {
            ChildrenAndRoles.Add(child, role);
            child.Parent = this;
        }

        /// <summary>Add all the ElementBuilders in <paramref name="children"/> as children of this, with ChildRole <paramref name="role"/>.</summary>
        private protected void AddChildrenWithRole(IEnumerable<IElementTreeNode> newChildren, ChildRole role) => newChildren.ToList().ForEach(newChild => AddChildWithRole(newChild, role));

        /// <summary>Add <paramref name="newChild"/> as a child of this, with ChildRole Unassigned.</summary>
        private protected void AddUnassignedChild(IElementTreeNode newChild) => AddChildWithRole(newChild, ChildRole.Unassigned);

        #region Tree structure

        /// <summary>The possible roles that a child IElementBuilder can have relative to a ParentElementBuilder.</summary>
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

        /// <summary>Return a list of the valid ChildRoles for <paramref name="child"/> as a child of this.</summary>
        public List<ChildRole> ValidRolesForChild(ElementBuilder child)
        {
            List<ChildRole> result = new List<ChildRole>();
            AddValidRolesForChildTo(result, child);
            return result;
        }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected abstract void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child);

        /// <summary>The central collection holding all the ElementBuilder children of a ParentElementBuilder and the roles of those children.</summary>
        /// <remarks>Many properties and methods operate upon this collection.</remarks>
        private Dictionary<IElementTreeNode, ChildRole> ChildrenAndRoles = new Dictionary<IElementTreeNode, ChildRole>();

        /// <summary>Return the immediate children of this</summary>
        public override IEnumerable<IElementTreeNode> Children => ChildrenAndRoles.Select(kvp => kvp.Key);

        /// <summary>Return the ChildRole assigned for <paramref name="child"/>.</summary>
        /// <exception cref="KeyNotFoundException"></exception>
        public ChildRole RoleFor(IElementTreeNode child) => ChildrenAndRoles[child];

        /// <summary>Return the immediate children of this having the supplied <paramref name="role"/>.</summary>
        private protected IEnumerable<IElementTreeNode> ChildrenWithRole(ChildRole role) => ChildrenAndRoles
            .Where(kvp => kvp.Value == role)
            .Select(kvp => kvp.Key);

        /// <summary>Return the children of this that have ChildRole Unassigned.</summary>
        private protected IEnumerable<IElementTreeNode> UnassignedChildren => Children
            .Where(child => RoleFor(child) == ChildRole.Unassigned);

        /// <summary>Return all the descendants of this.</summary>
        public override IEnumerable<IElementTreeNode> DescendentBuilders
        {
            get
            {
                List<IElementTreeNode> result = new List<IElementTreeNode>();
                AddDescendantsTo(result);
                return result;
            }
        }

        /// <summary>Return all the descendants of this, plus this.</summary>
        public override IEnumerable<IElementTreeNode> WithAllDescendentBuilders
        {
            get
            {
                List<IElementTreeNode> result = new List<IElementTreeNode> { this };
                AddDescendantsTo(result);
                return result;
            }
        }

        /// <summary>Add all descendants of this to <paramref name="list"/>.</summary>
        private protected void AddDescendantsTo(List<IElementTreeNode> list) => Children.ToList().ForEach(child => list.AddRange(child.WithAllDescendentBuilders));

        /// <summary>Return the child of this ParentElementBuilder that contains <paramref name="node"/> in its subtree.</summary>
        private IElementTreeNode ChildContaining(IElementTreeNode node) => Children.Single(child => child.WithAllDescendentBuilders.Contains(node));

        /// <summary>Return all the PartOfSpeechBuilders descended from this that have indexes between <paramref name="start"/> and <paramref name="end"/>, non-inclusive.</summary>
        internal IEnumerable<PartOfSpeechBuilder> PartsOfSpeechInSubtreeBetween(PartOfSpeechBuilder start, PartOfSpeechBuilder end) => GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
            .Where(posb => posb.ComesAfter(start) && posb.ComesBefore(end));

        /// <summary>Return a dictionary containing:
        /// <list type="table"><item>(Keys) the parts of speech in the subtree of this ParentElementBuilder; and</item>
        /// <item>(Values) the zero-based index of each part of speech relative to this.</item></list></summary>
        private protected Dictionary<PartOfSpeechBuilder, int> GetPartsOfSpeechAndIndices()
        {
            IEnumerable<PartOfSpeechBuilder> partsOfSpeech = GetElementsOfTypeInSubtree<PartOfSpeechBuilder>();
            SortedList<PartOfSpeechBuilder, object> sortedPartsOfSpeech = new SortedList<PartOfSpeechBuilder, object>();
            foreach (PartOfSpeechBuilder eachPartOfSpeech in partsOfSpeech)
            {
                sortedPartsOfSpeech.Add(eachPartOfSpeech, null);
            }
            Dictionary<PartOfSpeechBuilder, int> result = new Dictionary<PartOfSpeechBuilder, int>();
            partsOfSpeech.ToList().ForEach(partOfSpeech => result.Add(partOfSpeech, sortedPartsOfSpeech.IndexOfKey(partOfSpeech)));
            return result;
        }

        #region Child Ordering

        /// <summary>Represents the fact than within a certain ParentElementBuilder, one child comes before another</summary>
        public class ChildOrdering
        {
            public IElementTreeNode Before { get; set; }
            public IElementTreeNode After { get; set; }
        }

        /// <summary>ChildOrderings specify the ordering of children.</summary>
        public HashSet<ChildOrdering> ChildOrderings { get; private protected set; } = new HashSet<ChildOrdering>();

        /// <summary>Convert from index-based ordering to ordering based on ChildOrdering objects, maintaining the existing order of children.</summary>
        /// <remarks>CoreNLP returns a tree with index-based ordering, and we use that to construct the initial tree of ElementBuilders.
        /// But index-based ordering has limitations.  Index-based ordering is only applicable within a single tree, so the components of that tree CANNOT be relocated.
        /// The ChildOrdering scheme makes ElementBuilders relocatable, so they can be saved to the database and used as components in an editor.</remarks>
        public override void CreateChildOrderingsFromIndices()
        {
            List<IElementTreeNode> sortedChildren = Children.OrderBy(child => child.MinimumIndex).ToList();
            for (int beforeChildIndex = 0; beforeChildIndex < sortedChildren.Count - 1; beforeChildIndex++)
            {
                ChildOrderings.Add(new ChildOrdering
                {
                    Before = sortedChildren[beforeChildIndex],
                    After = sortedChildren[beforeChildIndex + 1]
                });
            }
        }

        /// <summary>Configure the ChildOrderings of this ParentElementBuilder so <paramref name="childToOrder"/> has relation <paramref name="relation"/> to <paramref name="childToOrderRelativeTo"/>.</summary>
        public void SetChildOrdering(IElementTreeNode childToOrder, IElementTreeNode childToOrderRelativeTo, NodeRelation relation)
        {
            switch (relation)
            {
                case NodeRelation.First:
                    IElementTreeNode firstExistingChild = Children.OrderBy(child => child).FirstOrDefault(child => !ChildOrderings.Any(ordering => ordering.After == child));
                    if (firstExistingChild != null) ChildOrderings.Add(new ChildOrdering { Before = childToOrder, After =  firstExistingChild });
                    break;
                case NodeRelation.Before:
                    ChildOrdering existingOrderingBeforeRelativeChild = ChildOrderings.FirstOrDefault(ordering => ordering.After == childToOrderRelativeTo);
                    if (existingOrderingBeforeRelativeChild != null)
                    {
                        ChildOrderings.Add(new ChildOrdering { Before = childToOrder, After = existingOrderingBeforeRelativeChild.After });
                        existingOrderingBeforeRelativeChild.After = childToOrder;
                    }
                    else ChildOrderings.Add(new ChildOrdering { Before = childToOrder, After = childToOrderRelativeTo });
                    break;
                case NodeRelation.After:
                    ChildOrdering existingOrderingAfterRelativeChild = ChildOrderings.FirstOrDefault(ordering => ordering.Before == childToOrderRelativeTo);
                    if (existingOrderingAfterRelativeChild != null)
                    {
                        ChildOrderings.Add(new ChildOrdering { Before = existingOrderingAfterRelativeChild.Before, After = childToOrder });
                        existingOrderingAfterRelativeChild.Before = childToOrder;
                    }
                    else ChildOrderings.Add(new ChildOrdering { Before = childToOrderRelativeTo, After = childToOrder });
                    break;
                case NodeRelation.Last:
                    IElementTreeNode lastExistingChild = Children.OrderBy(child => child).FirstOrDefault(child => !ChildOrderings.Any(ordering => ordering.Before == child));
                    if (lastExistingChild != null) ChildOrderings.Add(new ChildOrdering { Before = lastExistingChild, After = childToOrder });
                    break;
                default: throw new ArgumentException("Unhandled relation type while trying to set ChildOrdering");
            }
        }

        /// <summary>Configure the ChildOrderings of this ParentElementBuilder so the parts of speech in <paramref name="childToOrder"/> remain in the same order
        /// as they are found in the <paramref name="partsOfSpeech"/> Dictionary.</summary>
        void IParent.SetChildOrdering(IElementTreeNode childToOrder, Dictionary<PartOfSpeechBuilder, int> partsOfSpeech)
        {
            IEnumerable<PartOfSpeechBuilder> partsOfSpeechInChildToOrder = childToOrder.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>();
            IEnumerable<IGrouping<IElementTreeNode, PartOfSpeechBuilder>> partsOfSpeechGroupedByChild = GetElementsOfTypeInSubtree<PartOfSpeechBuilder>().GroupBy(pos => ChildContaining(pos));
            IElementTreeNode lastChildBefore = partsOfSpeechGroupedByChild
                .Where(group => group.All(pos => partsOfSpeech[pos] < partsOfSpeechInChildToOrder.Min(pos => partsOfSpeech[pos])))
                .OrderBy(group => group.Max(pos => partsOfSpeech[pos]))
                .Select(group => group.Key)
                .LastOrDefault();
            IElementTreeNode firstChildAfter = partsOfSpeechGroupedByChild
                .Where(group => group.All(pos => partsOfSpeech[pos] > partsOfSpeechInChildToOrder.Max(pos => partsOfSpeech[pos])))
                .OrderBy(group => group.Min(pos => partsOfSpeech[pos]))
                .Select(group => group.Key)
                .FirstOrDefault();
            if (lastChildBefore != null)
            {
                if (firstChildAfter != null) { }
                else SetChildOrdering(childToOrder, lastChildBefore, NodeRelation.Last);
            }
            else
            {
                if (firstChildAfter != null) SetChildOrdering(childToOrder, firstChildAfter, NodeRelation.First);
                else { }
            }
        }

        /// <summary>Do a comparison to determine the relative order of <paramref name="aDescendant"/> and <paramref name="anotherDescendant"/></summary>
        /// <remarks>This method is invoked by the CompareTo method that implements IComparable for PartOfSpeechBuilder.  Because this method is invoked
        /// on the most recent common ancestor of <paramref name="aDescendantPartOfSpeech"/> and <paramref name="anotherDescendantPartOfSpeech"/>, finding
        /// the ordering of those descendant parts of speech is equivalent to finding the ordering of the children of this common ancestor which are
        /// respective ancestors of those parts of speech.</remarks>
        public int CompareDescendants(IElementTreeNode aDescendant, IElementTreeNode anotherDescendant)
        {
            IElementTreeNode childAncestorOfADescendant = Children.Where(child => aDescendant.IsInSubtreeOf(child)).Single();
            IElementTreeNode childAncestorOfAnotherDescendant = Children.Where(child => anotherDescendant.IsInSubtreeOf(child)).Single();
            IElementTreeNode childToLookFor;
            // See if we can find a chain of ChildOrdering that connects aChild to anotherChild, in Before -> After order
            childToLookFor = childAncestorOfADescendant;
            do
            {
                ChildOrdering link = ChildOrderings.Where(ordering => ordering.Before == childToLookFor).FirstOrDefault();
                if (link == null)   // We're at the end of the chain and it didn't lead to what we're looking for
                    break;          // Out of the do loop
                else
                {
                    if (link.After == childAncestorOfAnotherDescendant) return -1;  // We found the chain we were looking for
                    else childToLookFor = link.After;           // Keep following the chain
                }
            } while (true);
            // We failed to find a chain connecting aChild to anotherChild in Before -> After order.
            // Now we'll see if such a chain exists in After -> Before order
            childToLookFor = childAncestorOfADescendant;
            do
            {
                ChildOrdering link = ChildOrderings.Where(ordering => ordering.After == childToLookFor).FirstOrDefault();
                if (link == null)   // We're at the end of the chain and it didn't lead to what we're looking for
                    break;          // Out of the do loop
                else
                {
                    if (link.Before == childAncestorOfAnotherDescendant) return 1;  // We found the chain we were looking for
                    else childToLookFor = link.Before;          // Keep following the chain
                }
            } while (true);
            // We couldn't find a chain between these two IElementTreeNodes in either direction.
            // As far as this parent is concerned, they are equal.
            return 0;            
        }

        #endregion Child Ordering

        #endregion Tree structure

        #region Configuration

        /// <summary>Propagate <paramref name="operateOn"/> to the children of this, and <paramref name="operateOn"/> this.</summary>
        public override void Propagate(ElementTreeNodeOperation operateOn)
        {
            Children.ToList().ForEach(child => child.Propagate(operateOn));
            operateOn(this);
        }

        /// <summary>Override of Configure for ParentElementBuilders.  If a subclass overrides this implementation, it should call this base form.</summary>
        public override void Configure() => UnassignedChildren.ToList().ForEach(unassignedChild =>
        {
            RemoveChild(unassignedChild);
            AssignRoleFor(unassignedChild);
        });

        /// <summary>Default override of Consolidate for ParentElementBuilders.</summary>
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

        /// <summary>Change the role of an existing child <paramref name="child"/> to <paramref name="newRole"/>.</summary>
        public void SetRoleOfChild(IElementTreeNode child, ChildRole newRole) => ChildrenAndRoles[child] = newRole;

        /// <summary>Find all children with assigned role <paramref name="originalRole"/>, and change their role to <paramref name="newRole"/>.</summary>
        private protected void ChangeChildRoles(ChildRole originalRole, ChildRole newRole) => ChildrenWithRole(originalRole).ToList()
            .ForEach(childToChange => SetRoleOfChild(childToChange, newRole));

        /// <summary>Sever the parent-child link between this and <paramref name="childToRemove"/>.</summary>
        public void RemoveChild(IElementTreeNode childToRemove)
        {
            ChildrenAndRoles.Remove(childToRemove);
            RemoveChildOrderingsThatReferTo(childToRemove);
            childToRemove.Parent = null;
        }

        /// <summary>Replace <paramref name="existingChild"/> with <paramref name="newChild"/>, using the same role and preserving the ordering of children.</summary>
        public void ReplaceChild(IElementTreeNode existingChild, IElementTreeNode newChild)
        {
            if (newChild != null)
            {
                ChildRole existingRole = RoleFor(existingChild);
                ChildrenAndRoles.Remove(existingChild);
                existingChild.Parent = null;
                AddChildWithRole(newChild, existingRole);
                UpdateChildOrderingReferences(existingChild, newChild);
            }
            else RemoveChild(existingChild);

            // Update the ChildOrdering references from oldReference to newReference
            void UpdateChildOrderingReferences(IElementTreeNode oldReference, IElementTreeNode newReference)
            {
                foreach (ChildOrdering eachChildOrdering in ChildOrderings)
                {
                    if (eachChildOrdering.Before == existingChild)
                        eachChildOrdering.Before = newChild;
                    if (eachChildOrdering.After == existingChild)
                        eachChildOrdering.After = newChild;
                }
            }
        }

        /// <summary>Remove any ChildOrderings that refer to <paramref name="child"/>.</summary>
        private protected void RemoveChildOrderingsThatReferTo(IElementTreeNode child)
        {
            ChildOrdering orderingWithTheRemovedChildAsBefore = ChildOrderings.FirstOrDefault(ordering => ordering.Before == child);
            ChildOrdering orderingWithTheRemovedChildAsAfter = ChildOrderings.FirstOrDefault(ordering => ordering.After == child);
            if (orderingWithTheRemovedChildAsBefore != null)
            {
                if (orderingWithTheRemovedChildAsAfter != null) // The removed child is between two other children
                {
                    orderingWithTheRemovedChildAsBefore.Before = orderingWithTheRemovedChildAsAfter.Before;
                    ChildOrderings.Remove(orderingWithTheRemovedChildAsAfter);
                }
                else ChildOrderings.Remove(orderingWithTheRemovedChildAsBefore);  // The removed child has no other children before it
            }
            else ChildOrderings.Remove(orderingWithTheRemovedChildAsAfter);  // The removed child has no other children after it
        }

        internal void MoveChildrenTo(ParentElementBuilder anotherParent)
        {
            // When we move the children, that will cause all our ChildOrderings to be removed.  So we'll maintain a temporary reference to them.
            HashSet<ChildOrdering> orderings = ChildOrderings;
            ChildrenAndRoles.ToList().ForEach(kvp => kvp.Key.MoveTo(anotherParent, kvp.Value));
            anotherParent.ChildOrderings = orderings;
        }

        #endregion Configuration

        /// <summary>Make a lightweight copy of the children in <paramref name="anotherParent"/>, and add the copied children to this ParentElementBuilder.</summary>
        /// <returns>this</returns>
        internal ParentElementBuilder LightweightCopyChildrenFrom(ParentElementBuilder anotherParent)
        {
            // Create a copy of each child and put the copies in a Dictionary so they can be looked up using the original child as a key
            Dictionary<IElementTreeNode, IElementTreeNode> copiedChildren = new Dictionary<IElementTreeNode, IElementTreeNode>();
            foreach (KeyValuePair<IElementTreeNode, ChildRole> eachChildAndRole in anotherParent.ChildrenAndRoles)
            {
                IElementTreeNode copiedChild = eachChildAndRole.Key.CopyLightweight();
                AddChildWithRole(copiedChild, eachChildAndRole.Value);
                copiedChildren.Add(eachChildAndRole.Key, copiedChild);
            }
            // Create copies of the ChildOrderings that refer to the copied children
            foreach (ChildOrdering eachOrdering in anotherParent.ChildOrderings)
            {
                ChildOrderings.Add(new ChildOrdering
                {
                    Before = copiedChildren[eachOrdering.Before],
                    After = copiedChildren[eachOrdering.After]
                });
            }
            return this;
        }

        #region Editing

        /// <summary>Return true if this ParentElementBuilder can add <paramref name="potentialChild"/> as a child.</summary>
        public bool CanAddChild(IElementTreeNode potentialChild) => CanAddChildOfType(potentialChild.GetType());

        /// <summary>Return true if this ParentElementBuilder can add a child of type <paramref name="potentialChildType"/> as a child.</summary>
        public virtual bool CanAddChildOfType(Type potentialChildType) => ChildTypesThatCanBeAdded.Contains(potentialChildType);

        /// <summary>The set of ElementBuilder types that can be added as children of this during editing by the user.</summary>
        /// <remarks>This default implementation allows no types of children to be added.  Subclasses should override to allow adding children during editing.</remarks>
        private protected virtual HashSet<Type> ChildTypesThatCanBeAdded { get; } = new HashSet<Type>();

        #endregion Editing

        #region Variations

        ///Return an IEnumerator for the variations of this
        public override IEnumerator<IElementTreeNode> GetVariationsEnumerator() => new Variations.Enumerator(this);

        /// <summary>Return the total number of forms specified for this parent element.</summary>
        /// <remarks>For parents, this is the cardinality of the Cartesian product of all descendents.</remarks>
        public override int CountForms() => new Variations(this).Count();

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
                    Reset();
                }

                private ParentElementBuilder Builder;

                private List<IEnumerator<IElementTreeNode>> Components;

                public IElementTreeNode Current => Builder;
                object IEnumerator.Current => Current;

                public void Dispose() { }

                public bool MoveNext() => MoveNext(0);

                /// <summary>Reset needs to:<list type="number">
                /// <item>leave the Builder in its Default state, AND</item>
                /// <item>leave the Enumerator ready to start enumerating.</item></list></summary>
                public void Reset()
                {
                    // This satisifes the first condition by leaving the builder in its default state
                    Components.ForEach(child => child.Reset());
                    // The external consumer will send the required MoveNext() to the IEnumerator at the beginning of the chain; but it will not do that for the other IEnumerators in the chain.
                    // Therefore we need to initialize the chain by sending MoveNext() to each of them, so their Current value becomes defined and we can begin enumerating combinations. 
                    Components.Where(component => component != Components[0]) // All the IEnumerators except the first one
                        .ToList()
                        .ForEach(childOtherThanTheFirstOne => childOtherThanTheFirstOne.MoveNext());
                }

                /// <summary>Try to pulse the <paramref name="componentIndex"/>'th IEnumerator in the chain</summary>
                private bool MoveNext(int componentIndex)
                {
                    if (!Components[componentIndex].MoveNext())
                    {
                        // If we're trying to pulse the last IEnumerator and we can't do it, then we're all done
                        if (componentIndex == Components.Count - 1)
                        {
                            Reset();
                            return false;
                        }
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

        #endregion Variations
    }
}

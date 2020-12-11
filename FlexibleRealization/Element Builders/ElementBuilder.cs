using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using SimpleNLG;
using FlexibleRealization.Dependencies;

namespace FlexibleRealization
{
    public abstract partial class ElementBuilder : IElementBuilder, IElementTreeNode, IIndexRange, INotifyPropertyChanged
    {
        #region Tree structure

        /// <summary>Can be called by an element anywhere in the subtree to raise the TreeStructureChanged event for the tree</summary>
        internal void OnTreeStructureChanged() => Root.OnTreeStructureChanged();

        public IParent Parent { get; set; }

        /// <summary>Return the number of parent-child relations between this ElementBuilder and the root of the graph containing it</summary>
        public int Depth => Parent is RootNode ? 0 : Parent.Depth + 1;

        /// <summary>Return the root ParentElementBuilder of the tree containing this.</summary>
        /// <remarks>The implementation is recursive, terminating with RootNode.</remarks>
        public RootNode Root => Parent.Root;

        /// <summary>Return true if this has the same parent as <paramref name="anotherElement"/></summary>
        internal bool HasSameParentAs(IElementTreeNode anotherElement) => Parent == anotherElement.Parent;

        /// <summary>Return the ElementBuilders that are direct children of this</summary>
        public abstract IEnumerable<IElementTreeNode> Children { get; }

        /// <summary>Return true if this is a direct child of <paramref name="prospectiveParent"/></summary>
        public bool IsChildOf(ParentElementBuilder prospectiveParent) => prospectiveParent.Children.Contains(this);

        ///Return true if this is a phrase head
        public virtual bool IsPhraseHead => AssignedRole == ParentElementBuilder.ChildRole.Head;

        /// <summary>Return this as a phrase of the appropriate type</summary>
        public virtual PhraseBuilder AsPhrase() => throw new InvalidOperationException("This ElementBuilder can't be converted to a phrase");

        /// <summary>Return all the IElementBuilders in the subtree of which this is the root</summary>
        public virtual IEnumerable<IElementTreeNode> DescendentBuilders => new List<IElementTreeNode>();

        /// <summary>Return all the ParentElementBuilders descended from this, NOT including this</summary>
        private protected IEnumerable<ParentElementBuilder> ChildParents => Children.Where(element => element is ParentElementBuilder).Cast<ParentElementBuilder>();

        /// <summary>Return all the ParentElementBuilders descended from this, NOT including this</summary>
        private IEnumerable<ParentElementBuilder> DescendentParents => DescendentBuilders.Where(element => element is ParentElementBuilder).Cast<ParentElementBuilder>();

        /// <summary>Return all the IElementBuilders in the subtree of which this is the root</summary>
        public virtual IEnumerable<IElementTreeNode> WithAllDescendentBuilders => new List<IElementTreeNode> { this };

        /// <summary>Return all the PartOfSpeechBuilders in the subtree of which this is the root</summary>
        public IEnumerable<PartOfSpeechBuilder> PartsOfSpeechInSubtree => this.WithAllDescendentBuilders.Where(element => element is PartOfSpeechBuilder).Cast<PartOfSpeechBuilder>();

        /// <summary>Return all the ParentElementBuilders in the subtree of which this is the root</summary>
        private IEnumerable<ParentElementBuilder> ParentsInSubtree => this.WithAllDescendentBuilders.Where(element => element is ParentElementBuilder).Cast<ParentElementBuilder>();

        /// <summary>Return all the PartOfSpeechBuilders in the subtree of which this is the root</summary>
        private IEnumerable<CoordinablePhraseBuilder> CoordinablePhrasesInSubtree => this.WithAllDescendentBuilders.Where(element => element is CoordinablePhraseBuilder).Cast<CoordinablePhraseBuilder>();

        /// <summary>Return the PartOfSpeechBuilder descended from this whose Token has the supplied <paramref name="index"/>, of null if there is no such PartOfSpeechBuilder</summary>
        public PartOfSpeechBuilder PartOfSpeechInSubtreeWithIndex(int index) => PartsOfSpeechInSubtree
            .Where(partOfSpeech => partOfSpeech.Token.Index == index)
            .FirstOrDefault();

        /// <summary>Return the smallest token index of the PartOfSpeechBuilders spanned by this</summary>
        public int MinTokenIndex => PartsOfSpeechInSubtree.Min(partOfSpeech => partOfSpeech.Token.Index);

        /// <summary>Return the largest token index of the PartOfSpeechBuilders spanned by this</summary>
        public int MaxTokenIndex => PartsOfSpeechInSubtree.Max(partOfSpeech => partOfSpeech.Token.Index);

        /// <summary>Return true if all PartOfSpeechBuilders spanned by this ElementBuilder precede all PartOfSpeechBuilders spanned by <paramref name="theOtherElement"/></summary>
        public bool ComesBefore(IIndexRange theOtherElement) => MaxTokenIndex < theOtherElement.MinTokenIndex;

        /// <summary>Return true if all PartOfSpeechBuilders spanned by <paramref name="theOtherElement"/> precede all PartOfSpeechBuilders spanned by this ElementBuilder</summary>
        public bool ComesAfter(IIndexRange theOtherElement) => MinTokenIndex > theOtherElement.MaxTokenIndex;

        /// <summary>Return the <see cref="int"/> distance between the index ranges of this and <paramref name="anotherElement"/>, or zero if their index ranges intersect</summary>
        public int DistanceFrom(IIndexRange anotherElement)
        {
            if (MinTokenIndex > anotherElement.MaxTokenIndex) return MinTokenIndex - anotherElement.MaxTokenIndex;
            else if (anotherElement.MinTokenIndex > MaxTokenIndex) return anotherElement.MinTokenIndex - MaxTokenIndex;
            else return 0;
        }

        /// <summary>Return the one of <paramref name="elements"/> that's nearest to this, based on their part of speech index ranges</summary>
        public IElementTreeNode NearestOf(IEnumerable<IElementTreeNode> elements) => elements.OrderBy(element => DistanceFrom(element)).First();

        /// <summary>The list of ChildRoles an instance can have if it has no parent.  Only one option.</summary>
        private static List<ParentElementBuilder.ChildRole> NoParentRolesList = new List<ParentElementBuilder.ChildRole> { ParentElementBuilder.ChildRole.NoParent };

        /// <summary>The list of valid ChildRoles this could have relative to its current parent</summary>
        public IEnumerable<ParentElementBuilder.ChildRole> ValidRolesInCurrentParent => Parent == null ? NoParentRolesList : Parent.ValidRolesForChild(this);

        /// <summary>The ChildRole of this relative to its parent</summary>
        public ParentElementBuilder.ChildRole AssignedRole
        {
            get => Parent?.RoleFor(this) ?? ParentElementBuilder.ChildRole.NoParent;
            set
            {
                if (Parent != null)
                {
                    Parent.SetRoleOfChild(this, value);
                    OnTreeStructureChanged();
                }
            }                 
        }

        /// <summary>Return true if this has ChildRole <paramref name="role"/> relative to its parent</summary>
        private bool HasRole(ParentElementBuilder.ChildRole role) => AssignedRole == role;

        /// <summary>The element roles that are "head-like"</summary>
        private static List<ParentElementBuilder.ChildRole> HeadLikeRoles = new List<ParentElementBuilder.ChildRole>
        {
            ParentElementBuilder.ChildRole.Head,
            ParentElementBuilder.ChildRole.Coordinated,
            ParentElementBuilder.ChildRole.Component
        };

        /// <summary>Return true if this has one of the roles that makes it "head-like" in its parent</summary>
        private bool HasHeadLikeRole => HeadLikeRoles.Contains(AssignedRole);

        /// <summary>Return true if this directly or indirectly acts as a head of <paramref name="phrase"/></summary>
        public bool ActsAsHeadOf(PhraseBuilder phrase) => (HasRole(ParentElementBuilder.ChildRole.Head) && Parent == phrase)
            || (HasHeadLikeRole && Parent.ActsAsHeadOf(phrase));
            
        /// <summary>Return true if this directly or indirectly acts with ChildRole <paramref name="role"/> in <paramref name="phrase"/></summary>
        public bool ActsWithRoleInAncestor(ParentElementBuilder.ChildRole role, ParentElementBuilder ancestor) => (HasRole(role) && Parent == ancestor)
            || (HasHeadLikeRole && Parent.ActsWithRoleInAncestor(role, ancestor));

        /// <summary>Return true if this has ChildRole <paramref name="role"/> within the same SyntaxHeadBuilder of which <paramref name="headElement"/> is a head,
        /// OR if this is a head or coordinated element of a SyntaxHeadBuilder that has the ChildRole <paramref name="role"/> relative to <paramref name="headElement"/>,
        /// OR so on recursively.</summary>
        private bool HasDirectOrIndirectRoleRelativeToHead(IElementTreeNode headElement, ParentElementBuilder.ChildRole role)
        {
            PhraseBuilder commonAncestorPhrase = LowestCommonAncestor<PhraseBuilder>(headElement);
            if (commonAncestorPhrase == null) return false;
            else return ActsWithRoleInAncestor(role, commonAncestorPhrase) && headElement.ActsAsHeadOf(commonAncestorPhrase);
        }

        /// <summary>Return true if this is anywhere inside a nominal modifier</summary>
        private protected bool IsPartOfANominalModifier => Ancestors.Any(ancestor => ancestor is NominalModifierBuilder);

        /// <summary>Search for an ancestor ElementBuilder relative to which this ElementBuilder has ChildRole <paramref name="role"/>, either directly or through one or more intercedent phrase head relations.</summary>
        /// <returns>The searched for ancestor if found, or null if not found</returns>
        public IParent AncestorOfWhichThisIsDirectlyOrIndirectlyA(ParentElementBuilder.ChildRole role)
        {
            if (AssignedRole == role)
                return Parent;
            else if (AssignedRole == ParentElementBuilder.ChildRole.Head)
                return Parent.AncestorOfWhichThisIsDirectlyOrIndirectlyA(role);
            else return null;
        }

        /// <summary>Return the ancestors of this, NOT including the Root</summary>
        public List<IElementTreeNode> Ancestors
        {
            get
            {
                List<IElementTreeNode> result = new List<IElementTreeNode>();
                IParent current = Parent;
                while (current is IElementTreeNode element)
                {
                    result.Add(element);
                    current = element.Parent;
                }
                return result;
            }
        }

        /// <summary>Return the lowest common ancestor of this and <paramref name="anElementTreeNode"/> which is of type <typeparamref name="TElementBuilder"/>, or null if no such common ancestor exists</summary>
        internal TElementBuilder LowestCommonAncestor<TElementBuilder>(IElementTreeNode anElementTreeNode) where TElementBuilder: ElementBuilder => Ancestors.Intersect(anElementTreeNode.Ancestors)
            .Where(ancestor => ancestor is TElementBuilder)
            .Cast<TElementBuilder>()
            .OrderBy(ancestor => ancestor.Depth)
            .LastOrDefault();

        /// <summary>Return true if this ElementBuilder has a syntactic role as the specifier of <paramref name="governor"/></summary>
        internal bool Specifies(IElementTreeNode governor) => HasDirectOrIndirectRoleRelativeToHead(governor, ParentElementBuilder.ChildRole.Specifier);

        /// <summary>If necessary, reconfigure the appropriate things so this becomes a specifier of <paramref name="governor"/></summary>
        public void Specify(IElementTreeNode governor)
        {
            if (!Specifies(governor))
            {
                if (HasSameParentAs(governor) && governor.IsPhraseHead)
                    AssignedRole = ParentElementBuilder.ChildRole.Specifier;
                else
                {
                    if (governor.IsPhraseHead)
                        MoveTo(governor.Parent, ParentElementBuilder.ChildRole.Specifier);
                    else
                        MoveTo(governor.AsPhrase(), ParentElementBuilder.ChildRole.Specifier);
                }
            }
        }

        /// <summary>Return true if this ElementBuilder has a syntactic role as a modifier of <paramref name="governor"/></summary>
        internal bool Modifies(IElementTreeNode governor) => HasDirectOrIndirectRoleRelativeToHead(governor, ParentElementBuilder.ChildRole.Modifier);

        /// <summary>If necessary, reconfigure the appropriate things so this becomes a modifier of <paramref name="governor"/></summary>
        public virtual void Modify(IElementTreeNode governor)
        {
            if (!Modifies(governor))
            {
                if (HasSameParentAs(governor) && governor.IsPhraseHead)
                    AssignedRole = ParentElementBuilder.ChildRole.Modifier;
                else
                {
                    if (governor.IsPhraseHead)
                        MoveTo(governor.Parent, ParentElementBuilder.ChildRole.Modifier);
                    else
                        MoveTo(governor.AsPhrase(), ParentElementBuilder.ChildRole.Modifier);
                }
            }
        }

        /// <summary>Return true if this ElementBuilder has a syntactic role as a complement of <paramref name="governor"/></summary>
        internal bool Completes(IElementTreeNode governor) => HasDirectOrIndirectRoleRelativeToHead(governor, ParentElementBuilder.ChildRole.Complement);

        /// <summary>If necessary, reconfigure the appropriate things so this becomes a complement of <paramref name="governor"/></summary>
        public void Complete(IElementTreeNode governor)
        {
            if (!Completes(governor))
            {
                if (HasSameParentAs(governor) && governor.IsPhraseHead)
                    AssignedRole = ParentElementBuilder.ChildRole.Complement;
                else
                {
                    if (governor.IsPhraseHead)
                        MoveTo(governor.Parent, ParentElementBuilder.ChildRole.Complement);
                    else
                    {
                        if (IsPhraseHead)
                            Parent.MoveTo(governor.AsPhrase(), ParentElementBuilder.ChildRole.Complement);
                        else
                            MoveTo(governor.AsPhrase(), ParentElementBuilder.ChildRole.Complement);
                    }
                }
            }
        }

        /// <summary>Return true if this is part of a compound word with <paramref name="anotherElementBuilder"/></summary>
        internal virtual bool IsCompoundedWith(ElementBuilder anotherElementBuilder) => IsDirectlyCompoundedWith(anotherElementBuilder);

        /// <summary>Return true if this and <paramref name="anotherElementBuilder"/> are both Components in the same CompoundBuilder</summary>
        private protected bool IsDirectlyCompoundedWith(ElementBuilder anotherElementBuilder) => Parent is CompoundBuilder
            && anotherElementBuilder.Parent is CompoundBuilder
            && Parent == anotherElementBuilder.Parent
            && AssignedRole == ParentElementBuilder.ChildRole.Component && anotherElementBuilder.AssignedRole == ParentElementBuilder.ChildRole.Component;

        /// <summary>Return the syntactic relations that have at least one endpoint in the subtree of this</summary>
        public IEnumerable<SyntacticRelation> SyntacticRelationsWithAtLeastOneEndpointInSubtree => PartsOfSpeechInSubtree
            .Aggregate(new List<SyntacticRelation>(), (relations, partOfSpeech) =>
                {
                    relations.AddRange(partOfSpeech.IncomingSyntacticRelations);
                    relations.AddRange(partOfSpeech.OutgoingSyntacticRelations);
                    return relations;
                })
            .Distinct();

        #endregion Tree structure

        #region Configuration

        /// <summary>Attempt to transform this into a structure that can be realized by SimpleNLG.</summary>
        /// <remarks>The propagated "Coordinate" operation causes CoordinablePhraseBuilders to coordinate themselves.  This process may cause those phrase builders to change form.
        /// This ElementBuilder does NOT need to be the root of the tree in which it resides.  This allows the UI to selectively realize portions of a tree.</remarks>
        /// <returns>An IElementBuilder representing the transformed tree, if the transformation succeeds</returns>
        /// <exception cref="TreeCannotBeTransformedToRealizableFormException">If the transformation fails</exception>
        public IElementTreeNode AsRealizableTree()
        {
            try
            {
                return new RootNode(CopyLightweight())
                    .Propagate(Coordinate)
                    .Tree;
            }
            catch (Exception transformationException)
            {
                throw new TreeCannotBeTransformedToRealizableFormException(transformationException);
            }
        }

        /// <summary>Propagate the operation specified by <paramref name="operateOn"/> through the subtree of which this is the root, in depth-first fashion.</summary>
        /// <param name="operateOn">The operation to be applied during propagation</param>
        /// <returns>The result of performing <paramref name="operateOn"/>(this) after <paramref name="operateOn"/> has been invoked on all its descendants</returns>
        public abstract void Propagate(ElementTreeNodeOperation operateOn);

        /// <summary>Configure <paramref name="target"/></summary>
        public static void Configure(IElementTreeNode target) => target.Configure();

        /// <summary>Consolidate <paramref name="target"/></summary>
        public static void Consolidate(IElementTreeNode target) => target.Consolidate();

        /// <summary>Coordinate <paramref name="target"/></summary>
        public static void Coordinate(IElementTreeNode target) => target.Coordinate();
        
        /// <summary>Apply dependencies for all the PartOfSpeechBuilders in the descendant tree</summary>
        public IElementTreeNode ApplyDependencies()
        {
            IEnumerable<IGrouping<PartOfSpeechBuilder, SyntacticRelation>> relationsGroupedByGovernor = SyntacticRelationsWithAtLeastOneEndpointInSubtree
                .GroupBy(relation => relation.Governor);
            foreach (IGrouping<PartOfSpeechBuilder, SyntacticRelation> relationsForGovernor in relationsGroupedByGovernor)
            {
                relationsForGovernor.Key.ApplyRelations(relationsForGovernor);
            }
            return this;
        }

        /// <summary>The default implementation of Configure.  All the interesting stuff takes place in subclass overrides.</summary>
        /// <returns>The result of applying Configure to this.  May or may not be the same object as this.</returns>
        public virtual void Configure() { }

        /// <summary>The default implementation of Coordinate.  All the interesting stuff takes place in subclass overrides.</summary>
        /// <returns>The result of applying Coordinate to this.  May or may not be the same object as this.</returns>
        public virtual void Coordinate() { }

        /// <summary>The default implementation of Consolidate.  All the interesting stuff takes place in subclass overrides.</summary>
        /// <returns>The result of applying Consolidate to this.  May or may not be the same object as this.</returns>
        public virtual void Consolidate() { }

        /// <summary>If this ElementBuilder has a parent, remove that parent's child relation to this</summary>
        public IElementTreeNode DetachFromParent()
        {
            Parent?.RemoveChild(this);
            Parent = null;
            return this;
        }

        /// <summary><list type="bullet">
        /// <item>Detach this from its current parent</item>
        /// <item>Add it as a child of <paramref name="newParent"/> with ChildRole <paramref name="newRole"/></item>
        /// </list></summary>
        public void MoveTo(IParent newParent, ParentElementBuilder.ChildRole newRole)
        {
            IElementTreeNode oldParent = Parent as IElementTreeNode;
            DetachFromParent();
            newParent.AddChildWithRole(this, newRole);
            oldParent?.Consolidate();
        }

        /// <summary><list type="bullet">
        /// <item>Detach this from its current parent</item>
        /// <item>Add it as a child of <paramref name="newParent"/> with a ChildRole selected by the new parent</item>
        /// <item>Notify listeners that the tree structure has changed</item>
        /// </list></summary>
        public void MoveTo(IParent newParent)
        {
            IElementTreeNode oldParent = Parent as IElementTreeNode;
            DetachFromParent();
            newParent.AddChild(this);
            oldParent?.Consolidate();
        }

        /// <summary>Update references from other objects so <paramref name="replacement"/> replaces this in the ElementBuilder tree</summary>
        /// <remarks>Invoking this method with <paramref name="replacement"/> == null will cause this to vanish from the tree.</remarks>
        /// <returns>The replacement IElementBuilder</returns>
        internal IElementTreeNode Become(IElementTreeNode replacement)
        {
            replacement?.DetachFromParent();
            Parent?.ReplaceChild(this, replacement);
            return replacement;
        }

        /// <summary>Return a "lighweight" copy of the subtree rooted in this ElementBuilder.</summary>
        /// <remarks>A lightweight copy has the following properties:
        /// <list type="bullet">
        /// <item>The NLGElement structure of the SimpleNLG spec to build is nulled out.  The lightweight tree is still capable of recreating this structure through BuildElement().</item>
        /// <item>Dependency relations between parts of speech are removed.  ApplyDependencies() can still be called on the lightweight tree, but it will have no effect.</item>
        /// </list>
        /// Before calling BuildElement() on a lightweight tree, the Coordinate operation should be propagated through it. 
        /// <para>Creating a copy allows the "heavyweight" tree to be edited in the user interface -- which process causes the tree structure to change -- while the realization process
        /// is tested on copies.</para></remarks>
        public abstract IElementTreeNode CopyLightweight();

        #endregion Configuration

        /// <summary>Build and return the <see cref="NLGElement"/> represented by this ElementBuilder</summary>
        public abstract NLGElement BuildElement();

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion Implementation of INotifyPropertyChanged
 
    }
}

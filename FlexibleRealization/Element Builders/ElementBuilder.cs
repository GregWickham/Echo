using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using SimpleNLG;
using FlexibleRealization.Dependencies;

namespace FlexibleRealization
{
    public abstract partial class ElementBuilder : IElementBuilder, IElementTreeNode, INotifyPropertyChanged, IComparable<IElementTreeNode>
    {
        #region Tree structure

        /// <summary>Can be called by an element anywhere in the subtree to raise the TreeStructureChanged event for the tree</summary>
        /// <remarks>This can happen when the tree is not fully constructed, and there's no intact chain of Ancestors leading to the Root.
        /// In that case the null checks will fail and the event will not be raised.</remarks>
        internal void OnTreeStructureChanged() => Root?.OnTreeStructureChanged();

        public IParent Parent { get; set; }

        /// <summary>Return the number of parent-child relations between this ElementBuilder and the root of the graph containing it.</summary>
        public int Depth => this == Stem ? 0 : Parent.Depth + 1;

        /// <summary>Return the root ParentElementBuilder of the tree containing this.  Provides a fixed object that clients can refer to, 
        /// which is guaranteed not to disappear during transformations of the tree.</summary>
        /// <remarks>The implementation is recursive, terminating with RootNode.</remarks>
        public RootNode Root => Parent?.Root;

        /// <summary>Return the topmost IElementTreeNode in the tree containing this.</summary>
        /// <remarks>In some situations a tree may not have a Root, but it always has a Stem.</remarks>
        public IElementTreeNode Stem => (Parent == null || Parent == Root) ? this : Parent.Stem;

        /// <summary>Return true if this has the same parent as <paramref name="anotherElement"/></summary>
        internal bool HasSameParentAs(IElementTreeNode anotherElement) => Parent == anotherElement.Parent;

        /// <summary>Return the ElementBuilders that are direct children of this.</summary>
        public abstract IEnumerable<IElementTreeNode> Children { get; }

        /// <summary>Return true if this is a direct child of <paramref name="prospectiveParent"/>.</summary>
        public bool IsChildOf(ParentElementBuilder prospectiveParent) => prospectiveParent.Children.Contains(this);

        /// <summary>Return the ancestors of this, NOT including the Root.</summary>
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

        public bool IsInSubtreeOf(IElementTreeNode node) => node == this || Ancestors.Contains(node);

        ///Return true if this is a phrase head.
        public virtual bool IsPhraseHead => AssignedRole == ParentElementBuilder.ChildRole.Head;

        /// <summary>Return this transformed into a phrase of the appropriate type, or null if this cannot be transformed into a phrase.</summary>
        public virtual PhraseBuilder AsPhrase() => null;

        /// <summary>If this can be converted to a phrase, return that phrase.  If not, return this.</summary>
        public virtual IElementTreeNode AsPhraseIfConvertible() => this.AsPhrase() ?? this;

        /// <summary>Return all the IElementBuilders in the subtree of which this is the root.</summary>
        public virtual IEnumerable<IElementTreeNode> DescendentBuilders => new List<IElementTreeNode>();

        /// <summary>Return all the IElementBuilders in the subtree of which this is the root.</summary>
        public virtual IEnumerable<IElementTreeNode> WithAllDescendentBuilders => new List<IElementTreeNode> { this };

        /// <summary>Return all the descendants of this of type TElement, NOT including this.</summary>
        public IEnumerable<TElement> GetDescendentElementsOfType<TElement>() where TElement : ElementBuilder => DescendentBuilders.Where(element => element is TElement).Cast<TElement>();

        /// <summary>Return all elements in the subtree of which this is the root, which are of type TElement.</summary>
        public IEnumerable<TElement> GetElementsOfTypeInSubtree<TElement>() where TElement : ElementBuilder => this.WithAllDescendentBuilders.Where(element => element is TElement).Cast<TElement>();

        /// <summary>Implements IComparable.</summary>
        /// <remarks>This implementation works ONLY with the relocatable ordering.  DO NOT call it before Tokens are removed and relocatable ordering is installed.</remarks>
        public virtual int CompareTo(IElementTreeNode node) => LowestCommonAncestor<ParentElementBuilder>(node).CompareDescendants(this, node);

        /// <summary>Return the WordElementBuilder descended from this which most immediately follows <paramref name="node"/>, of null if there is no such WordElementBuilder</summary>
        public WordElementBuilder WordFollowing(IElementTreeNode node) => Stem.GetElementsOfTypeInSubtree<WordElementBuilder>()
            .Where(word => word.Index > node.MaximumIndex)
            .OrderBy(word => word.Index)
            .FirstOrDefault();

        /// <summary>Return the index of <paramref name="partOfSpeech"/> within the subtree of this</summary>
        public int RelativeIndexOf(PartOfSpeechBuilder partOfSpeech) => GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
            .OrderBy(partOfSpeech => partOfSpeech)
            .ToList()
            .IndexOf(partOfSpeech) + 1;

        /// <summary>Return the smallest token index of the PartOfSpeechBuilders spanned by this</summary>
        /// <remarks>Because PartOfSpeechBuilder.Index works for both index-based and relocatable ordering, this method also works for both.</remarks>
        public int MinimumIndex => GetElementsOfTypeInSubtree<PartOfSpeechBuilder>().Min(partOfSpeech => partOfSpeech.Index);

        /// <summary>Return the largest token index of the PartOfSpeechBuilders spanned by this</summary>
        /// <remarks>Because PartOfSpeechBuilder.Index works for both index-based and relocatable ordering, this method also works for both.</remarks>
        public int MaximumIndex => GetElementsOfTypeInSubtree<PartOfSpeechBuilder>().Max(partOfSpeech => partOfSpeech.Index);

        /// <summary>Return true if all PartOfSpeechBuilders spanned by this ElementBuilder precede all PartOfSpeechBuilders spanned by <paramref name="theOtherElement"/></summary>
        public bool ComesBefore(IElementTreeNode theOtherElement) => CompareTo(theOtherElement) < 0;

        /// <summary>Return true if all PartOfSpeechBuilders spanned by <paramref name="theOtherElement"/> precede all PartOfSpeechBuilders spanned by this ElementBuilder</summary>
        public bool ComesAfter(IElementTreeNode theOtherElement) => CompareTo(theOtherElement) > 0;

        /// <summary>The list of ChildRoles an instance can have if it has no parent.  Only one option.</summary>
        private static readonly List<ParentElementBuilder.ChildRole> NoParentRolesList = new List<ParentElementBuilder.ChildRole> { ParentElementBuilder.ChildRole.NoParent };

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
        private static readonly List<ParentElementBuilder.ChildRole> HeadLikeRoles = new List<ParentElementBuilder.ChildRole>
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

        /// <summary>Return the Ancestors of this which are of type TElementBuilder</summary>
        IEnumerable<TElementBuilder> GetAncestorsOfType<TElementBuilder>() where TElementBuilder : ElementBuilder => Ancestors.Where(ancestor => ancestor is TElementBuilder).Cast<TElementBuilder>();

        /// <summary>Return the lowest Ancestor of this which is of type TElementBuilder, or null if no such Ancestor exists</summary>
        public TElementBuilder LowestAncestorOfType<TElementBuilder>() where TElementBuilder : ElementBuilder => GetAncestorsOfType<TElementBuilder>().OrderBy(ancestor => ancestor.Depth).LastOrDefault();

        /// <summary>Return the lowest common ancestor of this and <paramref name="anElementTreeNode"/> which is of type <typeparamref name="TElementBuilder"/>, or null if no such common ancestor exists</summary>
        internal TElementBuilder LowestCommonAncestor<TElementBuilder>(IElementTreeNode anElementTreeNode) where TElementBuilder: ElementBuilder => Ancestors.Intersect(anElementTreeNode.Ancestors)
            .Where(ancestor => ancestor is TElementBuilder)
            .Cast<TElementBuilder>()
            .OrderBy(ancestor => ancestor.Depth)
            .LastOrDefault();

        /// <summary>Return true if this ElementBuilder has a syntactic role as the specifier of <paramref name="governor"/></summary>
        internal bool Specifies(IElementTreeNode governor) => HasDirectOrIndirectRoleRelativeToHead(governor, ParentElementBuilder.ChildRole.Specifier);

        /// <summary>If necessary, reconfigure the appropriate things so this becomes a specifier of <paramref name="governor"/></summary>
        /// <returns>The IParent to which this was actually added as a Specifier, or null if no such addition was done.</returns>
        public IParent Specify(IElementTreeNode governor)
        {
            if (Specifies(governor)) return null;
            else
            {
                IParent specifiedParent;
                if (HasSameParentAs(governor) && governor.IsPhraseHead)
                {
                    specifiedParent = Parent;
                    AssignedRole = ParentElementBuilder.ChildRole.Specifier;
                }
                else
                {
                    specifiedParent = governor is PhraseBuilder phrase
                        ? phrase
                        : governor.IsPhraseHead
                            ? governor.Parent
                            : governor.AsPhrase();
                    MoveTo(specifiedParent, ParentElementBuilder.ChildRole.Specifier);
                }
                return specifiedParent;
            }
        }

        /// <summary>Return true if this ElementBuilder has a syntactic role as a modifier of <paramref name="governor"/></summary>
        internal bool Modifies(IElementTreeNode governor) => HasDirectOrIndirectRoleRelativeToHead(governor, ParentElementBuilder.ChildRole.Modifier);

        /// <summary>If necessary, reconfigure the appropriate things so this becomes a modifier of <paramref name="governor"/></summary>
        /// <returns>The IParent to which this was actually added as a Modifier, or null if no such addition was done.</returns>
        public virtual IParent Modify(IElementTreeNode governor)
        {
            if (Modifies(governor)) return null;
            else
            {
                IParent modifiedParent;
                if (HasSameParentAs(governor) && governor.IsPhraseHead)
                {
                    modifiedParent = Parent;
                    AssignedRole = ParentElementBuilder.ChildRole.Modifier;
                }
                else
                {
                    modifiedParent = governor is PhraseBuilder phrase
                        ? phrase
                        : governor.IsPhraseHead
                            ? governor.Parent
                            : governor.AsPhrase();
                    MoveTo(modifiedParent, ParentElementBuilder.ChildRole.Modifier);
                }
                return modifiedParent;
            }
        }

        /// <summary>Return true if this ElementBuilder has a syntactic role as a complement of <paramref name="governor"/></summary>
        internal bool Completes(IElementTreeNode governor) => HasDirectOrIndirectRoleRelativeToHead(governor, ParentElementBuilder.ChildRole.Complement);

        /// <summary>If necessary, reconfigure the appropriate things so this becomes a complement of <paramref name="governor"/></summary>
        /// <returns>The IParent to which this was actually added as a Complement, or null if no such addition was done.</returns>
        public IParent Complete(IElementTreeNode governor)
        {
            if (Completes(governor)) return null;
            else
            {
                IParent completedParent;
                if (HasSameParentAs(governor) && governor.IsPhraseHead)
                {
                    completedParent = Parent;
                    AssignedRole = ParentElementBuilder.ChildRole.Complement;
                }
                else
                {
                    completedParent = governor is PhraseBuilder phrase
                        ? phrase
                        : governor.IsPhraseHead
                            ? governor.Parent
                            : governor.AsPhrase();
                    MoveTo(completedParent, ParentElementBuilder.ChildRole.Complement);
                }
                return completedParent;
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
        public IEnumerable<SyntacticRelation> SyntacticRelationsWithAtLeastOneEndpointInSubtree => GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
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
                    .Stem;
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

        /// <summary>Use the part of speech indices from ParseTokens to order children in a relocatable way -- i.e., not based on absolute indices.</summary>
        public static void CreateChildOrderings(IElementTreeNode target) => target.CreateChildOrderingsFromIndices();

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
        public virtual void Configure() { }

        /// <summary>The default implementation of Coordinate.  All the interesting stuff takes place in subclass overrides.</summary>
        public virtual void Coordinate() { }

        /// <summary>The default implementation of Consolidate.  All the interesting stuff takes place in subclass overrides.</summary>
        public virtual void Consolidate() { }

        public abstract void CreateChildOrderingsFromIndices();

        /// <summary>If this ElementBuilder has a parent, remove that parent's child relation to this.</summary>
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
        public bool MoveTo(IParent newParent, ParentElementBuilder.ChildRole newRole)
        {
            IElementTreeNode oldParent = Parent as IElementTreeNode;
            DetachFromParent();
            newParent.AddChildWithRole(this, newRole);
            oldParent?.Consolidate();
            return true;
        }

        /// <summary><list type="bullet">
        /// <item>Detach this from its current parent;</item>
        /// <item>Add it as a child of <paramref name="newParent"/> with a ChildRole selected by the new parent;</item>
        /// <item>Notify listeners that the tree structure has changed;</item>
        /// <item>Return true to indicate success.</item>
        /// </list></summary>
        public bool MoveTo(IParent newParent)
        {
            IElementTreeNode oldParent = Parent as IElementTreeNode;
            DetachFromParent();
            newParent.AddChild(this);
            oldParent?.Consolidate();
            return true;
        }

        /// <summary>Remove this node from the tree that contains it.</summary>
        public void Remove()
        {
            //RootNode root = Root;
            Become(null);
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

        /// <summary>Insert this ElementBuilder into the tree containing <paramref name="insertPoint"/>, either before or after <paramref name="insertPoint"/> depending on <paramref name="relation"/></summary>
        public void SetOrderingRelativeTo(IElementTreeNode insertPoint, NodeRelation relation) => Parent.SetChildOrdering(this, insertPoint, relation);

        /// <summary>Return a "lighweight" copy of the subtree rooted in this ElementBuilder.</summary>
        /// <remarks>A lightweight copy has the following properties:
        /// <list type="bullet">
        /// <item>The NLGElement structure of the SimpleNLG spec to build is nulled out.  The lightweight tree is still capable of recreating this structure through BuildElement().</item>
        /// <item>Dependency relations between parts of speech are removed.  ApplyDependencies() can still be called on the lightweight tree, but it will have no effect.</item>
        /// <item>Elements that are capable of generating variations are resolved to one specific form.</item>
        /// </list>
        /// Before calling BuildElement() on a lightweight tree, the Coordinate operation should be propagated through it. 
        /// <para>Creating a copy allows the "heavyweight" tree to be edited in the user interface -- which process causes the tree structure to change -- while the realization process
        /// is carried out on lightweight copies.</para></remarks>
        public abstract IElementTreeNode CopyLightweight();

        #endregion Configuration

        #region Variations

        ///Return an IEnumerator for the variations of this
        public virtual IEnumerator<IElementTreeNode> GetVariationsEnumerator() => new List<IElementTreeNode> { this }.GetEnumerator();

        /// <summary>Return the total number of forms specified for this element.</summary>
        public abstract int CountForms();

        /// <summary>Return the realizable variations of this</summary>
        public virtual IEnumerable<IElementTreeNode> GetRealizableVariations() => new List<IElementTreeNode> { this.AsRealizableTree() };

        #endregion Variations

        #region Database

        /// <summary>The ID of this element in the Flex database</summary>
        public int FlexDB_ID { get; set; } = 0;

        #endregion Database

        #region Build and Realize

        /// <summary>Build and return the <see cref="NLGElement"/> represented by this ElementBuilder</summary>
        public abstract NLGElement BuildElement();

        /// <summary>Build the NLGElement and wrap it in an NLGSpec</summary>
        /// <returns><see cref="NLGSpec"/></returns>
        NLGSpec IElementBuilder.ToNLGSpec()
        {
            try
            {
                return new NLGSpec
                {
                    Item = new RequestType
                    {
                        Document = new DocumentElement
                        {
                            cat = documentCategory.DOCUMENT,
                            catSpecified = true,
                            child = new NLGElement[]
                            {
                                BuildElement()
                            }
                        }
                    }
                };
            }
            catch (Exception buildException)
            {
                throw new SpecCannotBeBuiltException(buildException);
            }
        }

        /// <summary>Try to transform this IElementTreeNode into realizable form and if successful, try to realize it.</summary>
        /// <returns>A <see cref="RealizationResult"/> containing:
        /// <list type="bullet"><item>The <see cref="RealizationOutcome"/></item>
        /// <item>The serialized XML if Transform / BuildElement / Serialize succeeded</item>
        /// <item>The realized text if realization succeeded</item></list></returns>
        RealizationResult IElementTreeNode.Realize()
        {
            RealizationResult result = new RealizationResult();
            try
            {
                IElementBuilder realizableTree = this.AsRealizableTree();
                NLGSpec spec = realizableTree.ToNLGSpec();
                result.XML = spec.Serialize();
                result.Text = SimpleNLG.Client.Realize(result.XML);
                result.Outcome = RealizationOutcome.Success;
            }
            catch (TreeCannotBeTransformedToRealizableFormException)
            {
                result.Outcome = RealizationOutcome.FailedToTransform;
            }
            catch (SpecCannotBeBuiltException)
            {
                result.Outcome = RealizationOutcome.FailedToBuildSpec;
            }
            return result;
        }

        #endregion Build and Realize

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        #endregion Implementation of INotifyPropertyChanged
 
    }
}

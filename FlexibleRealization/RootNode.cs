using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleRealization.Dependencies;

namespace FlexibleRealization
{
    public delegate void TreeStructureChanged_EventHandler(RootNode root);

    /// <summary>Every tree of IElementTreeNodes has exactly one RootNode at its root.</summary>
    /// <remarks>The RootNode is a fixed object that's always guaranteed to be there, for the life of the tree.  Clients outside the tree can use the RootNode
    /// to maintain a reference to the tree that will never change identity or go away, regardless of what transformations take place within the tree.</remarks>
    public class RootNode : IParent
    {
        /// <summary>Construct a new RootNode to be the root of <paramref name="tree"/></summary>
        public RootNode(IElementTreeNode stem) 
        { 
            Stem = stem;
            stem.Parent = this;
        }

        /// <summary>Notify listeners that the structure of the tree has changed</summary>
        public event TreeStructureChanged_EventHandler TreeStructureChanged;

        /// <summary>Notify listeners that the tree structure has changed</summary>
        public void OnTreeStructureChanged() => TreeStructureChanged?.Invoke(this);

        /// <summary>The element builder tree of which this is the RootNode</summary>
        public IElementTreeNode Stem { get; set; }

        /// <summary>Return a lightweight copy of this RootNode and its tree</summary>
        public RootNode CopyLightweight() => new RootNode(Stem.CopyLightweight());

        /// <summary>The CoreNLP parser gave us an unstructured list of semantic <paramref name="dependencies"/> between parts of speech.  Now that we've assembled a tree structure from the constituency
        /// parse, and all the PartOfSpeechBuilder elements are in place, we can go through the list of <paramref name="dependencies"/> and create corresponding
        /// SyntacticRelation objects that link our PartOfSpeechBuilder objects to one another.</summary>
        public RootNode AttachDependencies(List<(string Relation, string Specifier, int GovernorIndex, int DependentIndex)> dependencies)
        {
            foreach ((string Relation, string Specifier, int GovernorIndex, int DependentIndex) eachDependencyTuple in dependencies)
            {
                PartOfSpeechBuilder governor = Stem.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
                    .Where(partOfSpeech => partOfSpeech.Token.Index == eachDependencyTuple.GovernorIndex)
                    .FirstOrDefault();
                PartOfSpeechBuilder dependent = Stem.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
                    .Where(partOfSpeech => partOfSpeech.Token.Index == eachDependencyTuple.DependentIndex)
                    .FirstOrDefault();
                if (governor != null && dependent != null)
                {
                    SyntacticRelation
                        .OfType(eachDependencyTuple.Relation, eachDependencyTuple.Specifier)
                        .Between(governor, dependent)
                        .Install();
                }
            }
            return this;
        }

        /// <summary>Apply dependencies for all the PartOfSpeechBuilders in the <see cref="Tree"/></summary>
        public RootNode ApplyDependencies()
        {
            IEnumerable<IGrouping<PartOfSpeechBuilder, SyntacticRelation>> relationsGroupedByGovernor = Stem.SyntacticRelationsWithAtLeastOneEndpointInSubtree
                .GroupBy(relation => relation.Governor);
            foreach (IGrouping<PartOfSpeechBuilder, SyntacticRelation> relationsForGovernor in relationsGroupedByGovernor)
            {
                relationsForGovernor.Key.ApplyRelations(relationsForGovernor);
            }
            return this;
        }

        /// <summary>Transfer any necessary information from ParseTokens to PartOfSpeechBuilders, and strip the ParseTokens out of the tree</summary>
        /// <remarks>From this point forward, we might make changes in the UI that render the ParseTokens incorrect, and we also don't want to mess with them
        /// when saving elements to the database.</remarks>
        public RootNode RemoveParseTokens()
        {
            Stem.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>().ToList().ForEach(partOfSpeech =>
            {
                //partOfSpeech.Index = partOfSpeech.Token.Index;
                partOfSpeech.Token = null;
            });
            return this;
        }

        /// <summary>Propagate <paramref name="operateOn"/> through the <see cref="Tree"/></summary>
        public RootNode Propagate(ElementTreeNodeOperation operateOn)
        {
            Stem.Propagate(operateOn);
            return this;
        }

        #region Explicit implementation of IParent

        int IParent.Depth => throw new InvalidOperationException("RootNode Depth is undefined");

        RootNode IParent.Root => this;

        /// <summary>The only valid role for the child of root is NoParent</summary>
        List<ParentElementBuilder.ChildRole> IParent.ValidRolesForChild(ElementBuilder child) => noParentList;
        private static List<ParentElementBuilder.ChildRole> noParentList = new List<ParentElementBuilder.ChildRole> { ParentElementBuilder.ChildRole.NoParent };

        void IParent.AddChild(IElementTreeNode child) => throw new InvalidOperationException("RootNode can't add children");

        void IParent.AddChildWithRole(IElementTreeNode child, ParentElementBuilder.ChildRole role) => throw new InvalidOperationException("RootNode can't add children");

        /// <summary>The only valid role for the child of root is NoParent</summary>
        ParentElementBuilder.ChildRole IParent.RoleFor(IElementTreeNode child) => ParentElementBuilder.ChildRole.NoParent;

        void IParent.SetRoleOfChild(IElementTreeNode child, ParentElementBuilder.ChildRole newRole) => throw new InvalidOperationException("RootNode can't change child roles");

        void IParent.SetChildOrdering(IElementTreeNode childToOrder, IElementTreeNode childToOrderRelativeTo, NodeRelation relation) { }

        void IParent.SetChildOrdering(IElementTreeNode childToOrder, Dictionary<PartOfSpeechBuilder, int> partsOfSpeech) { }

        void IParent.RemoveChild(IElementTreeNode child) => throw new InvalidOperationException("RootNode can't remove its only child");

        void IParent.ReplaceChild(IElementTreeNode existingChild, IElementTreeNode newChild)
        {
            if (existingChild == Stem)
            {
                newChild.Parent = this;
                Stem = newChild;
            }
            else throw new InvalidOperationException("RootNode can't replace a child that it doesn't currently have");
        }

        bool IParent.MoveTo(IParent newParent, ParentElementBuilder.ChildRole role) => throw new InvalidOperationException("RootNode can't move");

        /// <summary>A RootNode doesn't participate in syntax</summary>
        bool IParent.ActsAsHeadOf(PhraseBuilder phrase) => false;

        /// <summary>A RootNode doesn't participate in syntax</summary>
        bool IParent.ActsWithRoleInAncestor(ParentElementBuilder.ChildRole role, ParentElementBuilder ancestor) => false;

        /// <summary>A RootNode doesn't have Ancestors, and it doesn't participate in syntax</summary>
        IParent IParent.AncestorOfWhichThisIsDirectlyOrIndirectlyA(ParentElementBuilder.ChildRole role) => null;

        #endregion Explicit implementation of IParent
    }
}

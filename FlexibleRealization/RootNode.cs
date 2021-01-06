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
        public RootNode(IElementTreeNode tree) 
        { 
            Tree = tree;
            tree.Parent = this;
        }

        /// <summary>Notify listeners that the structure of the tree has changed</summary>
        public event TreeStructureChanged_EventHandler TreeStructureChanged;

        /// <summary>Notify listeners that the tree structure has changed</summary>
        internal void OnTreeStructureChanged() => TreeStructureChanged?.Invoke(this);

        /// <summary>The <see cref="Tree"/> of which this is the RootNode</summary>
        public IElementTreeNode Tree { get; set; }

        /// <summary>Return a lightweight copy of this RootNode and its <see cref="Tree"/></summary>
        public RootNode CopyLightweight() => new RootNode(Tree.CopyLightweight());

        /// <summary>The CoreNLP parser gave us an unstructured list of semantic <paramref name="dependencies"/> between parts of speech.  Now that we've assembled a tree structure from the constituency
        /// parse, and all the PartOfSpeechBuilder elements are in place, we can go through the list of <paramref name="dependencies"/> and create corresponding
        /// SyntacticRelation objects that link our PartOfSpeechBuilder objects to one another.</summary>
        public RootNode AttachDependencies(List<(string Relation, string Specifier, int GovernorIndex, int DependentIndex)> dependencies)
        {
            foreach ((string Relation, string Specifier, int GovernorIndex, int DependentIndex) eachDependencyTuple in dependencies)
            {
                PartOfSpeechBuilder governor = Tree.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
                    .Where(partOfSpeech => partOfSpeech.Token.Index == eachDependencyTuple.GovernorIndex)
                    .FirstOrDefault();
                PartOfSpeechBuilder dependent = Tree.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
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
            IEnumerable<IGrouping<PartOfSpeechBuilder, SyntacticRelation>> relationsGroupedByGovernor = Tree.SyntacticRelationsWithAtLeastOneEndpointInSubtree
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
            Tree.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>().ToList().ForEach(partOfSpeech =>
            {
                partOfSpeech.Index = partOfSpeech.Token.Index;
                partOfSpeech.Token = null;
            });
            return this;
        }

        /// <summary>Re-index the parts of speech in the tree that contains <paramref name="insertPoint"/> to accommodate <paramref name="toBeInserted"/> before <paramref name="insertPoint"/> </summary>
        internal void InsertBefore(ElementBuilder insertPoint, ElementBuilder toBeInserted)
        {
            List<PartOfSpeechBuilder> partsOfSpeechToBeInserted = toBeInserted.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
                .OrderBy(partOfSpeech => partOfSpeech.Index)
                .ToList();
            List<PartOfSpeechBuilder> partsOfSpeechAfterInsertPoint = Tree.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>()
                .Where(partOfSpeech => partOfSpeech.Index >= insertPoint.MaximumIndex)
                .OrderBy(partOfSpeech => partOfSpeech.Index)
                .ToList();
            int movedIndex = insertPoint.MinimumIndex + partsOfSpeechToBeInserted.Count;
            int insertedIndex = insertPoint.MinimumIndex;
            foreach (PartOfSpeechBuilder eachMovedPartOfSpeech in partsOfSpeechAfterInsertPoint)
            {
                eachMovedPartOfSpeech.Index = movedIndex;
                movedIndex++;
            }
            foreach (PartOfSpeechBuilder eachInsertedPartOfSpeech in partsOfSpeechToBeInserted)
            {
                eachInsertedPartOfSpeech.Index = insertedIndex;
                insertedIndex++;
            }
        }

        /// <summary>Propagate <paramref name="operateOn"/> through the <see cref="Tree"/></summary>
        public RootNode Propagate(ElementTreeNodeOperation operateOn)
        {
            Tree.Propagate(operateOn);
            return this;
        }

        public int Depth => throw new InvalidOperationException("RootNode Depth is undefined");

        public RootNode Root => this;

        /// <summary>The only valid role for the child of root is NoParent</summary>
        public List<ParentElementBuilder.ChildRole> ValidRolesForChild(ElementBuilder child) => new List<ParentElementBuilder.ChildRole> { ParentElementBuilder.ChildRole.NoParent };

        public void AddChild(IElementTreeNode child) => throw new InvalidOperationException("RootNode can't add children");

        public void AddChildWithRole(IElementTreeNode child, ParentElementBuilder.ChildRole role) => throw new InvalidOperationException("RootNode can't add children");

        /// <summary>The only valid role for the child of root is NoParent</summary>
        public ParentElementBuilder.ChildRole RoleFor(IElementTreeNode child) => ParentElementBuilder.ChildRole.NoParent;

        public void SetRoleOfChild(IElementTreeNode child, ParentElementBuilder.ChildRole newRole) => throw new InvalidOperationException("RootNode can't change child roles");

        public void RemoveChild(IElementTreeNode child) => throw new InvalidOperationException("RootNode can't remove its only child");

        public void ReplaceChild(IElementTreeNode existingChild, IElementTreeNode newChild)
        {
            if (existingChild == Tree)
            {
                newChild.Parent = this;
                Tree = newChild;
            }
            else throw new InvalidOperationException("RootNode can't replace a child that it doesn't currently have");
        }

        public void MoveTo(IParent newParent, ParentElementBuilder.ChildRole role) => throw new InvalidOperationException("RootNode can't move");

        /// <summary>A RootNode doesn't participate in syntax</summary>
        public bool ActsAsHeadOf(PhraseBuilder phrase) => false;

        /// <summary>A RootNode doesn't participate in syntax</summary>
        public bool ActsWithRoleInAncestor(ParentElementBuilder.ChildRole role, ParentElementBuilder ancestor) => false;

        /// <summary>A RootNode doesn't have Ancestors, and it doesn't participate in syntax</summary>
        public IParent AncestorOfWhichThisIsDirectlyOrIndirectlyA(ParentElementBuilder.ChildRole role) => null;
    }
}

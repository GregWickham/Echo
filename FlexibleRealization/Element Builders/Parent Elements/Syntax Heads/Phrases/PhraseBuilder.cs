using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>The base class of all PhraseElement builders.</summary>
    /// <remarks>The inheritance hierarchy of PhraseBuilder roughly corresponds to the hierarchy of <see cref="PhraseElement"/>s defined by the SimpleNLG Realizer Schema.</remarks>
    public abstract class PhraseBuilder : SyntaxHeadBuilder
    {
        private protected PhraseBuilder() : base() { }
        
        public abstract phraseCategory PhraseCategory { get; }

        /// <summary>Since this is already a phrase, return this</summary>
        public override PhraseBuilder AsPhrase() => this;

        /// <summary>Return the heads of this phrase</summary>
        internal IEnumerable<IElementTreeNode> Heads => ChildrenWithRole(ChildRole.Head);

        /// <summary>Return the modifiers of this phrase</summary>
        internal IEnumerable<IElementTreeNode> Modifiers => ChildrenWithRole(ChildRole.Modifier);

        /// <summary>Return the complements of this phrase</summary>
        internal IEnumerable<IElementTreeNode> Complements => ChildrenWithRole(ChildRole.Complement);

        internal void AddHead(IElementTreeNode head) => AddChildWithRole(head, ChildRole.Head);
        private protected void AddHeads(IEnumerable<IElementTreeNode> newHeads) => newHeads.ToList().ForEach(newHead => AddHead(newHead));

        /// <summary>Return a dictionary containing:
        /// <list type="table"><item>(Keys) the children of this ParentElementBuilder with ChildRole = Head; and</item>
        /// <item>(Values) the zero-based index of each head, relative to this.</item></list></summary>
        private protected Dictionary<IElementTreeNode, int> GetHeadsAndIndices()
        {
            SortedList<IElementTreeNode, object> sortedChildren = new SortedList<IElementTreeNode, object>();
            Children.ToList().ForEach(child => sortedChildren.Add(child, null));
            Dictionary<IElementTreeNode, int> result = new Dictionary<IElementTreeNode, int>();
            Heads.ToList().ForEach(head => result.Add(head, sortedChildren.IndexOfKey(head)));
            return result;
        }

        private protected void AddModifier(IElementTreeNode modifier) => AddChildWithRole(modifier, ChildRole.Modifier);
        private protected void AddModifiers(IEnumerable<IElementTreeNode> newModifiers) => newModifiers.ToList().ForEach(newModifier => AddModifier(newModifier));

        /// <summary>Return a dictionary containing:
        /// <list type="table"><item>(Keys) the children of this ParentElementBuilder with ChildRole = Modifier; and</item>
        /// <item>(Values) the zero-based index of each modifier, relative to this.</item></list></summary>
        private protected Dictionary<IElementTreeNode, int> GetModifiersAndIndices()
        {
            SortedList<IElementTreeNode, object> sortedChildren = new SortedList<IElementTreeNode, object>();
            Children.ToList().ForEach(child => sortedChildren.Add(child, null));
            Dictionary<IElementTreeNode, int> result = new Dictionary<IElementTreeNode, int>();
            Modifiers.ToList().ForEach(modifier => result.Add(modifier, sortedChildren.IndexOfKey(modifier)));
            return result;
        }

        private protected void AddComplement(IElementTreeNode complement) => AddChildWithRole(complement, ChildRole.Complement);
        private protected void AddComplements(IEnumerable<IElementTreeNode> newComplements) => newComplements.ToList().ForEach(newComplement => AddComplement(newComplement));

        /// <summary>Return a dictionary containing:
        /// <list type="table"><item>(Keys) the children of this ParentElementBuilder with ChildRole = Complement; and</item>
        /// <item>(Values) the zero-based index of each complement, relative to this.</item></list></summary>
        private protected Dictionary<IElementTreeNode, int> GetComplementsAndIndices()
        {
            SortedList<IElementTreeNode, object> sortedChildren = new SortedList<IElementTreeNode, object>();
            Children.ToList().ForEach(child => sortedChildren.Add(child, null));
            Dictionary<IElementTreeNode, int> result = new Dictionary<IElementTreeNode, int>();
            Complements.ToList().ForEach(complement => result.Add(complement, sortedChildren.IndexOfKey(complement)));
            return result;
        }

        /// <summary>Default override of Consolidate for PhraseBuilders.</summary>
        /// <remarks>When a PhraseBuilder has exactly one child, we do NOT eliminate the PhraseBuilder and pass through that child, because this is
        /// the case when a phrase is used to define inflection features of its headword.</remarks>
        public override void Consolidate()
        {
            if (Children.Count() == 0) Become(null);
            // This handles the case where CoreNLP returns a modifier or determiner as the ONLY child of a phrase with no head
            else if (Children.Count() == 1 && !Children.Single().IsPhraseHead) Become(Children.Single().AsPhraseIfConvertible());
        }

        #region Phrase features

        public abstract bool DiscourseFunctionSpecified { get; set; }
        public abstract discourseFunction DiscourseFunction { get; set; }
        public abstract bool AppositiveSpecified { get; set; }
        public abstract bool Appositive { get; set; }

        #endregion Phrase features
    }
}

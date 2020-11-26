using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>The base class of all PhraseElement builders.</summary>
    /// <remarks>The inheritance hierarchy of PhraseBuilder roughly corresponds to the hierarchy of <see cref="PhraseElement"/>s defined by the SimpleNLG Realizer Schema.</remarks>
    public abstract class PhraseBuilder : SyntaxHeadBuilder
    {
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
        private protected void AddHeads(IEnumerable<IElementTreeNode> heads)
        {
            foreach (IElementTreeNode eachHead in heads) AddHead(eachHead);
        }

        internal void AddModifier(IElementTreeNode modifier) => AddChildWithRole(modifier, ChildRole.Modifier);
        private protected void AddModifiers(IEnumerable<IElementTreeNode> modifiers)
        {
            foreach (IElementTreeNode eachModifier in modifiers) AddModifier(eachModifier);
        }

        private protected void AddComplement(IElementTreeNode complement) => AddChildWithRole(complement, ChildRole.Complement); 
        private protected void AddComplements(IEnumerable<IElementTreeNode> complements)
        {
            foreach (IElementTreeNode eachComplement in complements) AddComplement(eachComplement);
        }

        /// <summary>Default override of Consolidate for PhraseBuilders.</summary>
        /// <remarks>When a PhraseBuilder has exactly one child, we do NOT eliminate the PhraseBuilder and pass through that child, because this is
        /// the case when a phrase is used to define inflection features of its headword.</remarks>
        public override IElementTreeNode Consolidate() => Children.Count() switch
        {
            0 => Become(null),
            _ => this
        };

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>Builds a SimpleNLG PPPhraseSpec</summary>
    public class PrepositionalPhraseBuilder : CoordinablePhraseBuilder<PPPhraseSpec>
    {
        public PrepositionalPhraseBuilder() : base() { }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Head);
            listOfRoles.Add(ChildRole.Complement);
            if (CoordinatorBuilder == null || CoordinatorBuilder == child) listOfRoles.Add(ChildRole.Coordinator);
        }

        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case PrepositionBuilder pb:
                    AddHead(pb);
                    break;
                case PrepositionalPhraseBuilder ppb:
                    AddHead(ppb);
                    break;
                case NounBuilder nb:
                    AddComplement(nb);
                    break;
                case NounPhraseBuilder npb:
                    AddComplement(npb);
                    break;
                case VerbPhraseBuilder vpb:
                    AddComplement(vpb);
                    break;
                case CoordinatedPhraseBuilder cpb:
                    AssignRoleFor(cpb);
                    break;
                case ConjunctionBuilder cb:
                    SetCoordinator(cb);
                    break;
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        private void AssignRoleFor(CoordinatedPhraseBuilder phrase)
        {
            switch (phrase.PhraseCategory)
            {
                case phraseCategory.PREPOSITIONAL_PHRASE:
                    AddHead(phrase);
                    break;
                case phraseCategory.NOUN_PHRASE:
                    AddComplement(phrase);
                    break;
                case phraseCategory.VERB_PHRASE:
                    AddComplement(phrase);
                    break;
                default:
                    AddUnassignedChild(phrase);
                    break;
            }
        }

        #endregion Initial assignment of children

        #region Configuration

        /// <summary>Transform this PrepositionalPhraseBuilder into a CoordinatedPhraseBuilder and return that CoordinatedPhraseBuilder.</summary>
        /// <remarks>The prepositional phrase's Complements do not get incorporated into the CoordinatedPhraseElement.  Instead, each of those elements 
        /// must be applied to one of the coordinated elements.  This will change the syntactic structure of the tree, but while doing that we want to preserve the original 
        /// word order.  To accomplish that we first create a series of dictionaries that temporarily store the various elements of the original (non-coordinated) phrase,
        /// along with the original order of those elements. <para>Then we add those elements into the CoordinatedPhraseBuilder, using the dictionaries to preserve
        /// ordering.</para></remarks>
        private protected sealed override CoordinatedPhraseBuilder AsCoordinatedPhrase()
        {
            Dictionary<PartOfSpeechBuilder, int> partsOfSpeech = GetPartsOfSpeechAndIndices();
            Dictionary<IElementTreeNode, int> heads = GetHeadsAndIndices();
            Dictionary<IElementTreeNode, int> complements = GetComplementsAndIndices();
            CoordinatedPhraseBuilder result = base.AsCoordinatedPhrase();
            foreach (IElementTreeNode eachComplement in complements.Keys)
            {
                eachComplement.DetachFromParent();
                eachComplement
                    .Complete(ElementWithIndexNearest(complements[eachComplement], heads))
                    ?.SetChildOrdering(eachComplement, partsOfSpeech);
            };
            return result;
        }

        #endregion Configuration

        #region Editing

        private protected override HashSet<Type> ChildTypesThatCanBeAdded { get; } = new HashSet<Type>
        {
            typeof(PrepositionBuilder),
            typeof(ConjunctionBuilder),
            typeof(PrepositionalPhraseBuilder),
            typeof(NounBuilder),
            typeof(NounPhraseBuilder),
            typeof(VerbPhraseBuilder)
        };

        #endregion Editing

        public override IElementTreeNode CopyLightweight() => new PrepositionalPhraseBuilder { Phrase = Phrase.CopyWithoutSpec() }
            .LightweightCopyChildrenFrom(this);

        public override NLGElement BuildElement()
        {
            Phrase.head = UnaryHead.BuildWord();
            Phrase.compl = Complements
                .Select(complement => complement.BuildElement())
                .ToArray();
            return Phrase;
        }
    }
}

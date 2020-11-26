using System;
using System.Linq;
using System.Text;
using SimpleNLG;

namespace FlexibleRealization
{
    public class NominalModifierBuilder : SyntaxHeadBuilder
    {
        private StringElement NominalModifier = new StringElement();

        #region Initial assignment of children

        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                case NounBuilder nb:
                    AssignRoleFor(nb);
                    break;
                case AdjectiveBuilder ab:
                    AssignRoleFor(ab);
                    break;
                case AdjectivePhraseBuilder apb:
                    AssignRoleFor(apb);
                    break;
                case NominalModifierBuilder nmb:
                    AssignRoleFor(nmb);
                    break;
                default: throw new InvalidOperationException("Nominal modifier can't find a role for this element");
            }
        }

        private void AssignRoleFor(NounBuilder noun) => AddChildWithRole(noun, ChildRole.Head);

        private void AssignRoleFor(AdjectiveBuilder adjective) => AddChildWithRole(adjective, ChildRole.Head);

        private void AssignRoleFor(AdjectivePhraseBuilder phrase) => AddChildWithRole(phrase, ChildRole.Modifier);

        private void AssignRoleFor(NominalModifierBuilder nominalModifier) => AddChildWithRole(nominalModifier, ChildRole.Modifier);

        #endregion Initial assignment of children

        public override NLGElement BuildElement()
        {
            PartOfSpeechBuilder[] orderedPartsOfSpeech = PartsOfSpeechInSubtree.OrderBy(child => child.MinTokenIndex).ToArray();
            StringBuilder stringValue = new StringBuilder();
            for (int childIndex = 0; childIndex < orderedPartsOfSpeech.Length - 1; childIndex++)
            {
                AddPartOfSpeech(orderedPartsOfSpeech[childIndex]);
                stringValue.Append(" ");
            }
            AddPartOfSpeech(orderedPartsOfSpeech.Last());
            NominalModifier.val = stringValue.ToString();
            return NominalModifier;

            void AddPartOfSpeech(PartOfSpeechBuilder partOfSpeech)
            {
                if (partOfSpeech is WordElementBuilder)
                {
                    WordElementBuilder eachWord = (WordElementBuilder)partOfSpeech;
                    stringValue.Append(eachWord.BuildWord().Base);
                }
            }
        }
    }
}

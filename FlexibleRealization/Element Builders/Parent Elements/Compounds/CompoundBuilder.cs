using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleNLG;

namespace FlexibleRealization
{
    public abstract class CompoundBuilder : ParentElementBuilder
    {
        public CompoundBuilder(lexicalCategory category) { Compound.PartOfSpeech = category; }

        private WordElement Compound = new WordElement();

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Component);
        }

        private protected override void AssignRoleFor(IElementTreeNode child) => AddComponent(child);

        internal void AddComponent(IElementTreeNode component) => AddChildWithRole(component, ChildRole.Component);

        public override NLGElement BuildElement() => BuildWord();

        public WordElement BuildWord()
        {
            WordElementBuilder[] componentWords = Children
                .Cast<WordElementBuilder>()
                .OrderBy(word => word.Index)
                .ToArray();
            StringBuilder concatenatedComponents = new StringBuilder();
            for (int wordIndex = 0; wordIndex < componentWords.Length - 1; wordIndex++)
            {
                concatenatedComponents.Append(componentWords[wordIndex].BuildWord().Base);
                concatenatedComponents.Append(' ');
            }
            concatenatedComponents.Append(componentWords[componentWords.Length - 1].BuildWord().Base);
            Compound.Base = concatenatedComponents.ToString();

            return Compound;
        }
    }
}

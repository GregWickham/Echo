using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization
{
    public abstract class WhWordPhraseBuilder : ParentElementBuilder
    {
        internal WordElementBuilder HeadWord { get; set; }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Head);
        }

        public override NLGElement BuildElement() => HeadWord.BuildWord();
    }
}

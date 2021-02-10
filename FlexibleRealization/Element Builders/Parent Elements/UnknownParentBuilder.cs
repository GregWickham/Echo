using System;
using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>This parent type can be returned by the constituency parse, but we eliminate it during Configuration</summary>
    public class UnknownParentBuilder : ParentElementBuilder
    {
        private protected override void AssignRoleFor(IElementTreeNode child)
        {
            switch (child)
            {
                default:
                    AddUnassignedChild(child);
                    break;
            }
        }

        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Unassigned);
        }

        public override NLGElement BuildElement() => throw new InvalidOperationException("Can't build an UnknownParent");


        public override IElementTreeNode CopyLightweight() => new UnknownParentBuilder();
    }
}

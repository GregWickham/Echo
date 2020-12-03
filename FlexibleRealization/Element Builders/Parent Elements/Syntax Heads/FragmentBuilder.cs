using System;
using System.Collections.Generic;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>FragmentBuilders can be created by the CoreNLP constituency parse, but they must be eliminated during the Configuration process</summary>
    public class FragmentBuilder : SyntaxHeadBuilder
    {
        /// <summary>Add the valid ChildRoles for <paramref name="child"/> to <paramref name="listOfRoles"/></summary>
        private protected override void AddValidRolesForChildTo(List<ChildRole> listOfRoles, ElementBuilder child)
        {
            listOfRoles.Add(ChildRole.Unassigned);
        }

        private protected sealed override void AssignRoleFor(IElementTreeNode child) => AddUnassignedChild(child);

        public override IElementTreeNode CopyLightweight() => new FragmentBuilder()
            .LightweightCopyChildrenFrom(this);

        public override NLGElement BuildElement() => throw new NotImplementedException("Can't build a fragment");
    }
}

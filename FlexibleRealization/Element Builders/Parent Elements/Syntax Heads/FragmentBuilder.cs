using System;
using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>FragmentBuilders can be created by the CoreNLP constituency parse, but they must be eliminated during the Configuration process</summary>
    public class FragmentBuilder : SyntaxHeadBuilder
    {
        private protected sealed override void AssignRoleFor(IElementTreeNode child) => AddUnassignedChild(child);

        public override NLGElement BuildElement() => throw new NotImplementedException("Can't build a fragment");
    }
}

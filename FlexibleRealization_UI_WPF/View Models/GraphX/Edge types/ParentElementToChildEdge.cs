using System;
using GraphX.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    internal class ParentElementToChildEdge : ElementEdge
    {
        internal ParentElementToChildEdge(ParentElementVertex sev, ElementBuilderVertex ebv, ParentElementBuilder.ChildRole role) : base(sev, ebv) => Role = role;

        private readonly ParentElementBuilder.ChildRole Role;

        public override string LabelText => Parent.ChildRole.Labels[Role];
        public override EdgeDashStyle ElementDashStyle => DashStyleFor(Role);

        /// <summary>Return the appropriate <see cref="EdgeDashStyle"/> for <paramref name="edge"/></summary>
        private static EdgeDashStyle DashStyleFor(ParentElementBuilder.ChildRole role) => role switch
        {
            ParentElementBuilder.ChildRole.Unassigned => EdgeDashStyle.Dot,
            ParentElementBuilder.ChildRole.Subject => EdgeDashStyle.Dash,
            ParentElementBuilder.ChildRole.Predicate => EdgeDashStyle.Solid,
            ParentElementBuilder.ChildRole.Head => EdgeDashStyle.Solid,
            ParentElementBuilder.ChildRole.Modifier => EdgeDashStyle.Dash,
            ParentElementBuilder.ChildRole.Complement => EdgeDashStyle.DashDot,
            ParentElementBuilder.ChildRole.Specifier => EdgeDashStyle.DashDotDot,
            ParentElementBuilder.ChildRole.Modal => EdgeDashStyle.DashDotDot,
            ParentElementBuilder.ChildRole.Coordinator => EdgeDashStyle.Dash,
            ParentElementBuilder.ChildRole.Coordinated => EdgeDashStyle.Solid,
            ParentElementBuilder.ChildRole.Complementizer => EdgeDashStyle.DashDotDot,
            ParentElementBuilder.ChildRole.Component => EdgeDashStyle.Solid,

            _ => throw new InvalidOperationException("No edge visual style is defined for this child role")
        };
    }
}

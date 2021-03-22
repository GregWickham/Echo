using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a ParentElementBuilder in a GraphX GraphArea</summary>
    internal class ParentElementVertex : ElementBuilderVertex
    {
        internal ParentElementVertex(ParentElementBuilder peb) => Model = peb;

        /// <summary>The data model of this view model, specifically typed</summary>
        internal ParentElementBuilder Model;

        /// <summary>The data model of this view model, generically typed</summary>
        internal override ElementBuilder Builder => Model;

        public override string LabelText => Parent.LabelFor(Model);

        private string Description => Parent.DescriptionFor(Model);

        internal override bool CanAcceptDropOf(Type nodeType) => Model.CanAddChildOfType(nodeType);

        internal override bool AcceptDropOf(IElementTreeNode node, DragDropEffects effects, int insertPoint)
        {
            List<IElementTreeNode> sortedChildrenBeforeDrop = Model.Children
               .OrderBy(node => node)
               .ToList();
            switch (effects)
            {
                case DragDropEffects.Move:
                    node.MoveTo(Model);
                    break;
                case DragDropEffects.Copy:
                    Model.AddChild(node);
                    break;
                default: break;
            }
            if (insertPoint == 0)
                Model.SetChildOrdering(node, null, NodeRelation.First);
            else if (insertPoint == sortedChildrenBeforeDrop.Count)
                Model.SetChildOrdering(node, null, NodeRelation.Last);
            else
                Model.SetChildOrdering(node, sortedChildrenBeforeDrop[insertPoint - 1], NodeRelation.After);
            Model.Root.OnTreeStructureChanged();
            return true;
        }

        /// <summary>Construct and return a <see cref="UIElement"/> with content based on the <see cref="Model"/> of this view model.</summary>
        public override UIElement ToolTipContent
        {
            get
            {
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                stackPanel.Children.Add(ToolTipTitle(Description));
                foreach (string propertyDescription in GetSpecifiedProperties())
                {
                    stackPanel.Children.Add(ToolTipListItem(propertyDescription));
                }
                return stackPanel;
            }
        }

        private IEnumerable<string> GetSpecifiedProperties() => Parent.SpecifiedFeaturesFor(Model);
    }
}

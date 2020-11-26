using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="ParentElementBuilder"/> in a GraphX GraphArea</summary>
    internal class ParentElementVertex : ElementBuilderVertex
    {
        internal ParentElementVertex(ParentElementBuilder peb) => Model = peb;

        /// <summary>The data model of this view model, specifically typed</summary>
        internal ParentElementBuilder Model;

        /// <summary>The data model of this view model, generically typed</summary>
        internal override ElementBuilder Builder => Model;

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

        public override string LabelText => Parent.LabelFor(Model);

        private string Description => Parent.DescriptionFor(Model);

        private IEnumerable<string> GetSpecifiedProperties() => Parent.SpecifiedFeaturesFor(Model);

    }
}

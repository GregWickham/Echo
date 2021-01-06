using System;
using System.Windows;
using System.Windows.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting the part-of-speech portion of a <see cref="WordElementBuilder"/> in a GraphX GraphArea</summary>
    internal class WordPartOfSpeechVertex : ElementBuilderVertex
    {
        internal WordPartOfSpeechVertex(WordElementBuilder web) => Model = web;

        /// <summary>The data model of this view model, specifically typed</summary>
        internal WordElementBuilder Model;

        /// <summary>The data model of this view model, generically typed</summary>
        internal override ElementBuilder Builder => Model;

        public override string LabelText => WordBuilder.LabelFor(Model);

        private string Description => WordBuilder.DescriptionFor(Model);

        internal override bool CanAcceptDropOf(IElementTreeNode draggedNode) => Model.CanAdd(draggedNode);

        internal override bool AcceptDropOf(IElementTreeNode draggedNode) => Model.Add(draggedNode);

        /// <summary>Construct and return a UIElement with content based on the Model of this view model.</summary>
        public override UIElement ToolTipContent
        {
            get
            {
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                stackPanel.Children.Add(ToolTipTitle(Description));
                return stackPanel;
            }
        }
    }
}

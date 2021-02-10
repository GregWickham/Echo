using System;
using System.Windows;
using System.Windows.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting an <see cref="IWordSource"/> in a GraphX GraphArea</summary>
    internal class WordContentVertex : ElementVertex
    {
        internal WordContentVertex(WordElementBuilder web) => Model = web;

        /// <summary>The data model of this view model</summary>
        internal WordElementBuilder Model;

        public override string LabelText => Model.WordSource.DefaultWord;

        /// <summary>The IsToken property is used by XAML style triggers</summary>
        public override bool IsWordContents => true;

        internal override bool CanAcceptDropOf(Type nodeType) => false;

        internal override bool AcceptDropOf(IElementTreeNode node, DragDropEffects effects, int insertPoint) => throw new InvalidOperationException("Can't drop on a word contents vertex");

        /// <summary>Construct and return a <see cref="UIElement"/> with content based on the <see cref="Model"/> of this view model.</summary>
        public override UIElement ToolTipContent
        {
            get
            {
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                stackPanel.Children.Add(ToolTipTitle($"Default: {Model.WordSource.DefaultWord}"));
                return stackPanel;
            }
        }

    }
}

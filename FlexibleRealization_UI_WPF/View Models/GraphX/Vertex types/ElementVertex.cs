using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GraphX.Common.Models;
using GraphX.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model base class for presenting a graph element in a GraphX GraphArea</summary>
    /// <remarks>An ElementVertex can represent the "content" part of a WordElementBuilder, the "part of speech" part of a WordElementBuilder, or a ParentElementBuilder</remarks>
    public abstract class ElementVertex : VertexBase, INotifyPropertyChanged
    {
        public ElementVertex() { }

        public override string ToString() => LabelText;
        public abstract string LabelText { get; }

        /// <summary>The IsWordContents property is used by XAML style triggers</summary>
        public abstract bool IsWordContents { get; }

        internal abstract bool CanAcceptDropOf(Type nodeType);

        /// <summary>Respond as appropriate to <paramref name="node"/> being dropped on this, and return true if successful.</summary>
        internal abstract bool AcceptDropOf(IElementTreeNode node, DragDropEffects effects, int insertPoint);

        private protected static readonly Thickness ToolTipBorderThickness = new Thickness(2);
        private protected static readonly CornerRadius ToolTipCornerRadius = new CornerRadius(8);
        private protected static readonly Thickness ToolTipMargin = new Thickness(4);

        /// <summary>Assign a ToolTip to <paramref name="control"/>, with content appropriate for <paramref name="control"/>'s model</summary>
        internal void SetToolTipFor(VertexControl control) 
        {
            control.ToolTip = new ToolTip
            {
                BorderBrush = Brushes.Transparent,
                Background = Brushes.Transparent,
                HorizontalOffset = 0,
                VerticalOffset = 0,
                Content = new Border
                {
                    BorderBrush = Brushes.Black,
                    BorderThickness = ToolTipBorderThickness,
                    CornerRadius = ToolTipCornerRadius,
                    Background = control.FindResource("LightGrayGradient") as Brush,
                    UseLayoutRounding = true,
                    Child = ToolTipContent
                }
            };
        }

        public abstract UIElement ToolTipContent { get; }

        /// <summary>Return a <see cref="TextBlock"/> containing <paramref name="title"/>, to be used as the first line in a <see cref="ToolTip"/></summary>
        private protected static TextBlock ToolTipTitle(string title) => new TextBlock
        {
            Text = title,
            Margin = ToolTipMargin,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 12,
            FontWeight = FontWeights.Bold
        };

        /// <summary>Return a <see cref="TextBlock"/> containing <paramref name="item"/>, to be used as one of several lines in the body of a <see cref="ToolTip"/></summary>
        private protected static TextBlock ToolTipListItem(string item) => new TextBlock
        {
            Text = item,
            Margin = ToolTipMargin,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontSize = 10,
            FontWeight = FontWeights.Normal
        };


        #region Standard implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Standard implementation of INotifyPropertyChanged
    }
}

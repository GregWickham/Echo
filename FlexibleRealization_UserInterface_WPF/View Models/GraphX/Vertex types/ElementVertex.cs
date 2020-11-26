using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GraphX.Common.Models;
using GraphX.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model base class for presenting a graph element in a GraphX GraphArea</summary>
    /// <remarks>Notice that this vertext type does not have a model, because there is no common base class of <see cref="ParseToken"/> and <see cref="ElementBuilder"/>.
    /// An <see cref="ElementVertex"/> can represent a <see cref="ParseToken"/>, a <see cref="PartOfSpeechBuilder"/>, or a <see cref="ParentElementBuilder"/></remarks>
    public abstract class ElementVertex : VertexBase
    {
        public ElementVertex() { }

        public override string ToString() => LabelText;
        public abstract string LabelText { get; }

        /// <summary>The IsToken property is used by XAML style triggers</summary>
        public abstract bool IsToken { get; }

        private protected static readonly Thickness ToolTipBorderThickness = new Thickness(2);
        private protected static readonly CornerRadius ToolTipCornerRadius = new CornerRadius(8);
        private protected static readonly Thickness ToolTipMargin = new Thickness(4);

        /// <summary>Assign a <see cref="ToolTip"/> to <paramref name="control"/>, with content appropriate for <paramref name="control"/>'s model</summary>
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
                    Background = control.FindResource("LightGradientBrush") as Brush,
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
    }
}

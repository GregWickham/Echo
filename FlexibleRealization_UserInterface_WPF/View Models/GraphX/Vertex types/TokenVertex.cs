using System.Windows;
using System.Windows.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>View Model for presenting a <see cref="ParseToken"/> in a GraphX GraphArea</summary>
    internal class TokenVertex : ElementVertex
    {
        internal TokenVertex(ParseToken t) => Model = t;

        /// <summary>The data model of this view model</summary>
        internal ParseToken Model;

        public override string LabelText => Model.Word;

        /// <summary>The IsToken property is used by XAML style triggers</summary>
        public override bool IsToken => true;

        /// <summary>Construct and return a <see cref="UIElement"/> with content based on the <see cref="Model"/> of this view model.</summary>
        public override UIElement ToolTipContent
        {
            get
            {
                StackPanel stackPanel = new StackPanel { Orientation = Orientation.Vertical };
                stackPanel.Children.Add(ToolTipTitle(Model.Word));
                stackPanel.Children.Add(ToolTipListItem($"Lemma: {Model.Lemma}"));
                stackPanel.Children.Add(ToolTipListItem($"POS Tag: {Model.PartOfSpeech}"));
                stackPanel.Children.Add(ToolTipListItem($"Index: {Model.Index}"));
                return stackPanel;
            }
        }

    }
}

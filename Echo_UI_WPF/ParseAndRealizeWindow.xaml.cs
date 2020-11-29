using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SimpleNLG;
using FlexibleRealization;
using FlexibleRealization.UserInterface;

namespace Echo.UserInterface
{
    /// <summary>Interaction logic for ParseAndRealizeWindow.xaml</summary>
    public partial class ParseAndRealizeWindow : Window
    {
        public ParseAndRealizeWindow()
        {
            InitializeComponent();
            GraphEditor.RealizationFailed += GraphEditor_RealizationFailed;
            GraphEditor.TextRealized += GraphEditor_TextRealized;
        }

        /// <summary>This event handler is called when the GraphEditor has successfully realized some text</summary>
        private void GraphEditor_TextRealized(string realizedText)
        {
            realizedTextBox.Background = Brushes.WhiteSmoke;
            realizedTextBox.Text = realizedText;
        }

        /// <summary>This event handler is called when the GraphEditor has tried to realize an IElementTreeNode, but failed</summary>
        private void GraphEditor_RealizationFailed(IElementTreeNode failed)
        {
            realizedTextBox.Background = RealizeFailedBrush;
            realizedTextBox.Text = "";
        }

        private static Brush RealizeFailedBrush = new SolidColorBrush(Color.FromArgb(100, 254, 0, 0));

        protected override void OnClosing(CancelEventArgs e)
        {
            GraphEditor.RealizationFailed -= GraphEditor_RealizationFailed;
            GraphEditor.TextRealized -= GraphEditor_TextRealized;
        }

        /// <summary>When the user changes a setting for the CoreNLP server, save its settings</summary>
        private void CoreNLP_SettingChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => Stanford.CoreNLP.Properties.Settings.Default.Save();

        /// <summary>When the user changes a setting for the SimpleNLG server, save its settings</summary>
        private void SimpleNLG_SettingChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => SimpleNLG.Properties.Settings.Default.Save();

        //private void GraphEditor_ElementBuilderSelected(ElementBuilder selectedBuilder) => RealizeAndDisplay(selectedBuilder);

        /// <summary>If there's text in the inputTextBox, parse it</summary>
        private void parseButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputTextBox.Text.Length > 0) GraphEditor.ParseText(inputTextBox.Text);
        }

        private void HandleTextInput(string text) => GraphEditor.ParseText(text);

        private void inputTextBox_TextInput(object sender, TextCompositionEventArgs e) => HandleTextInput(e.Text);
    }
}

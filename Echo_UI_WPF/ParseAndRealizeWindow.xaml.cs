using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using SimpleNLG;
using FlexibleRealization;
using FlexibleRealization.UserInterface;

namespace Echo.UserInterface
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class ParseAndRealizeWindow : Window
    {
        public ParseAndRealizeWindow()
        {
            InitializeComponent();
            GraphEditor.ElementBuilderSelected += GraphEditor_ElementBuilderSelected;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            GraphEditor.ElementBuilderSelected -= GraphEditor_ElementBuilderSelected;
        }

        /// <summary>When the user changes a setting for the CoreNLP server, save its settings</summary>
        private void CoreNLP_SettingChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => Stanford.CoreNLP.Properties.Settings.Default.Save();

        /// <summary>When the user changes a setting for the SimpleNLG server, save its settings</summary>
        private void SimpleNLG_SettingChanged(object sender, System.Windows.Controls.TextChangedEventArgs e) => SimpleNLG.Properties.Settings.Default.Save();

        private void GraphEditor_ElementBuilderSelected(ElementBuilder selectedBuilder) => RealizeAndDisplay(selectedBuilder);

        /// <summary>If there's text in the inputTextBox, parse it</summary>
        private void parseButton_Click(object sender, RoutedEventArgs e)
        {
            if (inputTextBox.Text.Length > 0) HandleTextInput(inputTextBox.Text);
        }

        /// <summary>Parse <paramref name="inputText"/>, display the result in the <see cref="ElementBuilderGraphEditor"/>, realize it, and display the realized text</summary>
        private void HandleTextInput(string inputText)
        {
            IElementBuilder tree = FlexibleRealizerFactory.ElementBuilderTreeFrom(inputText);
            GraphEditor.SetModel(tree);
            RealizeAndDisplay(tree);
        }

        private void inputTextBox_TextInput(object sender, TextCompositionEventArgs e) => HandleTextInput(e.Text);

        /// <summary>Realize the <paramref name="tree"/> and display the realized text in the <see cref="realizedTextBox"/></summary>
        private void RealizeAndDisplay(IElementBuilder tree)
        {
            NLGSpec spec = FlexibleRealizerFactory.RealizableSpecFrom(tree);
            string realized = SimpleNLG.Client.Realize(spec);
            realizedTextBox.Text = realized;
        }


    }
}

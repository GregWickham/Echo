using System.Windows.Controls;
using System.Windows;
using GraphX.Controls;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    public delegate void ElementBuilderSelected_EventHandler(ElementBuilder selectedBuilder);

    /// <summary>Interaction logic for ElementBuilderGraphEditor.xaml</summary>
    public partial class ElementBuilderGraphEditor : UserControl
    {
        public ElementBuilderGraphEditor()
        {
            InitializeComponent();
            ZoomControl.SetViewFinderVisibility(ZoomCtrl, Visibility.Hidden);
            GraphArea.VertexSelected += GraphArea_VertexSelected;
            Loaded += ElementBuilderGraphEditor_Loaded;
        }

        /// <summary>Hook a handler to the containing <see cref="Window"/>'s Closing event</summary>
        private void ElementBuilderGraphEditor_Loaded(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Closing += Window_Closing;
        }

        /// <summary>Tear down this <see cref="ElementBuilderGraphEditor"/></summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GraphArea.VertexSelected -= GraphArea_VertexSelected;
            Loaded -= ElementBuilderGraphEditor_Loaded;
        }

        /// <summary>Assign <paramref name="elementBuilderTree"/> as the model for this editor</summary>
        public void SetModel(IElementBuilder elementBuilderTree)
        {
            ElementBuilderGraph graph = ElementBuilderGraphFactory.GraphOf(elementBuilderTree);
            GraphArea.LogicCore = new ElementBuilderLogicCore(graph);
            GraphArea.GenerateGraph(true, true);
            ElementDescription.DataContext = GraphArea;
            Properties.DataContext = GraphArea;
            GraphArea.SetSelectedVertex(graph.Root);
            ZoomCtrl.ZoomToFill();
        }

        /// <summary>If the selected <see cref="ElementVertex"/> has an associated <see cref="ElementBuilder"/>, notify listeners of the selection</summary>
        private void GraphArea_VertexSelected(object sender, GraphX.Controls.Models.VertexSelectedEventArgs args)
        {
            ElementVertex selectedVertex = (ElementVertex)args.VertexControl.Vertex;
            switch (selectedVertex)
            {
                case ParentElementVertex pev:
                    OnElementBuilderSelected(pev.Builder);
                    break;
                case PartOfSpeechVertex psv:
                    OnElementBuilderSelected(psv.Builder);
                    break;
                default: break;
            }
        }

        /// <summary>Register for this event to be notified when an <see cref="ElementBuilder"/> is selected in the graph</summary>
        public event ElementBuilderSelected_EventHandler ElementBuilderSelected;
        private void OnElementBuilderSelected(ElementBuilder builder) => ElementBuilderSelected?.Invoke(builder);
    }
}

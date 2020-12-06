using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using GraphX.Controls;using GraphX.Controls.Models;
using SimpleNLG;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    public delegate void ElementBuilderSelected_EventHandler(ElementBuilder selectedBuilder);

    public delegate void RealizationFailed_EventHandler(IElementTreeNode failedBuilder);

    public delegate void TextRealized_EventHandler(string realizedText);

    /// <summary>Interaction logic for ElementBuilderGraphEditor.xaml</summary>
    public partial class ElementBuilderGraphEditor : UserControl, INotifyPropertyChanged
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

        /// <summary>Tear down this ElementBuilderGraphEditor</summary>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GraphArea.VertexSelected -= GraphArea_VertexSelected;
            Loaded -= ElementBuilderGraphEditor_Loaded;
            Window.GetWindow(this).Closing -= Window_Closing;
        }

        /// <summary>Generate an editable tree from <paramref name="text"/>, then try to realize that tree</summary>
        public void ParseText(string text)
        {
            IElementTreeNode editableTree = FlexibleRealizerFactory.EditableTreeFrom(text);
            editableTree.Root.TreeStructureChanged += ElementBuilderTree_TreeStructureChanged;
            SetModel(editableTree);
            SelectNode(editableTree);
        }

        /// <summary>Assign <paramref name="elementBuilderTree"/> as the model for this editor</summary>
        private void SetModel(IElementTreeNode elementBuilderTree)
        {
            ElementBuilderGraph graph = ElementBuilderGraphFactory.GraphOf(elementBuilderTree);
            GraphArea.LogicCore = new ElementBuilderLogicCore(graph);
            GraphArea.GenerateGraph(true, true);
            ElementDescription.DataContext = GraphArea;
            Properties.DataContext = GraphArea;
            XmlLabel.DataContext = this;
            ZoomCtrl.ZoomToFill();
        }

        /// <summary>The tree is notifying us that its structure has changed</summary>
        private void ElementBuilderTree_TreeStructureChanged(RootNode root) => SetModel(root.Tree);

        /// <summary>Try to transform <paramref name="editableTree"/> into realizable form and if successful, try to realize it</summary>
        /// <remarks>Raise an event indicating whether the process succeeded or not</remarks>
        private void TryToRealize(IElementTreeNode editableTree)
        {
            try
            {
                IElementBuilder realizableTree = FlexibleRealizerFactory.RealizableTreeFrom(editableTree);
                NLGSpec spec = FlexibleRealizerFactory.SpecFrom(realizableTree);
                XmlSpec = spec.Serialize();
                string realized = SimpleNLG.Client.Realize(XmlSpec);
                OnTextRealized(realized);
            }
            catch (Exception ex) when (ex is TreeCannotBeTransformedToRealizableFormException || ex is SpecCannotBeBuiltException)
            {
                XmlSpec = null;
                OnRealizationFailed(editableTree);
            }
        }

        /// <summary>Private field for holding the XML spec of our realized element graph.  Accessed by the <see cref="XmlSpec"/> and <see cref="XmlSpecLocalized"/> properties</summary>
        private string xmlSpec;

        /// <summary>If we succeed in building an element graph that can be realized, we'll put the serialized XML form of that graph here so it can be displayed</summary>
        private string XmlSpec 
        {
            get => xmlSpec;
            set
            {
                xmlSpec = value;
                OnPropertyChanged("XmlSpecLocalized");
            }
        }

        /// <summary>Return the XML spec formatted for display in the user interface</summary>
        public string XmlSpecLocalized
        {
            get
            {
                if (xmlSpec == null) return null;
                else
                {
                    // Strip out the namespace declarations to make the XML more compact, so it looks nice in the user interface
                    XDocument document = XDocument.Parse(xmlSpec);
                    document.Descendants()
                       .Attributes()
                       .Where(x => x.IsNamespaceDeclaration)
                       .Remove();
                    foreach (var elem in document.Descendants())
                        elem.Name = elem.Name.LocalName;
                    foreach (var attr in document.Descendants().Attributes())
                    {
                        var elem = attr.Parent;
                        attr.Remove();
                        elem.Add(new XAttribute(attr.Name.LocalName, attr.Value));
                    }
                    return document.ToString();
                }
            }
        }

        /// <summary>Set <paramref name="node"/> as the selected element</summary>
        private void SelectNode(IElementTreeNode node)
        {
            GraphArea.SetSelectedNode(node);
            TryToRealize(node);
        }

        /// <summary>If the selected ElementVertex has an associated ElementBuilder, notify listeners of the selection</summary>
        private void GraphArea_VertexSelected(object sender, VertexSelectedEventArgs args)
        {
            ElementVertex selectedVertex = (ElementVertex)args.VertexControl.Vertex;
            ElementBuilder selectedBuilder = null;
            switch (selectedVertex)
            {
                case ParentElementVertex pev:
                    selectedBuilder = pev.Builder;
                    OnElementBuilderSelected(pev.Builder);
                    break;
                case PartOfSpeechVertex psv:
                    selectedBuilder = psv.Builder;
                    OnElementBuilderSelected(psv.Builder);
                    break;
                default: break;
            }
            if (args.MouseArgs.RightButton == MouseButtonState.Pressed)
            {
                if (selectedBuilder != null)
                {
                    SetDropTargetsFor(selectedBuilder);
                    DragDrop.DoDragDrop(args.VertexControl, selectedBuilder, DragDropEffects.Move);
                }
            }

            // Configure the appropriate vertexes to be drop targets for the supplied ElementBuilder
            void SetDropTargetsFor(ElementBuilder builder)
            {
                foreach (KeyValuePair<ElementVertex, VertexControl> eachKVP in GraphArea.VertexList)
                {
                    eachKVP.Value.PreviewDrop -= VertexControl_PreviewDrop;
                    if (eachKVP.Key is ParentElementVertex pev && pev.Model.CanAddChild(builder))
                    {
                        eachKVP.Value.AllowDrop = true;
                        eachKVP.Value.PreviewDrop += VertexControl_PreviewDrop;
                    }
                    else eachKVP.Value.AllowDrop = false;
                }
            }
        }

        /// <summary>Configure all vertexes to NOT be drop targets</summary>
        private void ClearDropTargets()
        {
            foreach (KeyValuePair<ElementVertex, VertexControl> eachKVP in GraphArea.VertexList)
            {
                eachKVP.Value.PreviewDrop -= VertexControl_PreviewDrop;
                eachKVP.Value.AllowDrop = false;
            }
        }

        /// <summary>A vertex drag has ended with a drop</summary>
        private void VertexControl_PreviewDrop(object sender, DragEventArgs e)
        {
            ClearDropTargets();
            VertexControl dropTarget = (VertexControl)sender;
            ElementVertex targetVertex = (ElementVertex)dropTarget.Vertex;
            string[] formats = e.Data.GetFormats();
            string droppedBuilderFormat = formats[0];
            if (e.Data.GetDataPresent(droppedBuilderFormat))
            {
                IElementTreeNode dropped = (IElementTreeNode)e.Data.GetData(droppedBuilderFormat);
                if (targetVertex is ParentElementVertex parentVertex)
                {
                    ParentElementBuilder target = parentVertex.Model;
                    dropped.MoveTo(target);
                    SelectNode(target);
                }
            }
        }

        /// <summary>Register for this event to be notified when an ElementBuilder is selected in the graph</summary>
        public event ElementBuilderSelected_EventHandler ElementBuilderSelected;
        private void OnElementBuilderSelected(ElementBuilder builder)
        {
            ElementBuilderSelected?.Invoke(builder);
            TryToRealize(builder);
        }

        /// <summary>Notify listeners that this ElementBuilderGraphEditor has failed to realize text for an ElementBuilder</summary>
        public event RealizationFailed_EventHandler RealizationFailed;
        private void OnRealizationFailed(IElementTreeNode failed) => RealizationFailed?.Invoke(failed);

        /// <summary>Notify listeners that this ElementBuilderGraphEditor has successfully realized some text</summary>
        public event TextRealized_EventHandler TextRealized;
        private void OnTextRealized(string realizedText) => TextRealized?.Invoke(realizedText);


        #region Standard implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Standard implementation of INotifyPropertyChanged
    }
}

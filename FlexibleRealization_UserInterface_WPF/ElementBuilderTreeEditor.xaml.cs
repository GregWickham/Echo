﻿using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;
using GraphX.Controls;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    public delegate void RealizationFailed_EventHandler(IElementTreeNode failedBuilder);

    public delegate void TextRealized_EventHandler(string realizedText);

    /// <summary>Interaction logic for ElementBuilderGraphEditor.xaml</summary>
    public partial class ElementBuilderTreeEditor : UserControl, INotifyPropertyChanged
    {
        public ElementBuilderTreeEditor()
        {
            InitializeComponent();
            ZoomControl.SetViewFinderVisibility(ZoomCtrl, Visibility.Hidden);
        }

        /// <summary>Hook a handler to the containing <see cref="Window"/>'s Closing event</summary>
        private void ElementBuilderTreeEditor_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Window window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Closing += Window_Closing;
                }
            }
        }

        /// <summary>Tear down this ElementBuilderGraphEditor</summary>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Loaded -= ElementBuilderTreeEditor_Loaded;
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
            ElementBuilderGraph graph = ElementBuilderGraph.Of(elementBuilderTree);
            ElementGraphArea.LogicCore = new ElementBuilderLogicCore(graph);
            ElementGraphArea.GenerateGraph(true, true);
            ElementDescription.DataContext = this;
            Properties.DataContext = this;
            XmlLabel.DataContext = this;
            // I think the animation looks cool when we put a new tree in the GraphArea, but we don't want to trigger that animation every
            // time the selected vertex changes.
            ZoomCtrl.IsAnimationEnabled = true;
            ZoomCtrl.ZoomToFill();
            ZoomCtrl.IsAnimationEnabled = false;
        }

        public void ClearModel() => ElementGraphArea.ClearLayout(true, true, true);

        /// <summary>Because SelectedBuilder is always a valid reference, we can use it as our reference to the entire tree model</summary>
        public IElementTreeNode Model => SelectedBuilder.Root.Tree;

        /// <summary>Monitored by the ElementDescription TextBlock to display the description of the selected vertex's element</summary>
        public string SelectedElementDescription => SelectedElementProperties?.Description ?? "";

        /// <summary>Monitored by the PropertyGrid control to decide which element's properties to display</summary>
        public ElementProperties SelectedElementProperties { get; private set; }

        /// <summary>The tree is notifying us that its structure has changed.  We respond by setting the tree in its new form as our model.</summary>
        private void ElementBuilderTree_TreeStructureChanged(RootNode root) => SetModel(root.Tree);

        /// <summary>Try to transform <paramref name="editableTree"/> into realizable form and if successful, try to realize it</summary>
        /// <remarks>Raise an event indicating whether the process succeeded or not</remarks>
        private RealizationResult TryToRealize(IElementTreeNode editableTree)
        {
            RealizationResult result = editableTree.Realize();
            switch (result.Outcome)
            {
                case RealizationOutcome.Success:
                    XmlSpec = result.XML;
                    OnTextRealized(result.Text);
                    break;
                case RealizationOutcome.FailedToTransform:
                case RealizationOutcome.FailedToBuildSpec:
                    XmlSpec = null;
                    OnRealizationFailed(editableTree);
                    break;
                default: break;
            }
            return result;
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
            ElementGraphArea.SetSelectedNode(node);
            TryToRealize(node);
        }

        /// <summary>SelectedBuilder is controlled by the ElementGraphArea and it should never be null</summary>
        public ElementBuilder SelectedBuilder => ElementGraphArea.SelectedBuilder;

        private void GraphArea_SelectedBuilderChanged()
        {
            SetSelectedElementProperties(ElementProperties.For(SelectedBuilder));
            TryToRealize(SelectedBuilder);
            SelectedBuilderChanged?.Invoke();
        }

        private void SetSelectedElementProperties(ElementProperties properties)
        {
            SelectedElementProperties = properties;
            OnPropertyChanged("SelectedElementProperties");
            OnPropertyChanged("SelectedElementDescription");
        }

        /// <summary>Register for this event to be notified when an ElementBuilder is selected in the graph</summary>
        public event SelectedBuilderChanged_EventHandler SelectedBuilderChanged;

        /// <summary>Notify listeners that this ElementBuilderGraphEditor has failed to realize text for an ElementBuilder</summary>
        public event RealizationFailed_EventHandler RealizationFailed;
        private void OnRealizationFailed(IElementTreeNode failed) => RealizationFailed?.Invoke(failed);

        /// <summary>Notify listeners that this ElementBuilderGraphEditor has successfully realized some text</summary>
        public event TextRealized_EventHandler TextRealized;
        private void OnTextRealized(string realizedText) => TextRealized?.Invoke(realizedText);

        public void OnElementDragStarted(ElementBuilder dragged)
        {
            if (ElementGraphArea.VertexList.Count == 0)
            {
                ZoomCtrl.AllowDrop = true;
                ZoomCtrl.DragEnter += ZoomCtrl_DragEnter;
                ZoomCtrl.DragLeave += ZoomCtrl_DragLeave;
                ZoomCtrl.Drop += ZoomCtrl_Drop;
            }
            else
            {
                ElementGraphArea.SetDropTargetsFor(dragged);
            }
        }

        public void OnElementDragCancelled(ElementBuilder dragged)
        {
            ZoomCtrl.AllowDrop = false;
            ZoomCtrl.DragEnter -= ZoomCtrl_DragEnter;
            ZoomCtrl.DragLeave -= ZoomCtrl_DragLeave;
            ZoomCtrl.Drop -= ZoomCtrl_Drop;
            ElementGraphArea.ClearDropTargets();
        }

        public void OnElementDropCompleted(ElementBuilder dragged)
        {
            ZoomCtrl.AllowDrop = false;
            ZoomCtrl.DragEnter -= ZoomCtrl_DragEnter;
            ZoomCtrl.DragLeave -= ZoomCtrl_DragLeave;
            ZoomCtrl.Drop -= ZoomCtrl_Drop;
            ElementGraphArea.ClearDropTargets();
        }

        private void ZoomCtrl_DragEnter(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void ZoomCtrl_DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void ZoomCtrl_Drop(object sender, DragEventArgs e)
        {
            string[] formats = e.Data.GetFormats();
            string droppedBuilderFormat = formats[0];
            if (e.Data.GetDataPresent(droppedBuilderFormat))
            {
                IElementTreeNode droppedNode = (IElementTreeNode)e.Data.GetData(droppedBuilderFormat);
                // When a parent element comes from a database browser it has no root, so we need to create one
                new RootNode(droppedNode);
                SetModel(droppedNode);
                SelectNode(droppedNode);
            }
        }

        #region Standard implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Standard implementation of INotifyPropertyChanged
    }
}

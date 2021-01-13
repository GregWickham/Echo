using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphX.Controls;
using GraphX.Controls.Animations;
using GraphX.Controls.Models;

namespace FlexibleRealization.UserInterface.ViewModels
{
    public delegate void GraphRootAdded(IElementTreeNode root);

    public delegate void SelectedBuilderChanged_EventHandler();

    public class ElementBuilderGraphArea : GraphArea<ElementVertex, ElementEdge, ElementBuilderGraph>
    {
        public ElementBuilderGraphArea() : base()
        {
            ControlFactory = new ElementBuilderControlFactory(this);
            SetVerticesDrag(true);
            VertexSelected += ElementBuilderGraphArea_VertexSelected;
            VertexClicked += ElementBuilderGraphArea_VertexClicked;
        }

        public override void GenerateGraph(bool generateAllEdges = true, bool dataContextToDataItem = true)
        {
            base.GenerateGraph(generateAllEdges, dataContextToDataItem);
            RemoveEdgeLabelsFromPartsOfSpeech();
            RegisterForVertexModelChangeNotifications();
            AssignVertexToolTips();
        }

        /// <summary>Labels are not needed on edges that connect a part of speech to its token</summary>
        private void RemoveEdgeLabelsFromPartsOfSpeech()
        {
            IEnumerable<EdgeControl> partOfSpeechToTokenEdges = EdgesList
                .Where(kvp => kvp.Key is PartOfSpeechToContentEdge)
                .Select(kvp => kvp.Value);
            foreach (EdgeControl eachPartOfSpeechToContentEdge in partOfSpeechToTokenEdges)
            {
                eachPartOfSpeechToContentEdge.GetLabelControls().First().ShowLabel = false;
            }
        }

        /// <summary>Register to receive change notifications from each ElementBuilderVertex in this graph area</summary>
        private void RegisterForVertexModelChangeNotifications()
        {
            foreach (KeyValuePair<ElementVertex, VertexControl> kvp in VertexList)
            {
                if (kvp.Key is ElementBuilderVertex ebv)
                {
                    ebv.Builder.PropertyChanged += Builder_PropertyChanged;
                }
            }
        }

        /// <summary>One of the ElementBuilders represented in this graph raised an event to inform us that it changed</summary>
        private void Builder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ElementBuilder changedBuilder = sender as ElementBuilder;
            if (changedBuilder != null)
            {
                ElementVertex changedVertex = VertexForNode(changedBuilder);
                changedVertex.SetToolTipFor(VertexList[changedVertex]);
            }
        }

        /// <summary>Return the ElementVertex whose model is <paramref name="node"/></summary>
        private ElementVertex VertexForNode(IElementTreeNode node) => VertexList.Keys.Single(vertex => vertex is ElementBuilderVertex ebv && ebv.Builder == node);

        /// <summary>Assign ToolTips for each VertexControl based on the state of its corresponding ElementVertex</summary>
        private void AssignVertexToolTips() 
        { 
            foreach (KeyValuePair<ElementVertex, VertexControl> kvp in VertexList) 
            { 
                kvp.Key.SetToolTipFor(kvp.Value); 
            } 
        }

        internal ElementBuilder SelectedBuilder { get; private set; }

        /// <summary>The user has selected a vertex in the graph.</summary>
        private void ElementBuilderGraphArea_VertexSelected(object sender, VertexSelectedEventArgs args) => SetSelectedVertex((ElementVertex)args.VertexControl.Vertex);

        internal void SetSelectedNode(IElementTreeNode node) => SetSelectedVertex(VertexForNode(node));

        /// <summary>Set <paramref name="selectedVertex"/> as the selection in the graph</summary>
        private void SetSelectedVertex(ElementVertex selectedVertex)
        {
            // Clear any previously selected vertices
            foreach (VertexControl eachVertexControl in VertexList.Values)
            {
                HighlightBehaviour.SetHighlighted(eachVertexControl, false);
                DragBehaviour.SetIsTagged(eachVertexControl, false);
            }
            TagAndHighlight(selectedVertex);
            if (selectedVertex is WordPartOfSpeechVertex partOfSpeechVertex)
                TagAndHighlight(WordContentsCorrespondingTo(partOfSpeechVertex));
            SelectedBuilder = selectedVertex switch
            {
                WordPartOfSpeechVertex wposv => wposv.Model,
                ParentElementVertex pev => pev.Model,
                _ => null
            };
            OnSelectedBuilderChanged();

            void TagAndHighlight(ElementVertex vertex)
            {
                VertexControl correspondingControl = VertexList[vertex];
                HighlightBehaviour.SetHighlighted(correspondingControl, true);
                DragBehaviour.SetIsTagged(correspondingControl, true);
                DragBehaviour.SetUpdateEdgesOnMove(correspondingControl, false);
            }

            WordContentVertex WordContentsCorrespondingTo(WordPartOfSpeechVertex partOfSpeechVertex) => VertexList.Keys
                .Where(vertex => vertex is WordContentVertex wcv && wcv.Model == partOfSpeechVertex.Model.WordSource)
                .Cast<WordContentVertex>()
                .Single();
        }

        internal event GraphRootAdded GraphRootAdded;
        private void OnGraphRootAdded(IElementTreeNode root) => GraphRootAdded?.Invoke(root);

        internal event SelectedBuilderChanged_EventHandler SelectedBuilderChanged;
        private void OnSelectedBuilderChanged() => SelectedBuilderChanged?.Invoke();

        /// <summary>We set vertexClickPosition when a vertex is first clicked, then use it during mouse move to decide whether to start a drag operation</summary>
        private Point vertexClickPosition;
        private void ElementBuilderGraphArea_VertexClicked(object sender, VertexClickedEventArgs args) => vertexClickPosition = args.MouseArgs.GetPosition(this);

        /// <summary>Figure out whether to start a drag operation</summary>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point currentMousePosition = e.GetPosition(this);
            Vector mouseDownDistanceMoved = vertexClickPosition - currentMousePosition;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(mouseDownDistanceMoved.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(mouseDownDistanceMoved.Y) > SystemParameters.MinimumVerticalDragDistance) &&
                (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
            {
                if (SelectedBuilder != null)
                {
                    SetDropTargetsFor(SelectedBuilder);
                    DragDrop.DoDragDrop(this, SelectedBuilder, DragDropEffects.Move | DragDropEffects.None);
                }
            }
        }

        /// <summary>Configure the appropriate vertexes to be drop targets for the supplied ElementBuilder</summary>
        internal void SetDropTargetsFor(ElementBuilder builder)
        {
            foreach (KeyValuePair<ElementVertex, VertexControl> eachKVP in VertexList)
            {
                if (eachKVP.Key.CanAcceptDropOf(builder))
                {
                    eachKVP.Value.AllowDrop = true;
                    eachKVP.Value.DragEnter += VertexDropTarget_DragEnter;
                    eachKVP.Value.DragLeave += VertexDropTarget_DragLeave;
                    eachKVP.Value.Drop += VertexDropTarget_Drop;
                    eachKVP.Value.Background = (Brush)FindResource("GradientBrushYes");
                }
                else
                {
                    eachKVP.Value.Background = eachKVP.Key.IsWordContents ? (Brush)FindResource("GhostWhiteBrush") : (Brush)FindResource("GradientBrushNo");
                }
            }
        }


        /// <summary>Configure all vertexes to NOT be drop targets.</summary>
        internal void ClearDropTargets()
        {
            foreach (KeyValuePair<ElementVertex, VertexControl> eachKVP in VertexList)
            {
                eachKVP.Value.AllowDrop = false;
                eachKVP.Value.DragEnter -= VertexDropTarget_DragEnter;
                eachKVP.Value.DragLeave -= VertexDropTarget_DragLeave;
                eachKVP.Value.Drop -= VertexDropTarget_Drop;
                eachKVP.Value.Background = eachKVP.Key.IsWordContents ? (Brush)FindResource("GhostWhiteBrush") : (Brush)FindResource("DarkGradientBrush");
            }
        }

        /// <summary>A drag has entered a vertex that is an active drop target</summary>
        private void VertexDropTarget_DragEnter(object sender, DragEventArgs e)
        {
            if (sender == this)
                e.Effects = DragDropEffects.Move;
            else
                e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        /// <summary>A drag has left a vertex that is an active drop target</summary>
        private void VertexDropTarget_DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        /// <summary>A drag has ended with a drop onto a vertex that is an active drop target</summary>
        /// <remarks>There are some non-obvious things about this method.  A successful drop will cause the underlying ElementBuilder tree to change form, which
        /// causes the ElementBuilderGraph to be regenerated and the ElementBuilderGraphArea to be redrawn.  Then we set the graph selection to the drop target.
        /// All of this means that the identity of all the user interface objects will change during the process.  The only thing we can count on to remain constant
        /// is the underlying model -- and even it changes form.</remarks>
        private void VertexDropTarget_Drop(object sender, DragEventArgs e)
        {
            VertexControl dropTarget = (VertexControl)sender;
            ElementVertex targetVertex = (ElementVertex)dropTarget.Vertex;
            if (targetVertex != null && VertexList.ContainsKey(targetVertex))
            {
                string[] formats = e.Data.GetFormats();
                string droppedBuilderFormat = formats[0];
                if (e.Data.GetDataPresent(droppedBuilderFormat))
                {
                    IElementTreeNode droppedNode = (IElementTreeNode)e.Data.GetData(droppedBuilderFormat);
                    if (targetVertex.AcceptDropOf(droppedNode))
                    {
                        ElementBuilder targetBuilder = targetVertex switch
                        {
                            ElementBuilderVertex ebv => ebv.Builder,
                            _ => null
                        };
                        if (targetBuilder != null)
                        SetSelectedNode(targetBuilder);
                    }
                }
            }
        }
    }
}

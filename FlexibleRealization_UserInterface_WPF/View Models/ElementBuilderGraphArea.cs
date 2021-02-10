using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using GraphX.Controls;
using GraphX.Controls.Models;

namespace FlexibleRealization.UserInterface.ViewModels
{
    public delegate void GraphRootAdded_EventHandler(IElementTreeNode root);

    public delegate void SelectedNodeChanged_EventHandler();

    public class ElementBuilderGraphArea : GraphArea<ElementVertex, ElementEdge, ElementBuilderGraph>
    {
        public ElementBuilderGraphArea() : base()
        {
            ControlFactory = new ElementBuilderControlFactory(this);
            VertexMouseMove += ElementBuilderGraphArea_VertexMouseMove;
            VertexMouseUp += ElementBuilderGraphArea_VertexMouseUp;
            SetVerticesDrag(false);
        }

        public override void GenerateGraph(bool generateAllEdges = true, bool dataContextToDataItem = true)
        {
            base.GenerateGraph(generateAllEdges, dataContextToDataItem);
            RemoveEdgeLabelsFromPartsOfSpeech();
            RegisterForVertexModelChangeNotifications();
            AssignVertexToolTips();
        }

        /// <summary>Clear the graph area.</summary>
        internal void Clear()
        {
            SelectedVertexControl = null;
            ClearLayout(true, true, true);
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

        /// <summary>Return the VertexControl whose ElementVertex is the view model for <paramref name="node"/></summary>
        private VertexControl VertexControlForNode(IElementTreeNode node) => VertexList.Values.Single(vertexControl => vertexControl.Vertex is ElementBuilderVertex ebv && ebv.Builder == node);

        /// <summary>Return the VertexControls that represent the children of <paramref name="parent"/></summary>
        private IEnumerable<VertexControl> ChildVerticesFor(ParentElementVertex parent) => VertexList
            .Where(kvp => kvp.Key is ElementBuilderVertex ebv && parent.Model.Children.Contains(ebv.Builder))
            .Select(kvp => kvp.Value)
            .OrderBy(control => ((ElementBuilderVertex)control.Vertex).Builder);

        /// <summary>Return the VertexControl that represents the word contents that correspond to <paramref name="partOfSpeechVertex"/></summary>
        private VertexControl WordContentsVertexControlCorrespondingTo(WordPartOfSpeechVertex partOfSpeechVertex) => VertexList
            .Where(kvp => kvp.Key is WordContentVertex wcv && wcv.Model == partOfSpeechVertex.Model)
            .Select(kvp => kvp.Value)
            .Single();

        /// <summary>Assign ToolTips for each VertexControl based on the state of its corresponding ElementVertex</summary>
        private void AssignVertexToolTips() 
        { 
            foreach (KeyValuePair<ElementVertex, VertexControl> kvp in VertexList) 
            { 
                kvp.Key.SetToolTipFor(kvp.Value); 
            } 
        }

        /// <summary>The selected VertexControl</summary>
        private VertexControl SelectedVertexControl;
        /// <summary>The IElementTreeNode represented by the selected VertexControl</summary>
        internal IElementTreeNode SelectedNode => ((ElementBuilderVertex)SelectedVertexControl?.Vertex).Builder;

        private void Highlight(VertexControl vertexControl)
        {
            HighlightBehaviour.SetHighlighted(vertexControl, true);
            if (SelectedVertexControl.Vertex is WordPartOfSpeechVertex wposv)
            {
                VertexControl correspondingWordContents = WordContentsVertexControlCorrespondingTo(wposv);
                HighlightBehaviour.SetHighlighted(correspondingWordContents, true);
            }
        }

        private void UnHighlight(VertexControl vertexControl)
        {
            HighlightBehaviour.SetHighlighted(vertexControl, false);
            if (vertexControl.Vertex is WordPartOfSpeechVertex wposv)
            {
                VertexControl correspondingWordContents = WordContentsVertexControlCorrespondingTo(wposv);
                HighlightBehaviour.SetHighlighted(correspondingWordContents, false);
            }
        }

        /// <summary>The user has started to move (NOT drag) <paramref name="movingVertexControl"/></summary>
        private void StartMoving(VertexControl movingVertexControl)
        {
            SetVerticesDrag(true);
            DragBehaviour.SetIsDragEnabled(movingVertexControl, true);
            DragBehaviour.SetIsTagged(movingVertexControl, true);
        }

        /// <summary>The user has stopped moving (NOT dragging) <paramref name="movingVertexControl"/></summary>
        private void StopMoving(VertexControl movingVertexControl)
        {
            DragBehaviour.SetIsTagged(movingVertexControl, false);
            DragBehaviour.SetIsDragEnabled(movingVertexControl, false);
            SetVerticesDrag(false);
        }

        /// <summary>Set <paramref name="node"/> as the selection in the graph</summary>
        internal void SetSelectedNode(IElementTreeNode node) => SetSelectedVertexControl(VertexControlForNode(node));

        /// <summary>Un-select the previously selected vertex control if any, and select <paramref name="selected"/></summary>
        private void SetSelectedVertexControl(VertexControl newlySelectedVertexControl)
        {
            if (SelectedVertexControl != null)
            {
                UnHighlight(SelectedVertexControl);
            }
            SelectedVertexControl = newlySelectedVertexControl;
            Highlight(newlySelectedVertexControl);
            OnSelectedNodeChanged();
        }

        internal event GraphRootAdded_EventHandler GraphRootAdded;
        private void OnGraphRootAdded(IElementTreeNode root) => GraphRootAdded?.Invoke(root);

        /// <summary>Subscribe to this event to be notified when the selected node changes.</summary>
        internal event SelectedNodeChanged_EventHandler SelectedNodeChanged;
        private void OnSelectedNodeChanged() => SelectedNodeChanged?.Invoke();

        /// <summary>We set vertexClickPosition when a vertex is first clicked, then use it during mouse move to decide whether to start a drag operation.</summary>
        private Point mouseDownPosition;

        #region Vertex Move and Drag / Drop

        /// <summary>If the mouse down event happened on a vertex, select that vertex.  If Left-Ctrl is pressed, start moving the selected vertex.</summary>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseDown(e);
            mouseDownPosition = e.GetPosition(this);
            VertexControl mouseDownVertexControl = GetVertexControlAt(mouseDownPosition);
            // Don't allow selection of a word content vertex
            if (mouseDownVertexControl != null && !(mouseDownVertexControl.Vertex is WordContentVertex))
            {
                SetSelectedVertexControl(mouseDownVertexControl);
                // Don't allow the user to move a part of speech vertex
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && !(mouseDownVertexControl.Vertex is WordPartOfSpeechVertex)) StartMoving(mouseDownVertexControl);
            }
        }

        /// <summary>If the user is holding down the mouse button, and Left-Ctrl is NOT pressed, and we've passed beyond the drag distance threshold, start dragging.</summary>
        private void ElementBuilderGraphArea_VertexMouseMove(object sender, VertexMovedEventArgs e)
        {
            if (e.Args.LeftButton == MouseButtonState.Pressed && !Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                Point currentMousePosition = e.Args.GetPosition(this);
                Vector mouseDownDistanceMoved = mouseDownPosition - currentMousePosition;
                if (Math.Abs(mouseDownDistanceMoved.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(mouseDownDistanceMoved.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    VertexControl draggedVertexControl = e.VertexControl;
                    StopMoving(draggedVertexControl);
                    ElementVertex draggedVertex = (ElementVertex)draggedVertexControl.Vertex;
                    if (draggedVertex is ElementBuilderVertex ebv)
                    {
                        Type draggedType = ebv.Builder.GetType();
                        SetDropTargetsFor(draggedType);
                        DataObject dataObject = new DataObject();
                        dataObject.SetData(typeof(IElementTreeNode), ebv.Builder);
                        DragDrop.DoDragDrop(this, dataObject, DragDropEffects.Move | DragDropEffects.None);
                        ClearDropTargets();
                    }
                }
                // If we're intercepting this event to start a drag operation, set it as handled.
                // Otherwise let it propagate, so it can be used by the GraphX framework for moving the vertex control
                e.Args.Handled = true;
            }
        }

        private void ElementBuilderGraphArea_VertexMouseUp(object sender, VertexSelectedEventArgs args) => StopMoving(SelectedVertexControl);

        /// <summary>Configure the appropriate vertexes to be drop targets for the supplied ElementBuilder.</summary>
        internal void SetDropTargetsFor(Type nodeType)
        {
            foreach (KeyValuePair<ElementVertex, VertexControl> eachKVP in VertexList)
            {
                if (eachKVP.Key.CanAcceptDropOf(nodeType))
                {
                    eachKVP.Value.AllowDrop = true;
                    eachKVP.Value.DragEnter += VertexDropTarget_DragEnter;
                    eachKVP.Value.DragLeave += VertexDropTarget_DragLeave;
                    eachKVP.Value.DragOver += VertexDropTarget_DragOver;
                    eachKVP.Value.Drop += VertexDropTarget_Drop;
                    eachKVP.Value.Background = (Brush)FindResource("VertexYesGradient");
                }
                else
                {
                    eachKVP.Value.Background = eachKVP.Key.IsWordContents ? (Brush)FindResource("GhostWhiteBrush") : (Brush)FindResource("VertexNoGradient");
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
                eachKVP.Value.DragOver -= VertexDropTarget_DragOver;
                eachKVP.Value.Drop -= VertexDropTarget_Drop;
                eachKVP.Value.Background = eachKVP.Key.IsWordContents ? (Brush)FindResource("GhostWhiteBrush") : (Brush)FindResource("DarkGrayGradient");
            }
        }

        /// <summary>A drag has entered a vertex that is an active drop target.</summary>
        private void VertexDropTarget_DragEnter(object sender, DragEventArgs e)
        {
            if ((e.AllowedEffects & DragDropEffects.Move) == DragDropEffects.Move)
                e.Effects = DragDropEffects.Move;
            else if ((e.AllowedEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
                e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        /// <summary>A drag has left a vertex that is an active drop target.</summary>
        private void VertexDropTarget_DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (activeInsertZone != null)
            {
                VertexControl dropTarget = (VertexControl)sender;
                AdornerLayer.GetAdornerLayer(dropTarget).Remove(activeInsertPointAdorner);
                activeInsertZone = null;
                activeInsertPointAdorner = null;
            }
            e.Handled = true;
        }

        private int? activeInsertZone = null;
        private ChildNodeInsertPoint activeInsertPointAdorner;
        private void VertexDropTarget_DragOver(object sender, DragEventArgs e)
        {
            VertexControl dropTarget = (VertexControl)sender; 
            switch (dropTarget.Vertex)
            {
                case ParentElementVertex parentVertex:
                    List<VertexControl> childVertices = ChildVerticesFor(parentVertex).ToList();
                    //int zoneCount = childVertices.Count + 1;
                    //double positionX = e.GetPosition(dropTarget).X;
                    //int zone = ZoneFromMousePosition(e.GetPosition(dropTarget).X, childVertices.Count + 1);
                    ShowInsertPointInParentZone(ZoneFromMousePosition(e.GetPosition(dropTarget).X, childVertices.Count + 1));
                    e.Handled = true;
                    break;

                    void ShowInsertPointInParentZone(int zone)
                    {
                        if (zone != activeInsertZone)
                        {
                            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(dropTarget);
                            if (activeInsertPointAdorner != null) adornerLayer.Remove(activeInsertPointAdorner);
                            if (zone == 0)
                            {
                                activeInsertPointAdorner = new ChildNodeInsertPoint(dropTarget, NodeRelation.First);
                            }
                            else if (zone == childVertices.Count)
                            {
                                activeInsertPointAdorner = new ChildNodeInsertPoint(dropTarget, NodeRelation.Last);
                            }
                            else
                            {
                                activeInsertPointAdorner = new ChildNodeInsertPoint(dropTarget, childVertices[zone - 1], childVertices[zone]);
                            }
                            adornerLayer.Add(activeInsertPointAdorner);
                            activeInsertZone = zone;
                        }
                    }
                case WordPartOfSpeechVertex wordVertex:
                    //double positionX = e.GetPosition(dropTarget).X;
                    int zone = ZoneFromMousePosition(e.GetPosition(dropTarget).X, 2);
                    ShowInsertPointInWordZone(ZoneFromMousePosition(e.GetPosition(dropTarget).X, 2)); // A word vertex has just two insert zones: before and after
                    e.Handled = true;
                    break;

                    void ShowInsertPointInWordZone(int zone)
                    {
                        if (zone != activeInsertZone)
                        {
                            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(dropTarget);
                            if (activeInsertPointAdorner != null) adornerLayer.Remove(activeInsertPointAdorner);
                            if (zone == 0)
                            {
                                activeInsertPointAdorner = new ChildNodeInsertPoint(dropTarget, NodeRelation.First);
                            }
                            else if (zone == 1)
                            {
                                activeInsertPointAdorner = new ChildNodeInsertPoint(dropTarget, NodeRelation.Last);
                            }
                            adornerLayer.Add(activeInsertPointAdorner);
                            activeInsertZone = zone;
                        }
                    }
            }

            int ZoneFromMousePosition(double x, int zoneCount) => (int)(x / (dropTarget.ActualWidth / zoneCount));
        }

        /// <summary>A drag has ended with a drop onto a vertex that is an active drop target.</summary>
        /// <remarks>There are some non-obvious things about this method.  A successful drop will cause the underlying ElementBuilder tree to change form, which
        /// causes the ElementBuilderGraph to be regenerated and the ElementBuilderGraphArea to be redrawn.  Then we set the graph selection to the drop target.
        /// All of this means that the identity of all the user interface objects will change during the process.  The only thing we can count on to remain constant
        /// is the underlying model -- and even it changes form.</remarks>
        private async void VertexDropTarget_Drop(object sender, DragEventArgs e)
        {
            VertexControl dropTarget = (VertexControl)sender;
            ElementVertex targetVertex = (ElementVertex)dropTarget.Vertex;
            if (targetVertex != null && VertexList.ContainsKey(targetVertex))
            {
                IElementTreeNode droppedNode = null;
                // We could get a dropped IElementTree node in one of two forms:
                // 1.  It's in the IDataObject as an IElementTreeNode, ready to use; or
                // 2.  It's in the IDataObject as a Task<ElementBuilder> that we can run to get the IElementTreeNode
                if (e.Data.GetDataPresent(typeof(IElementTreeNode)))
                {
                    droppedNode = (IElementTreeNode)e.Data.GetData(typeof(IElementTreeNode));
                }
                else if (e.Data.GetDataPresent(typeof(Task)))
                {
                    Task <ElementBuilder> getElementBuilderTask = (Task<ElementBuilder>)e.Data.GetData(typeof(Task));
                    droppedNode = await getElementBuilderTask;
                }
                if (droppedNode != null)
                {
                    if (targetVertex.AcceptDropOf(droppedNode, e.Effects, (int)activeInsertZone))
                    {
                        ElementBuilder targetBuilder = targetVertex switch
                        {
                            ElementBuilderVertex ebv => ebv.Builder,
                            _ => null
                        };
                        if (targetBuilder != null)
                        SetSelectedNode(targetBuilder.Stem);
                    }
                }
            }
            activeInsertZone = null;
            activeInsertPointAdorner = null;
        }

        #endregion Vertex Move and Drag / Drop
    }
}

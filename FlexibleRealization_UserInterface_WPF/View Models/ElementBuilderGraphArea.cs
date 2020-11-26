using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using GraphX.Common.Enums;
using GraphX.Controls;
using GraphX.Controls.Models;

namespace FlexibleRealization.UserInterface.ViewModels
{
    internal class ElementBuilderGraphArea : GraphArea<ElementVertex, ElementEdge, ElementBuilderGraph>, INotifyPropertyChanged
    {
        public ElementBuilderGraphArea() : base()
        {
            ControlFactory = new ElementBuilderControlFactory(this);
            SetVerticesDrag(true);
            VertexSelected += ElementBuilderGraphArea_VertexSelected;
        }

        public override void GenerateGraph(bool generateAllEdges = true, bool dataContextToDataItem = true)
        {
            base.GenerateGraph(generateAllEdges, dataContextToDataItem);
            RemoveEdgeLabelsFromPartsOfSpeech();
            RegisterForVertexModelChangeNotifications();
            AssignVertexToolTips();
        }

        /// <summary>Monitored by the ElementDescription <see cref="TextBlock"/> to display the description of the selected vertex's element</summary>
        public string SelectedElementDescription => SelectedElementProperties?.Description ?? "";

        /// <summary>Monitored by the PropertyGrid control to decide which element's properties to display</summary>
        public ElementProperties SelectedElementProperties { get; private set; }

        /// <summary>Labels are not needed on edges that connect a part of speech to its token</summary>
        private void RemoveEdgeLabelsFromPartsOfSpeech()
        {
            IEnumerable<EdgeControl> partOfSpeechToTokenEdges = EdgesList
                .Where(kvp => kvp.Key is PartOfSpeechToTokenEdge)
                .Select(kvp => kvp.Value);
            foreach (EdgeControl eachPartOfSpeechToTokenEdge in partOfSpeechToTokenEdges)
            {
                eachPartOfSpeechToTokenEdge.GetLabelControls().First().ShowLabel = false;
            }
        }

        /// <summary>Register to receive change notifications from each <see cref="ElementBuilderVertex"/> in this graph area</summary>
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

        /// <summary>One of the <see cref="ElementBuilder"/>s represented in this graph raised an event to inform us that it changed</summary>
        private void Builder_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ElementBuilder changedBuilder = sender as ElementBuilder;
            if (changedBuilder != null)
            {
                ElementVertex changedVertex = VertexForBuilder(changedBuilder);
                changedVertex.SetToolTipFor(VertexList[changedVertex]);
            }
        }

        /// <summary>Return the <see cref="ElementVertex"/> whose model is <paramref name="builder"/></summary>
        private ElementVertex VertexForBuilder(ElementBuilder builder) => VertexList.Keys.Single(vertex => vertex is ElementBuilderVertex ebv && ebv.Builder == builder);

        /// <summary>Assign <see cref="ToolTip"/>s for each <see cref="VertexControl"/> based on the state of its corresponding <see cref="ElementVertex"/></summary>
        private void AssignVertexToolTips() 
        { 
            foreach (KeyValuePair<ElementVertex, VertexControl> kvp in VertexList) 
            { 
                kvp.Key.SetToolTipFor(kvp.Value); 
            } 
        }

        /// <summary>The user has selected a vertex in the graph.</summary>
        private void ElementBuilderGraphArea_VertexSelected(object sender, VertexSelectedEventArgs args) => SetSelectedVertex((ElementVertex)args.VertexControl.Vertex);

        /// <summary>Update SelectedElementDescription and PropertyGrid to display the vertex selected in the graph</summary>
        internal void SetSelectedVertex(ElementVertex selectedVertex)
        {
            ClearAllTaggedVertices();
            VertexControl selectedControl = VertexList[selectedVertex];
            TagAndHighlight(selectedControl, true);
            switch (selectedVertex)
            {
                case PartOfSpeechVertex posv:
                    SetSelectedElementProperties(PartOfSpeechProperties.For(posv.Model));
                    break;
                case ParentElementVertex pev:
                    SetSelectedElementProperties(ParentProperties.For(pev.Model));
                    break;
                default: break;
            }

            /// <summary>Un-tag and un-highlight all vertices</summary>
            void ClearAllTaggedVertices() 
            { 
                foreach (VertexControl eachVertex in VertexList.Values) 
                { 
                    TagAndHighlight(eachVertex, false); 
                } 
            }

            /// <summary>Set the <see cref="DragBehavior.IsTaggedProperty"/> and <see cref="HighlightBehavior.HighlightedProperty"/> of <paramref name="vertex"/> to <paramref name="newState"/></summary>
            void TagAndHighlight(VertexControl vertex, bool newState)
            {
                HighlightBehaviour.SetHighlighted(vertex, newState);
                DragBehaviour.SetIsTagged(vertex, newState);
            }

            void SetSelectedElementProperties(ElementProperties properties)
            {
                SelectedElementProperties = properties;
                OnPropertyChanged("SelectedElementProperties");
                OnPropertyChanged("SelectedElementDescription");
            }
        }

        #region Standard implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        #endregion Standard implementation of INotifyPropertyChanged
    }
}

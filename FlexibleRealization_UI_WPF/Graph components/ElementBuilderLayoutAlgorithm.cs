using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GraphX.Measure;
using GraphX.Common.Interfaces;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    internal class ElementBuilderLayoutAlgorithm : IExternalLayout<ElementVertex, ElementEdge>
    {
        public ElementBuilderLayoutAlgorithm() { }
        public ElementBuilderLayoutAlgorithm(ElementBuilderGraph graph) => Graph = graph;

        private ElementBuilderGraph Graph;
        public IDictionary<ElementVertex, Point> VertexPositions { get; private set; } = new Dictionary<ElementVertex, Point>();
        public IDictionary<ElementVertex, Size> VertexSizes { get; set; }
        public bool NeedVertexSizes => true;
        public bool SupportsObjectFreeze => true;

        public void ResetGraph(IEnumerable<ElementVertex> vertices, IEnumerable<ElementEdge> edges)
        {
            Graph = default;
            Graph.AddVertexRange(vertices);
            Graph.AddEdgeRange(edges);
        }

        private const double VerticalGapBetweenElements = 80;
        private const double VerticalGapBetweenPartsOfSpeechAndTokens = 40;
        private const double HorizontalGap = 2;

        public void Compute(CancellationToken cancellationToken)
        {
            // Parent layers ordered by depth, bottom-to-top
            IEnumerable<IGrouping<int, ElementBuilderVertex>> parentElementLayers = ParentLayers
                .OrderByDescending(layer => layer.Key);
            double partsOfSpeechY = parentElementLayers.Count() * VerticalGapBetweenElements;
            SetPartOfSpeechPositions(partsOfSpeechY);
            double contentY = partsOfSpeechY + VerticalGapBetweenPartsOfSpeechAndTokens;
            SetWordContentPositions(contentY);
            foreach (IGrouping<int, ElementBuilderVertex> eachParentElementLayer in parentElementLayers)
            {
                SetPositionsForParentElementLayer(eachParentElementLayer);
            }
        }

        private IEnumerable<IGrouping<int, ParentElementVertex>> ParentLayers => Graph.ParentElements.GroupBy(parentVertex => parentVertex.Builder.Depth);

        /// <summary>Set the positions of the vertices in the layer for word parts of speech</summary>
        private void SetPartOfSpeechPositions(double centerY)
        {
            IEnumerable<WordPartOfSpeechVertex> partsOfSpeechLayer = Graph.PartsOfSpeech
                .OrderBy(partOfSpeechVertex => partOfSpeechVertex.Model);

            double nextLeftEdge = 0;
            foreach (WordPartOfSpeechVertex eachPartOfSpeechVertex in partsOfSpeechLayer)
            {
                double partOfSpeechX = nextLeftEdge + (VertexSizes[eachPartOfSpeechVertex].Width / 2);
                VertexPositions.Add(eachPartOfSpeechVertex, new Point(partOfSpeechX, centerY));
                nextLeftEdge = partOfSpeechX + (VertexSizes[eachPartOfSpeechVertex].Width / 2) + HorizontalGap;
            }
        }

        /// <summary>Set the positions of the vertices in the layer for word contents.  This is the lowest layer in the graph.</summary>
        private void SetWordContentPositions(double centerY)
        {
            foreach (WordPartOfSpeechVertex eachPartOfSpeechVertex in Graph.PartsOfSpeech)
            {
                WordContentVertex correspondingWordContentVertex = Graph.WordContentsCorrespondingTo(eachPartOfSpeechVertex);
                Point partOfSpeechPosition = VertexPositions[eachPartOfSpeechVertex];
                VertexPositions.Add(correspondingWordContentVertex, new Point(partOfSpeechPosition.X, centerY));
            }
        }

        private void SetPositionsForParentElementLayer(IGrouping<int, ElementBuilderVertex> parentElementLayer)
        {
            double centerY = (parentElementLayer.Key) * VerticalGapBetweenElements;
            foreach (ParentElementVertex eachParentElementVertex in parentElementLayer.ToList())
            {
                double horizontalCenterOfSpannedPartsOfSpeech = Graph
                    .ChildrenOf(eachParentElementVertex.Model)
                    .Average(posv => CenterOf(posv).X);  // Average the CENTERS of the spanned part of speech vertices
                double leftEdgeOfThisSyntaxElement = horizontalCenterOfSpannedPartsOfSpeech - (VertexSizes[eachParentElementVertex].Width / 2);
                VertexPositions.Add(eachParentElementVertex, new Point(leftEdgeOfThisSyntaxElement, centerY));
            }
        }

        private Point CenterOf(ElementVertex vertex) => new Point(VertexPositions[vertex].X + (VertexSizes[vertex].Width / 2), VertexPositions[vertex].Y + (VertexSizes[vertex].Height / 2));
    }
}

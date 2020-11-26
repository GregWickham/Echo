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

        private const double VerticalGapBetweenElements = 70;
        private const double VerticalGapBetweenPartsOfSpeechAndTokens = 40;
        private const double HorizontalGap = 2;

        public void Compute(CancellationToken cancellationToken)
        {
            IEnumerable<IGrouping<int, ElementBuilderVertex>> ParentElementLayers = Graph.ParentElements
                .GroupBy(parentVertex => parentVertex.Builder.Depth);
            double tokensY = (ParentElementLayers.Count() * VerticalGapBetweenElements) + VerticalGapBetweenPartsOfSpeechAndTokens;
            SetTokenPositions(tokensY);
            double partsOfSpeechY = tokensY - VerticalGapBetweenPartsOfSpeechAndTokens;
            SetPartOfSpeechPositions(partsOfSpeechY);
            foreach (IGrouping<int, ElementBuilderVertex> eachParentElementLayer in ParentElementLayers)
            {
                SetPositionsForParentElementLayer(eachParentElementLayer);
            }
        }

        private void SetTokenPositions(double centerY)
        {
            IEnumerable<TokenVertex> tokensLayer = Graph.Tokens
                .OrderBy(tokenVertex => tokenVertex.Model.Index);

            double nextLeftEdge = 0;
            foreach (TokenVertex eachTokenVertex in tokensLayer)
            {
                double tokenX = nextLeftEdge + (VertexSizes[eachTokenVertex].Width / 2);
                VertexPositions.Add(eachTokenVertex, new Point(tokenX, centerY));
                nextLeftEdge = tokenX + (VertexSizes[eachTokenVertex].Width / 2) + HorizontalGap;
            }
        }

        private void SetPartOfSpeechPositions(double centerY)
        {
            foreach (TokenVertex eachTokenVertex in Graph.Tokens)
            {
                PartOfSpeechVertex correspondingPartOfSpeechVertex = Graph.PartOfSpeechCorrespondingTo(eachTokenVertex);
                Point tokenPosition = VertexPositions[eachTokenVertex];
                VertexPositions.Add(correspondingPartOfSpeechVertex, new Point(tokenPosition.X, centerY));
            }
        }

        private void SetPositionsForParentElementLayer(IGrouping<int, ElementBuilderVertex> parentElementLayer)
        {
            double centerY = (parentElementLayer.Key) * VerticalGapBetweenElements;
            foreach (ParentElementVertex eachParentElementVertex in parentElementLayer.ToList())
            {
                double horizontalCenterOfSpannedPartsOfSpeech = Graph
                    .PartsOfSpeechSpannedBy(eachParentElementVertex.Model)
                    .Average(posv => CenterOf(posv).X);  // Average the CENTERS of the spanned part of speech vertices
                double leftEdgeOfThisSyntaxElement = horizontalCenterOfSpannedPartsOfSpeech - (VertexSizes[eachParentElementVertex].Width / 2);
                VertexPositions.Add(eachParentElementVertex, new Point(leftEdgeOfThisSyntaxElement, centerY));
            }
        }

        private Point CenterOf(ElementVertex vertex) => new Point(VertexPositions[vertex].X + (VertexSizes[vertex].Width / 2), VertexPositions[vertex].Y + (VertexSizes[vertex].Height / 2));
    }
}

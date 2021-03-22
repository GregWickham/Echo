using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GraphX.Measure;
using GraphX.Common.Interfaces;
using GraphX.Logic.Algorithms.OverlapRemoval;
using MathNet.Spatial.Euclidean;
using MathNet.Spatial.Units;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    public class ElementBuilderOverlapRemovalAlgorithm : IExternalOverlapRemoval<ElementVertex>
    {
        public IDictionary<ElementVertex, Rect> Rectangles { get; set; }

        public void Compute(CancellationToken cancellationToken)
        {
            RunFSA(cancellationToken);
            RealignPartsOfSpeechWithTokens();
            CenterParentElementsOverChildren();
            AdjustParentsUsingExclusionZones();
        }

        private void RunFSA(CancellationToken cancellationToken)
        {
            OverlapRemovalParameters parameters = new OverlapRemovalParameters
            {
                HorizontalGap = 20,
                VerticalGap = 30
            };
            var oneWayFSA = new FSAAlgorithm<ElementVertex>(Rectangles, parameters);
            oneWayFSA.Compute(cancellationToken);
            Rectangles = oneWayFSA.Rectangles;
        }

        private const double DesiredHorizontalSeparation = 15;

        private IEnumerable<ParentElementVertex> ParentElements => Rectangles
            .Where(kvp => kvp.Key is ParentElementVertex)
            .Select(kvp => kvp.Key)
            .Cast<ParentElementVertex>();

        private IEnumerable<IGrouping<double, ParentElementVertex>> ParentLayers => Rectangles
            .Where(kvp => kvp.Key is ParentElementVertex)
            .Select(kvp => kvp.Key)
            .Cast<ParentElementVertex>()
            .GroupBy(parent => Rectangles[parent].GetCenter().Y);

        private IEnumerable<WordPartOfSpeechVertex> PartsOfSpeech => Rectangles
            .Where(kvp => kvp.Key is WordPartOfSpeechVertex)
            .Select(kvp => kvp.Key)
            .Cast<WordPartOfSpeechVertex>();

        private IEnumerable<WordContentVertex> WordContents => Rectangles
            .Where(kvp => kvp.Key is WordContentVertex)
            .Select(kvp => kvp.Key)
            .Cast<WordContentVertex>();

        private WordPartOfSpeechVertex PartOfSpeechCorrespondingTo(WordContentVertex contentVertex) => PartsOfSpeech.Single(partOfSpeechVertex => partOfSpeechVertex.Model == contentVertex.Model);
            //.Where(partOfSpeechVertex => partOfSpeechVertex.Model == contentVertex.Model)
            //.First();

        private IEnumerable<ElementBuilderVertex> ChildrenOf(ParentElementBuilder parentElement) => Rectangles
            .Where(kvp => kvp.Key is ElementBuilderVertex ebv && parentElement.Children.Contains(ebv.Builder))
            .Select(kvp => kvp.Key)
            .Cast<ElementBuilderVertex>()
            .OrderBy(ebv => ebv.Builder)
            .ToList();

        private List<ParentChildConnector> GetParentChildConnectors()
        {
            List<ParentChildConnector> result = new List<ParentChildConnector>();
            foreach (ParentElementVertex eachParentVertex in ParentElements)
            {
                Point parentCenter = Rectangles[eachParentVertex].GetCenter();
                Point2D parentCenter2D = new Point2D(parentCenter.X, parentCenter.Y);
                foreach (ElementBuilderVertex eachChildVertex in ChildrenOf(eachParentVertex.Model))
                {
                    Point childCenter = Rectangles[eachChildVertex].GetCenter();
                    Point2D childCenter2D = new Point2D(childCenter.X, childCenter.Y);
                    result.Add(new ParentChildConnector(eachParentVertex, eachChildVertex));
                }
            }
            return result;
        }

        /// <summary>Return the ParentChildConnectors that pass through the horizontal layer defined by <paramref name="yPosition"/></summary>
        private IEnumerable<ParentChildConnector> GetParentChildConnectorsThatCross(double yPosition) => GetParentChildConnectors()
            .Where(connector => Rectangles[connector.Parent].GetCenter().Y < yPosition && Rectangles[connector.Child].GetCenter().Y > yPosition);

        private IEnumerable<LayerExclusionZone> GetExclusionZonesForLayer(double layerY) => GetParentChildConnectorsThatCross(layerY)
            .Select(connector => new LayerExclusionZone(connector, this));

        //private IEnumerable<WordPartOfSpeechVertex> PartsOfSpeechSpannedBy(ParentElementBuilder parentElement)
        //{
        //    IEnumerable<PartOfSpeechBuilder> partsOfSpeechInSubtree = parentElement.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>();
        //    return PartsOfSpeech.Where(partOfSpeechVertex => partsOfSpeechInSubtree.Contains(partOfSpeechVertex.Model));
        //}

        private void RealignPartsOfSpeechWithTokens()
        {
            IDictionary<ElementVertex, Rect> newRectangles = new Dictionary<ElementVertex, Rect>();
            foreach (ElementVertex eachElementVertex in Rectangles.Keys)
            {
                newRectangles.Add(eachElementVertex, Rectangles[eachElementVertex]);
            }
            foreach (WordContentVertex eachContentVertex in WordContents)
            {
                Rect tokenRectangle = Rectangles[eachContentVertex];
                WordPartOfSpeechVertex correspondingPartOfSpeech = PartOfSpeechCorrespondingTo(eachContentVertex);
                Rect oldPartOfSpeechRectangle = Rectangles[correspondingPartOfSpeech];
                double newPartOfSpeechX = tokenRectangle.X + ((tokenRectangle.Width - oldPartOfSpeechRectangle.Width) / 2);
                newRectangles[correspondingPartOfSpeech] = new Rect(new Point(newPartOfSpeechX, oldPartOfSpeechRectangle.Y), oldPartOfSpeechRectangle.Size);
            }
            Rectangles = newRectangles;
        }

        private void CenterParentElementsOverChildren()
        {
            foreach (ParentElementVertex eachParentVertex in ParentElements.ToList())
            {
                double horizontalCenterOfChildren = ChildrenOf(eachParentVertex.Model)
                    .Average(posv => Rectangles[posv].GetCenter().X);  // Average the CENTERS of the child vertices
                Rect oldRectForThisParent = Rectangles[eachParentVertex];
                double newLeftEdgeOfThisParent = horizontalCenterOfChildren - (Rectangles[eachParentVertex].Width / 2);
                Point newTopLeft = new Point(newLeftEdgeOfThisParent, Rectangles[eachParentVertex].Y);
                Rectangles[eachParentVertex] = new Rect(newTopLeft, oldRectForThisParent.Size);
            }
        }

        private void AdjustParentsUsingExclusionZones()
        {
            foreach (IGrouping<double, ParentElementVertex> eachLayer in ParentLayers.OrderByDescending(layer => layer.Key))
            {
                List<LayerExclusionZone> exclusionZones = GetExclusionZonesForLayer(eachLayer.Key).ToList();
                foreach (ParentElementVertex eachParentVertex in eachLayer)
                {
                    LayerExclusionZone zoneThatShouldBeToTheLeftOfThisVertex = exclusionZones
                        .Where(zone => zone.MaximumIndex < eachParentVertex.Model.MinimumIndex)
                        .OrderBy(zone => zone.MaximumIndex)
                        .LastOrDefault();
                    LayerExclusionZone zoneThatShouldBeToTheRightOfThisVertex = exclusionZones
                        .Where(zone => zone.MinimumIndex > eachParentVertex.Model.MaximumIndex)
                        .OrderBy(zone => zone.MaximumIndex)
                        .FirstOrDefault();
                    CheckForAnyCollisions();

                    void CheckForAnyCollisions()
                    {
                        bool collisionOnTheLeft = zoneThatShouldBeToTheLeftOfThisVertex != null
                            && zoneThatShouldBeToTheLeftOfThisVertex.HasCollisionWith(eachParentVertex, Direction.Left);
                        bool collisionOnTheRight = zoneThatShouldBeToTheRightOfThisVertex != null
                            && zoneThatShouldBeToTheRightOfThisVertex.HasCollisionWith(eachParentVertex, Direction.Right);
                        if (collisionOnTheLeft && collisionOnTheRight)
                        {
                            Squeeze();
                        }
                        else if (collisionOnTheLeft && !collisionOnTheRight)
                        {
                            Point newTopLeft = new Point(zoneThatShouldBeToTheLeftOfThisVertex.SuggestedLeftOf(eachParentVertex), Rectangles[eachParentVertex].Top);
                            Rectangles[eachParentVertex] = new Rect(newTopLeft, Rectangles[eachParentVertex].Size);
                            CheckForNewlyCreatedCollision(Direction.Right);
                        }
                        else if (!collisionOnTheLeft && collisionOnTheRight)
                        {
                            Point newTopLeft = new Point(zoneThatShouldBeToTheRightOfThisVertex.SuggestedRightOf(eachParentVertex) - Rectangles[eachParentVertex].Width, Rectangles[eachParentVertex].Top);
                            Rectangles[eachParentVertex] = new Rect(newTopLeft, Rectangles[eachParentVertex].Size);
                            CheckForNewlyCreatedCollision(Direction.Left);
                        }
                    }

                    // After moving a vertex in one direction to correct a collision on one side, we might have created a new collision on the other side
                    void CheckForNewlyCreatedCollision(Direction directionWhereTheNewCollisionMayHaveBeenCreated)
                    {
                        if (directionWhereTheNewCollisionMayHaveBeenCreated == Direction.Left)
                        {
                            if (zoneThatShouldBeToTheLeftOfThisVertex != null
                                && zoneThatShouldBeToTheLeftOfThisVertex.HasCollisionWith(eachParentVertex, Direction.Left)) Squeeze();
                        }
                        else
                        {
                            if (zoneThatShouldBeToTheRightOfThisVertex != null
                                && zoneThatShouldBeToTheRightOfThisVertex.HasCollisionWith(eachParentVertex, Direction.Right)) Squeeze();
                        }
                    }

                    // We don't have enough space to accommodate DesireHorizontalSeparation on both the left and right, so split the difference
                    void Squeeze()
                    {
                        double suggestedLeftFromTheLeft = zoneThatShouldBeToTheLeftOfThisVertex.SuggestedLeftOf(eachParentVertex);
                        double suggestedLeftFromTheRight = zoneThatShouldBeToTheRightOfThisVertex.SuggestedRightOf(eachParentVertex) - Rectangles[eachParentVertex].Width;
                        double averageSuggestedLeft = (suggestedLeftFromTheLeft + suggestedLeftFromTheRight) / 2;
                        Point newTopLeft = new Point(averageSuggestedLeft, Rectangles[eachParentVertex].Top);
                        Rectangles[eachParentVertex] = new Rect(newTopLeft, Rectangles[eachParentVertex].Size);
                    }
                }
            }
        }

        private enum Direction { Left, Right }

        /// <summary>Represents the connection between a ParentElementVertex and one of its child vertices</summary>
        private class ParentChildConnector
        {
            internal ParentChildConnector(ParentElementVertex parent, ElementBuilderVertex child)
            {
                Parent = parent;
                Child = child;
            }

            internal ParentElementVertex Parent { get; set; }
            internal ElementBuilderVertex Child { get; set; }
        }

        /// <summary>Does calculations to adjust the horizontal position of a vertex so it doesn't overlap with a ParentChildConnector</summary>
        private class LayerExclusionZone
        {
            internal LayerExclusionZone(ParentChildConnector connector, ElementBuilderOverlapRemovalAlgorithm overlapRemoval)
            {
                Connector = connector;
                Algorithm = overlapRemoval; 
                Point2D parentCenter = new Point2D(Algorithm.Rectangles[Connector.Parent].GetCenter().X, Algorithm.Rectangles[Connector.Parent].GetCenter().Y);
                Point2D childCenter = new Point2D(Algorithm.Rectangles[Connector.Child].GetCenter().X, Algorithm.Rectangles[Connector.Child].GetCenter().Y);
                ParentChildCentersConnectingLine = new LineSegment2D(parentCenter, childCenter);
            }

            private ParentChildConnector Connector;
            private ElementBuilderOverlapRemovalAlgorithm Algorithm;
            private LineSegment2D ParentChildCentersConnectingLine;

            internal bool HasCollisionWith(ElementVertex vertex, Direction desiredRelationToVertex)
            {
                if (desiredRelationToVertex == Direction.Left)
                {
                    if (ConnectorSlopesUp)
                        return Algorithm.Rectangles[vertex].Left - XIntersectWith(Algorithm.Rectangles[vertex].Top) < DesiredHorizontalSeparation;
                    else
                        return Algorithm.Rectangles[vertex].Left - XIntersectWith(Algorithm.Rectangles[vertex].Bottom) < DesiredHorizontalSeparation;
                }
                else
                {
                    if (ConnectorSlopesUp)
                        return XIntersectWith(Algorithm.Rectangles[vertex].Bottom) - Algorithm.Rectangles[vertex].Right < DesiredHorizontalSeparation;
                    else
                        return XIntersectWith(Algorithm.Rectangles[vertex].Top) - Algorithm.Rectangles[vertex].Right < DesiredHorizontalSeparation;
                }
            }

            internal double SuggestedLeftOf(ElementVertex vertex)
            {
                if (ConnectorSlopesUp)
                    return XIntersectWith(Algorithm.Rectangles[vertex].Top) + DesiredHorizontalSeparation;
                else
                    return XIntersectWith(Algorithm.Rectangles[vertex].Bottom) + DesiredHorizontalSeparation;
            }

            internal double SuggestedRightOf(ElementVertex vertex)
            {
                if (ConnectorSlopesUp)
                    return XIntersectWith(Algorithm.Rectangles[vertex].Bottom) - DesiredHorizontalSeparation;
                else
                    return XIntersectWith(Algorithm.Rectangles[vertex].Top) - DesiredHorizontalSeparation;
            }

            private double Left => Math.Min(Algorithm.Rectangles[Connector.Parent].Left, Algorithm.Rectangles[Connector.Child].Left);
            private double Right => Math.Max(Algorithm.Rectangles[Connector.Parent].Right, Algorithm.Rectangles[Connector.Child].Right);
            private LineSegment2D HorizontalLineSegmentAt(double yCoordinate) => new LineSegment2D(new Point2D(Left, yCoordinate), new Point2D(Right, yCoordinate));
            private double XIntersectWith(double yCoordinate)
            {
                Point2D intersect;
                if (ParentChildCentersConnectingLine.TryIntersect(HorizontalLineSegmentAt(yCoordinate), out intersect, Angle.FromDegrees(0)))
                {
                    return intersect.X;
                }
                else throw new InvalidOperationException("Parent-child connecting line does not intercept the horizontal line segment");
            }

            private bool ConnectorSlopesUp => Algorithm.Rectangles[Connector.Parent].GetCenter().X > Algorithm.Rectangles[Connector.Child].GetCenter().X;
            private bool ConnectorSlopesDown => Algorithm.Rectangles[Connector.Child].GetCenter().X > Algorithm.Rectangles[Connector.Parent].GetCenter().X;

            internal int MinimumIndex => Connector.Child.Builder.MinimumIndex;
            internal int MaximumIndex => Connector.Child.Builder.MaximumIndex;
        }
    }
}

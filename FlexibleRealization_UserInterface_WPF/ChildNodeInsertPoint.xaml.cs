using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using GraphX.Controls;

namespace FlexibleRealization.UserInterface
{
    internal partial class ChildNodeInsertPoint : Adorner
    {
        /// <summary>Use this constructor to make a ChildNodeInsertPoint between two existing child nodes</summary>
        internal ChildNodeInsertPoint(VertexControl adornedVertex, VertexControl before, VertexControl after) : base(adornedVertex)
        {
            Center = adornedVertex.GetDesiredCenter();
            Vector offset = Center - adornedVertex.GetCenterPosition();
            Point beforeVertexCenter = before.GetCenterPosition() + offset;
            Vector beforeVector = beforeVertexCenter - Center;
            Point afterVertexCenter = after.GetCenterPosition() + offset;
            Vector afterVector = afterVertexCenter - Center;
            double vectorLength = adornedVertex.DesiredSize.Height * 2;
            beforeVector = beforeVector * (vectorLength / beforeVector.Length);
            afterVector = afterVector * (vectorLength / afterVector.Length);
            BeforePoint = Center + beforeVector;
            AfterPoint = Center + afterVector;
            Radius = vectorLength;
            // Setting IsHitTestVisible=false prevents the adorner from stealing DragOver events from the adorned vertex
            IsHitTestVisible = false;
        }

        /// <summary>Use this constructor to make a ChildNodeInsertPoint at either the first or last position (not somewhere in the middle)</summary>
        internal ChildNodeInsertPoint(VertexControl adornedVertex, NodeRelation relation) : base(adornedVertex)
        {
            Center = adornedVertex.GetDesiredCenter();
            Vector beforeVector;
            Matrix beforeMatrix = Matrix.Identity;
            Vector afterVector;
            Matrix afterMatrix = Matrix.Identity;
            switch (relation)
            {
                case NodeRelation.First:
                    Vector leftVector = Center - new Point(Center.X - adornedVertex.DesiredSize.Height * 2.5, Center.Y);
                    beforeMatrix.Rotate(30);
                    beforeVector = beforeMatrix.Transform(leftVector);
                    afterMatrix.Rotate(-30);
                    afterVector = afterMatrix.Transform(leftVector);
                    BeforePoint = Center - beforeVector;
                    AfterPoint = Center - afterVector;
                    Radius = afterVector.Length;
                    break;
                case NodeRelation.Last:
                    Vector rightVector = Center - new Point(Center.X + adornedVertex.DesiredSize.Height * 2.5, Center.Y);
                    beforeMatrix.Rotate(30);
                    beforeVector = beforeMatrix.Transform(rightVector);
                    afterMatrix.Rotate(-30);
                    afterVector = afterMatrix.Transform(rightVector);
                    BeforePoint = Center - beforeVector;
                    AfterPoint = Center - afterVector;
                    break;
            }
            // Setting IsHitTestVisible=false prevents the adorner from stealing DragOver events from the adorned vertex
            IsHitTestVisible = false;
        }

        /// <summary>The Brush to use for rendering</summary>
        private static Brush RenderBrush = new RadialGradientBrush(Colors.LightBlue, Colors.Transparent);

        /// <summary>The center point of the pie wedge</summary>
        private Point Center;
        /// <summary>The point at the leading (left) outer edge of the pie wedge</summary>
        private Point BeforePoint;
        /// <summary>The point at the trailing (right) outer edge of the pie wedge</summary>
        private Point AfterPoint;
        /// <summary>The radius of the pie wedge</summary>
        private double Radius;

        protected override void OnRender(DrawingContext drawingContext)
        {
            PathGeometry pieWedge = new PathGeometry
            {
                Figures = new PathFigureCollection
                {
                    new PathFigure
                    {
                        StartPoint = Center,
                        IsClosed = true,
                        Segments = new PathSegmentCollection
                        {
                            new LineSegment(BeforePoint, false),
                            new LineSegment(AfterPoint, false),
                            //new ArcSegment(AfterPoint, new Size(Radius, Radius), 0, false, SweepDirection.Counterclockwise, false ),
                            new LineSegment(Center, false)
                        }
                    }
                }
            };
            drawingContext.DrawGeometry(RenderBrush, null, pieWedge);
        }

    }
}

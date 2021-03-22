using System.Windows;
using GraphX.Controls;

namespace FlexibleRealization.UserInterface
{
    internal static class VertexControl_Extensions
    {
        internal static Point GetDesiredCenter(this VertexControl vertexControl)
        {
            Rect vertexRect = new Rect(vertexControl.DesiredSize);
            return new Point(vertexRect.X + vertexRect.Width / 2, vertexRect.Y + vertexRect.Height / 2);
        }

    }
}

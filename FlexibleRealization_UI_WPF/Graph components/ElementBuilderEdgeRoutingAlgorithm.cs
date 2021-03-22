using System.Threading;
using System.Collections.Generic;
using GraphX.Measure;
using GraphX.Common.Interfaces;
using FlexibleRealization.UserInterface.ViewModels;
using GraphX.Logic.Algorithms.EdgeRouting;

namespace FlexibleRealization.UserInterface
{ 
    public class ElementBuilderEdgeRoutingAlgorithm : IExternalEdgeRouting<ElementVertex, ElementEdge>
    {
        //public ElementBuilderEdgeRoutingAlgorithm()

        internal ElementBuilderGraph Graph { get; set; }

        public void Compute(CancellationToken cancellationToken)
        {
            SimpleERParameters serParameters = new SimpleERParameters
            {
                BackStep = 10,
                SideStep = 10
            };
            SimpleEdgeRouting<ElementVertex, ElementEdge, ElementBuilderGraph> simpleER = new SimpleEdgeRouting<ElementVertex, ElementEdge, ElementBuilderGraph>(Graph, VertexPositions, VertexSizes, serParameters);
            simpleER.Compute(cancellationToken);
            EdgeRoutes = simpleER.EdgeRoutes;
        }

        public IDictionary<ElementVertex, Rect> VertexSizes { get; set; }

        public IDictionary<ElementVertex, Point> VertexPositions { get; set; }

        //readonly Dictionary<ElementEdge, Point[]> _edgeRoutes = new Dictionary<ElementEdge, Point[]>();
        public IDictionary<ElementEdge, Point[]> EdgeRoutes { get; private set; }

        public Point[] ComputeSingle(ElementEdge edge) { return null; }

        public void UpdateVertexData(ElementVertex vertex, Point position, Rect size) { }

        public Rect AreaRectangle { get; set; }
    }
}

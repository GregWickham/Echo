using GraphX.Logic.Models;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    internal class ElementBuilderLogicCore : GXLogicCore<ElementVertex, ElementEdge, ElementBuilderGraph> 
    { 
        internal ElementBuilderLogicCore(ElementBuilderGraph ebGraph)
        {            
            ExternalLayoutAlgorithm = new ElementBuilderLayoutAlgorithm(ebGraph);
            ExternalOverlapRemovalAlgorithm = new ElementBuilderOverlapRemovalAlgorithm();
            ExternalEdgeRoutingAlgorithm = new ElementBuilderEdgeRoutingAlgorithm { Graph = ebGraph };
            AsyncAlgorithmCompute = false;

            Graph = ebGraph;
        }        
    }
}

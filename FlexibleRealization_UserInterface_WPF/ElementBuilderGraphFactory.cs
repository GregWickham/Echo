using System;
using FlexibleRealization.Dependencies;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    internal static class ElementBuilderGraphFactory
    {
        internal static ElementBuilderGraph GraphOf(IElementBuilder model)
        {
            ElementBuilderGraph graph = new ElementBuilderGraph();
            AddSubtreeIncludingRoot(model);
            //AddSyntacticRelationEdges();
            return graph;

            ElementBuilderVertex AddSubtreeIncludingRoot(IElementBuilder builder, ElementBuilderVertex parentVertex = null)
            {
                switch (builder)
                {
                    case ParentElementBuilder peb:
                        return AddSubtree(peb);
                    case WordElementBuilder web:
                        return AddLeafVertexFor(web);
                    default: throw new InvalidOperationException("ElementGraphBuilder doesn't handle this ElementBuilder type");
                }

                ParentElementVertex AddSubtree(ParentElementBuilder parentElementBuilder)
                {
                    ParentElementVertex parentElementVertex = new ParentElementVertex(parentElementBuilder);
                    graph.AddVertex(parentElementVertex);

                    foreach (ElementBuilder eachChild in parentElementBuilder.Children)
                    {
                        ElementBuilderVertex eachChildVertex = AddSubtreeIncludingRoot(eachChild, parentElementVertex);
                        graph.AddEdge(new ParentElementToChildEdge(parentElementVertex, eachChildVertex, parentElementBuilder.RoleFor(eachChild)));
                    }

                    return parentElementVertex;
                }

                WordPartOfSpeechVertex AddLeafVertexFor(WordElementBuilder wordElementBuilder)
                {
                    WordPartOfSpeechVertex partOfSpeechVertex = new WordPartOfSpeechVertex(wordElementBuilder);
                    graph.AddVertex(partOfSpeechVertex);
                    // The token node is the one actually containing the word
                    WordContentVertex contentVertex = new WordContentVertex(wordElementBuilder.WordSource);
                    graph.AddVertex(contentVertex);
                    PartOfSpeechToContentEdge contentEdge = new PartOfSpeechToContentEdge(partOfSpeechVertex, contentVertex);
                    graph.AddEdge(contentEdge);
                    return partOfSpeechVertex;
                }
            }

            //void AddSyntacticRelationEdges()
            //{
            //    foreach (PartOfSpeechVertex eachPartOfSpeechVertex in graph.PartsOfSpeech)
            //    {
            //        PartOfSpeechBuilder eachPartOfSpeechBuilder = eachPartOfSpeechVertex.Model;
            //        foreach (SyntacticRelation eachDependency in eachPartOfSpeechBuilder.IncomingSyntacticRelations)
            //        {
            //            graph.AddEdge(new DependencyEdge(graph.TokenVertexFor(eachDependency.Governor), graph.TokenVertexFor(eachDependency.Dependent), eachDependency.Relation));
            //        }
            //    }
            //}
        }
    }
}


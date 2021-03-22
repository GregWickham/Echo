using System;
using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    public class ElementBuilderGraph : BidirectionalGraph<ElementVertex, ElementEdge> 
    {
        /// <summary>Return all the vertices that represent ElementBuilders</summary>
        internal IEnumerable<ElementBuilderVertex> ElementBuilders => Vertices.Where(vertex => vertex is ElementBuilderVertex).Cast<ElementBuilderVertex>();

        /// <summary>Return all the vertices that represent ParentElementBuilders</summary>
        internal IEnumerable<ParentElementVertex> ParentElements => Vertices.Where(vertex => vertex is ParentElementVertex).Cast<ParentElementVertex>();

        /// <summary>Return all the vertices that represent word parts of speech</summary>
        internal IEnumerable<WordPartOfSpeechVertex> PartsOfSpeech => Vertices.Where(vertex => vertex is WordPartOfSpeechVertex).Cast<WordPartOfSpeechVertex>();

        /// <summary>Return all the vertices that represent word contents</summary>
        internal IEnumerable<WordContentVertex> WordContents => Vertices.Where(vertex => vertex is WordContentVertex).Cast<WordContentVertex>();

        /// <summary>Return the vertex that represents the RootNode of the ElementBuilder tree</summary>
        internal ElementBuilderVertex Root => ElementBuilders.Where(vertex => vertex.Builder.Parent is RootNode).Single();

        /// <summary>Return the vertex representing the word contents that correspond to <paramref name="partOfSpeech"/></summary>
        internal WordContentVertex WordContentsCorrespondingTo(WordPartOfSpeechVertex partOfSpeech) => WordContents.Single(vertex => vertex.Model == partOfSpeech.Model);

        internal WordContentVertex WordContentVertexFor(WordElementBuilder partOfSpeech) => WordContents.Single(vertex => vertex.Model == partOfSpeech);

        internal WordPartOfSpeechVertex PartOfSpeechCorrespondingTo(WordContentVertex token) => PartsOfSpeech.Single(partOfSpeech => partOfSpeech.Model == token.Model);

        internal IEnumerable<ElementBuilderVertex> ChildrenOf(ParentElementBuilder parentElement) => ElementBuilders
            .Where(elementBuilderVertex => parentElement.Children.Contains(elementBuilderVertex.Builder));

        internal IEnumerable<WordPartOfSpeechVertex> PartsOfSpeechSpannedBy(ParentElementBuilder parentElement)
        {
            IEnumerable<PartOfSpeechBuilder> partsOfSpeechInSubtree = parentElement.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>();
            return PartsOfSpeech.Where(partOfSpeechVertex => partsOfSpeechInSubtree.Contains(partOfSpeechVertex.Model));
        }

        internal static ElementBuilderGraph Of(IElementBuilder model)
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
                    WordContentVertex contentVertex = new WordContentVertex(wordElementBuilder);
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

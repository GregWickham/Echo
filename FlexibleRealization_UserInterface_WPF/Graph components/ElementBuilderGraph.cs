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
        internal WordContentVertex WordContentsCorrespondingTo(WordPartOfSpeechVertex partOfSpeech) => WordContents
            .Where(vertex => vertex.Model == partOfSpeech.Model.WordSource)
            .Single();

        internal WordContentVertex WordContentVertexFor(WordElementBuilder partOfSpeech) => WordContents
            .Where(vertex => vertex.Model == partOfSpeech.WordSource)
            .Single();

        internal WordPartOfSpeechVertex PartOfSpeechCorrespondingTo(WordContentVertex token) => PartsOfSpeech
            .Where(partOfSpeech => partOfSpeech.Model.Token == token.Model)
            .Single();

        internal IEnumerable<WordPartOfSpeechVertex> PartsOfSpeechSpannedBy(ParentElementBuilder parentElement)
        {
            IEnumerable<PartOfSpeechBuilder> partsOfSpeechInSubtree = parentElement.GetElementsOfTypeInSubtree<PartOfSpeechBuilder>();
            return PartsOfSpeech.Where(partOfSpeechVertex => partsOfSpeechInSubtree.Contains(partOfSpeechVertex.Model));
        }
    }    
}

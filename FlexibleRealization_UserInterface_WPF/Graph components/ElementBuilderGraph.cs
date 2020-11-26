using System.Collections.Generic;
using System.Linq;
using QuickGraph;
using FlexibleRealization.UserInterface.ViewModels;

namespace FlexibleRealization.UserInterface
{
    public class ElementBuilderGraph : BidirectionalGraph<ElementVertex, ElementEdge> 
    {
        internal IEnumerable<ElementBuilderVertex> ElementBuilders => Vertices.Where(vertex => vertex is ElementBuilderVertex).Cast<ElementBuilderVertex>();

        internal IEnumerable<ParentElementVertex> ParentElements => Vertices.Where(vertex => vertex is ParentElementVertex).Cast<ParentElementVertex>();

        internal IEnumerable<PartOfSpeechVertex> PartsOfSpeech => Vertices.Where(vertex => vertex is PartOfSpeechVertex).Cast<PartOfSpeechVertex>();

        internal IEnumerable<TokenVertex> Tokens => Vertices.Where(vertex => vertex is TokenVertex).Cast<TokenVertex>();

        internal ElementBuilderVertex Root => ElementBuilders.Where(vertex => vertex.Builder.IsRoot).Single();

        internal TokenVertex TokenCorrespondingTo(PartOfSpeechVertex partOfSpeech) => Tokens
            .Where(vertex => vertex.Model == partOfSpeech.Model.Token)
            .Single();

        internal TokenVertex TokenVertexFor(PartOfSpeechBuilder partOfSpeech) => Tokens
            .Where(vertex => vertex.Model == partOfSpeech.Token)
            .Single();

        internal PartOfSpeechVertex PartOfSpeechCorrespondingTo(TokenVertex token) => PartsOfSpeech
            .Where(partOfSpeech => partOfSpeech.Model.Token == token.Model)
            .Single();

        internal IEnumerable<PartOfSpeechVertex> PartsOfSpeechSpannedBy(ParentElementBuilder parentElement)
        {
            IEnumerable<PartOfSpeechBuilder> partsOfSpeechInSubtree = parentElement.PartsOfSpeechInSubtree;
            return PartsOfSpeech.Where(partOfSpeechVertex => partsOfSpeechInSubtree.Contains(partOfSpeechVertex.Model));
        }
    }    
}

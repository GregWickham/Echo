using System;
using System.Collections.Generic;
using System.Linq;
using FlexibleRealization.Dependencies;

namespace FlexibleRealization
{
    /// <summary>The base class of all PartOfSpeechBuilders</summary>
    public abstract class PartOfSpeechBuilder : ElementBuilder
    {
        public PartOfSpeechBuilder(ParseToken token) => Token = token;

        public ParseToken Token { get; private set; }

        public readonly List<SyntacticRelation> OutgoingSyntacticRelations = new List<SyntacticRelation>();

        public readonly List<SyntacticRelation> IncomingSyntacticRelations = new List<SyntacticRelation>();

        internal void AddOutgoingRelation(SyntacticRelation relation) => OutgoingSyntacticRelations.Add(relation);
        internal void AddIncomingRelation(SyntacticRelation relation) => IncomingSyntacticRelations.Add(relation);

        /// <summary>Return the token index "distance" between this and <paramref name="anotherPartOfSpeech"/></summary>
        /// <returns>The <see cref="int"/> absolute value of the difference in token indices</returns>
        internal int DistanceFrom(PartOfSpeechBuilder anotherPartOfSpeech) => Math.Abs(Token.Index - anotherPartOfSpeech.Token.Index);

        /// <summary>Because a PartOfSpeechBuilder is not a ParentElementBuilder, it has no children -- therefore return an empty List.</summary>
        public override IEnumerable<IElementTreeNode> Children => new List<ElementBuilder>();

        /// <summary>Because a PartOfSpeechBuilder is not a ParentElementBuilder, it has no descendants -- therefore return a List containing only this.</summary>
        public override IEnumerable<IElementTreeNode> WithAllDescendentBuilders => new List<IElementTreeNode> { this };

        /// <summary>Apply the <paramref name="relations"/> for which this is the governor</summary>
        internal void ApplyRelations(IEnumerable<SyntacticRelation> relations) 
        {
            foreach (SyntacticRelation eachRelation in relations)
            {
                Console.WriteLine($"Applying D: {eachRelation.Dependent.Token.Word} -> {eachRelation.Relation} -> G: {eachRelation.Governor.Token.Word}");
                eachRelation.Apply();
            }
        }

        //IEnumerable<SyntacticRelation> SelectClosestRelationsFrom(IEnumerable<SyntacticRelation> relations) => relations
        //    .GroupBy(relation => relation.Relation)
        //    .Select(grouping => grouping
        //    .OrderBy(relation => relation.Dependent.DistanceFrom(relation.Governor))
        //    .First());
    }
}

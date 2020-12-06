using System;
using System.Collections.Generic;
using FlexibleRealization.Dependencies;

namespace FlexibleRealization
{
    /// <summary>The base class of all PartOfSpeechBuilders</summary>
    public abstract class PartOfSpeechBuilder : ElementBuilder
    {
        public PartOfSpeechBuilder(ParseToken token) => Token = token;

        public ParseToken Token { get; private set; }

        /// <summary>The PartOfSpeech string stored in the parse token</summary>
        /// <remarks>This property can be edited by the UI</remarks>
        public string PartOfSpeech
        {
            get => Token.PartOfSpeech;
            set
            {
                if (!value.Equals(Token.PartOfSpeech))
                {
                    Token.PartOfSpeech = value;
                    PartOfSpeechBuilder replacement = PartOfSpeechBuilder.FromToken(Token);
                    Become(replacement);
                    replacement.OnTreeStructureChanged();
                }
            }
        }

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

        /// <summary>Since a PartOfSpeechBuilder doesn't have IElementTreeNode children, propagating <paramref name="operateOn"/> through it simply means invoking <paramref name="operateOn"/> on this</summary>
        public override void Propagate(ElementTreeNodeOperation operateOn) => operateOn(this);

        /// <summary>Return a new PartOfSpeechBuilder constructed from <paramref name="token"/></summary>
        public static PartOfSpeechBuilder FromToken(ParseToken token)
        {
            return token.PartOfSpeech switch
            {
                "CC" => Conjunction(),                                      // Coordinating conjunction
                //"CD" =>                                                   // Cardinal number
                "DT" => Determiner(),                                       // Determiner 
                //"EX" =>                                                   // Existential there 
                //"FW" =>                                                   // Foreign word 
                "IN" => Preposition(),                                      // Preposition 
                "JJ" => Adjective(),                                        // Adjective 
                "JJR" => Adjective(),                                       // Adjective, comparative
                "JJS" => Adjective(),                                       // Adjective, superlative 
                //"LS" =>                                                   // List item marker
                "MD" => Modal(),                                            // Modal verb
                "NN" => Noun(),                                             // Noun, singular or mass 
                "NNS" => Noun(),                                            // Noun, plural 
                "NNP" => Noun(),                                            // Proper noun, singular
                "NNPS" => Noun(),                                           // Proper noun, plural
                "PDT" => Determiner(),                                      // Predeterminer
                "POS" => PossessiveEnding(),                                // Possessive ending 
                "PRP" => Pronoun(),                                         // Personal pronoun
                "PRP$" => Pronoun(),                                        // Possessive pronoun
                "RB" => Adverb(),                                           // Adverb
                "RBR" => Adverb(),                                          // Adverb, comparative 
                "RBS" => Adverb(),                                          // Adverb, superlative 
                "RP" => Particle(),                                         // Particle 
                //"SYM" =>                                                  // Symbol 
                //"TO" =>                                                   // Infinitival to
                //"UH" =>                                                   // Interjection
                "VB" => Verb(),                                             // Verb, base form
                "VBD" => Verb(),                                            // Verb, past tense
                "VBG" => Verb(),                                            // Verb, gerund / present participle
                "VBN" => Verb(),                                            // Verb, past participle
                "VBP" => Verb(),                                            // Verb, non-3rd person singular present
                "VBZ" => Verb(),                                            // Verb, 3rd person singular present
                "WDT" => WhDeterminer(),                                    // Wh- determiner
                "WP" => WhPronoun(),                                        // Wh- pronoun
                "WP$" => Pronoun(),                                         // Possessive Wh- pronoun
                "WRB" => WhAdverb(),                                        // Wh- adverb

                //Punctuation
                "#" => Punctuation(),                                       // Pound sign
                "$" => Punctuation(),                                       // Dollar sign
                "." => Punctuation(),                                       // Sentence-final punctuation
                "," => Punctuation(),                                       // Comma
                ":" => Punctuation(),                                       // Colon, semicolon
                "(" => Punctuation(),                                       // Left bracket character
                ")" => Punctuation(),                                       // Right bracket character
                "\"" => Punctuation(),                                      // Straight double quote
                "‘" => Punctuation(),                                       // Left open single quote
                "“" => Punctuation(),                                       // Left open double quote
                "’" => Punctuation(),                                       // Right close single quote
                "”" => Punctuation(),                                       // Right close double quote

                _ => throw new NotImplementedException()
            };

            WordElementBuilder Noun() => new NounBuilder(token);

            WordElementBuilder Pronoun() => new PronounBuilder(token);

            WordElementBuilder WhPronoun() => new WhPronounBuilder(token);

            WordElementBuilder Determiner() => new DeterminerBuilder(token);

            WordElementBuilder WhDeterminer() => new WhDeterminerBuilder(token);

            WordElementBuilder Verb() => new VerbBuilder(token);

            WordElementBuilder Adjective() => new AdjectiveBuilder(token);

            WordElementBuilder Adverb() => new AdverbBuilder(token);

            WordElementBuilder WhAdverb() => new WhAdverbBuilder(token);

            WordElementBuilder Preposition() => new PrepositionBuilder(token);

            WordElementBuilder Conjunction() => new ConjunctionBuilder(token);

            WordElementBuilder Modal() => new ModalBuilder(token);

            WordElementBuilder Particle() => new ParticleBuilder(token);

            PunctuationBuilder Punctuation() => new PunctuationBuilder(token);

            PossessiveEnding PossessiveEnding() => new PossessiveEnding(token);
        }
    }
}

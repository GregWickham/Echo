﻿using System.Collections.Generic;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.ie.util;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.semgraph;
using edu.stanford.nlp.util;
using edu.stanford.nlp.trees;
using Stanford.CoreNLP;
using SimpleNLG;
using System;

namespace FlexibleRealization
{
    public static class FlexibleRealizerFactory
    {
        private static readonly java.lang.Class IndexAnnotationClass = new CoreAnnotations.IndexAnnotation().getClass();

        public static NLGSpec SpecFrom(string text) => RealizableSpecFrom(ElementBuilderTreeFrom(text));

        /// <summary>Send <paramref name="text"/> to the CoreNLP server, and transform the resulting annotations into a directed graph (tree) form that can:
        /// <list type="bullet">
        /// <item>Build a data structure that can be serialized as XML and realized by SimpleNLG</item>
        /// <item>Be displayed and edited in a GUI</item></list></summary>
        /// <remarks>
        /// <para>First, a tree of IElementBuilders is constructed that parallels the constituency parse generated by the CoreNLP annotation pipeline.</para>
        /// <para>Once the tree has been assembled, syntactic relations from the enhanced++ dependency parse are attached to the appropriate PartOfSpeechBuilders in the tree.</para>
        /// <para>Then, a series of transformations are applied to the tree in order to transform it into a form that will build a correct realization spec.
        /// We refer to this process generally as "Configuration."</para>
        /// <para>The Propagate method causes an operation to propagate through a tree depth-first, being applied to all nodes in that tree bottom-to-top.</para>
        /// <list type="bullet">
        /// <item>ApplyDependencies causes the syntactic relations from the CoreNLP enhanced++ dependency parse to be applied.  This process may
        /// cause elements in the tree to change form and/or move around within the tree.</item>
        /// <item>Consolidate causes ElementBuilders to "clean themselves up" following a potentially destructive operation -- e.g. ApplyDependencies.</item>
        /// <item>Configure causes ElementBuilders to configure themselves using context information from the entire tree that was not available during initial construction.</item>
        /// <item>Coordinate causes CoordinablePhraseBuilders to coordinate themselves.  This process may cause those phrase builders to change form.</item></list></remarks>
        /// <returns>The IElementBuilder at the root of the resulting tree</returns>
        public static IElementBuilder ElementBuilderTreeFrom(string text)
        {
            ParseResult parse = Stanford.CoreNLP.Client.Parse(text);
            return ElementBuilderTreeFrom(parse, parse.Constituents)
                .AttachDependencies(parse.Dependencies)
                .ApplyDependencies()
                .Propagate(ElementBuilder.Consolidate)
                .Propagate(ElementBuilder.Configure)
                .Propagate(ElementBuilder.Coordinate);
        }

        /// <summary>Wrap <paramref name="elementBuilderTree"/> in the necessary objects to prepare it for realization.</summary>
        /// <returns>An <see cref="NLGSpec"/> that's ready to be serialized as XML and sent to a SimpleNLG server</returns>
        public static NLGSpec RealizableSpecFrom(IElementBuilder elementBuilderTree)
        {
            return new NLGSpec
            {
                Item = new RequestType
                {
                    Document = new DocumentElement
                    {
                        cat = documentCategory.DOCUMENT,
                        catSpecified = true,
                        child = new NLGElement[]
                        {
                            elementBuilderTree.BuildElement()
                        }
                    }
                }
            };
        }

        /// <summary>Recursively traverse <paramref name="constituent"/> depth-first, creating a parallel tree of ElementBuilders</summary>
        /// <returns>The IElementTreeNode at the root of the created tree</returns>
        private static IElementTreeNode ElementBuilderTreeFrom(ParseResult parse, Tree constituent)
        {
            IElementBuilder theBuilder = ElementBuilderParsedFrom(constituent, parse);
            switch (theBuilder)
            {
                case PartOfSpeechBuilder posBuilder:
                    return posBuilder;
                case ParentElementBuilder parentElementBuilder:
                    Tree[] childConstituents = constituent.children();
                    for (int childConstituentIndex = 0; childConstituentIndex < childConstituents.Length; childConstituentIndex++)
                    {
                        parentElementBuilder.AddChild(ElementBuilderTreeFrom(parse, constituent.getChild(childConstituentIndex)));
                    }
                    return parentElementBuilder;
                default: throw new ArgumentException();
            }
        }

        /// <summary>Use the information in <paramref name="tokenConstituent"/> and <paramref name="parse"/> to generate an ElementBuilder
        /// corresponding to a single node in the CoreNLP constituency parse tree.</summary>
        /// <param name="tokenConstituent">The java object corresponding to a node in the CoreNLP constituency parse</param>
        /// <param name="parse">Information about the constituent from other CoreNLP annotations</param>
        /// <returns>An IElementBuilder that will represent the constituent node in the initial form of the ElementBuilder tree, 
        /// prior to its configuration / transformation.</returns>
        private static IElementBuilder ElementBuilderParsedFrom(Tree tokenConstituent, ParseResult parse)
        {
            string posTag = tokenConstituent.label().toString();
            return posTag switch
            {
                #region Switch on constituent tag

                // http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.9.8216&rep=rep1&type=pdf
                // http://www.surdeanu.info/mihai/teaching/ista555-fall13/readings/PennTreebankConstituents.html

                // Parent types
                "FRAG" => new FragmentBuilder(),                            // Non-syntactic fragment
                "ADJP" => new AdjectivePhraseBuilder(),                     // Adjective Phrase
                "ADVP" => new AdverbPhraseBuilder(),                        // Adverb Phrase
                "NP" => new NounPhraseBuilder(),                            // Noun phrase
                "NML" => new NominalModifierBuilder(),
                "PP" => new PrepositionalPhraseBuilder(),                   // Prepositional phrase
                "PRT" => new ParticleParent(),                              // Particle
                "S" => new IndependentClauseBuilder(),                      // Simple declarative clause
                "SBAR" => new SubordinateClauseBuilder(),                   // Subordinate clause
                //"SBARQ" =>                                                // Direct question introduced by Wh- element
                //"SINV" =>                                                 // Declarative sentence with subject - aux inversion
                //"SQ" =>                                                   // Yes/ no questions and subconstituent of SBARQ excluding Wh- element
                "VP" => new VerbPhraseBuilder(),                            // Verb phrase
                //"WHADJP" => new WhAdjectivePhraseBuilder(),                 // Wh- adjective phrase
                "WHADVP" => new WhAdverbPhraseBuilder(),                    // Wh- adverb phrase
                "WHNP" => new WhNounPhraseBuilder(),                        // Wh- noun phrase
                "WHPP" => new PrepositionalPhraseBuilder(),                 // Wh- prepositional phrase
                //"X" =>                                                    // Constituent of unknown or uncertain category
                //"*" =>                                                    // “Understood” subject of infinitive or imperative
                //"0" =>                                                    // Zero variant of that in subordinate clauses
                //"T" =>                                                    // Trace of Wh-Constituent

                // Parts of Speech
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

                #endregion Switch on constituent tag
            };

            WordElementBuilder Noun() => new NounBuilder(GetLeafToken());
            
            WordElementBuilder Pronoun() => new PronounBuilder(GetLeafToken());

            WordElementBuilder WhPronoun() => new WhPronounBuilder(GetLeafToken());

            WordElementBuilder Determiner() => new DeterminerBuilder(GetLeafToken());

            WordElementBuilder WhDeterminer() => new WhDeterminerBuilder(GetLeafToken());

            WordElementBuilder Verb() => new VerbBuilder(GetLeafToken());

            WordElementBuilder Adjective() => new AdjectiveBuilder(GetLeafToken());

            WordElementBuilder Adverb() => new AdverbBuilder(GetLeafToken());

            WordElementBuilder WhAdverb() => new WhAdverbBuilder(GetLeafToken());

            WordElementBuilder Preposition() => new PrepositionBuilder(GetLeafToken());

            WordElementBuilder Conjunction() => new ConjunctionBuilder(GetLeafToken());

            WordElementBuilder Modal() => new ModalBuilder(GetLeafToken());

            WordElementBuilder Particle() => new ParticleBuilder(GetLeafToken());

            PunctuationBuilder Punctuation() => new PunctuationBuilder(GetLeafToken());

            PossessiveEnding PossessiveEnding() => new PossessiveEnding(GetLeafToken());

            ParseToken GetLeafToken()
            {
                Tree leaf = tokenConstituent.firstChild();
                CoreLabel leafLabel = leaf.label() as CoreLabel;
                int leafIndex = (leafLabel.get(IndexAnnotationClass) as java.lang.Integer).intValue();
                return parse.TokenWithIndex(leafIndex);
            }
        }
    }
}

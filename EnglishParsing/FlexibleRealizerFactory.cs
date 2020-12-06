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
    /// <summary>Thrown we we can't transform a tree from editable form to realizable form</summary>
    /// <remarks>Most often results from a problem with phrase coordination</remarks>
    public class TreeCannotBeTransformedToRealizableFormException : Exception
    {
        public TreeCannotBeTransformedToRealizableFormException(Exception inner) : base("Element Tree could not be transformed to realizable form", inner) { }
    }

    /// <summary>Thrown when a tree ostensibly in realizable form fails to build its NLGElement</summary>
    public class SpecCannotBeBuiltException : Exception
    {
        public SpecCannotBeBuiltException(Exception inner) : base("SimpleNLG specification cannot be built from tree", inner) { }
    }

    /// <summary>Provides static methods for creating and transforming element builder trees</summary>
    public static class FlexibleRealizerFactory
    {
        private static readonly java.lang.Class IndexAnnotationClass = new CoreAnnotations.IndexAnnotation().getClass();

        public static NLGSpec SpecFrom(string text) => RealizableSpecFrom(
            RealizableTreeFrom(
                EditableTreeFrom(text)));

        public static NLGSpec SpecFrom(IElementTreeNode editableTree) => RealizableSpecFrom(
            RealizableTreeFrom(editableTree));

        public static NLGSpec SpecFrom(IElementBuilder realizableTree) => RealizableSpecFrom(realizableTree);

        /// <summary>Send <paramref name="text"/> to the CoreNLP server, and transform the resulting annotations into a directed graph (tree) form that can be displayed and edited in a GUI.</summary>
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
        /// <item>Configure causes ElementBuilders to configure themselves using context information from the entire tree that was not available during initial construction.</item></list>
        /// </remarks>
        /// <returns>The IElementTreeNode at the root of the resulting tree</returns>
        public static IElementTreeNode EditableTreeFrom(string text)
        {
            ParseResult parse = Stanford.CoreNLP.Client.Parse(text);
            return RootOfElementBuilderTreeFrom(parse, parse.Constituents)
                .AttachDependencies(parse.Dependencies)
                .ApplyDependencies()
                .Propagate(ElementBuilder.Consolidate)
                .Propagate(ElementBuilder.Configure)
                .Tree;
        }

        /// <summary>Attempt to transform <paramref name="editableTree"/> into a structure that can be serialized as XML and realized by SimpleNLG.</summary>
        /// <remarks>The propagated "Coordinate" operation causes CoordinablePhraseBuilders to coordinate themselves.  This process may cause those phrase builders to change form.
        /// <paramref name="editableTree"/> does NOT need to be the root of the tree in which it resides.  This allows the UI to selectively realize portions of a tree.</remarks>
        /// <returns>An IElementBuilder representing the transformed tree, if the transformation succeeds</returns>
        /// <exception cref="TreeCannotBeTransformedToRealizableFormException">If the transformation fails</exception>
        public static IElementBuilder RealizableTreeFrom(IElementTreeNode editableTree)
        {
            try
            {
                return new RootNode(editableTree.CopyLightweight())
                    .Propagate(ElementBuilder.Coordinate)
                    .Tree;
            }
            catch (Exception transformationException)
            {
                throw new TreeCannotBeTransformedToRealizableFormException(transformationException);
            }
        }

        /// <summary>Wrap <paramref name="elementBuilderTree"/> in the necessary objects to prepare it for realization.</summary>
        /// <returns>An <see cref="NLGSpec"/> that's ready to be serialized as XML and sent to a SimpleNLG server</returns>
        /// <exception cref="SpecCannotBeBuiltException"></exception>
        public static NLGSpec RealizableSpecFrom(IElementBuilder realizableTree)
        {
            try
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
                                realizableTree.BuildElement()
                            }
                        }
                    }
                };
            }
            catch (Exception buildException)
            {
                throw new SpecCannotBeBuiltException(buildException);
            }
        }

        /// <summary>Return the RootNode of an element tree constructed from <paramref name="parse"/> and <paramref name="constituent"/></summary>
        private static RootNode RootOfElementBuilderTreeFrom(ParseResult parse, Tree constituent) => new RootNode(ElementBuilderTreeFrom(parse, constituent));

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
                //"WHADJP" => new WhAdjectivePhraseBuilder(),               // Wh- adjective phrase
                "WHADVP" => new WhAdverbPhraseBuilder(),                    // Wh- adverb phrase
                "WHNP" => new WhNounPhraseBuilder(),                        // Wh- noun phrase
                "WHPP" => new PrepositionalPhraseBuilder(),                 // Wh- prepositional phrase
                //"X" =>                                                    // Constituent of unknown or uncertain category
                //"*" =>                                                    // “Understood” subject of infinitive or imperative
                //"0" =>                                                    // Zero variant of that in subordinate clauses
                //"T" =>                                                    // Trace of Wh-Constituent

                // Parts of Speech
                "CC" => PartOfSpeech(),                                     // Coordinating conjunction
                //"CD" =>                                                   // Cardinal number
                "DT" => PartOfSpeech(),                                     // Determiner 
                //"EX" =>                                                   // Existential there 
                //"FW" =>                                                   // Foreign word 
                "IN" => PartOfSpeech(),                                     // Preposition 
                "JJ" => PartOfSpeech(),                                     // Adjective 
                "JJR" => PartOfSpeech(),                                    // Adjective, comparative
                "JJS" => PartOfSpeech(),                                    // Adjective, superlative 
                //"LS" =>                                                   // List item marker
                "MD" => PartOfSpeech(),                                     // Modal verb
                "NN" => PartOfSpeech(),                                     // Noun, singular or mass 
                "NNS" => PartOfSpeech(),                                    // Noun, plural 
                "NNP" => PartOfSpeech(),                                    // Proper noun, singular
                "NNPS" => PartOfSpeech(),                                   // Proper noun, plural
                "PDT" => PartOfSpeech(),                                    // Predeterminer
                "POS" => PartOfSpeech(),                                    // Possessive ending 
                "PRP" => PartOfSpeech(),                                    // Personal pronoun
                "PRP$" => PartOfSpeech(),                                   // Possessive pronoun
                "RB" => PartOfSpeech(),                                     // Adverb
                "RBR" => PartOfSpeech(),                                    // Adverb, comparative 
                "RBS" => PartOfSpeech(),                                    // Adverb, superlative 
                "RP" => PartOfSpeech(),                                     // Particle 
                //"SYM" =>                                                  // Symbol 
                //"TO" =>                                                   // Infinitival to
                //"UH" =>                                                   // Interjection
                "VB" => PartOfSpeech(),                                     // Verb, base form
                "VBD" => PartOfSpeech(),                                    // Verb, past tense
                "VBG" => PartOfSpeech(),                                    // Verb, gerund / present participle
                "VBN" => PartOfSpeech(),                                    // Verb, past participle
                "VBP" => PartOfSpeech(),                                    // Verb, non-3rd person singular present
                "VBZ" => PartOfSpeech(),                                    // Verb, 3rd person singular present
                "WDT" => PartOfSpeech(),                                    // Wh- determiner
                "WP" => PartOfSpeech(),                                     // Wh- pronoun
                "WP$" => PartOfSpeech(),                                    // Possessive Wh- pronoun
                "WRB" => PartOfSpeech(),                                    // Wh- adverb

                //Punctuation
                "#" => PartOfSpeech(),                                      // Pound sign
                "$" => PartOfSpeech(),                                      // Dollar sign
                "." => PartOfSpeech(),                                      // Sentence-final punctuation
                "," => PartOfSpeech(),                                      // Comma
                ":" => PartOfSpeech(),                                      // Colon, semicolon
                "(" => PartOfSpeech(),                                      // Left bracket character
                ")" => PartOfSpeech(),                                      // Right bracket character
                "\"" => PartOfSpeech(),                                     // Straight double quote
                "‘" => PartOfSpeech(),                                      // Left open single quote
                "“" => PartOfSpeech(),                                      // Left open double quote
                "’" => PartOfSpeech(),                                      // Right close single quote
                "”" => PartOfSpeech(),                                      // Right close double quote

                _ => throw new NotImplementedException()

                #endregion Switch on constituent tag
            };

            PartOfSpeechBuilder PartOfSpeech() => PartOfSpeechBuilder.FromToken(GetLeafToken());

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

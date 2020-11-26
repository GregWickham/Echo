using System.Collections.Generic;
using System.Linq;
using edu.stanford.nlp.coref.data;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.ie.machinereading.structure;
using edu.stanford.nlp.ie.util;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.semgraph;
using edu.stanford.nlp.util;
using edu.stanford.nlp.trees;
using FlexibleRealization;

namespace Stanford.CoreNLP
{
    /// <summary>Provides an interface to a server running the Stanford CoreNLP annotation pipeline.</summary>
    /// <remarks>Using CoreNLP in its server form allows new versions of CoreNLP to be incorporated without waiting for the new version to ber ported to OpenNLP.
    /// At the same time, we need to use some java types defined in OpenNLP for interpreting the results returnewd from the pipeline.</remarks>
    public static class Client
    {
        private static readonly int ClientThreads = 2;
        private static readonly StanfordCoreNLPClient ServerPipeline;

        static Client()
        {
            // Create a StanfordCoreNLPClient object with POS tagging, lemmatization, NER, parsing, and coreference resolution
            java.util.Properties props = new java.util.Properties();
            props.setProperty("annotators", "tokenize, ssplit, pos, lemma, ner, parse, dcoref");
            props.setProperty("coref.algorithm", "neural");
            ServerPipeline = new StanfordCoreNLPClient(props, Properties.Settings.Default.CoreNLP_ServerHost, Properties.Settings.Default.CoreNLP_ServerPort, ClientThreads);
        }

        internal static ParseResult Parse(string text) => new ParseResult(Annotate(text));

        private static Annotation Annotate(string text)
        {
            // create an empty Annotation just with the given text
            Annotation document = new Annotation(text);
            // run all Annotators on this text
            ServerPipeline.annotate(document);
            return document;
        }
    }

    /// <summary>A <see cref="ParseResult"/> is an intermediate form containing the information produced when the CoreNLP pipeline annotates some text.</summary>
    internal class ParseResult
    {
        private static readonly java.lang.Class SentencesAnnotationClass = new CoreAnnotations.SentencesAnnotation().getClass();
        private static readonly java.lang.Class TreeAnnotationClass = new TreeCoreAnnotations.TreeAnnotation().getClass();
        private static readonly java.lang.Class TokensAnnotationClass = new CoreAnnotations.TokensAnnotation().getClass();
        private static readonly java.lang.Class DependencyAnnotationClass = new SemanticGraphCoreAnnotations.EnhancedPlusPlusDependenciesAnnotation().getClass();
        private static readonly java.lang.Class PartOfSpeechAnnotationClass = new CoreAnnotations.PartOfSpeechAnnotation().getClass();
        private static readonly java.lang.Class NamedEntityTagAnnotationClass = new CoreAnnotations.NamedEntityTagAnnotation().getClass();
        private static readonly java.lang.Class MentionsAnnotationClass = new CoreAnnotations.MentionsAnnotation().getClass();
        private static readonly java.lang.Class MentionTokenAnnotationClass = new CoreAnnotations.MentionTokenAnnotation().getClass();
        private static readonly java.lang.Class EntityMentionIndexAnnotationClass = new CoreAnnotations.EntityMentionIndexAnnotation().getClass();
        private static readonly java.lang.Class GenderAnnotationClass = new MachineReadingAnnotations.GenderAnnotation().getClass();

        /// <summary>Return a new <see cref="ParseResult"/> constructed from <paramref name="annotation"/></summary>
        internal ParseResult(Annotation annotation)
        {
            java.util.AbstractList sentences = annotation.get(SentencesAnnotationClass) as java.util.AbstractList;
            CoreMap sentence = sentences.get(0) as CoreMap;
            LabeledScoredTreeNode constituencyParse = sentence.get(TreeAnnotationClass) as LabeledScoredTreeNode;
            // Skip the ROOT
            Tree childOfRoot = constituencyParse.firstChild();
            Constituents = childOfRoot;
            Constituents.indexLeaves();

            // Build the collection of tokens
            var parsedTokens = sentence.get(TokensAnnotationClass) as java.util.AbstractList; 
            var mentions = sentence.get(MentionsAnnotationClass);
            for (int tokenIndex = 0; tokenIndex < parsedTokens.size(); tokenIndex++)
            {
                CoreLabel source = parsedTokens.get(tokenIndex) as CoreLabel;
                var tokenMentions = source.get(MentionTokenAnnotationClass);
                var tokenGender = source.get(GenderAnnotationClass);
                Tokens.Add(new ParseToken
                {
                    Index = source.index(),
                    Word = source.word(),
                    Lemma = source.lemma(),
                    PartOfSpeech = source.get(PartOfSpeechAnnotationClass) as string,
                    NamedEntityClass = source.get(NamedEntityTagAnnotationClass) as string,
                });
            }

            // Create the list of dependencies between tokens
            SemanticGraph dependencyGraph = sentence.get(DependencyAnnotationClass) as SemanticGraph;
            //java.util.List dependencies = dependencyGraph.edgeListSorted();
            java.util.Iterator dependencyGraphEdges = dependencyGraph.edgeIterable().iterator();
            while (dependencyGraphEdges.hasNext())
            {
                SemanticGraphEdge edge = dependencyGraphEdges.next() as SemanticGraphEdge;

                string relationName = edge.getRelation().getShortName();
                string relationSpecifier = edge.getRelation().getSpecific();
                IndexedWord governor = edge.getGovernor();
                IndexedWord dependent = edge.getDependent();

                Dependencies.Add((relationName, relationSpecifier, governor.index(), dependent.index()));
            }
        }

        internal Tree Constituents { get; private set; }

        internal List<(string Relation, string Specifier, int GovernorIndex, int DependentIndex)> Dependencies = new List<(string Relation, string Specifier, int GovernorIndex, int DependentIndex)>();

        private List<ParseToken> Tokens = new List<ParseToken>();

        internal ParseToken TokenWithIndex(int index) => Tokens.Single(t => t.Index == index);
    }
}



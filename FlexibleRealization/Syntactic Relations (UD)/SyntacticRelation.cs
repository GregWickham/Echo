using System;
using System.Diagnostics;

namespace FlexibleRealization.Dependencies
{
    [DebuggerDisplay("D: {Dependent.Token.Word} <- {Relation}{Specifier?.ToString()} <- G: {Governor.Token.Word}")]
    public class SyntacticRelation
    {
        /// <summary>Return a new <see cref="SyntacticRelation"/> of the type described by <paramref name="relation"/> and <paramref name="specifier"/></summary>
        /// <remarks>In come cases, relation subtype descriptors are passed in <paramref name="relation"/> and delimited by a colon.  In other cases,
        /// the subtype descriptor is found in <paramref name="specifier"/>.  In the interest of backward compatibility, we try to handle both cases.</remarks>
        public static SyntacticRelation OfType(string relation, string specifier)
        {
            SyntacticRelation newDependency = relation switch
            {
                // Core arguments
                "nsubj" => new NominalSubject(),
                "csubj" => new ClausalSubject(),
                "obj" => new Object(),
                "ccomp" => new ClausalComplement(),
                "iobj" => new IndirectObject(),
                "xcomp" => new OpenClausalComplement(),

                // Non-core dependencies
                "obl" => new ObliqueNominal(),
                "advcl" => new AdverbialClauseModifier(),
                "advmod" => new AdverbialModifier(),
                "aux" => specifier switch
                {
                    "pass" => new AuxiliaryPassive(),
                    _ => new Auxiliary()
                },
                "vocative" => new Vocative(),
                "discourse" => new DiscourseElement(),
                "cop" => new Copula(),
                "expletive" => new Expletive(),
                "mark" => new Marker(),
                "dislocated" => new Dislocated(),

                // Nominal dependents
                "nmod" => specifier switch
                {
                    "poss" => new NominalModifierPossessive(),
                    _ => new NominalModifier()
                },
                "nmod:poss" => new NominalModifierPossessive(),
                "acl" => specifier switch
                {
                    "relcl" => new RelativeClauseModifier(),
                    _ => new AdjectivalClause()
                },
                "acl:relcl" => new RelativeClauseModifier(),
                "amod" => new AdjectivalModifier(),
                "det" => new Determiner(),
                "appos" => new AppositionalModifier(),
                "clf" => new Classifier(),
                "nummod" => new NumericModifier(),
                "case" => new CaseMarking(),
                "ref" => new Referent(),

                // Non dependency relations
                "conj" => new Conjuct(),
                "fixed" => new FixedMultiWordExpression(),
                "list" => new List(),
                "orphan" => new Orphan(),
                "punct" => new Punctuation(),
                "cc" => new CoordinatingConjunction(),
                "flat" => new FlatMultiWordExpression(),
                "parataxis" => new Parataxis(),
                "goeswith" => new GoesWith(),
                "root" => new Root(),
                "compound" => specifier switch
                {
                    "prt" => new CompoundParticleVerb(),
                    _ => new Compound(),
                },
                "compound:prt" => new CompoundParticleVerb(),
                "reparandum" => new OverriddenDisfluency(),
                "dep" => new UnspecifiedDependency(),

                _ => throw new ArgumentException("Unknown syntactic dependency type")
            };
            newDependency.Relation = relation;
            newDependency.Specifier = specifier;
            return newDependency;
        }

        /// <summary>Configure the <paramref name="governor"/> and <paramref name="dependent"/> of this.</summary>
        /// <returns>this, so this method can participate in quasi-infix syntax for initial creation and setup</returns>
        public SyntacticRelation Between(PartOfSpeechBuilder governor, PartOfSpeechBuilder dependent)
        {
            Governor = governor;
            Dependent = dependent;
            return this;
        }

        public void Install()
        {
            Console.WriteLine($"Installing {Governor.Token.Word} <- ({Relation}) <- {Dependent.Token.Word}");
            Governor.AddIncomingRelation(this);
            Dependent.AddOutgoingRelation(this);
        }

        public string Relation { get; private set; }
        public string Specifier { get; private set; }
        public PartOfSpeechBuilder Governor { get; private set; }
        public PartOfSpeechBuilder Dependent { get; private set; }

        public virtual void Apply() { }
    }
}

using System;

namespace FlexibleRealization
{
    /// <summary>Thrown when a tree ostensibly in realizable form fails to build its NLGElement</summary>
    public class SpecCannotBeBuiltException : Exception
    {
        public SpecCannotBeBuiltException(Exception inner) : base("SimpleNLG specification cannot be built from tree", inner) { }
    }

}

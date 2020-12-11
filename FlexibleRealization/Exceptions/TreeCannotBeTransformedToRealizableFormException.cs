using System;

namespace FlexibleRealization
{
    /// <summary>Thrown when we can't transform a tree from editable form to realizable form</summary>
    /// <remarks>Most often results from a problem with phrase coordination</remarks>
    public class TreeCannotBeTransformedToRealizableFormException : Exception
    {
        public TreeCannotBeTransformedToRealizableFormException(Exception inner) : base("Element Tree could not be transformed to realizable form", inner) { }
    }
}

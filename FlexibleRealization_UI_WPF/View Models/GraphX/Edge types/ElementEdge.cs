using GraphX.Common.Models;
using GraphX.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    public abstract class ElementEdge : EdgeBase<ElementVertex>
    {
        /// <summary>Default parameterless constructor (for serialization compatibility)</summary>
        public ElementEdge() : base(null, null, 1) { }
        public ElementEdge(ElementVertex source, ElementVertex target, double weight = 1) : base(source, target, weight) { }

        public abstract string LabelText { get; }
        public abstract EdgeDashStyle ElementDashStyle { get; }

        public override string ToString() => LabelText;
    }
}

using GraphX.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    internal class PartOfSpeechToContentEdge : ElementEdge
    {
        internal PartOfSpeechToContentEdge(WordPartOfSpeechVertex wposv, WordContentVertex wcv) : base(wposv, wcv) { }

        public override string LabelText => "";
        public override EdgeDashStyle ElementDashStyle => EdgeDashStyle.Solid;
    }
}

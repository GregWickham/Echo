using GraphX.Controls;

namespace FlexibleRealization.UserInterface.ViewModels
{
    internal class PartOfSpeechToTokenEdge : ElementEdge
    {
        internal PartOfSpeechToTokenEdge(PartOfSpeechVertex posv, TokenVertex tv) : base(posv, tv) { }

        public override string LabelText => "";
        public override EdgeDashStyle ElementDashStyle => EdgeDashStyle.Solid;

    }
}

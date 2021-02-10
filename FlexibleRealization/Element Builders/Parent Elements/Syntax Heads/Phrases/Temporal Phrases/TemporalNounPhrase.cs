

namespace FlexibleRealization
{
    public class TemporalNounPhraseBuilder : NounPhraseBuilder
    {
        public TemporalNounPhraseBuilder() : base() { }

        public override IElementTreeNode CopyLightweight() => new TemporalNounPhraseBuilder()
            .LightweightCopyChildrenFrom(this);
    }
}

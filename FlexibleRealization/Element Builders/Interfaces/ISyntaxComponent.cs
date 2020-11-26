namespace FlexibleRealization
{
    /// <summary>An object that can participate in syntactic arrangements</summary>
    public interface ISyntaxComponent
    {
        bool IsPhraseHead { get; }

        bool ActsAsHeadOf(PhraseBuilder phrase);

        PhraseBuilder AsPhrase();

        void Specify(IElementTreeNode governor);

        void Modify(IElementTreeNode governor);

        void Complete(IElementTreeNode governor);
    }
}

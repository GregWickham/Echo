namespace FlexibleRealization
{
    /// <summary>An object that can participate in syntactic arrangements</summary>
    public interface ISyntaxComponent
    {
        bool IsPhraseHead { get; }

        bool ActsAsHeadOf(PhraseBuilder phrase);

        PhraseBuilder AsPhrase();

        IElementTreeNode AsPhraseIfConvertible();

        IParent Specify(IElementTreeNode governor);

        IParent Modify(IElementTreeNode governor);

        IParent Complete(IElementTreeNode governor);
    }
}

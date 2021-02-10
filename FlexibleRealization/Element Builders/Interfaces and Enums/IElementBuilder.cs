using SimpleNLG;

namespace FlexibleRealization
{
    /// <summary>An object that can build an NLGElement</summary>
    public interface IElementBuilder
    {
        NLGElement BuildElement();

        NLGSpec ToNLGSpec();
    }
}

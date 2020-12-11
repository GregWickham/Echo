using System.Collections;

namespace FlexibleRealization
{
    public interface IWordSource : IEnumerator
    {
        string GetWord();
    }
}

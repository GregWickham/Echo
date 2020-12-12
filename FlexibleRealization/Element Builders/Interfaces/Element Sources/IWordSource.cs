using System.Collections.Generic;

namespace FlexibleRealization
{
    public interface IWordSource
    {
        string GetWord();

        IEnumerator<string> EnumerateVariations();
    }
}

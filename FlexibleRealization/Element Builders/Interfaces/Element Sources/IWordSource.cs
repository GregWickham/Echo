using System.Collections.Generic;

namespace FlexibleRealization
{
    public interface IWordSource
    {
        string GetWord();

        string DefaultWord { get; }

        IEnumerator<string> GetStringVariationsEnumerator();
    }
}

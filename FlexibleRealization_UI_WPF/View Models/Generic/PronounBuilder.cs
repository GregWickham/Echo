using System.Collections.Generic;

namespace FlexibleRealization.UserInterface.ViewModels
{
    /// <summary>Provides utility methods for UI-friendly rendering of PronounBuilder features</summary>
    internal static class Pronoun
    {
        internal static Dictionary<PronounCase, string> CaseStrings { get; } = new Dictionary<PronounCase, string>
        {
            { PronounCase.Nominative, "Nominative" },
            { PronounCase.Objective, "Objective" },
            { PronounCase.Possessive, "Possessive" },
            { PronounCase.Reflexive, "Reflexive" }
        };
    }
}

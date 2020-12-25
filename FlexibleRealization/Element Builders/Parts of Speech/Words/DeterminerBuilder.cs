using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SimpleNLG;

namespace FlexibleRealization
{
    public class DeterminerBuilder : WordElementBuilder
    {
        /// <summary>This constructor is using during parsing</summary>
        public DeterminerBuilder(ParseToken token) : base(lexicalCategory.DETERMINER, token) { }

        /// <summary>This constructor is used during LightweightCopy().</summary>
        private protected DeterminerBuilder(int index, string word) : base(lexicalCategory.DETERMINER, index, word) { }

        /// <summary>This constructor is used by the UI for changing the part of speech of a word in the graph</summary>
        public DeterminerBuilder() : base(lexicalCategory.DETERMINER) { }

        public override IElementTreeNode CopyLightweight() => new DeterminerBuilder(Index, WordSource.GetWord());

        internal override string SelectWord()
        {
            string selectedWord = WordSource.GetWord();
            return IsIndefiniteArticle(selectedWord)
                ? AppropriateIndefiniteArticleToPrecede(WordFollowing(this))
                : selectedWord;
        }

        private static bool IsIndefiniteArticle(string word) => IndefiniteArticles.Contains(word);

        private static readonly IEnumerable<string> IndefiniteArticles = new List<string> { "a", "an" };

        /// <summary>Based on a bunch of weird irregular rules of mostly phonology, decide whether this indefinite article should be "a" or "an"</summary>
        /// <param name="followingWordBuilder">The word that comes after this article</param>
        private static string AppropriateIndefiniteArticleToPrecede(WordElementBuilder followingWordBuilder)
        {
            if (followingWordBuilder == null) return "a";
            else
            {

                string followingWord = followingWordBuilder.SelectWord();

                var lowercaseWord = followingWord.ToLower();
                foreach (string anword in new string[] { "euler", "heir", "honest", "hono" })
                    if (lowercaseWord.StartsWith(anword))
                        return "an";

                if (lowercaseWord.StartsWith("hour") && !lowercaseWord.StartsWith("houri"))
                    return "an";

                if (lowercaseWord.StartsWith("one ") || lowercaseWord.StartsWith("one-"))
                    return "a";

                var char_list = new char[] { 'a', 'e', 'd', 'h', 'i', 'l', 'm', 'n', 'o', 'r', 's', 'x' };
                if (lowercaseWord.Length == 1)
                {
                    if (lowercaseWord.IndexOfAny(char_list) == 0)
                        return "an";
                    else
                        return "a";
                }

                if (Regex.Match(followingWord, "(?!FJO|[HLMNS]Y.|RY[EO]|SQU|(F[LR]?|[HL]|MN?|N|RH?|S[CHKLMNPTVW]?|X(YL)?)[AEIOU])[FHLMNRSX][A-Z]").Success)
                    return "an";

                foreach (string regex in new string[] { "^e[uw]", "^onc?e\b", "^uni([^nmd]|mo)", "^u[bcfhjkqrst][aeiou]" })
                {
                    if (Regex.IsMatch(lowercaseWord, regex))
                        return "a";
                }

                if (Regex.IsMatch(followingWord, "^U[NK][AIEO]"))
                    return "a";
                else if (followingWord == followingWord.ToUpper())
                {
                    if (lowercaseWord.IndexOfAny(char_list) == 0)
                        return "an";
                    else
                        return "a";
                }

                if (lowercaseWord.IndexOfAny(new char[] { 'a', 'e', 'i', 'o', 'u' }) == 0)
                    return "an";

                if (Regex.IsMatch(lowercaseWord, "^y(b[lor]|cl[ea]|fere|gg|p[ios]|rou|tt)"))
                    return "an";

                return "a";
            }
        }
    }
}

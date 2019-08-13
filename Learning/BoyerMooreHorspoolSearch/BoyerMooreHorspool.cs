using System.Collections.Generic;

namespace Learning.BoyerMooreHorspoolSearch
{
    public class BoyerMooreHorspool
    {
        private IBadMatchTable _badMatchTable;

        public BoyerMooreHorspool(IBadMatchTable badMatchTable)
        {
            _badMatchTable = badMatchTable;
        }

        public IEnumerable<StringSearchMatch> Search(string text, string pattern)
        {
            int currentStartIndex = 0;
            while (currentStartIndex <= text.Length - pattern.Length)
            {
                int charactersLeftToMatch = pattern.Length - 1;
                while (charactersLeftToMatch >= 0 && string.Equals(pattern[charactersLeftToMatch], text[currentStartIndex + charactersLeftToMatch]))
                {
                    charactersLeftToMatch--;
                }
                if (charactersLeftToMatch < 0)
                {
                    yield return new StringSearchMatch { StartIndex = currentStartIndex, Length = pattern.Length };
                    currentStartIndex += pattern.Length;
                }
                else
                {
                        
                    currentStartIndex += _badMatchTable.NextJump(text[currentStartIndex + pattern.Length - 1]) ;
                }
            }
        }
    }
}

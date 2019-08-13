using System;
using System.Collections.Generic;

namespace Learning.BoyerMooreHorspoolSearch
{
    public interface IBadMatchTable
    {
        Dictionary<int, int> Table { get; }
        int NextJump(int character);
    }

    public class BadMatchTable : IBadMatchTable
    {
        private readonly Lazy<Dictionary<int, int>> _table;
        private readonly string _pattern;

        public BadMatchTable(string pattern)
        {
            _table = new Lazy<Dictionary<int, int>>(() => GenerateTable(pattern));
            _pattern = pattern;
        }

        public Dictionary<int, int> Table => _table.Value;

        public int NextJump(int character)
        {
            try
            {
                return _table.Value[character];
            }
            catch
            {
                // return default value when there is nothing in the bad match table.
                return _pattern.Length;
            }
        }

        private Dictionary<int, int> GenerateTable(string pattern)
        {
            var table = new Dictionary<int, int>(pattern.Length);

            // Last character distance value has to be equal to pattern length, so we will just ignore that for now.
            for (int idx = 0; idx < pattern.Length - 1; idx++)
            {
                table[pattern[idx]] = pattern.Length - idx - 1;
            }

            return table;
        }
    }
}

using System.Collections.Generic;

namespace Learning.Hash
{
    public class BasicHash
    {
        private readonly List<List<int>> _bucket;

        public BasicHash(int bucketLength)
        {
            _bucket = new List<List<int>>(bucketLength);
        }

        public void Add(int value)
        {
            var chain = _bucket[HashFunction(value)];

            // Don't want to add anything that is already added in the chain.
            if (!chain.Contains(value))
            {
                chain.Add(value);
            }
        }

        public void Remove(int value)
        {
            var chain = _bucket[HashFunction(value)];
            if (chain.Contains(value))
            {
                chain.Remove(value);
            }
        }

        private int HashFunction(int value)
        {
            return value % _bucket.Count;
        }
    }
}

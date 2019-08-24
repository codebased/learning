using System.Collections.Generic;

namespace Learning.Heap
{

    public class MinHeap
    {
        private List<int> _elements;

        public MinHeap()
        {
            _elements = new List<int>();
        }

        public void Add(int info)
        {
            _elements.Add(info);
            ApplyMinHeapRuleUp();
        }

        public int Remove()
        {
            var result = _elements[0];
            _elements[0] = _elements[_elements.Count - 1];
            _elements.Remove(_elements.Count - 1);

            ApplyMinHeapRuleDown();

            return result;
        }

        private void ApplyMinHeapRuleDown()
        {
            int index = 0;
            while (HasLeftChild(index))
            {
                var smallerIndex = LeftChildIndex(index);
                if (HasRightChild(index) && RightChild(index) < LeftChild(smallerIndex))
                {
                    smallerIndex = RightChildIndex(index);
                }

                if (_elements[smallerIndex] >= _elements[index])
                {
                    break;
                }

                Swap(smallerIndex, index);
                index = smallerIndex;
            }
        }

        private void ApplyMinHeapRuleUp()
        {
            var newElementIndex = _elements.Count - 1;

            while (newElementIndex != 0 && _elements[newElementIndex] < Parent(newElementIndex))
            {
                int parentIndex = ParentIndex(newElementIndex);
                Swap(newElementIndex, parentIndex);
                newElementIndex = parentIndex;
            }
        }

        private int LeftChildIndex(int elementIndex) => 2 * elementIndex + 1;

        private int RightChildIndex(int elementIndex) => LeftChildIndex(elementIndex) + 1;

        private int ParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private int LeftChild(int elementIndex) => _elements[LeftChildIndex(elementIndex)];

        private int RightChild(int elementIndex) => _elements[RightChildIndex(elementIndex)];

        private int Parent(int elementIndex) => _elements[ParentIndex(elementIndex)];

        private bool HasLeftChild(int elementIndex) => LeftChildIndex(elementIndex) < _elements.Count;

        private bool HasRightChild(int elementIndex) => RightChildIndex(elementIndex) < _elements.Count;

        private void Swap(int index, int index2)
        {
            _elements[index] += _elements[index2];
            _elements[index2] = _elements[index] - _elements[index2];
            _elements[index] = _elements[index] - _elements[index2];
        }
    }
}
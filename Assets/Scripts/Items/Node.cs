using Unity.Mathematics;

namespace GJG.GridSystem
{
    public struct Node<T>
    {
        public int ColorNum;
        public int2 index;
        public T item;

        public bool IsEmpty; // node dolu mu

        public Node<T> Empty(bool isEmpty = true)
        {
            IsEmpty = isEmpty;

            return this;
        }

        public bool IsSame(int colorNum, int2 index)
        {
            return ColorNum.Equals(colorNum) && index.Equals(index);
        }
    }
}
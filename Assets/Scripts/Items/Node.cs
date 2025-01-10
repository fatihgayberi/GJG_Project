using GJG.Items;

namespace GJG.GridSystem
{
    public struct Node<T>
    {
        public ItemColorType ColorType;
        public T item;

        public bool IsEmpty; // node dolu mu

        public Node<T> Empty(bool isEmpty = true)
        {
            IsEmpty = isEmpty;

            return this;
        }

        public bool IsSame(ItemColorType colorType)
        {
            return ColorType == colorType;
        }
    }
}
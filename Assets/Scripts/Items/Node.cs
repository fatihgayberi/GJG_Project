using GJG.Items;
using UnityEngine;

namespace GJG.GridSystem
{
    public struct Node
    {
        public ItemColorType ColorType;
        public ItemBase item;
        public Vector3 nodePos;

        public bool IsEmpty; // node dolu mu

        public Node Empty(bool isEmpty = true)
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
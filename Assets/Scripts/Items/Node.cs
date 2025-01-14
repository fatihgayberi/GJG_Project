using UnityEngine;
using GJG.Items;
using System;

namespace GJG.GridSystem
{
    [Serializable]
    public struct Node
    {
        private ItemBase _itemBase;
        public Vector3 nodePos;

        public bool IsEmpty => _itemBase == null; // node dolu mu
        public ItemBase ItemBase => _itemBase;

        public void AddItem(ItemBase itemBase)
        {
            _itemBase = itemBase;
        }

        public void RemoveItem()
        {
            _itemBase = null;
        }

        public readonly bool IsSame(ItemBase itemBase)
        {
            return _itemBase.IsSame(itemBase);
        }
    }
}
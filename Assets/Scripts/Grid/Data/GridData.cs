using System;
using GJG.Items;
using UnityEngine;
using Unity.Mathematics;
using GJG.Items.ItemColor;

namespace GJG.GridSystem
{
    [CreateAssetMenu(fileName = "GridData", menuName = "GridData", order = 0)]
    public class GridData : ScriptableObject
    {
        [SerializeField, Tooltip("M - N")] private int2 gridSize; // M N
        [SerializeField, Tooltip("K")] private ColorData[] colorData; // K
        [SerializeField, Tooltip("A - B - C ...")] private GroupData[] groupData; // A - B - C ...

        public int2 GridSize => gridSize;

        public ItemType GetItemType(int groupCount)
        {
            if (groupCount == 1) return ItemType.Default;
            if (groupCount == 2) return ItemType.Default;
            if (groupCount == 3) return ItemType.Default;
            if (groupCount == 4) return ItemType.Default;

            if (groupCount == 5) return ItemType.A;
            if (groupCount == 6) return ItemType.A;
            if (groupCount == 7) return ItemType.A;

            if (groupCount == 8) return ItemType.B;
            if (groupCount == 9) return ItemType.B;

            return ItemType.C;
        }

        [Serializable]
        public class GroupData
        {
            [SerializeField] private ItemType itemType;
            [SerializeField] private int maxCount;

            public ItemType ItemType => itemType;
            public int MaxCount => maxCount;
        }
    }
}
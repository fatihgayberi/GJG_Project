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

        [Serializable]
        public class GroupData
        {
            [SerializeField] private ItemType itemType;
            [SerializeField] private int maxCount;
        }
    }
}
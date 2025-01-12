using System.Collections.Generic;
using GJG.GridSystem.Match;
using Unity.Mathematics;
using UnityEngine;
using GJG.Items;
using System;

namespace GJG.GridSystem
{
    [CreateAssetMenu(fileName = "GridData", menuName = "GridData", order = 0)]
    public class GridData : ScriptableObject
    {
        [SerializeField] private List<int2> obstacleIndex;
        [SerializeField] private MatchStrategyType matchStrategyType;
        [SerializeField] private int poolOffset;
        [SerializeField, Tooltip("M - N")] private int2 gridSize; // M N
        // [SerializeField, Tooltip("K")] private ColorInitData[] colorInitData; // K
        [SerializeField, Tooltip("K")] private GridCategoryColorData[] gridCategoryColorData; // K
        [SerializeField, Tooltip("A - B - C ...")] private GroupData[] groupData; // A - B - C ...

        public MatchStrategyType MatchStrategyType => matchStrategyType;
        public int2 GridSize => gridSize;
        public int PoolOffset => poolOffset;
        public GroupData[] GroupDatas => groupData;
        // public ColorInitData[] ColorInitDatas => colorInitData;
        public GridCategoryColorData[] GridCategoryColorDatas => gridCategoryColorData;
        public List<int2> ObstacleIndex => obstacleIndex;


        [Serializable]
        public class GroupData
        {
            [SerializeField] private ItemType itemType;
            [SerializeField] private int maxCount;

            public ItemType ItemType => itemType;
            public int MaxCount => maxCount;
        }

        [Serializable]
        public class GridCategoryColorData
        {
            [SerializeField] private ItemCategoryType itemCategoryType;
            [SerializeField] private ColorInitData[] colorInitData;

            public ItemCategoryType ItemCategoryType => itemCategoryType;
            public ColorInitData[] ColorInitDatas => colorInitData;
        }

        [Serializable]
        public class ColorInitData
        {
            [SerializeField] private ItemColorType itemColorType;
            [SerializeField] private int initialPower;

            public ItemColorType ItemColorType => itemColorType;
            public int InitialPower => initialPower;
        }

    }
}
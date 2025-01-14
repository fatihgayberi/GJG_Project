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
        [SerializeField] private MatchStrategyType matchStrategyType;
        [SerializeField] private int poolOffset;
        [SerializeField] private int minGroupCount;
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector3 startPos;
        [SerializeField, Tooltip("M - N")] private int2 gridSize; // M N
        [SerializeField, Tooltip("K")] private GridCategoryColorData[] gridCategoryColorData; // K
        [SerializeField, Tooltip("A - B - C ...")] private GroupData[] groupData; // A - B - C ...
        [SerializeField] private List<int2> obstacleIndex;

        public MatchStrategyType MatchStrategyType => matchStrategyType;
        public int2 GridSize => gridSize;
        public int PoolOffset => poolOffset;
        public int MinGroupCount => minGroupCount;
        public GroupData[] GroupDatas => groupData;
        public GridCategoryColorData[] GridCategoryColorDatas => gridCategoryColorData;
        public List<int2> ObstacleIndex => obstacleIndex;
        public Vector2 CellSize => cellSize;
        public Vector3 StartPos => startPos;


        [Serializable]
        public class GroupData
        {
            [SerializeField] private ItemType itemType;
            [SerializeField] private int minCount;

            public ItemType ItemType => itemType;
            public int MinCount => minCount;
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
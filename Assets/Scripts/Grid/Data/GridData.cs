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
        [SerializeField, Tooltip("M - N")] private int2 gridSize; // M N
        [SerializeField, Tooltip("K")] private ColorInitData[] colorInitData; // K
        [SerializeField, Tooltip("A - B - C ...")] private GroupData[] groupData; // A - B - C ...

        public MatchStrategyType MatchStrategyType => matchStrategyType;
        public int2 GridSize => gridSize;
        public GroupData[] GroupDatas => groupData;
        public ColorInitData[] ColorInitDatas => colorInitData;


        [Serializable]
        public class GroupData
        {
            [SerializeField] private ItemType itemType;
            [SerializeField] private int maxCount;

            public ItemType ItemType => itemType;
            public int MaxCount => maxCount;
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
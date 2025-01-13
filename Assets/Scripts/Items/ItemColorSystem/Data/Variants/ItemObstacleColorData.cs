using Unity.Mathematics;
using UnityEngine;
using System;

namespace GJG.Items.ItemColor
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ColorData/ItemObstacleColorData", order = 0)]
    public class ItemObstacleColorData : ItemColorData
    {
        [Serializable]
        public class UVDataObstacle : UVData
        {
            [SerializeField] private int health;

            public int Health => health;
        }

        [SerializeField] private UVDataObstacle[] uVData;

        public override int2 GetUV(int health)
        {
            for (int i = 0; i < uVData.Length; i++)
            {
                if (uVData[i].Health != health) continue;
                return uVData[i].UVs[0];
            }

            return new int2(-1, -1);
        }
    }
}
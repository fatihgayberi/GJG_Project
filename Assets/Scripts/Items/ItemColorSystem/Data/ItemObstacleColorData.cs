using System;
using Unity.Mathematics;
using UnityEngine;


namespace GJG.Items.ItemColor
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ColorData/ItemObstacleColorData", order = 0)]
    public class ItemObstacleColorData : ItemColorData
    {
        [Serializable]
        public class UVData
        {
            [SerializeField] private int health;
            [SerializeField] private int2[] uVs;

            public int Health => health;
            public int2[] UVs => uVs;
        }

        [SerializeField] private UVData[] uVData;

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
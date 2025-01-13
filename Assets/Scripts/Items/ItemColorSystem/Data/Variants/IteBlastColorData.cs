using Unity.Mathematics;
using UnityEngine;
using System;

namespace GJG.Items.ItemColor
{
    [CreateAssetMenu(fileName = "ColorData", menuName = "ColorData/IteBlastColorData", order = 0)]
    public class IteBlastColorData : ItemColorData
    {
        [Serializable]
        public class UVDataBlast : UVData
        {
            [SerializeField] private ItemType itemType;

            public ItemType ItemType => itemType;
        }

        [SerializeField] private UVDataBlast[] uVData;

        public override int2 GetUV(int itemType)
        {
            for (int i = 0; i < uVData.Length; i++)
            {
                if (uVData[i].ItemType != (ItemType)itemType) continue;
                return uVData[i].UVs[0];
            }

            return new int2(-1, -1);
        }
    }
}
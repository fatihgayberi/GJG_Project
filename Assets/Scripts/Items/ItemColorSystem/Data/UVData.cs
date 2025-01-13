using Unity.Mathematics;
using UnityEngine;
using System;

namespace GJG.Items.ItemColor
{
    [Serializable]
    public class UVData
    {
        [SerializeField] private int2[] uVs;

        public int2[] UVs => uVs;
    }
}

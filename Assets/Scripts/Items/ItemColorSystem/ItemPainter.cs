using System;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace GJG.Items.ItemColor
{
    public class ItemPainter : MonoBehaviour, IInitializable
    {
        [Serializable]
        private class PainterData
        {
            public ColorData colorData;
            [HideInInspector] public MaterialPropertyBlock materialPropertyBlock;
        }

        [SerializeField] private ItemController itemPrefab;
        [SerializeField] private PainterData[] painterData;
        private Dictionary<ItemColorType, PainterData> _painterDataDictionary = new();

        public void Initialize()
        {
            foreach (PainterData data in painterData)
            {
                // daha sonradan hizli search yapabilmek icin dictionarye tasidik datayi, cunku inspectorden direkt verilmiyordu
                _painterDataDictionary.Add(data.colorData.ItemColorType, data);

                // propertyBlocklari initialize ettik 
                MaterialPropertyBlockInitializer(data);
            }
        }

        public void Paint(ItemController item, ItemColorType colorType, int2 uv)
        {
            // dictionary kullandigim icin datayi cashlemedim cunku burasi surekli calisacak O(1)

            GetPainterData(colorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Row, uv.y);
            GetPainterData(colorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Column, uv.x);

            item.itemRenderer.SetPropertyBlock(GetPainterData(colorType)?.materialPropertyBlock);
        }

        public void Paint(ItemController item, ItemColorType colorType, ItemType itemType)
        {
            // dictionary kullandigim icin datayi cashlemedim cunku burasi surekli calisacak O(1)

            GetPainterData(colorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Row, GetPainterData(colorType).colorData.GetUV(itemType).y);
            GetPainterData(colorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Column, GetPainterData(colorType).colorData.GetUV(itemType).x);

            item.itemRenderer.SetPropertyBlock(GetPainterData(colorType)?.materialPropertyBlock);
        }

        private PainterData GetPainterData(ItemColorType itemColorType)
        {
            if (_painterDataDictionary == null) return null;
            if (!_painterDataDictionary.ContainsKey(itemColorType)) return null;

            return _painterDataDictionary[itemColorType];
        }

        private void MaterialPropertyBlockInitializer(PainterData painterData)
        {
            painterData.materialPropertyBlock = new();

            // property blocklari her udpate islemi icin const degerleri bunlar olacagi icin bunları cashledik
            itemPrefab.itemRenderer.GetPropertyBlock(painterData.materialPropertyBlock);

            painterData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.GrayscaleIntensity, painterData.colorData.GrayscaleIntensity);
            painterData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.Brightness, painterData.colorData.Brightness);
            painterData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.Contrast, painterData.colorData.Contrast);
            painterData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.R, painterData.colorData.ColorLuminance.x);
            painterData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.G, painterData.colorData.ColorLuminance.y);
            painterData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.B, painterData.colorData.ColorLuminance.z);
            painterData.materialPropertyBlock.SetColor(ShaderPopertyIDData.TintColor, painterData.colorData.TintColor);
        }
    }
}
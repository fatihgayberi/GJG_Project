using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine;
using System;

namespace GJG.Items.ItemColor
{
    public class ItemPainter : MonoBehaviour, IInitializable
    {
        [Serializable]
        private class PainterData
        {
            public ItemBase itemPrefab;
            public PropertyData[] propertyDatas;
        }

        [Serializable]
        private class PropertyData
        {
            public ColorData colorData;
            [HideInInspector] public MaterialPropertyBlock materialPropertyBlock;
        }

        [SerializeField] private PainterData painterData;

        private Dictionary<ItemColorType, PropertyData> _propertyDataDictionary = new();

        public void Initialize()
        {
            foreach (PropertyData data in painterData.propertyDatas)
            {
                // daha sonradan hizli search yapabilmek icin dictionarye tasidik datayi, cunku inspectorden direkt verilmiyordu
                _propertyDataDictionary.Add(data.colorData.ItemColorType, data);

                // propertyBlocklari initialize ettik 
                PropertyDataInitializer(data);
            }
        }

        public void Paint(IPaintable paintable, ItemColorType colorType, int2 uv)
        {
            // dictionary kullandigim icin datayi cashlemedim cunku burasi surekli calisacak O(1)

            GetPropertyData(colorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Row, uv.y);
            GetPropertyData(colorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Column, uv.x);

            paintable.Renderer.SetPropertyBlock(GetPropertyData(colorType)?.materialPropertyBlock);
        }

        public void Paint(IPaintable paintable, ItemColorType colorType, ItemType itemType)
        {
            // dictionary kullandigim icin datayi cashlemedim cunku burasi surekli calisacak O(1)

            Paint(paintable, colorType, GetPropertyData(colorType).colorData.GetUV(itemType));
        }

        private PropertyData GetPropertyData(ItemColorType itemColorType)
        {
            if (_propertyDataDictionary == null) return null;
            if (!_propertyDataDictionary.ContainsKey(itemColorType)) return null;

            return _propertyDataDictionary[itemColorType];
        }

        private void PropertyDataInitializer(PropertyData propertyData)
        {
            propertyData.materialPropertyBlock = new();

            // property blocklari her udpate islemi icin const degerleri bunlar olacagi icin bunları cashledik
            painterData.itemPrefab.Renderer.GetPropertyBlock(propertyData.materialPropertyBlock);

            propertyData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.GrayscaleIntensity, propertyData.colorData.GrayscaleIntensity);
            propertyData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.Brightness, propertyData.colorData.Brightness);
            propertyData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.Contrast, propertyData.colorData.Contrast);
            propertyData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.R, propertyData.colorData.ColorLuminance.x);
            propertyData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.G, propertyData.colorData.ColorLuminance.y);
            propertyData.materialPropertyBlock.SetFloat(ShaderPopertyIDData.B, propertyData.colorData.ColorLuminance.z);
            propertyData.materialPropertyBlock.SetColor(ShaderPopertyIDData.TintColor, propertyData.colorData.TintColor);
        }
    }
}
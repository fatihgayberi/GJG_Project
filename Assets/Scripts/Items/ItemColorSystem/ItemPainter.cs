using System.Collections.Generic;
using Unity.VisualScripting;
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
            public ItemColorData colorData;
            [HideInInspector] public MaterialPropertyBlock materialPropertyBlock;
        }

        [SerializeField] private PainterData[] painterDatas;

        private Dictionary<ItemColorType, PropertyData> _propertyDataDictionary = new();

        public void Initialize()
        {
            foreach (PainterData painterData in painterDatas)
            {
                foreach (PropertyData data in painterData.propertyDatas)
                {
                    // daha sonradan hizli search yapabilmek icin dictionarye tasidik datayi, cunku inspectorden direkt verilmiyordu
                    _propertyDataDictionary.Add(data.colorData.ItemColorType, data);

                    // propertyBlocklari initialize ettik 
                    PropertyDataInitializer(data);
                }
            }
        }

        public void Paint(IPaintable paintable, int itemType)
        {
            // dictionary kullandigim icin dataya erisim O(1)
            GetPropertyData(paintable.ColorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Row, GetPropertyData(paintable.ColorType).colorData.GetUV(itemType).y);
            GetPropertyData(paintable.ColorType)?.materialPropertyBlock.SetInt(ShaderPopertyIDData.Column, GetPropertyData(paintable.ColorType).colorData.GetUV(itemType).x);

            paintable.Renderer.SetPropertyBlock(GetPropertyData(paintable.ColorType)?.materialPropertyBlock);
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

            foreach (PainterData painterData in painterDatas)
            {
                // property blocklari her udpate islemi icin const degerleri bunlar olacagi icin bunları cashledik
                painterData.itemPrefab.Renderer.GetPropertyBlock(propertyData.materialPropertyBlock);
            }

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
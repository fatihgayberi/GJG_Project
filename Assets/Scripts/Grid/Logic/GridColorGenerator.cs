using System.Collections.Generic;
using Wonnasmith.Extensions;
using GJG.Items;
using System;

namespace GJG.GridSystem
{
    [Serializable]
    public class GridColorGenerator
    {
        private Dictionary<ItemCategoryType, ColorPower> _colorCategories;

        private class ColorPower
        {
            public List<ItemColorType> colorTypes = new();
            public int colorTypeIndex;
        }

        public GridColorGenerator(GridData gridData)
        {
            // bir listeye power kadar ekleyip shuffle yapip yapip kullaniyoruz ki random kontrollu gelsin
            _colorCategories = new();

            foreach (var category in gridData.GridCategoryColorDatas)
            {
                _colorCategories.Add(category.ItemCategoryType, new());

                foreach (var initData in category.ColorInitDatas)
                {
                    ColorPowerGenerator(_colorCategories[category.ItemCategoryType], initData);
                }
            }
        }

        private ColorPower ColorPowerGenerator(ColorPower colorPower, GridData.ColorInitData colorInitData)
        {
            for (int i = 0; i < colorInitData.InitialPower; i++)
            {
                colorPower.colorTypes.Add(colorInitData.ItemColorType);
            }

            colorPower.colorTypes.Shuffle();

            return colorPower;
        }

        public ItemColorType GetColorType(ItemCategoryType itemCategoryType)
        {
            ColorPower colorPower = _colorCategories[itemCategoryType];

            colorPower.colorTypeIndex++;

            if (colorPower.colorTypeIndex >= colorPower.colorTypes.Count)
            {
                colorPower.colorTypeIndex = 0;
                colorPower.colorTypes.Shuffle();
            }

            return colorPower.colorTypes[colorPower.colorTypeIndex];
        }
    }
}
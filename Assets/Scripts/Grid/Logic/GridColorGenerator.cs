using System.Collections.Generic;
using Wonnasmith.Extensions;
using GJG.Items;
using System;

namespace GJG.GridSystem
{
    [Serializable]
    public class GridColorGenerator
    {
        private int _colorTypeIndex;
        private List<ItemColorType> _colorTypes;

        public GridColorGenerator(GridData gridData)
        {
            // bir listeye power kadar ekleyip shuffle yapip yapip kullaniyoruz ki random kontrollu gelsin
            _colorTypes = new();

            foreach (var colorInitData in gridData.ColorInitDatas)
            {
                for (int i = 0; i < colorInitData.InitialPower; i++)
                {
                    _colorTypes.Add(colorInitData.ItemColorType);
                }
            }

            _colorTypes.Shuffle();
        }

        public ItemColorType GetColorType()
        {
            _colorTypeIndex++;

            if (_colorTypeIndex >= _colorTypes.Count)
            {
                _colorTypeIndex = 0;
                _colorTypes.Shuffle();
            }

            return _colorTypes[_colorTypeIndex];
        }
    }
}
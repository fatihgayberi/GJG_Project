using Wonnasmith.Pooling;
using Unity.Mathematics;
using UnityEngine;
using GJG.Items;
using GJG.Items.ItemColor;

namespace GJG.GridSystem
{
    public class GridGenerator
    {
        [SerializeField] private GridCoordinatData _gridCoordinatData;
        [SerializeField] private GridData _gridData;
        [SerializeField] private Pool<ItemBlast> _itemPool;

        private GameGrid _gameGrid;
        private ItemPainter _itemPainter;
        private GridColorGenerator _colorGenerator;
        private int2 index;

        public GameGrid Grid => _gameGrid;

        public GridGenerator(GridCoordinatData gridCoordinatData, GridData gridData, Pool<ItemBlast> itemPool, ItemPainter itemPainter)
        {
            _gridCoordinatData = gridCoordinatData;
            _itemPainter = itemPainter;
            _itemPool = itemPool;
            _gridData = gridData;

            GridPrepare();
            GridGenerate();
        }

        private void GridPrepare()
        {
            // itemler icin pool generate edildi
            _itemPool.Initialize(_gridData.GridSize.x * _gridData.GridSize.y + _gridData.PoolOffset);

            // grid hazirlandi
            _colorGenerator = new(_gridData);
            _gameGrid = new GameGrid(_gridData, _gridCoordinatData);
        }

        private void GridGenerate()
        {
            ItemBase item;
            Vector3 itemPos = Vector3.zero;

            for (index.x = 0; index.x < _gameGrid.RowLength; index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.ColumnLength; index.y++)
                {
                    item = GetNewItem();
                    item.gameObject.SetActive(true);

                    itemPos.x = _gridCoordinatData.CellSize.x * index.x;
                    itemPos.y = _gridCoordinatData.CellSize.y * index.y;

                    item.transform.position = itemPos + _gridCoordinatData.StartPos;

                    _gameGrid.AddItem(index, item, itemPos + _gridCoordinatData.StartPos);
                }
            }
        }

        public void RefreshGrid()
        {
            for (index.x = 0; index.x < _gameGrid.RowLength; index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.ColumnLength; index.y++)
                {
                    _gameGrid.UpdateNodeStatus(index, _colorGenerator.GetColorType());
                }
            }
        }

        public ItemBlast GetNewItem()
        {
            ItemBlast itemBlast = _itemPool.GetPoolObject();
            itemBlast.ColorType = _colorGenerator.GetColorType();

            _itemPainter.Paint(itemBlast, itemBlast.ColorType, ItemType.Default);

            return itemBlast;
        }

        public void RePoolObject(ItemBlast itemBlast)
        {
            _itemPool.RePoolObject(itemBlast);
        }
    }
}

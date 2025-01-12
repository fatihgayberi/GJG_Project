using GJG.Items.ItemColor;
using Wonnasmith.Pooling;
using Unity.Mathematics;
using UnityEngine;
using GJG.Items;
using System;

namespace GJG.GridSystem
{
    [Serializable]
    public class GridGenerator
    {
        [SerializeField] private GridCoordinatData _gridCoordinatData;
        [SerializeField] private GridData _gridData;
        [SerializeField] private Pool<ItemBlast> _itemPool;
        [SerializeField] private Pool<ItemBlast> _obstaclePool;

        private GameGrid _gameGrid;
        private ItemPainter _itemPainter;
        private GridColorGenerator _colorGenerator;
        private int2 index;

        public GameGrid Grid => _gameGrid;

        public GridGenerator(GridCoordinatData gridCoordinatData, GridData gridData, Pool<ItemBlast> itemPool, Pool<ItemBlast> obstaclePool, ItemPainter itemPainter)
        {
            _gridCoordinatData = gridCoordinatData;
            _itemPainter = itemPainter;
            _itemPool = itemPool;
            _obstaclePool = obstaclePool;
            _gridData = gridData;

            GridPrepare();
            GridGenerate();
        }

        private void GridPrepare()
        {
            // itemler icin pool generate edildi
            _itemPool.Initialize(_gridData.GridSize.x * _gridData.GridSize.y + _gridData.PoolOffset);
            _obstaclePool.Initialize();

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
                    _gameGrid.UpdateNodeStatus(index, _colorGenerator.GetColorType(ItemCategoryType.Blast));
                }
            }
        }

        public ItemBlast GetNewItem()
        {
            ItemBlast itemBlast = _itemPool.GetPoolObject();
            itemBlast.ColorType = _colorGenerator.GetColorType(ItemCategoryType.Blast);

            _itemPainter.Paint(itemBlast, itemBlast.ColorType, ItemType.Level_1);

            return itemBlast;
        }

        public ItemBlast GetNewObstacle()
        {
            ItemBlast itemBlast = _obstaclePool.GetPoolObject();
            itemBlast.ColorType = _colorGenerator.GetColorType(ItemCategoryType.Obstacle);

            _itemPainter.Paint(itemBlast, itemBlast.ColorType, ItemType.Level_1);

            return itemBlast;
        }

        public void RePoolObject(ItemBlast itemBlast)
        {
            _itemPool.RePoolObject(itemBlast);
        }
    }
}

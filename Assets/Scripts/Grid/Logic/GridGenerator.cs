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
        [SerializeField] private GridData _gridData;
        [SerializeField] private Pool<ItemBase> _itemPool;
        [SerializeField] private Pool<ItemBase> _obstaclePool;

        private GameGrid _gameGrid;
        private ItemPainter _itemPainter;
        private GridColorGenerator _colorGenerator;
        private int2 index;

        public GameGrid Grid => _gameGrid;
        public ItemPainter ItemPainter => _itemPainter;

        public GridGenerator(GridData gridData, Pool<ItemBase> itemPool, Pool<ItemBase> obstaclePool, ItemPainter itemPainter)
        {
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
            _gameGrid = new GameGrid(_gridData);
        }

        private void GridGenerate()
        {
            ItemBase item;
            Vector3 itemPos = Vector3.zero;

            for (index.x = 0; index.x < _gameGrid.RowLength; index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.ColumnLength; index.y++)
                {
                    item = _gridData.ObstacleIndex.Contains(index) ? GetNewObstacle() : GetNewItem();

                    item.gameObject.SetActive(true);

                    itemPos.x = _gridData.CellSize.x * index.x;
                    itemPos.y = _gridData.CellSize.y * index.y;

                    item.transform.position = itemPos + _gridData.StartPos;

                    _gameGrid.AddItem(index, item, itemPos + _gridData.StartPos);
                }
            }
        }

        public void RefreshGrid()
        {
            for (index.x = 0; index.x < _gameGrid.RowLength; index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.ColumnLength; index.y++)
                {
                    if (_gameGrid.GetItem(index) is IBlastableItem)
                    {
                        _gameGrid.UpdateNodeStatus(index, _colorGenerator.GetColorType(ItemCategoryType.Blast));
                    }
                }
            }
        }

        public ItemBase GetNewItem()
        {
            ItemBase itemBlast = _itemPool.GetPoolObject();
            itemBlast.ColorType = _colorGenerator.GetColorType(ItemCategoryType.Blast);

            _itemPainter.Paint(itemBlast, (int)ItemType.Default);

            return itemBlast;
        }

        public ItemBase GetNewObstacle()
        {
            ItemBase itemBlast = _obstaclePool.GetPoolObject();
            itemBlast.ColorType = _colorGenerator.GetColorType(ItemCategoryType.Obstacle);

            if (itemBlast is IBreakableItem breakableItem)
            {
                _itemPainter.Paint(itemBlast, breakableItem.Health);
            }

            return itemBlast;
        }

        public void RePoolObject(ItemBase itemBase)
        {
            if (itemBase == null) return;

            if (itemBase.ItemCategory == ItemCategoryType.Blast)
            {
                _itemPool.RePoolObject(itemBase);
            }
            else if (itemBase.ItemCategory == ItemCategoryType.Obstacle)
            {
                _obstaclePool.RePoolObject(itemBase);
            }
        }
    }
}

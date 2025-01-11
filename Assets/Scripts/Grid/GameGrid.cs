using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using GJG.Items;

namespace GJG.GridSystem
{
    public class GameGrid
    {
        private GridData _gridData;
        private GridCoordinatData _gridCoordinatData;

        private Node[,] _grid;

        private HashSet<int2> _indexContainer = new();

        public int RowLength => _grid.GetLength(0);
        public int ColumnLength => _grid.GetLength(1);

        public GameGrid(GridData gridData, GridCoordinatData gridCoordinatData)
        {
            _gridData = gridData;
            _gridCoordinatData = gridCoordinatData;

            Create();
        }

        /// <summary> Grid olusturulur </summary>
        public void Create()
        {
            _grid = new Node[_gridData.GridSize.x, _gridData.GridSize.y];

            for (int i = 0; i < _gridData.GridSize.x; i++)
            {
                for (int j = 0; j < _gridData.GridSize.y; j++)
                {
                    _grid[i, j].Empty();
                }
            }
        }

        /// <summary> Grid e ekleme yapar </summary>
        public bool AddItem(int2 index, ItemBase item, ItemColorType itemColorType, Vector3 pos)
        {
            if (!IsValidIndex(index)) return false;
            if (!_grid[index.x, index.y].IsEmpty) return false;

            _grid[index.x, index.y].item = item;
            _grid[index.x, index.y].IsEmpty = false;
            _grid[index.x, index.y].ColorType = itemColorType;
            _grid[index.x, index.y].nodePos = pos;

            return true;
        }

        /// <summary> verilen indexteki sutunu get eder </summary>
        public HashSet<int2> GetColumn(int rowIndex, int firstColumnIndex = 0)
        {
            _indexContainer.Clear();

            int2 selectIndex = new(rowIndex, firstColumnIndex);

            for (; selectIndex.y < _grid.GetLength(1); selectIndex.y++)
            {
                _indexContainer.Add(selectIndex);
            }

            return _indexContainer;
        }

        /// <summary> verilen indexteki sutunu get eder </summary>
        public HashSet<int2> GetRow(int columnIndex, int firstRowIndex = 0)
        {
            _indexContainer.Clear();

            int2 selectIndex = new(firstRowIndex, columnIndex);

            for (; selectIndex.x < _grid.GetLength(0); selectIndex.x++)
            {
                _indexContainer.Add(selectIndex);
            }

            return _indexContainer;
        }

        /// <summary> Grid icinden eleman cikarir </summary>
        public bool RemoveItem(int2 index)
        {
            if (!IsValidIndex(index)) return false;

            _grid[index.x, index.y].Empty();

            return true;
        }

        /// <summary> Grid Node return eder </summary>
        public Node GetNode(int2 index)
        {
            if (!IsValidIndex(index)) return default;

            return _grid[index.x, index.y];
        }

        public void UpdateNodeStatus(int2 index, ItemColorType colorType)
        {
            _grid[index.x, index.y].ColorType = colorType;
        }

        public int2 WorldPosToItemIndex(Vector3 worldPosition)
        {
            int2 index;

            index.x = Mathf.RoundToInt((worldPosition.x - _gridCoordinatData.StartPos.x) / _gridCoordinatData.CellSize.x);
            index.y = Mathf.RoundToInt((worldPosition.y - _gridCoordinatData.StartPos.y) / _gridCoordinatData.CellSize.y);

            return index;
        }

        /// <summary> Grid itemini return eder </summary>
        public ItemBase GetItem(int2 index)
        {
            if (!IsValidIndex(index)) return null;

            return _grid[index.x, index.y].item;
        }

        /// <summary> Grid iteminin indexini return eder </summary>
        public int2 GetItemIndex(Node item)
        {
            for (int i = 0; i < _grid.GetLength(0); i++)
            {
                for (int j = 0; j < _grid.GetLength(1); j++)
                {
                    if (!_grid[i, j].Equals(item)) continue;
                    return new int2(i, j);
                }
            }

            return new int2(-1, -1);
        }

        /// <summary> Grid itemini return eder </summary>
        public void Swap(int2 index1, int2 index2)
        {
            if (!IsValidIndex(index1)) return;
            if (!IsValidIndex(index2)) return;

            var temp1 = _grid[index1.x, index1.y];
            var temp2 = _grid[index2.x, index2.y];

            _grid[index1.x, index1.y].ColorType = temp2.ColorType;
            _grid[index1.x, index1.y].item = temp2.item;
            _grid[index1.x, index1.y].IsEmpty = temp2.IsEmpty;

            _grid[index2.x, index2.y].ColorType = temp1.ColorType;
            _grid[index2.x, index2.y].item = temp1.item;
            _grid[index2.x, index2.y].IsEmpty = temp1.IsEmpty;
        }

        /// <summary> Grid boyutlarini kontrol eder eger boyutlari asiyorsa false doner </summary>
        public bool IsValidIndex(int2 index)
        {
            return index.x >= 0 && index.y >= 0
                && index.x < _grid.GetLength(0)
                && index.y < _grid.GetLength(1);
        }
    }
}

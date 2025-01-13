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

        private List<int2> _indexContainer = new();

        public int RowLength => _grid.GetLength(0);
        public int ColumnLength => _grid.GetLength(1);

        public GameGrid(GridData gridData, GridCoordinatData gridCoordinatData)
        {
            _gridData = gridData;
            _gridCoordinatData = gridCoordinatData;

            _grid = new Node[_gridData.GridSize.x, _gridData.GridSize.y];
        }

        /// <summary> Grid e ekleme yapar </summary>
        public bool AddItem(int2 index, ItemBase item, Vector3 pos)
        {
            if (!IsValidIndex(index)) return false;
            if (!_grid[index.x, index.y].IsEmpty) return false;

            _grid[index.x, index.y].nodePos = pos;
            _grid[index.x, index.y].AddItem(item);

            return true;
        }

        /// <summary> verilen indexteki sutunu get eder </summary>
        public List<int2> GetColumn(int rowIndex, int firstColumnIndex = 0)
        {
            return GetColumn(rowIndex, _grid.GetLength(1), firstColumnIndex);
        }

        /// <summary> verilen indexler arasi sutunu get eder </summary>
        public List<int2> GetColumn(int rowIndex, int endColumnIndex, int firstColumnIndex = 0)
        {
            _indexContainer.Clear();

            int2 selectIndex = new(rowIndex, firstColumnIndex);

            for (; selectIndex.y < endColumnIndex; selectIndex.y++)
            {
                _indexContainer.Add(selectIndex);
            }

            return _indexContainer;
        }

        /// <summary> verilen indexteki sutunu get eder </summary>
        public List<int2> GetRow(int columnIndex, int firstRowIndex = 0)
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

            _grid[index.x, index.y].RemoveItem();

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
            _grid[index.x, index.y].ItemBase.ColorType = colorType;
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

            return _grid[index.x, index.y].ItemBase;
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

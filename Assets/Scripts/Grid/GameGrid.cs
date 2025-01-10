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

        public GameGrid(GridData gridData, GridCoordinatData gridCoordinatData)
        {
            _gridData = gridData;
            _gridCoordinatData = gridCoordinatData;

            Create();
        }

        public Node[,] Grid => _grid;


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
        public bool AddItem(int2 index, ItemController item, ItemColorType itemColorType)
        {
            if (!IsValidIndex(index)) return false;
            if (!_grid[index.x, index.y].IsEmpty) return false;

            _grid[index.x, index.y].item = item;
            _grid[index.x, index.y].ColorType = itemColorType;

            return true;
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

        public Vector3 WorldPosToGridIndex(Vector3 worldPosition)
        {
            int2 index;

            index.x = Mathf.RoundToInt((worldPosition.x - _gridCoordinatData.StartPos.x) / _gridCoordinatData.CellSize.x);
            index.y = Mathf.RoundToInt((worldPosition.y - _gridCoordinatData.StartPos.y) / _gridCoordinatData.CellSize.y);

            return GetItem(index).gameObject.transform.position;
        }

        /// <summary> Grid itemini return eder </summary>
        public ItemController GetItem(int2 index)
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

        /// <summary> Grid boyutlarini kontrol eder eger boyutlari asiyorsa false doner </summary>
        public bool IsValidIndex(int2 index)
        {
            return index.x >= 0 && index.y >= 0
                && index.x < _grid.GetLength(0)
                && index.y < _grid.GetLength(1);
        }
    }
}

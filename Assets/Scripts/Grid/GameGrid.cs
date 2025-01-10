using Unity.Mathematics;
using UnityEngine;

namespace GJG.GridSystem
{
    public class GameGrid<T>
    {
        private GridLogic gridLogic;
        private GridData gridData;

        private Node<T>[,] _grid;

        public GameGrid(GridLogic gridLogic, GridData gridData)
        {
            this.gridLogic = gridLogic;
            this.gridData = gridData;

            Create();
        }

        public Node<T>[,] Grid => _grid;


        /// <summary> Grid olusturulur </summary>
        public void Create()
        {
            _grid = new Node<T>[gridData.GridSize.x, gridData.GridSize.y];

            for (int i = 0; i < gridData.GridSize.x; i++)
            {
                for (int j = 0; j < gridData.GridSize.y; j++)
                {
                    _grid[i, j].Empty();
                }
            }
        }

        /// <summary> Grid e ekleme yapar </summary>
        public bool AddItem(int2 index, T item)
        {
            if (!IsValidIndex(index))
            {
                Debug.Log("return false::");
                return false;
            }
            if (!_grid[index.x, index.y].IsEmpty)
            {
                Debug.Log("return false::");
                return false;
            }

            _grid[index.x, index.y].item = item;

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
        public Node<T> GetNode(int2 index)
        {
            if (!IsValidIndex(index)) return default;

            return _grid[index.x, index.y];
        }

        /// <summary> Grid itemini return eder </summary>
        public T GetItem(int2 index)
        {
            if (!IsValidIndex(index)) return default;

            return _grid[index.x, index.y].item;
        }

        /// <summary> Grid itemini return eder </summary>
        public void Swap(Node<T> item1, Node<T> item2)
        {
            Swap(GetItemIndex(item1), GetItemIndex(item2));
        }

        /// <summary> Grid itemini return eder </summary>
        public void Swap(int2 index1, int2 index2)
        {
            if (!IsValidIndex(index1)) return;
            if (!IsValidIndex(index2)) return;

            (_grid[index2.x, index2.y], _grid[index1.x, index1.y]) = (_grid[index1.x, index1.y], _grid[index2.x, index2.y]);
        }

        /// <summary> Grid iteminin indexini return eder </summary>
        public int2 GetItemIndex(Node<T> item)
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

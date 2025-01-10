using GJG.Items;
using GJG.Items.ItemColor;
using Unity.Mathematics;
using UnityEngine;
using Wonnasmith.Pooling;

namespace GJG.GridSystem
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private GridData gridData;
        [SerializeField] private ItemPainter itemPainter;
        [SerializeField] private GridCoordinatData gridCoordinatData;
        [SerializeField] private GroupChecker groupChecker;
        [SerializeField] private Pool<ItemController> itemPool;

        private GameGrid<ItemController> _gameGrid;

        private void Start()
        {
            _gameGrid = new GameGrid<ItemController>(gridData);

            itemPool.Initialize();
            itemPainter.Initialize();

            int2 index;
            ItemController item;
            ItemColorType colorType;
            Vector3 itemPos = Vector3.zero;

            for (index.x = 0; index.x < _gameGrid.Grid.GetLength(0); index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.Grid.GetLength(1); index.y++)
                {
                    item = itemPool.GetPoolObject();
                    item.gameObject.SetActive(true);
                    colorType = (ItemColorType)UnityEngine.Random.Range(1, 7);

                    itemPos.x = gridCoordinatData.CellSize.x * index.x;
                    itemPos.y = gridCoordinatData.CellSize.y * index.y;

                    item.transform.position = itemPos + gridCoordinatData.StartPos;

                    _gameGrid.AddItem(index, item, colorType, ItemType.Default);
                }
            }

            groupChecker.Init(_gameGrid);

            groupChecker.Check();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                RefreshGrid();
            }
            if (Input.GetKey(KeyCode.S))
            {
                RefreshGrid();
            }
        }

        public void RefreshGrid()
        {
            int2 index;
            ItemColorType colorType;

            for (index.x = 0; index.x < _gameGrid.Grid.GetLength(0); index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.Grid.GetLength(1); index.y++)
                {
                    colorType = (ItemColorType)UnityEngine.Random.Range(1, 6);

                    _gameGrid.UpdateNodeStatus(index, colorType);
                }
            }

            groupChecker.Check();
        }
    }
}

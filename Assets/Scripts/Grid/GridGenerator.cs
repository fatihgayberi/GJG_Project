using GJG.Items.ItemColor;
using Wonnasmith.Pooling;
using Unity.Mathematics;
using GJG.GJGInput;
using UnityEngine;
using GJG.Items;

namespace GJG.GridSystem
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private ItemPainter itemPainter;
        [SerializeField] private InputManager InputManager;
        [SerializeField] private GridCoordinatData gridCoordinatData;
        [SerializeField] private GridData gridData;
        [SerializeField] private Pool<ItemController> itemPool;

        private GameGrid _gameGrid;
        private GridColorGenerator _colorGenerator;
        private GroupChecker _groupChecker;
        private int2 index;

        private void Start()
        {
            _gameGrid = new GameGrid(gridData, gridCoordinatData);

            itemPool.Initialize(gridData.GridSize.x * gridData.GridSize.y);
            itemPainter.Initialize();
            InputManager.Initialize(_gameGrid);
            _colorGenerator = new(gridData);

            Generate();
        }

        private void Generate()
        {
            ItemController item;
            ItemColorType colorType;
            Vector3 itemPos = Vector3.zero;

            for (index.x = 0; index.x < _gameGrid.Grid.GetLength(0); index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.Grid.GetLength(1); index.y++)
                {
                    item = itemPool.GetPoolObject();
                    item.gameObject.SetActive(true);
                    colorType = _colorGenerator.GetColorType();

                    itemPos.x = gridCoordinatData.CellSize.x * index.x;
                    itemPos.y = gridCoordinatData.CellSize.y * index.y;

                    item.transform.position = itemPos + gridCoordinatData.StartPos;

                    _gameGrid.AddItem(index, item, colorType);
                }
            }

            _groupChecker = new GroupChecker(_gameGrid, gridData, itemPainter);
            _groupChecker.CheckAllGrid();
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
            for (index.x = 0; index.x < _gameGrid.Grid.GetLength(0); index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.Grid.GetLength(1); index.y++)
                {
                    _gameGrid.UpdateNodeStatus(index, _colorGenerator.GetColorType());
                }
            }

            _groupChecker.CheckAllGrid();
        }
    }
}

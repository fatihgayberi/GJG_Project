using GJG.Items;
using GJG.Items.ItemColor;
using Unity.Mathematics;
using UnityEngine;
using Wonnasmith.Pooling;

namespace GJG.GridSystem
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField] private GridLogic gridLogic;
        [SerializeField] private GridData gridData;
        [SerializeField] private ItemPainter itemPainter;
        [SerializeField] private GridCoordinatData gridCoordinatData;
        [SerializeField] private Pool<ItemController> itemPool;

        private GameGrid<ItemController> _gameGrid;

        private void Start()
        {
            _gameGrid = new GameGrid<ItemController>(gridLogic, gridData);

            itemPool.Initialize();
            itemPainter.Initialize();

            int2 index;
            ItemController item;
            Vector3 itemPos = Vector3.zero;

            for (index.x = 0; index.x < _gameGrid.Grid.GetLength(0); index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.Grid.GetLength(1); index.y++)
                {
                    item = itemPool.GetPoolObject();
                    item.gameObject.SetActive(true);

                    itemPainter.Paint(item, (ItemColorType)UnityEngine.Random.Range(1, 7), new int2(UnityEngine.Random.Range(0, 3), UnityEngine.Random.Range(0, 3)));

                    itemPos.x = gridCoordinatData.CellSize.x * index.x;
                    itemPos.y = gridCoordinatData.CellSize.y * index.y;

                    item.transform.position = itemPos + gridCoordinatData.StartPos;

                    _gameGrid.AddItem(index, item);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKey(KeyCode.A))
            {
                RefreshGrid();
            }
        }
#endif

        public void RefreshGrid()
        {
            int2 index;
            int2 uv;
            ItemColorType colorType;

            for (index.x = 0; index.x < _gameGrid.Grid.GetLength(0); index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.Grid.GetLength(1); index.y++)
                {
                    uv.x = UnityEngine.Random.Range(0, 3);
                    uv.y = UnityEngine.Random.Range(0, 3);
                    colorType = (ItemColorType)UnityEngine.Random.Range(1, 6);

                    itemPainter.Paint(_gameGrid.GetNode(index).item, colorType, uv);
                }
            }
        }
    }
}

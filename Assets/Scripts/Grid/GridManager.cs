using GJG.Items.ItemColor;
using Wonnasmith.Pooling;
using GJG.BlastSystem;
using GJG.GJGInput;
using UnityEngine;
using GJG.Items;

namespace GJG.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private ItemPainter itemPainter;
        [SerializeField] private GridData gridData;
        [SerializeField] private Pool<ItemBase> itemPool;
        [SerializeField] private Pool<ItemBase> _obstaclePool;

        private GridGenerator _gridGenerator;
        public GridDropper _gridDropper;

        private GroupChecker _groupChecker;

        private void Start()
        {
            // boyama icin hazirlik yapildi
            itemPainter.Initialize();

            // grid uretildi
            _gridGenerator = new GridGenerator(gridData, itemPool, _obstaclePool, itemPainter);

            // grid gruplama hazirlandi
            _groupChecker = new GroupChecker(_gridGenerator.Grid, gridData, itemPainter);

            // grid gruplara bolundu ve boyandi
            _groupChecker.CheckAllGrid();

            // gridden item dusurme islemi
            _gridDropper = new GridDropper(_gridGenerator.Grid, _groupChecker, _gridGenerator);

            // patlatma islemlerine hazirlandi
            new Blast(_gridGenerator.Grid, gridData, _gridDropper, _gridGenerator);


            GridEvents.MoveFinishAlltItem += OnMoveFinishAlltItem;
        }

        private void OnMoveFinishAlltItem()
        {
            InputEvents.ScreenTouchLock?.Invoke();
            RefreshGrid();
            InputEvents.ScreenTouchUnLock?.Invoke();
        }

        // [ContextMenu("RefreshGrid")]
        private void RefreshGrid()
        {
            _gridGenerator.RefreshGrid();
            _groupChecker.CheckAllGrid();
        }

        public void RefreshGridButtonForTest()
        {
            if (_gridDropper.MoveFinishCheck())
            {
                Debug.LogWarning("Hareket halinde olan itemler var");
                return;
            }

            RefreshGrid();
        }
    }
}
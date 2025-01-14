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
        [SerializeField] private GridCoordinatData gridCoordinatData;
        [SerializeField] private GridData gridData;
        [SerializeField] private Pool<ItemBase> itemPool;
        [SerializeField] private Pool<ItemBase> _obstaclePool;

        private GridGenerator _gridGenerator;

        private GroupChecker _groupChecker;
        public Blast _blast;

        private void Start()
        {
            // boyama icin hazirlik yapildi
            itemPainter.Initialize();

            // grid uretildi
            _gridGenerator = new GridGenerator(gridCoordinatData, gridData, itemPool, _obstaclePool, itemPainter);

            // grid gruplama hazirlandi
            _groupChecker = new GroupChecker(_gridGenerator.Grid, gridData, itemPainter);

            // grid gruplara bolundu ve boyandi
            _groupChecker.CheckAllGrid();

            // patlatma islemlerine hazirlandi
            _blast = new Blast(_gridGenerator.Grid, gridData, _groupChecker, _gridGenerator);

            GridEvents.MoveFinishAlltItem += OnMoveFinishAlltItem;
        }

        private void OnMoveFinishAlltItem()
        {
            InputEvents.ScreenTouchLock?.Invoke();
            RefreshGrid();
            InputEvents.ScreenTouchUnLock?.Invoke();
        }

        // [ContextMenu("RefreshGrid")]
        public void RefreshGrid()
        {
            _gridGenerator.RefreshGrid();
            _groupChecker.CheckAllGrid();
        }
    }
}
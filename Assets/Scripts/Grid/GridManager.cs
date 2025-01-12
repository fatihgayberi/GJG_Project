using GJG.Items.ItemColor;
using Wonnasmith.Pooling;
using GJG.BlastSystem;
using UnityEngine;
using GJG.Items;

namespace GJG.GridSystem
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private ItemPainter itemPainter;
        [SerializeField] private BlastManager blastManager;
        [SerializeField] private GridCoordinatData gridCoordinatData;
        [SerializeField] private GridData gridData;
        [SerializeField] private Pool<ItemBase> itemPool;
        [SerializeField] private Pool<ItemBase> _obstaclePool;

        private GridGenerator _gridGenerator;

        private GroupChecker _groupChecker;

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
            blastManager.Initialize(_gridGenerator.Grid, gridData, _groupChecker, _gridGenerator);
        }

        public void TestUpdate()
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
            _gridGenerator.RefreshGrid();
            _groupChecker.CheckAllGrid();
        }
    }
}
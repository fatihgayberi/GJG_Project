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
        [SerializeField] private Pool<ItemBlast> itemPool;

        private GridGenerator _gridGenerator;

        private GroupChecker _groupChecker;

        private void Start()
        {
            // grid uretildi
            _gridGenerator = new GridGenerator(gridCoordinatData, gridData, itemPool);

            // boyama icin hazirlik yapildi
            itemPainter.Initialize();

            // grid gruplama hazirlandi
            _groupChecker = new GroupChecker(_gridGenerator.Grid, gridData, itemPainter);

            // grid gruplara bolundu ve boyandi
            _groupChecker.CheckAllGrid();

            // patlatma islemlerine hazirlandi
            blastManager.Initialize(_gridGenerator.Grid, gridData);
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
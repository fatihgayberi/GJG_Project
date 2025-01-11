using GJG.GridSystem;
using UnityEngine;

namespace GJG.BlastSystem
{
    public class BlastManager : MonoBehaviour
    {
        private GameGrid _gameGrid;

        public void Initialize(GameGrid gameGrid, GridData gridData, GroupChecker groupChecker)
        {
            _gameGrid = gameGrid;

            new Blast(gameGrid, gridData, groupChecker);
        }
    }
}
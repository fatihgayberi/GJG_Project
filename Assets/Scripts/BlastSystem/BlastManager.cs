using GJG.GridSystem;
using UnityEngine;

namespace GJG.BlastSystem
{
    public class BlastManager : MonoBehaviour
    {
        private GameGrid _gameGrid;
        private Blast _blast;

        public void Initialize(GameGrid gameGrid, GridData gridData)
        {
            _gameGrid = gameGrid;

            _blast = new Blast(gameGrid, gridData);
        }
    }
}
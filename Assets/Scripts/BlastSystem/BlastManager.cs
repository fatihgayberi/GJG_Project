using GJG.GridSystem;
using UnityEngine;

namespace GJG.BlastSystem
{
    public class BlastManager : MonoBehaviour
    {
        public Blast blast;

        public void Initialize(GameGrid gameGrid, GridData gridData, GroupChecker groupChecker, GridGenerator gridGenerator)
        {
            blast = new Blast(gameGrid, gridData, groupChecker, gridGenerator);
        }
    }
}
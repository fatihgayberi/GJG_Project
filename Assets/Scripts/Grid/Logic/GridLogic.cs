using UnityEngine;

namespace GJG.GridSystem
{
    public class GridLogic : MonoBehaviour
    {
        [SerializeField] private MatchCheckerBase matchCheckerBase;

        public void Start()
        {
            matchCheckerBase.Initialize();
        }
    }
}
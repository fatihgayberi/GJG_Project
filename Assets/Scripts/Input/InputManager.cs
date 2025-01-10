using GJG.GridSystem;
using GJG.Items;
using UnityEngine;

namespace GJG.GJGInput
{
    public class InputManager : MonoBehaviour
    {
        private GameGrid _gameGrid;

        private Vector3 itemPos;
        Vector3 worldPosition;

        public void Initialize(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0)) // Sol tıklama
            {
                if (_gameGrid == null) return;

                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPosition.z = 0;
                itemPos = _gameGrid.WorldPosToGridIndex(worldPosition);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(itemPos, Vector3.one * 0.4f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(worldPosition, 0.4f);
        }
    }
}
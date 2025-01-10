using UnityEngine;

namespace GJG.GridSystem
{
    [CreateAssetMenu(fileName = "GridCoordinatData", menuName = "GridCoordinatData", order = 0)]
    public class GridCoordinatData : ScriptableObject
    {
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector3 startPos;

        public Vector2 CellSize => cellSize;
        public Vector3 StartPos => startPos;
    }
}
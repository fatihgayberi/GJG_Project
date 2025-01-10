using GJG.GridSystem;
using GJG.GJGInput;
using UnityEngine;
using GJG.GridSystem.Match;
using GJG.Items;
using Unity.Mathematics;
using System.Collections.Generic;

namespace GJG
{
    public class ItemBlast : MonoBehaviour
    {
        private MatchStrategy _matchStrategy;
        private GameGrid _gameGrid;

        private Vector3? itemPos;
        private Vector3 _worldPosition;

        public void Initialize(GameGrid gameGrid)
        {
            _gameGrid = gameGrid;

            InputEvents.ScreenTouch += OnScreenTouch;

            _matchStrategy = new MatchStrategy(_gameGrid, MatchStrategyType.BFS);
        }

        private void OnScreenTouch(Vector3 inputPosition)
        {
            if (_gameGrid == null) return;

            _worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
            _worldPosition.z = 0;

            int2 itemIndex = _gameGrid.WorldPosToItemIndex(_worldPosition);

            HashSet<int2> matchsItem = _matchStrategy.Strategy.GetMatchesItem(itemIndex);

            foreach (var matchsItemIndex in matchsItem)
            {
                _gameGrid.GetItem(matchsItemIndex).gameObject.SetActive(false);
            }
        }

        private void OnDrawGizmos()
        {
            if (itemPos != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireCube((Vector3)itemPos, Vector3.one * 0.4f);
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_worldPosition, 0.4f);
        }
    }
}
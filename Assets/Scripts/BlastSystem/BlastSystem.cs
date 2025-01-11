using System.Collections.Generic;
using GJG.GridSystem.Match;
using Unity.Mathematics;
using GJG.GridSystem;
using GJG.GJGInput;
using UnityEngine;
using GJG.Items;

namespace GJG.BlastSystem
{
    public class Blast
    {
        private MatchStrategy _matchStrategy;
        private GameGrid _gameGrid;
        private GridDropper _gridDropper;

        private Vector3 _worldPosition;

        public Blast(GameGrid gameGrid, GridData gridData, GroupChecker groupChecker)
        {
            _gameGrid = gameGrid;
            _gridDropper = new GridDropper(_gameGrid, groupChecker);
            InputEvents.ScreenTouch += OnScreenTouch;

            _matchStrategy = new MatchStrategy(_gameGrid, gridData.MatchStrategyType);
        }

        private void OnScreenTouch(Vector3 inputPosition)
        {
            if (_gameGrid == null) return;

            _worldPosition = Camera.main.ScreenToWorldPoint(inputPosition);
            _worldPosition.z = 0;

            int2 itemIndex = _gameGrid.WorldPosToItemIndex(_worldPosition);

            ItemBase itemBase = _gameGrid.GetItem(itemIndex);

            // bos bir yere tiklandi o yuden item null geldi
            if (itemBase == null) return;

            HashSet<int2> matchsItem = _matchStrategy.Strategy.GetMatchesItem(itemIndex);

            HashSet<int> dropedColumn = new();

            foreach (var matchsItemIndex in matchsItem)
            {
                _gameGrid.GetItem(matchsItemIndex).gameObject.SetActive(false);
                _gameGrid.RemoveItem(matchsItemIndex);

                dropedColumn.Add(matchsItemIndex.x);
            }

            foreach (var item in dropedColumn)
            {
                _gridDropper.Drop(item);
            }

        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_worldPosition, 0.4f);
        }
    }
}
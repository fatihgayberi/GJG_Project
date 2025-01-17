using System.Collections.Generic;
using GJG.GridSystem.Match;
using Unity.Mathematics;
using GJG.GridSystem;
using GJG.GJGInput;
using UnityEngine;
using GJG.Global;
using GJG.Items;
using System;

namespace GJG.BlastSystem
{
    [Serializable]
    public class Blast
    {
        private MatchStrategy _matchStrategy;
        private GameGrid _gameGrid;
        private GridGenerator _gridGenerator;
        private GridDropper _gridDropper;

        private Vector3 _worldPosition;
        private int _minGroupCount;

        public Blast(GameGrid gameGrid, GridData gridData, GridDropper gridDropper, GridGenerator gridGenerator)
        {
            _gameGrid = gameGrid;
            _gridGenerator = gridGenerator;
            _gridDropper = gridDropper;

            _matchStrategy = new MatchStrategy(_gameGrid, gridData.MatchStrategyType);
            _minGroupCount = gridData.MinGroupCount;

            InputEvents.ScreenTouch += OnScreenTouch;
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
            if (itemBase is not ISellectableItem) return;

            HashSet<int2> matchsItem = _matchStrategy.Strategy.GetMatchesItem(itemIndex);

            if (matchsItem.Count < _minGroupCount) return;

            var obstacleBlast = ObstacleBlastCheck(matchsItem);
            matchsItem.UnionWith(obstacleBlast);

            HashSet<int> dropedColumn = new();
            Dictionary<int, List<int2>> dropedRow = new();

            foreach (var matchsItemIndex in matchsItem)
            {
                _gameGrid.GetItem(matchsItemIndex).gameObject.SetActive(false);
                _gameGrid.RemoveItem(matchsItemIndex);

                _gridGenerator.RePoolObject(_gameGrid.GetItem(matchsItemIndex));

                dropedRow.TryAdd(matchsItemIndex.x, new List<int2>());
                dropedRow[matchsItemIndex.x].Add(matchsItemIndex);

                dropedColumn.Add(matchsItemIndex.x);
            }

            _gridDropper.Drop(dropedColumn, dropedRow, obstacleBlast.Count > 0);
        }

        private HashSet<int2> ObstacleBlastCheck(HashSet<int2> matchsItem)
        {
            HashSet<int2> blastObstacleIndex = new();

            foreach (var matchsIndex in matchsItem)
            {
                foreach (var neighbourIndex in Neigbours.NeighbourIndex)
                {
                    if (_gameGrid.GetItem(matchsIndex + neighbourIndex) is not ItemObstacle obstacle) continue;

                    obstacle.health--;
                    _gridGenerator.ItemPainter.Paint(obstacle, obstacle.health);

                    if (obstacle.health <= 0)
                    {
                        blastObstacleIndex.Add(matchsIndex + neighbourIndex);
                    }
                }
            }

            return blastObstacleIndex;
        }
    }
}
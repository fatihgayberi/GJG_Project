using System.Collections.Generic;
using GJG.GridSystem.Match;
using Unity.Mathematics;
using GJG.GridSystem;
using GJG.GJGInput;
using UnityEngine;
using GJG.Items;
using System;

namespace GJG.BlastSystem
{
    [Serializable]
    public class Blast
    {
        private MatchStrategy _matchStrategy;
        private GameGrid _gameGrid;
        private GridDropper _gridDropper;
        private GridGenerator _gridGenerator;

        private Vector3 _worldPosition;

        private static readonly int2[] _neighbourIndex = new int2[]
        {
            new (1, 0),
            new (-1, 0),
            new (0, 1),
            new (0, -1),
        };

        public Blast(GameGrid gameGrid, GridData gridData, GroupChecker groupChecker, GridGenerator gridGenerator)
        {
            _gameGrid = gameGrid;
            _gridGenerator = gridGenerator;

            _gridDropper = new GridDropper(_gameGrid, groupChecker, gridGenerator);
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
            if (itemBase is not ISellectableItem) return;

            HashSet<int2> matchsItem = _matchStrategy.Strategy.GetMatchesItem(itemIndex);

            matchsItem.UnionWith(ObstacleBlastCheck(matchsItem));

            HashSet<int> dropedColumn = new();
            Dictionary<int, int> dropedRow = new();

            foreach (var matchsItemIndex in matchsItem)
            {
                _gameGrid.GetItem(matchsItemIndex).gameObject.SetActive(false);
                _gameGrid.RemoveItem(matchsItemIndex);

                if (_gameGrid.GetItem(itemIndex) is ItemBlast itemBlast)
                {
                    _gridGenerator.RePoolObject(itemBlast, ItemCategoryType.Blast);
                }
                else if (_gameGrid.GetItem(itemIndex) is ItemObstacle itemObstacle)
                {
                    _gridGenerator.RePoolObject(itemObstacle, ItemCategoryType.Obstacle);
                }

                dropedRow.TryAdd(matchsItemIndex.x, 0);
                ++dropedRow[matchsItemIndex.x];

                dropedColumn.Add(matchsItemIndex.x);
            }

            _gridDropper.Drop(dropedColumn, dropedRow);
        }

        private HashSet<int2> ObstacleBlastCheck(HashSet<int2> matchsItem)
        {
            HashSet<int2> neighBours = new();
            HashSet<int2> blastObstacleIndex = new();

            foreach (var matchsIndex in matchsItem)
            {
                foreach (var item in _neighbourIndex)
                {
                    neighBours.Add(item + matchsIndex);
                }
            }

            foreach (var neighbour in neighBours)
            {
                if (_gameGrid.GetItem(neighbour) is ItemObstacle obstacle)
                {
                    obstacle.health--;

                    if (obstacle.health <= 0)
                    {
                        blastObstacleIndex.Add(neighbour);
                    }
                }
            }

            return blastObstacleIndex;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_worldPosition, 0.4f);
        }
    }
}
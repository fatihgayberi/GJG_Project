using System.Collections.Generic;
using GJG.GridSystem.Match;
using GJG.Items.ItemColor;
using Unity.Mathematics;
using UnityEngine;
using GJG.Items;

namespace GJG.GridSystem
{
    public class GroupChecker : MonoBehaviour
    {
        [SerializeField] private MatchCheckerBase matchCheckerBase;
        [SerializeField] private GridData gridData;
        [SerializeField] private ItemPainter painter;
        private GameGrid<ItemController> _gameGrid;

        public void Init(GameGrid<ItemController> gameGrid)
        {
            _gameGrid = gameGrid;
            matchCheckerBase.Initialize(_gameGrid);
        }

        public void Check()
        {
            int2 index;
            HashSet<int2> toRemove = new();

            for (index.x = 0; index.x < _gameGrid.Grid.GetLength(0); index.x++)
            {
                for (index.y = 0; index.y < _gameGrid.Grid.GetLength(1); index.y++)
                {
                    toRemove = matchCheckerBase.FindMatches(index, _gameGrid.GetNode(index).ColorType);

                    painter.Paint(_gameGrid.GetItem(index), _gameGrid.GetNode(index).ColorType, gridData.GetItemType(toRemove.Count));
                }
            }
        }
    }
}
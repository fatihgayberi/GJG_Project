using System.Collections.Generic;
using Unity.Mathematics;
using GJG.Global;

namespace GJG.GridSystem.Match
{
    public class MatchCheckerDFS : MatchCheckerBase
    {
        private Stack<int2> _stack;

        public MatchCheckerDFS(GameGrid grid) : base(grid)
        {
            _stack = new();
        }

        public override HashSet<int2> GetMatchesItem(int2 selectItemIndex)
        {
            selectedItem = _grid.GetNode(selectItemIndex).ItemBase;

            _stack.Clear();
            _toRemove.Clear();
            _stack.Push(selectItemIndex);

            while (_stack.Count > 0)
            {
                _currentIndex = _stack.Pop();

                if (!IsMatchable()) continue;

                _toRemove.Add(_currentIndex);

                foreach (var item in Neigbours.NeighbourIndex)
                {
                    _stack.Push(item + _currentIndex);
                }
            }

            return _toRemove;
        }
    }
}
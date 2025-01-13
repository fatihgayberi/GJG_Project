using System.Collections.Generic;
using Unity.Mathematics;
using GJG.Global;

namespace GJG.GridSystem.Match
{
    public class MatchCheckerBFS : MatchCheckerBase
    {
        private Queue<int2> _queue;

        public MatchCheckerBFS(GameGrid grid) : base(grid)
        {
            _queue = new();
        }

        public override HashSet<int2> GetMatchesItem(int2 selectItemIndex)
        {
            selectedItem = _grid.GetItem(selectItemIndex);
            if (selectedItem == null) return null;

            _queue.Clear();
            _toRemove.Clear();
            _queue.Enqueue(selectItemIndex);

            while (_queue.Count > 0)
            {
                _currentIndex = _queue.Dequeue();

                if (!IsMatchable()) continue;

                _toRemove.Add(_currentIndex);

                foreach (var item in Neigbours.NeighbourIndex)
                {
                    _queue.Enqueue(item + _currentIndex);
                }
            }

            return _toRemove;
        }
    }
}
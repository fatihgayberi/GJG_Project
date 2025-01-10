using System.Collections.Generic;
using Unity.Mathematics;

namespace GJG.GridSystem
{
    public class MatchCheckerBFS : MatchCheckerBase
    {
        private Queue<int2> _queue;
        private int2 _currentIndex;

        public override void Initialize()
        {
            _queue = new();
        }

        public override void FindMatches(int2 selectItemIndex, int2 itemUV, int colorNum, HashSet<int2> toRemove)
        {
            _queue.Clear();
            _queue.Enqueue(selectItemIndex);

            while (_queue.Count > 0)
            {
                _currentIndex = _queue.Dequeue();

                if (grid.IsValidIndex(_currentIndex)) continue;
                // if (grid.GetItem(_currentIndex).IsSame(colorNum, itemUV)) continue;
                if (toRemove.Contains(_currentIndex)) continue;

                toRemove.Add(_currentIndex);

                _queue.Enqueue(new int2(_currentIndex.x + 1, _currentIndex.y));  // Sag
                _queue.Enqueue(new int2(_currentIndex.x - 1, _currentIndex.y));  // Sol
                _queue.Enqueue(new int2(_currentIndex.x, _currentIndex.y + 1));  // Ust
                _queue.Enqueue(new int2(_currentIndex.x, _currentIndex.y - 1));  // Alt
            }
        }
    }
}
using System.Collections.Generic;
using Unity.Mathematics;

namespace GJG.GridSystem
{
    public class MatchCheckerIterative : MatchCheckerBase
    {
        private Stack<int2> _stack;
        private int2 _currentIndex;

        public override void Initialize()
        {
            _stack = new();
        }

        public override void FindMatches(int2 selectItemIndex, int2 itemUV, int colorNum, HashSet<int2> toRemove)
        {
            _stack.Clear();
            _stack.Push(selectItemIndex);

            while (_stack.Count > 0)
            {
                _currentIndex = _stack.Pop();

                if (grid.IsValidIndex(_currentIndex)) continue;
                // if (grid.GetItem(_currentIndex).IsSame(colorNum, itemUV)) continue;
                if (toRemove.Contains(_currentIndex)) continue;

                toRemove.Add(_currentIndex);

                _stack.Push(new int2(_currentIndex.x + 1, _currentIndex.y));  // Sag
                _stack.Push(new int2(_currentIndex.x - 1, _currentIndex.y));  // Sol
                _stack.Push(new int2(_currentIndex.x, _currentIndex.y + 1));  // Ust
                _stack.Push(new int2(_currentIndex.x, _currentIndex.y - 1));  // Alt
            }
        }
    }
}
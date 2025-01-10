using System.Collections.Generic;
using Unity.Mathematics;
using GJG.Items;

namespace GJG.GridSystem.Match
{
    public class MatchCheckerIterative : MatchCheckerBase
    {
        private Stack<int2> _stack;
        private int2 _currentIndex, _neighbourIndex;

        public override void Initialize(GameGrid<ItemController> grid)
        {
            base.Initialize(grid);
            _stack = new();
        }

        public override HashSet<int2> FindMatches(int2 selectItemIndex, ItemColorType colorType)
        {
            HashSet<int2> toRemove = new();

            _stack.Clear();
            _stack.Push(selectItemIndex);

            while (_stack.Count > 0)
            {
                _currentIndex = _stack.Pop();

                if (!_grid.IsValidIndex(_currentIndex)) continue;
                if (!_grid.GetNode(_currentIndex).IsSame(colorType)) continue;
                if (toRemove.Contains(_currentIndex)) continue;

                toRemove.Add(_currentIndex);

                for (_neighbourIndex.x = -1; _neighbourIndex.x <= 1; _neighbourIndex.x++)
                {
                    for (_neighbourIndex.y = -1; _neighbourIndex.y <= 1; _neighbourIndex.y++)
                    {
                        // komsulari ekliyoruz
                        _stack.Push(_neighbourIndex);
                    }
                }
            }

            return toRemove;
        }
    }
}
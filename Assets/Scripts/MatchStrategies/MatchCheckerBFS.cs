using System.Collections.Generic;
using Unity.Mathematics;
using GJG.Items;

namespace GJG.GridSystem.Match
{
    public class MatchCheckerBFS : MatchCheckerBase
    {
        private Queue<int2> _queue;
        private int2 _currentIndex, _neighbourIndex;

        public override void Initialize(GameGrid<ItemController> grid)
        {
            base.Initialize(grid);
            _queue = new();
        }

        public override HashSet<int2> FindMatches(int2 selectItemIndex, ItemColorType colorType)
        {
            HashSet<int2> toRemove = new();

            _queue.Clear();
            _queue.Enqueue(selectItemIndex);

            while (_queue.Count > 0)
            {
                _currentIndex = _queue.Dequeue();

                if (!_grid.IsValidIndex(_currentIndex)) continue;
                if (!_grid.GetNode(_currentIndex).IsSame(colorType)) continue;
                if (toRemove.Contains(_currentIndex)) continue;

                toRemove.Add(_currentIndex);

                for (_neighbourIndex.x = -1; _neighbourIndex.x <= 1; _neighbourIndex.x++)
                {
                    for (_neighbourIndex.y = -1; _neighbourIndex.y <= 1; _neighbourIndex.y++)
                    {
                        // komsulari ekliyoruz
                        _queue.Enqueue(_neighbourIndex);
                    }
                }
            }

            return toRemove;
        }
    }
}
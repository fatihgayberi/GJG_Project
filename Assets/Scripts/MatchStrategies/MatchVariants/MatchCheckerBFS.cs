using System.Collections.Generic;
using Unity.Mathematics;
using GJG.Items;

namespace GJG.GridSystem.Match
{
    public class MatchCheckerBFS : MatchCheckerBase
    {
        private Queue<int2> _queue;
        private HashSet<int2> _toRemove;
        private int2 _currentIndex;
        private ItemColorType _colorType;

        private int _counter;

        private static readonly int2[] _neighbourIndex = new int2[]
        {
            new (1, 0),
            new (-1, 0),
            new (0, 1),
            new (0, -1),
        };

        public MatchCheckerBFS(GameGrid grid) : base(grid)
        {
            _queue = new();
            _toRemove = new();
        }

        public override HashSet<int2> GetMatchesItem(int2 selectItemIndex)
        {
            _colorType = _grid.GetNode(selectItemIndex).ColorType;

            _queue.Clear();
            _toRemove.Clear();
            _queue.Enqueue(selectItemIndex);

            while (_queue.Count > 0)
            {
                _currentIndex = _queue.Dequeue();

                if (!_grid.IsValidIndex(_currentIndex)) continue;
                if (_grid.GetNode(_currentIndex).IsEmpty) continue;
                if (!_grid.GetNode(_currentIndex).IsSame(_colorType)) continue;
                if (_toRemove.Contains(_currentIndex)) continue;

                _toRemove.Add(_currentIndex);

                foreach (var item in _neighbourIndex)
                {
                    _queue.Enqueue(item + _currentIndex);
                }
            }

            return _toRemove;
        }

        public override int GetMatchesCount(int2 selectItemIndex)
        {
            _colorType = _grid.GetNode(selectItemIndex).ColorType;

            _queue.Clear();
            _queue.Enqueue(selectItemIndex);

            _counter = 0;

            while (_queue.Count > 0)
            {
                _currentIndex = _queue.Dequeue();

                if (!_grid.IsValidIndex(_currentIndex)) continue;
                if (!_grid.GetNode(_currentIndex).IsSame(_colorType)) continue;

                _counter++;

                foreach (var item in _neighbourIndex)
                {
                    _queue.Enqueue(item + _currentIndex);
                }
            }

            return _counter;
        }
    }
}
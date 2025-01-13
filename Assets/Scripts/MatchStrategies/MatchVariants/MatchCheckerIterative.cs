using System.Collections.Generic;
using Unity.Mathematics;
using GJG.Items;

namespace GJG.GridSystem.Match
{
    public class MatchCheckerIterative : MatchCheckerBase
    {
        private Stack<int2> _stack;
        private HashSet<int2> _toRemove;
        private int2 _currentIndex;
        private ItemBase selectedItem;

        private int _counter;

        private static readonly int2[] _neighbourIndex = new int2[]
        {
            new (1, 0),
            new (-1, 0),
            new (0, 1),
            new (0, -1),
        };

        public MatchCheckerIterative(GameGrid grid) : base(grid)
        {
            _stack = new();
            _toRemove = new();
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

                if (!_grid.IsValidIndex(_currentIndex)) continue;
                if (_grid.GetNode(_currentIndex).IsEmpty) continue;

                if (_grid.GetItem(_currentIndex) is ItemBlast itemBlast)
                {
                    if (!itemBlast.canMatch) continue;
                }
                else
                {
                    continue;
                }

                if (!_grid.GetNode(_currentIndex).IsSame(selectedItem)) continue;
                if (_toRemove.Contains(_currentIndex)) continue;

                _toRemove.Add(_currentIndex);

                foreach (var item in _neighbourIndex)
                {
                    _stack.Push(item + _currentIndex);
                }
            }

            return _toRemove;
        }

        public override int GetMatchesCount(int2 selectItemIndex)
        {
            selectedItem = _grid.GetNode(selectItemIndex).ItemBase;

            _stack.Clear();
            _stack.Push(selectItemIndex);

            _counter = 0;

            while (_stack.Count > 0)
            {
                _currentIndex = _stack.Pop();

                if (!_grid.IsValidIndex(_currentIndex)) continue;
                if (!_grid.GetNode(_currentIndex).IsSame(selectedItem)) continue;

                _counter++;

                foreach (var item in _neighbourIndex)
                {
                    _stack.Push(item + _currentIndex);
                }
            }

            return _counter;
        }
    }
}
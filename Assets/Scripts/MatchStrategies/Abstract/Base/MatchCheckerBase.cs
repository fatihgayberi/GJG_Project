using System.Collections.Generic;
using Unity.Mathematics;
using GJG.Items;

namespace GJG.GridSystem.Match
{
    public abstract class MatchCheckerBase
    {
        protected GameGrid _grid;
        protected HashSet<int2> _toRemove;
        protected ItemBase selectedItem;
        protected int2 _currentIndex;

        public abstract HashSet<int2> GetMatchesItem(int2 selectItemIndex);

        public MatchCheckerBase(GameGrid grid)
        {
            _grid = grid;
            _toRemove = new();
        }

        protected bool IsMatchable()
        {
            if (!_grid.IsValidIndex(_currentIndex)) return false;
            if (_grid.GetNode(_currentIndex).IsEmpty) return false;
            if (_grid.GetItem(_currentIndex) is not IMatchableItem matchableItem) return false;
            if (!matchableItem.CanMatch) return false;
            if (!_grid.GetNode(_currentIndex).IsSame(selectedItem)) return false;
            if (_toRemove.Contains(_currentIndex)) return false;

            return true;
        }
    }
}
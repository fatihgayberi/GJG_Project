using System.Collections.Generic;
using GJG.Items;
using Unity.Mathematics;
using UnityEngine;

namespace GJG.GridSystem.Match
{
    public abstract class MatchCheckerBase
    {
        protected GameGrid _grid;

        public abstract HashSet<int2> GetMatchesItem(int2 selectItemIndex);
        public abstract int GetMatchesCount(int2 selectItemIndex);

        public MatchCheckerBase(GameGrid grid)
        {
            _grid = grid;
        }
    }
}
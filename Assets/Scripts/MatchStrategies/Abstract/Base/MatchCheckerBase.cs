using System.Collections.Generic;
using GJG.Items;
using Unity.Mathematics;
using UnityEngine;

namespace GJG.GridSystem.Match
{
    public abstract class MatchCheckerBase : MonoBehaviour
    {
        protected GameGrid<ItemController> _grid;

        public abstract HashSet<int2> FindMatches(int2 selectItemIndex, ItemColorType colorType);

        public virtual void Initialize(GameGrid<ItemController> grid)
        {
            _grid = grid;
        }
    }
}
using System.Collections.Generic;
using GJG.Items;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace GJG.GridSystem
{
    public abstract class MatchCheckerBase : MonoBehaviour, IInitializable
    {
        [SerializeField] protected GameGrid<ItemController> grid;

        public abstract void Initialize();
        public abstract void FindMatches(int2 selectItemIndex, int2 itemUV, int colorNum, HashSet<int2> toRemove);
    }
}
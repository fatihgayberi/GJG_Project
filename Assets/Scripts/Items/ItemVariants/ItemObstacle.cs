using UnityEngine;

namespace GJG.Items
{
    public class ItemObstacle : ItemBase, IBreakableItem
    {
        [SerializeField] public int health = 2;

        public int Health { get => health; set => health = value; }
    }
}
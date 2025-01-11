using UnityEngine;

namespace GJG.Items
{
    public abstract class ItemBase : MonoBehaviour, IPaintable
    {
        [SerializeField] protected Renderer itemRenderer;

        public Renderer Renderer => itemRenderer;
    }
}
using UnityEngine;

namespace GJG.Items
{
    public abstract class ItemBase : MonoBehaviour, IPaintable
    {
        [SerializeField] protected Renderer itemRenderer;
        private ItemColorType _colorType;

        public Renderer Renderer => itemRenderer;

        public ItemColorType ColorType { get => _colorType; set => _colorType = value; }

        public virtual bool IsSame(ItemBase itemBase)
        {
            return itemBase._colorType == _colorType;
        }
    }
}
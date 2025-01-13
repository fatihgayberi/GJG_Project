using UnityEngine;

namespace GJG.Items
{
    public abstract class ItemBase : MonoBehaviour, IPaintable
    {
        [SerializeField] private ItemCategoryType itemCategory;
        [SerializeField] protected Renderer itemRenderer;

        public Renderer Renderer => itemRenderer;
        public ItemCategoryType ItemCategory => itemCategory;

        private ItemColorType _colorType;
        public ItemColorType ColorType { get => _colorType; set => _colorType = value; }

        public virtual bool IsSame(ItemBase itemBase)
        {
            return itemBase._colorType == _colorType;
        }
    }
}
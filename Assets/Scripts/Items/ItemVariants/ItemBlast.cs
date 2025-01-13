namespace GJG.Items
{
    public class ItemBlast : ItemBase, ISellectableItem, IBlastableItem, IMatchableItem
    {
        private bool _canSelect;
        public bool CanSelect { get => _canSelect; set => _canSelect = value; }

        private bool _canMatch = true;
        public bool CanMatch { get => _canMatch; set => _canMatch = value; }
    }
}
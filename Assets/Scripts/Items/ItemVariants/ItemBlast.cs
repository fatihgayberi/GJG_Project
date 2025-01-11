namespace GJG.Items
{
    public class ItemBlast : ItemBase, ISellectableItem, IBlastableItem
    {
        private bool _canSelect;

        public bool CanSelect => _canSelect;

        public void Blast()
        {

        }
    }
}
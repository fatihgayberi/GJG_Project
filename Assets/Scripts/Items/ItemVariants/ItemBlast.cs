namespace GJG.Items
{
    public class ItemBlast : ItemBase, ISellectableItem, IBlastableItem
    {
        private bool _canSelect;

        public bool canMatch = true;

        public bool CanSelect { get => _canSelect; set => _canSelect = value; }

        public override void AddedGrid()
        {

        }

        public override void RemovedGrid()
        {

        }

        public void Blast()
        {

        }
    }
}
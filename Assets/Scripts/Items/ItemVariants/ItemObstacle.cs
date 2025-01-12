namespace GJG.Items
{
    public class ItemObstacle : ItemBase
    {
        public int health = 0;

        public override void AddedGrid()
        {
            health = 2;
        }

        public override void RemovedGrid()
        {

        }
    }
}
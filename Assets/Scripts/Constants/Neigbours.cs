using Unity.Mathematics;

namespace GJG.Global
{
    public static class Neigbours
    {
        public static readonly int2[] NeighbourIndex = new int2[]
        {
            new (1, 0),
            new (-1, 0),
            new (0, 1),
            new (0, -1),
        };
    }
}
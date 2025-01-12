using UnityEngine;

namespace GJG.Items
{
    public interface IPaintable
    {
        ItemColorType ColorType { get; set; }
        Renderer Renderer { get; }
    }
}
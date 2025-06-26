using Microsoft.Xna.Framework;

namespace Squence.Core.Interfaces
{
    internal interface ICollidable
    {
        Vector2 Center { get; }
        float Radius { get; }
    }
}

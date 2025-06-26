using Microsoft.Xna.Framework;
using System;

namespace Squence.Core.Interfaces
{
    internal interface IRenderable
    {
        Guid Guid { get; }
        string TextureName { get; }
        Vector2 TexturePosition { get; }
        int TextureWidth { get; }
        int TextureHeight { get; }

        float Rotation { get => 0f; }
    }
}

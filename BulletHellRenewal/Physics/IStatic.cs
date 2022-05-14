using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Physics
{
    public interface IStatic : ICollisionActor
    {
        public bool DebugCollisionShape { get; set; }
        public Color DebugColor { get; set; }
        public void Draw(SpriteBatch spriteBatch);
    }
}

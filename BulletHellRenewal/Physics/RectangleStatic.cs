using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Physics
{
    public class RectangleStatic : IStatic
    {
        public bool DebugCollisionShape { get; set; }
        public Color DebugColor { get; set; }

        public IShapeF Bounds { get; }

        public RectangleStatic(Vector2 position, Vector2 size)
        {
            Bounds = new RectangleF(position - (size / 2), size);
            DebugColor = Color.Green;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (DebugCollisionShape)
                spriteBatch.DrawRectangle((RectangleF)Bounds, DebugColor, 3);
        }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            DebugColor = Color.AliceBlue;
        }
    }
}

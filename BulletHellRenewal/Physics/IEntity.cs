using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Physics
{
    public interface IEntity : ICollisionActor
    {
        public bool DebugCollisionShape { get; set; }
        public Color DebugColor { get; set; }
        public bool GravityExempt { get; set; }
        public bool Remove { get; }
        public void Update(GameTime time);
        public void Draw(SpriteBatch spriteBatch);
        public void ApplyGravity(Vector2 gravity);
    }
}

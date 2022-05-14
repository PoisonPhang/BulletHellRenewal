using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Physics
{
    public class CircleEntity : IEntity
    {
        public IShapeF Bounds { get; }
        public bool DebugCollisionShape { get; set; }
        public Color DebugColor { get; set; }
        public bool GravityExempt { get; set; }
        public bool Remove { get; protected set; }
        public Vector2 Velocity { get; protected set; }
        public Vector2 Acceleration { get; protected set; }
        public float Mass { get; protected set; }

        protected readonly Game _game;

        public CircleEntity(Game game, Vector2 position, float radius)
        {
            _game = game;
            Bounds = new CircleF(position, radius);
            Mass = ((CircleF)Bounds).Diameter;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (DebugCollisionShape)
                spriteBatch.DrawCircle((CircleF)Bounds, 16, Color.Red, 3);
        }

        public virtual void Update(GameTime time)
        {
            Velocity += Acceleration * time.GetElapsedSeconds();
            Bounds.Position += Velocity * time.GetElapsedSeconds();
        }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            Bounds.Position -= collisionInfo.PenetrationVector * 1.1f;
        }

        public void ApplyGravity(Vector2 gravity)
        {
            if (!GravityExempt)
                Velocity += gravity;
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Physics
{
    public class RectangleEntity : IEntity
    {
        public IShapeF Bounds { get; }
        public Vector2 Size { get; }
        public bool DebugCollisionShape { get; set; }
        public Color DebugColor { get; set; }
        public bool GravityExempt { get; set; }
        public bool Remove { get; protected set; }
        public Vector2 Acceleration { get; set; }
        public Vector2 Velocity { get; protected set; }
        public bool IsGrounded { get; protected set; }
        public float Mass { get; protected set; }

        protected readonly Game _game;

        public RectangleEntity(Game game, Vector2 position, Vector2 size)
        {
            _game = game;
            Size = size;
            Bounds = new RectangleF(position - (Size / 2), Size);
            Mass = ((RectangleF)Bounds).Height;
            DebugColor = Color.Red;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (DebugCollisionShape)
                spriteBatch.DrawRectangle((RectangleF)Bounds, DebugColor, 3);
        }

        public virtual void Update(GameTime time)
        {
            if (!IsGrounded)
            {
                Acceleration = new Vector2(Acceleration.X * 0.75f, Acceleration.Y);
            }

            Velocity += Acceleration * time.GetElapsedSeconds();

            if (IsGrounded && Math.Abs(Acceleration.X) == 0)
                if (Math.Abs(Velocity.X) > 1)
                    Velocity = new Vector2(Velocity.X * 0.4f, Velocity.Y);
                else
                    Velocity = new Vector2(0, Velocity.Y);

            Velocity = new Vector2(MathHelper.Clamp(Velocity.X, -Mass * 8, Mass * 8), Velocity.Y);

            Bounds.Position += Velocity * time.GetElapsedSeconds();

            IsGrounded = false;
        }

        public virtual void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (collisionInfo.PenetrationVector.Y > 0)
                IsGrounded = true;
            if (collisionInfo.PenetrationVector.Y < 0)
                ApplyGravity(new Vector2(0, -collisionInfo.PenetrationVector.Y));

            Bounds.Position -= collisionInfo.PenetrationVector * 1.1f;
            Acceleration *= -1;
        }

        public void ApplyGravity(Vector2 gravity)
        {
            if (!GravityExempt && !IsGrounded)
                Velocity += gravity * Mass;
            else if (!GravityExempt && IsGrounded)
                Velocity = new Vector2(Velocity.X, 0);
        }
    }
}

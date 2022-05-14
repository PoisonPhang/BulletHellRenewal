using BulletHellRenewal.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Entities
{
    public class Bullet : CircleEntity
    {
        public bool Collided;
        public bool IsPlayers;

        private Vector2 _targetVelocity;

        public Bullet(Game game, Vector2 origin, Vector2 target, float speed, float radius, bool isPlayers) : base(game, origin, radius)
        {
            IsPlayers = isPlayers;
            _targetVelocity = target - origin;
            _targetVelocity.Normalize();
            _targetVelocity *= speed;
            GravityExempt = true;
        }

        public override void Update(GameTime time)
        {
            Bounds.Position += _targetVelocity;
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            Collided = true;
            Remove = true;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 size)
        {
            spriteBatch.Draw(texture, Bounds.Position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            base.Draw(spriteBatch);
        }
    }
}

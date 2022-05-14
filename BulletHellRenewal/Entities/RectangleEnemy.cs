using BulletHellRenewal.Particles;
using BulletHellRenewal.Physics;
using BulletHellRenewal.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Entities
{
    public class RectangleEnemy : RectangleEntity, IEnemy
    {
        public Vector2 Target { get; set; }
        public EnemyGas Particles { get; protected set; }
        public uint HitPoints { get; protected set; }

        private BasicBulletManager _bulletManager;
        private float _offset;
        private float _speed;
        private Vector2 _size;
        private Color _color;
        private World _world;
        private SoundEffect _sound;


        public RectangleEnemy(Game game, World world, Vector2 size, float speed) : base(game, Vector2.Zero, size)
        {
            GravityExempt = true;
            HitPoints = 5;
            _bulletManager = new BasicBulletManager(game, world, "bullet", 16, new Vector2(6, 6), false);
            _world = world;
            _speed = speed;
            _size = size;
            _color = Color.Green;

            _offset = size.Y * 1.5f;

            Particles = new EnemyGas(_game, Bounds.Position, Color.Green, Color.Purple);
        }

        public void LoadContent(Vector2 position)
        {
            Bounds.Position = (position - _size / 2) - new Vector2(0, _size.Y * 24);
            _bulletManager.LoadContent();
            _sound = _game.Content.Load<SoundEffect>("shoot");
            Particles.LoadContent();
        }

        public override void Update(GameTime time)
        {
            _bulletManager.Update(time);

            if (Math.Sin(time.TotalGameTime.TotalSeconds * 10) >= 0.99)
            {
                _bulletManager.SpawnBullet(new Vector2(Bounds.Position.X, Bounds.Position.Y + _offset), Target);
            }

            float bob = (float)Math.Sin(time.TotalGameTime.TotalSeconds * 5) * _speed;
            Acceleration = new Vector2(Target.X - Bounds.Position.X, Acceleration.Y);
            Acceleration.Normalize();
            Acceleration *= _speed;

            Bounds.Position += new Vector2(-bob, bob) * time.GetElapsedSeconds();
            Bounds.Position = new Vector2(MathHelper.Clamp(Bounds.Position.X, 0, 1024), MathHelper.Clamp(Bounds.Position.Y, 0, 400));

            Particles.Update(time, Bounds.Position + _size / 2, -Velocity);

            base.Update(time);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle((RectangleF)Bounds, _color, thickness: 3);
            _bulletManager.Draw(spriteBatch);
            _color = Color.Green;
            base.Draw(spriteBatch);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (_color == Color.Green && collisionInfo.Other.GetType() == typeof(Bullet))
            {
                if ((collisionInfo.Other as Bullet).IsPlayers)
                {
                    _sound.Play();
                    _color = Color.White;
                    HitPoints--;
                }
            }


            base.OnCollision(collisionInfo);
        }

        public void Dispose()
        {
            _world.RemoveEntity(this);
            Particles.Dispose();
        }
    }
}

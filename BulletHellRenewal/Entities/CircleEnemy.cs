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
    public class CircleEnemy : CircleEntity, IEnemy
    {
        public Vector2 Target { get; set; }
        public EnemyGas Particles { get; protected set; }
        public uint HitPoints { get; protected set; }

        private BasicBulletManager _bulletManager;
        private uint _bulletIndex;
        private Dictionary<uint, Vector2> _shootOrigin;
        private float _speed;
        private Color _color;
        private World _world;
        private SoundEffect _sound;


        public CircleEnemy(Game game, World world, float radius, float speed) : base(game, Vector2.Zero, radius)
        {
            GravityExempt = true;
            HitPoints = 5;
            _bulletManager = new BasicBulletManager(game, world, "bullet", 16, new Vector2(6, 6), false);
            _shootOrigin = new Dictionary<uint, Vector2>();
            _world = world;
            _speed = speed;
            _color = Color.Blue;

            float offset = radius * 1.5f;

            _shootOrigin.Add(0, new Vector2(-offset, 0));
            _shootOrigin.Add(1, new Vector2(offset, 0));
            _shootOrigin.Add(2, new Vector2(0, -offset));
            _shootOrigin.Add(3, new Vector2(0, offset));
            Particles = new EnemyGas(_game, Bounds.Position, Color.Blue, Color.White);
        }

        public void LoadContent(Vector2 position)
        {
            Bounds.Position = position;
            _bulletManager.LoadContent();
            _sound = _game.Content.Load<SoundEffect>("shoot");
            Particles.LoadContent();
        }

        public override void Update(GameTime time)
        {
            _bulletManager.Update(time);

            if (Math.Sin(time.TotalGameTime.TotalSeconds * 10) >= 0.999)
            {
                _shootOrigin.TryGetValue(_bulletIndex, out Vector2 offset);
                _bulletManager.SpawnBullet(Bounds.Position + offset, Bounds.Position + offset * 2);
                _bulletIndex++;
            }

            if (_bulletIndex == 4)
                _bulletIndex = 0;

            float bob = (float)Math.Sin(time.TotalGameTime.TotalSeconds * 5) * _speed;
            Acceleration = new Vector2(Acceleration.X, Target.Y - Bounds.Position.Y);
            Acceleration.Normalize();
            Acceleration *= _speed;

            Bounds.Position += new Vector2(-bob, bob) * time.GetElapsedSeconds();
            Bounds.Position = new Vector2(MathHelper.Clamp(Bounds.Position.X, 0, 1024), MathHelper.Clamp(Bounds.Position.Y, 0, 400));

            Particles.Update(time, Bounds.Position, -Velocity);

            base.Update(time);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle((CircleF)Bounds, 32, _color, thickness: 3);
            _bulletManager.Draw(spriteBatch);
            _color = Color.Blue;
            base.Draw(spriteBatch);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (_color == Color.Blue && collisionInfo.Other.GetType() == typeof(Bullet))
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

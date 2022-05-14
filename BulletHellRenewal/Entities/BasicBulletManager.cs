using BulletHellRenewal.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Entities
{
    public class BasicBulletManager
    {
        public readonly bool IsPlayerSource;

        private readonly string _assetName;
        private List<Bullet> _bullets;
        private readonly Game _game;
        private readonly World _world;
        private float _speed;
        private Vector2 _size;
        private Texture2D _texture;

        public BasicBulletManager(Game game, World world, string assetName, float speed, Vector2 size, bool isPlayerSource)
        {
            IsPlayerSource = isPlayerSource;
            _bullets = new List<Bullet>();
            _game = game;
            _world = world;
            _assetName = assetName;
            _speed = speed;
            _size = size;
        }

        public void SpawnBullet(Vector2 origin, Vector2 target)
        {
            Bullet bullet = new Bullet(_game, origin, target, _speed, _size.X / 2, IsPlayerSource);
            _world.InsertEntity(bullet);
            _bullets.Add(bullet);
        }

        public void LoadContent()
        {
            _texture = _game.Content.Load<Texture2D>(_assetName);
        }

        public void Update(GameTime time)
        {
            List<Bullet> bullets = new List<Bullet>();

            foreach (Bullet bullet in _bullets)
            {
                bullet.Update(time);

                if (!bullet.Collided)
                    bullets.Add(bullet);
            }

            _bullets = bullets;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Bullet bullet in _bullets)
                bullet.Draw(spriteBatch, _texture, _size);
        }
    }
}

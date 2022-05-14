using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

using BulletHellRenewal.Controlls;
using BulletHellRenewal.Map;
using BulletHellRenewal.Physics;
using Microsoft.Xna.Framework.Audio;

namespace BulletHellRenewal.Entities
{
    public class PlayerCharacter : RectangleEntity
    {
        public uint HitPoints { get; protected set; }
        public bool HasShot { get; private set; }
        private BasicBulletManager _basicBulletManager;
        private IController _controller;
        private float _speed;
        private float _jump;
        private SoundEffect _sound;
        private Color _color;

        public Vector2 Target { get; private set; }

        public PlayerCharacter(Game game, World world, Vector2 size, float speed, float jump, IController controller) : base(game, size, size)
        {
            HitPoints = 3;
            _controller = controller;
            _speed = speed * ((RectangleF)Bounds).Width;
            _jump = (float)Math.Pow(jump, 2.3) * ((RectangleF)Bounds).Height;
            _color = Color.Red;
            _basicBulletManager = new BasicBulletManager(game, world, "bullet", 16, new Vector2(6, 6), true);
        }

        public void LoadContent(Vector2 position)
        {
            Bounds.Position = position;
            _sound = _game.Content.Load<SoundEffect>("Hit_Hurt");
            _basicBulletManager.LoadContent();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _basicBulletManager.Draw(spriteBatch);
            spriteBatch.DrawRectangle((RectangleF)Bounds, DebugColor, 3);
            spriteBatch.DrawCircle(new CircleF(Target, 8f), 8, Color.Red, thickness: 3);

            _color = Color.Red;
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime time)
        {
            _controller.Update();
            Target = _controller.Target;

            _basicBulletManager.Update(time);
            SetAcceleration();

            if (_controller.TriggerShoot)
            {
                HasShot = true;
                var origin = GetFixedDistanceOrigin(Bounds.Position + Size / 2, _controller.Target, Size.X * 0.75f);
                _basicBulletManager.SpawnBullet(origin, _controller.Target);
            }

            base.Update(time);
        }

        public override void OnCollision(CollisionEventArgs collisionInfo)
        {
            if (_color == Color.Red && collisionInfo.Other.GetType() == typeof(Bullet))
            {
                if (!(collisionInfo.Other as Bullet).IsPlayers)
                {
                    _sound.Play();
                    _color = Color.White;
                    HitPoints--;
                }
            }
            base.OnCollision(collisionInfo);
        }

        private Vector2 GetFixedDistanceOrigin(Vector2 p1, Vector2 p2, float d)
        {
            float dX = p2.X - p1.X;
            float dY = p2.Y - p1.Y;
            float D = d / (float)Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));

            return new Vector2(p1.X + D * dX, p1.Y + D * dY);
        }

        private void SetAcceleration()
        {
            float velocityX = _controller.Direction.X * _speed;
            float velocityY;

            if (IsGrounded && _controller.Direction.Y < 0)
                velocityY = _controller.Direction.Y * _jump;
            else
                velocityY = 0;

            Acceleration = new Vector2(velocityX, velocityY);
        }
    }
}

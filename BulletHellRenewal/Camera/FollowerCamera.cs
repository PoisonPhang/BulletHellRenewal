using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Camera
{
    public class FollowerCamera
    {
        public Vector2 Position { get; private set; }
        public Vector2 Target { get; private set; } = Vector2.UnitY;
        public Vector2 CameraSpeed { get; private set; }

        private OrthographicCamera _camera;
        private readonly Game _game;
        private float _leftBound;
        private float _rightBound;

        public FollowerCamera(Game game, Vector2 speed)
        {
            _game = game;
            CameraSpeed = speed;
        }

        public FollowerCamera(Game game, Vector2 speed, float leftBound, float rightBound)
        {
            _game = game;
            CameraSpeed = speed;
            _leftBound = leftBound + game.GraphicsDevice.Viewport.Width / 2;
            _rightBound = rightBound - game.GraphicsDevice.Viewport.Width / 2;
        }

        public Matrix GetViewMatrix()
        {
            return _camera.GetViewMatrix();
        }

        public void Initialize()
        {
            ViewportAdapter viewportAdapter = new BoxingViewportAdapter(
               _game.Window,
               _game.GraphicsDevice,
               _game.GraphicsDevice.Viewport.Width,
               _game.GraphicsDevice.Viewport.Height
           );

            _camera = new OrthographicCamera(viewportAdapter);

        }

        public void Update(Vector2 position)
        {
            float posX = Position.X, posY = 200;
            float offsetX = position.X - _camera.BoundingRectangle.X;
            float offsetY = position.Y - _camera.BoundingRectangle.Y;

            if (offsetX < _camera.BoundingRectangle.Width * 0.25 || offsetX > _camera.BoundingRectangle.Width * 0.75)
            {
                posX = MathHelper.Clamp(position.X, _leftBound, _rightBound);
                Target = new Vector2(posX, posY);
            }

            UpdatePosition();

            _camera.LookAt(new Vector2(Position.X, 200));
        }

        private void UpdatePosition()
        {
            if (Target != Vector2.UnitY)
            {
                Vector2 offset = Target - Position;
                offset.Normalize();

                Position += new Vector2(offset.X * CameraSpeed.X, 0);

                if (Math.Abs(Target.X - Position.X) < _camera.BoundingRectangle.X * 0.1)
                    Target = Vector2.UnitY;
            }
        }
    }
}

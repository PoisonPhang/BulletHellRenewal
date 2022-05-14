using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Controlls
{
    public class KeyboardController : IController
    {
        public Vector2 Direction { get; private set; }
        public Vector2 Target { get; private set; }
        public bool TriggerShoot { get; private set; }

        private int _centerX;
        private int _centerY;
        private Vector2 _center;


        public KeyboardController(float screenWidth, float screenHeight)
        {
            _center = new Vector2(screenWidth / 2, screenHeight / 2);
            _centerX = (int)_center.X;
            _centerY = (int)_center.Y;
        }

        private KeyboardState _currKeyboardState;
        private KeyboardState _lastKeyboardState;
        private MouseState _currMouseState;
        private MouseState _lastMouseState;
        public void Update()
        {
            Vector2 direction = Vector2.Zero;
            TriggerShoot = false;

            _lastKeyboardState = _currKeyboardState;
            _currKeyboardState = Keyboard.GetState();
            _lastMouseState = _currMouseState;
            _currMouseState = Mouse.GetState();

            if (_currKeyboardState.IsKeyDown(Keys.A) || _currKeyboardState.IsKeyDown(Keys.Left))
                direction -= Vector2.UnitX;

            if (_currKeyboardState.IsKeyDown(Keys.D) || _currKeyboardState.IsKeyDown(Keys.Right))
                direction += Vector2.UnitX;

            if (_currKeyboardState.IsKeyDown(Keys.Space))
                direction -= Vector2.UnitY;

            if (direction != Vector2.Zero)
                direction.Normalize();

            Target += _currMouseState.Position.ToVector2() - _center;
            Target = new Vector2(MathHelper.Clamp(Target.X, 0, 1120), MathHelper.Clamp(Target.Y, -30, 432));

            if (_currMouseState.LeftButton == ButtonState.Pressed && _lastMouseState.LeftButton == ButtonState.Released)
            {
                TriggerShoot = true;
            }

            Direction = direction;

            Mouse.SetPosition(_centerX, _centerY);
        }
    }
}

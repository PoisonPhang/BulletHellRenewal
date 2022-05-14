using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal
{
    public class Title
    {
        private const int WIDTH = 258;
        private const int HEIGHT = 64;
        private const int FRAMES = 3;

        private Texture2D _texture;
        private double _animationTimer;
        private short _animationFrame;
        private float _windowWidth;
        private float _windowHeight;
        private Vector2 _sourceVec;

        public Title(float windowWidth, float windowHeight)
        {
            _windowWidth = windowWidth;
            _windowHeight = windowHeight;
            _sourceVec = new Vector2((windowWidth / 2) - (WIDTH / 2), windowHeight / 6);
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("BH_Title");
        }

        public void Draw(SpriteBatch spriteBatch, GameTime time)
        {
            _animationTimer += time.GetElapsedSeconds();

            if (_animationTimer > 0.7)
            {
                if (_animationFrame++ > FRAMES) _animationFrame = 0;
                _animationTimer = 0;
            }

            var source = new Rectangle(_animationFrame * WIDTH, 0, WIDTH, HEIGHT);

            spriteBatch.Draw(_texture, _sourceVec, source, Color.White);
        }
    }
}

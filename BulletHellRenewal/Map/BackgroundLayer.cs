using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Map
{
    public class BackgroundLayer
    {
        public Vector2 Size;
        private string _assetName;
        private Vector2 _position;
        private Vector2 _scale;
        private Texture2D _texture;

        public BackgroundLayer(string assetName, Vector2 position, Vector2 scale)
        {
            _assetName = assetName;
            _position = position;
            _scale = scale;
        }

        public BackgroundLayer(string assetName, Vector2 scale)
        {
            _assetName = assetName;
            _scale = scale;
            _position = Vector2.Zero;
        }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_assetName);
            Size = new Vector2(_texture.Width, _texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Map
{
    public class BackgroundRenderer
    {
        private BackgroundLayer[] _backgroundLayers;
        private Vector2 _position;

        public BackgroundRenderer(BackgroundLayer[] backgroundLayers)
        {
            _backgroundLayers = backgroundLayers;
        }

        public void Update(Vector2 position)
        {
            _position = position;
        }

        public void LoadContent(ContentManager content)
        {
            foreach (BackgroundLayer layer in _backgroundLayers)
                layer.LoadContent(content);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Matrix transform;
            float offset;

            for (int i = 0; i < _backgroundLayers.Length; i++)
            {
                offset = -1 * MathHelper.Clamp(_position.X, 0, _backgroundLayers[i].Size.X);
                transform = Matrix.CreateTranslation(offset * i / _backgroundLayers.Length, 0, 0);

                spriteBatch.Begin(transformMatrix: transform);
                _backgroundLayers[i].Draw(spriteBatch);
                spriteBatch.End();
            }
        }

    }
}

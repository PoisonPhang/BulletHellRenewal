using System;
using System.Collections.Generic;
using System.Text;
using MonoGame.Extended;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using BulletHellRenewal.Physics;

namespace BulletHellRenewal.Map
{
    public class LevelMap
    {
        public List<Vector2> SpawnPoints;

        private readonly Game _game;
        private readonly string _mapName;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        public LevelMap(Game game, string mapName)
        {
            SpawnPoints = new List<Vector2>();

            _game = game;
            _mapName = mapName;
        }

        public void LoadContent()
        {
            _tiledMap = _game.Content.Load<TiledMap>(_mapName);
            _tiledMapRenderer = new TiledMapRenderer(_game.GraphicsDevice, _tiledMap);

            TiledMapObject[] spawns = _tiledMap.GetLayer<TiledMapObjectLayer>("spawns").Objects;

            foreach (TiledMapRectangleObject spawn in spawns)
            {
                SpawnPoints.Add(spawn.Position);
            }
        }

        public void Update(GameTime time)
        {
            _tiledMapRenderer.Update(time);
        }

        public void Draw(Matrix viewMatrix)
        {
            _tiledMapRenderer.Draw(viewMatrix);
        }

        public void LoadCollision(World world)
        {
            TiledMapTileLayer layer = _tiledMap.GetLayer<TiledMapTileLayer>("ground");
            Vector2 position;
            int uX = layer.TileWidth;
            int uY = layer.TileHeight;

            foreach (TiledMapTile tile in layer.Tiles)
            {
                // Skip blank tiles
                if (tile.IsBlank)
                    continue;

                position = new Vector2(tile.X * uX + uX / 2, tile.Y * uY + uY / 2);

                RectangleStatic body = new RectangleStatic(position, new Vector2(uX, uY));

                world.InsertStatic(body);
            }
        }
    }
}

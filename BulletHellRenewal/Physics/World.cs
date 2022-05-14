using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Physics
{
    public class World
    {
        private CollisionComponent _collisionComponent;
        private List<IEntity> _entities;
        private List<IStatic> _statics;
        private Vector2 _gravity;
        private float _width;
        private float _height;

        public World(float width, float height, Vector2 gravity)
        {
            _width = width;
            _height = height;
            _gravity = gravity;
            _collisionComponent = new CollisionComponent(new RectangleF(0, 0, _width, _height));
            _entities = new List<IEntity>();
            _statics = new List<IStatic>();
        }

        public void Update(GameTime time)
        {
            _collisionComponent.Update(time);
            List<IEntity> entities = new List<IEntity>();

            foreach (IEntity entity in _entities)
            {
                entity.ApplyGravity(_gravity * time.GetElapsedSeconds());
                if (entity.Remove)
                    _collisionComponent.Remove(entity);
                else
                    entities.Add(entity);
            }

            _entities = entities;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (RectangleStatic rec in _statics)
                rec.Draw(spriteBatch);
        }

        public void InsertEntity(IEntity entity)
        {
            _entities.Add(entity);
            _collisionComponent.Insert(entity);
        }

        public void InsertStatic(IStatic @static)
        {
            _statics.Add(@static);
            _collisionComponent.Insert(@static);
        }

        public void RemoveEntity(IEntity entity)
        {
            _collisionComponent.Remove(entity);
            _entities.Remove(entity);
        }
    }
}

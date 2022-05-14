using BulletHellRenewal.Particles;
using BulletHellRenewal.Physics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Entities
{
    public interface IEnemy : IDisposable, IEntity
    {
        public uint HitPoints { get; }
        public Vector2 Target { get; set; }
        public EnemyGas Particles { get; }
    }
}

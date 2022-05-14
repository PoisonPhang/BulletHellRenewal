using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulletHellRenewal.Controlls
{
    public interface IController
    {
        public Vector2 Direction { get; }
        public bool TriggerShoot { get; }
        public Vector2 Target { get; }

        public void Update();
    }
}

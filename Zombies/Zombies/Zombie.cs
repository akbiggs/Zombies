using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombies
{
    public class Zombie : GameObject
    {
        const float MAX_SPEED_X = 6f;
        const int SIZE_X = 50;
        const int SIZE_Y = 30;
        const int HEALTH = 1;

        public Zombie(World world, Vector2 position)
            : base(world, position, new Vector2(-MAX_SPEED_X, 0), new Vector2(SIZE_X, SIZE_Y), TextureBin.Get("Pixel"), true, true, HEALTH)
        {
            Color = Color.Red;
        }

        public override void Update()
        {
            base.Update();
            if (Health <= 0)
                world.Zombies.BufferRemove(this);
        }
    }
}

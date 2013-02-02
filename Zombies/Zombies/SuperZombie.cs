using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombies
{
    public class SuperZombie : Zombie
    {
        public SuperZombie(World world, Vector2 position)
            : base(world, position)
        {
            Size *= 2;
        }
    }
}

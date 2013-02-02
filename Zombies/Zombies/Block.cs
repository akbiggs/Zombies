using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombies
{
    public class Block : GameObject
    {
        public Block(World world, Vector2 position, Vector2 size)
            : base(world, position, Vector2.Zero, size, TextureBin.Get("Pixel"), false, false)
        {

        }

        public override void Update()
        {
            base.Update();

            if (world.Player.Position.X - Right > Engine.ScreenResolution.X / 2)
                world.Blocks.BufferRemove(this);
        }
    }
}

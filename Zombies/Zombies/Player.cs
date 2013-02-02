using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombies
{
    public class Player : GameObject
    {
        const float MAX_SPEED_X = 5.5f;
        const float MAX_SPEED_Y = 20f;

        const int SIZE_X = 30;
        const int SIZE_Y = 50;
        public Player(World world, Vector2 position)
            : base(world, position, new Vector2(MAX_SPEED_X, 0), new Vector2(SIZE_X, SIZE_Y), TextureBin.Get("Pixel"), true)
        {

        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch spr)
        {
            base.Draw(spr);
        }

        public virtual void Die()
        {

        }
    }
}

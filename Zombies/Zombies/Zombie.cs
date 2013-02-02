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
        const int SIZE_X = 40;
        const int SIZE_Y = 40;
        const int HEALTH = 1;

        public Zombie(World world, Vector2 position)
            : base(world, position, new Vector2(0, 0), new Vector2(SIZE_X, SIZE_Y), new List<AnimationSet>
            {
                new AnimationSet("Main", TextureBin.Get("Zombie"), 3, 16, 3, true, 0),
            }, "Main", true, true, HEALTH)
        {
        }

        public override void Update()
        {
            if (world.Player != null && MathHelper.Distance(Position.X, world.Player.Position.X) < 300)
                Velocity.X = -MAX_SPEED_X;

            base.Update();
            if (Health <= 0)
                world.Zombies.BufferRemove(this);
        }

        public override void Die()
        {
            Explode();
            base.Die();
        }
    }
}

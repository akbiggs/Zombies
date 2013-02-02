using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Zombies
{
    public class ShrinkParticle : Particle
    {
        int startStretch;
        public ShrinkParticle(World world, Texture2D texture, Vector2 position, Vector2 velocity, int life, int stretch)
            : base(world, texture, position, velocity, life)
        {
            startStretch = stretch;
        }

        public override void Update()
        {
            this.StretchScalar = MathHelper.SmoothStep(0f, startStretch, (float)Life / (float)LifeStart);
            base.Update();
        }
    }
}

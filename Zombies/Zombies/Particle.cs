using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Appathon;

namespace Zombies
{
    public class Particle
    {
        // Default values.
        public const float DefaultVelFriction = 0.01f;
        public const float DefaultMaxSpeed = 12f;

        // Current physical properties of the particle.
        public World World;
        public Vector2 Position;
        public int Life;
        public Vector2 Velocity = Vector2.Zero;
        private Vector2 Acceleration = Vector2.Zero;
        public float MaxSpeed = DefaultMaxSpeed;
        public float Friction = DefaultVelFriction;
        public int LifeStart;
        public int LifeDeplete = 1;

        // Current texture properties of the particle.
        public Texture2D Sprite;
        public Vector2 SpriteOrigin;
        public Color Tint = Color.White;
        public Vector2 Stretch = new Vector2(1, 1);
        public float StretchScalar { set { Stretch = Vector2.One * value;} }

        public Particle(World world, Texture2D texture, Vector2 position, Vector2 velocity, int life)
        {
            World = world;
            Position = position;
            Velocity = velocity;
            Sprite = texture;
            Life = life;
            LifeStart = life;
            SpriteOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public virtual void Update()
        {
            // Apply forces.
            Velocity += Acceleration;
            MathExtra.RestrictVectorLength(ref Velocity, MaxSpeed);
            Position += Velocity;
            Life -= LifeDeplete;
            if (Life <= 0 || this.Position.X % Engine.ScreenResolution.X < 0 || this.Position.Y < 0 || this.Position.Y > Engine.ScreenResolution.Y) //|| this.Position.X > World.Camera.X + Engine.ScreenResolution.X   //)
                World.Particles.BufferRemove(this);

            // Applies friction.
            MathExtra.AddToVectorLength(ref Velocity, -Friction);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(Position.X - World.Camera.X, Position.Y), null, Tint, 0f, SpriteOrigin, Stretch, SpriteEffects.None, 0);
        }

        public static void ParticleBurstGravity(World world, Vector2 position, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Particle n = new ShrinkParticle(world, TextureBin.Get("Pixel"), position + MathExtra.GetRandomUnitVector() * MathExtra.RandomFloat() * 10, MathExtra.GetRandomUnitVector() * MathExtra.RandomFloat() * 10, 40 + MathExtra.RandomInt(40), 10);
                n.StretchScalar = 10;
                n.Acceleration = MathExtra.GetRandomUnitVector() * 0.4f;
                world.Particles.Add(n);
            }
        }

        public static void ParticleBurstGravitySpin(World world, Vector2 position, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                Particle n = new ShrinkParticle(world, TextureBin.Get("Pixel"), position + MathExtra.GetRandomUnitVector() * MathExtra.RandomFloat() * 10, MathExtra.GetRandomUnitVector() * MathExtra.RandomFloat() * 20, 40 + MathExtra.RandomInt(40), 10);
                n.StretchScalar = 10;
                n.Acceleration = MathExtra.GetRandomUnitVector() * 1f;
                world.Particles.Add(n);
            }
        }
    }
}

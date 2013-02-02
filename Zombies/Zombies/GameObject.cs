using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombies
{
    /// <summary>
    /// A game object is the most generic object that can be added to the world;
    /// it has terrian collisons built in if that property is set.
    /// </summary>
    public class GameObject
    {
        // Private variables.
        protected World world;

        public List<AnimationSet> Animations;
        public AnimationSet CurAnimation;
        public Color Color = Color.White;

        // Physical fields.
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Size;
        protected float slope;
        bool gravitable;

        // Properties.
        public int Health;
        protected bool CollidesWithTerrain = true;
        public const float MIN_Y = -64;

        // Convience getter and setters.
        // Note that we do object positions relative to their bottom center -- keep that in mind when drawing.
        public float Left { get { return this.Position.X - this.Size.X / 2; } set { this.Position.X = value + this.Size.X / 2; } }
        public float Right { get { return this.Position.X + this.Size.X / 2; } set { this.Position.X = value - this.Size.X / 2; } }
        public float Top { get { return this.Position.Y - this.Size.Y; } set { this.Position.Y = value + this.Size.Y; } }
        public float Bottom { get { return this.Position.Y; } set { this.Position.Y = value; } }
        public Vector2 Center { get { return new Vector2(this.Position.X, this.Position.Y - this.Size.Y / 2); } }
        public Rectangle BoundingBox { get { return new Rectangle((int)this.Left, (int)this.Top, (int)this.Size.X, (int)this.Size.Y); } }

        /// <summary>
        /// Makes a new animated object.
        /// </summary>
        /// <param name="world"></param>
        /// <param name="position"></param>
        /// <param name="velocity"></param>
        /// <param name="size"></param>
        /// <param name="animations">All the animations of the object.</param>
        /// <param name="startAnimation">The name of the first animation to play of this object.</param>
        /// <param name="gravitable"></param>
        public GameObject(World world, Vector2 position, Vector2 velocity, Vector2 size, List<AnimationSet> animations, String startAnimation,
            bool gravitable, bool collidesWithTerrain, int health)
        {
            this.world = world;
            this.Position = position;
            this.Velocity = velocity;
            this.Size = size;
            this.gravitable = gravitable;
            this.Animations = animations;
            this.CurAnimation = GetAnimationByName(startAnimation);
            this.CollidesWithTerrain = collidesWithTerrain;
            this.Health = health;
        }

        /// <summary>
        /// Makes a new unanimated object.
        /// </summary>
        /// <param name="world">The world in which the object exists.</param>
        /// <param name="position">The position of the object.</param>
        /// <param name="velocity">The velocity at which the object is moving initially.</param>
        /// <param name="size">The size of the object.</param>
        /// <param name="texture">The texture of the object.</param>
        public GameObject(World world, Vector2 position, Vector2 velocity, Vector2 size, Texture2D texture, bool gravitable, bool collidesWithTerrain,
            int health)
            : this(world, position, velocity, size, new List<AnimationSet>
            {
                new AnimationSet("_", texture, 1, texture.Width, 1, false, 0)
            }, "_", gravitable, collidesWithTerrain, health)
        {
        }

        /// <summary>
        /// Updates the state of the object.
        /// The default behaviour for updating a GameObject moves the object with collisions (if set).
        /// </summary>s
        public virtual void Update()
        {
            this.Move();
        }

        /// <summary>
        /// Moves the object.
        /// Updates the position, colliding it against things if it should.
        /// </summary>
        protected void Move()
        {
            if (CollidesWithTerrain)
            {
                float minX = float.NegativeInfinity;
                float maxX = float.PositiveInfinity;
                foreach (Block b in world.Blocks)
                {
                    if (this.Top < b.Bottom && this.Bottom > b.Top)
                    {
                        if (b.Right < this.Center.X && b.Right > minX)
                            minX = b.Right;
                        if (b.Left > this.Center.X && b.Left < maxX)
                            maxX = b.Left;
                    }
                }
                this.Position.X += this.Velocity.X;
                if (this.Right > maxX)
                {
                    this.Right = maxX;
                    this.Velocity.X = 0;
                }
                else if (this.Left < minX)
                {
                    this.Left = minX;
                    this.Velocity.X = 0;
                }

                float minY = MIN_Y;
                float maxY = float.PositiveInfinity;
                foreach (Block b in world.Blocks)
                {
                    if (this.Right > b.Left && this.Left < b.Right)
                    {
                        if (b.Bottom < this.Center.Y && b.Bottom > minY)
                            minY = b.Bottom;
                        if (b.Top > this.Center.Y && b.Top < maxY)
                            maxY = b.Top;
                    }
                }
                if (gravitable)
                    Velocity.Y += world.Gravity;
                this.Position.Y += this.Velocity.Y;

                if (this.Position.Y > maxY)
                {
                    this.Position.Y = maxY;
                    this.HitFloor();
                }
                if (this.Top < minY)
                {
                    this.Top = minY;
                    this.HitCeiling();
                }
            }
            else
                this.Position += this.Velocity;
        }

        /// <summary>
        /// Collides the object against the ceiling.
        /// </summary>
        protected virtual void HitCeiling()
        {
            this.Velocity.Y = 0;
        }

        /// <summary>
        /// Collides the object against the floor.
        /// </summary>
        protected virtual void HitFloor()
        {
            this.Velocity.Y = 0;
        }

        /// <summary>
        /// Returns the intersection between two objects.
        /// </summary>
        /// <param name="obj1">The other object</param>
        /// <returns>Whether or not the objects are intersecting</returns>
        public virtual bool Intersects(GameObject other)
        {
            return (this.Right > other.Left && this.Left < other.Right && this.Bottom > other.Top && this.Top < other.Bottom);
        }

        /// <summary>
        /// Draws the object.
        /// </summary>
        /// <param name="spr">The sprite batch of the game.</param>
        public virtual void Draw(SpriteBatch spr)
        {
            spr.Draw(CurAnimation.GetTexture(), new Rectangle((int)(Left - world.Camera.X), (int)Top, (int)Size.X, (int)Size.Y), CurAnimation.GetFrameRect(),
                Color, 0, Vector2.Zero, SpriteEffects.None, 0);
        }

        public virtual void Damage(int amount)
        {
            Health -= amount;
        }

        /// <summary>
        ///     Changes the animation being played. Doesn't do anything if called with the name of the currently
        ///     playing animation.
        /// </summary>
        /// <param name="name">The name of the new animation.</param>
        /// <exception cref="System.InvalidOperationException">Specified animation doesn't exist.</exception>
        protected virtual void ChangeAnimation(string name)
        {
            if (!CurAnimation.IsCalled(name))
            {
                AnimationSet newAnimation = GetAnimationByName(name);
                if (newAnimation == null)
                    throw new InvalidOperationException("Specified animation doesn't exist.");
                newAnimation.Reset();
                newAnimation.Update();
                CurAnimation = newAnimation;
            }
        }

        private AnimationSet GetAnimationByName(string name)
        {
            return Animations.First(animset => animset.IsCalled(name));
        }
    }
}

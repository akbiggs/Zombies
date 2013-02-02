using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Zombies.Support;
using Zombies;
using Appathon;

namespace Zombies
{
    public class Laser : GameObject
    {
        /// <summary>
        /// The thickness of the laser in pixels.
        /// </summary>
        const int THICKNESS = 100;
        const int DAMAGE_RADIUS_AROUND_PLAYER = 75;
        const int LASER_SPEED = 1000;
        const int LASER_DAMAGE = 1;
        const int CLOSE_COLLISION_FACTOR = 65;
        const int COLLISION_FORGIVENESS = 10;
        const int LASER_LIFESPAN = 5;

        /// <summary>
        /// The amount of damage the laser does.
        /// </summary>
        public int DamageInflicted;
        int timeLeft;
        public Vector2 Direction;
        Color Pencil = new Color(255, 238, 147);
        
        BufferedList<Vector2> endPoints = new BufferedList<Vector2>();
        BufferedList<Vector2> collisionEndPoints = new BufferedList<Vector2>();
        private bool alreadyChecked = false;

        public Laser(World world, Vector2 position, Vector2 direction)
            : base(world, position, direction, new Vector2(1, 1), TextureBin.Get("Pixel"), false, false)
        {
            this.Direction = direction;
            this.Velocity = world.Player.Velocity + direction * LASER_SPEED;
            this.DamageInflicted = LASER_DAMAGE;
            this.timeLeft = LASER_LIFESPAN;
            this.CollidesWithTerrain = false;

            Vector2 side = Vector2.Normalize(MathExtra.GetPerpendicularVector(this.Velocity));
            Vector2 p1 = this.Position + side * THICKNESS;
            Vector2 cp1 = p1 + side * (THICKNESS + COLLISION_FORGIVENESS);
            Vector2 p2 = this.Position - side * THICKNESS;
            Vector2 cp2 = p2 - side * (THICKNESS + COLLISION_FORGIVENESS);

            endPoints.Add(p1);
            endPoints.Add(p2);

            // the collision endpoints are more forgiving
            collisionEndPoints.Add(cp1);
            collisionEndPoints.Add(cp2);

            for (int i = 0; i < endPoints.Count; i++)
                endPoints[i] += Velocity;
            for (int i = 0; i < collisionEndPoints.Count; i++)
                collisionEndPoints[i] += Velocity;
        }

        public override void Update()
        {
            if (!alreadyChecked)
            {
                foreach (GameObject mob in world.Mobs)
                    if (this.Intersects(mob))
                    {
                        mob.Damage(DamageInflicted);
                    }
                alreadyChecked = true;
            }

            timeLeft--;
            if (timeLeft <= 0)
                world.Lasers.BufferRemove(this);

            this.Position += Velocity;
            for (int i = 0; i < endPoints.Count; i++)
                endPoints[i] += Velocity;
            for (int i = 0; i < collisionEndPoints.Count; i++)
                collisionEndPoints[i] += Velocity;
        }

        public override bool Intersects(GameObject other)
        {
            return (Vector2.DistanceSquared(world.Player.Center + Direction * CLOSE_COLLISION_FACTOR, other.Center) < Math.Pow(DAMAGE_RADIUS_AROUND_PLAYER, 2)) ||
                MathExtra.PointInTriangle(world.Player.Center, collisionEndPoints[0], collisionEndPoints[1], other.Center);
        }

        public override void Draw(SpriteBatch spr)
        {
            PrimitiveBatch prim = Engine.PrimitiveBatch;
            prim.Begin(PrimitiveType.TriangleList);
            //if (Engine.DebugMode)
            //{
            //    prim.AddVertex(collisionEndPoints[0], Color.Red);
            //    prim.AddVertex(collisionEndPoints[1], Color.Red);
            //    prim.AddVertex(world.Player.Center, Color.Red);
            //}
            prim.AddVertex(endPoints[0], Pencil);
            prim.AddVertex(endPoints[1], Pencil);
            prim.AddVertex(world.Player.Center, Pencil);
            prim.End();
        }

        public void DrawBang(SpriteBatch spr)
        {
            Texture2D tex = TextureBin.Get("Circle");
            float scale = (1f / (float)tex.Width) * (2 * DAMAGE_RADIUS_AROUND_PLAYER);
            spr.Draw(tex, world.Player.Center + Direction * CLOSE_COLLISION_FACTOR, null, Pencil * ((timeLeft )/ LASER_LIFESPAN), 0f, new Vector2(tex.Width, tex.Height) / 2, scale, SpriteEffects.None, 0);
        }
    }
}

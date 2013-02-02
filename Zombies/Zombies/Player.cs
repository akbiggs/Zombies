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
        const float MAX_SPEED_X = 12f;
        const float MAX_SPEED_Y = 20f;

        const int HEALTH = 3;
        const float JUMP_SPEED = MAX_SPEED_Y - 7f;

        const int SIZE_X = 50;
        const int SIZE_Y = 100;

        public bool CanJump = false;
        public bool CanDoubleJump = false;
        public bool IsAlive = true;

        public Player(World world, Vector2 position)
            : base(world, position, new Vector2(MAX_SPEED_X, 0), new Vector2(SIZE_X, SIZE_Y), new List<AnimationSet>
            {
                new AnimationSet("Run", TextureBin.Get("RunGreenHat"), 5, 16, 2, true, 0),
            }, "Run", true, true, HEALTH)
        {

        }

        public override void Update()
        {
            Velocity.X = MAX_SPEED_X;
            // preventing jumping when player falls off of ledge
            if (Velocity.Y >= 2f)
                CanJump = false;

            // basic actions are firing gun and jumping
            if (Input.ScreenTapped)
            {
                if (Input.TapPosition.X <= World.PLAYER_CAMERA_OFFSET)
                {
                    if (CanJump || CanDoubleJump)
                        DoJump();
                }
                else
                    FireGun(new Vector2(Input.TapPosition.X + world.Camera.X, Input.TapPosition.Y));
            }

            // kill player if they fall off-screen or get hit by a zombie
            if (Top > Engine.ScreenResolution.Y || world.Mobs.Any((mob) => this.Intersects(mob)))
                Damage(1);

            if (Health <= 0)
                Die();

            base.Update();
        }


        private void DoJump()
        {
            this.Velocity.Y = -JUMP_SPEED;
            if (!CanJump)
                CanDoubleJump = false;
            CanJump = false;
        }

        protected override void HitFloor()
        {
            CanJump = true;
            CanDoubleJump = true;

            base.HitFloor();
        }

        private void FireGun(Vector2 firePos)
        {
            world.Lasers.BufferAdd(new Laser(world, Center, Vector2.Normalize(firePos - Center)));
        }

        public override void Draw(SpriteBatch spr)
        {
            base.Draw(spr);
        }

        public void Die()
        {
            // TODO: Death animation stuff.
            world.GameOver();
        }
    }
}

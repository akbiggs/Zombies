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
        const float MAX_SPEED_X = 7f;
        const float MAX_SPEED_Y = 20f;

        const int HEALTH = 3;
        const float JUMP_SPEED = MAX_SPEED_Y - 5f;

        const int SIZE_X = 30;
        const int SIZE_Y = 50;

        public bool CanJump = false;
        public bool IsAlive = true;

        public Player(World world, Vector2 position)
            : base(world, position, new Vector2(MAX_SPEED_X, 0), new Vector2(SIZE_X, SIZE_Y), TextureBin.Get("Pixel"), true, true, HEALTH)
        {

        }

        public override void Update()
        {
            // preventing jumping when player falls off of ledge
            if (Velocity.Y >= 2f)
                CanJump = false;

            // basic actions are firing gun and jumping
            if (Input.ScreenTapped)
            {
                if (Input.TapPosition.X <= World.PLAYER_CAMERA_OFFSET)
                {
                    if (CanJump)
                        DoJump();
                }
                else
                    FireGun(Input.TapPosition);
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
            CanJump = false;
        }

        protected override void HitFloor()
        {
            CanJump = true;

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

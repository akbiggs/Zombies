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

        const float JUMP_SPEED = MAX_SPEED_Y;

        const int SIZE_X = 30;
        const int SIZE_Y = 50;

        public bool CanJump = false;

        public Player(World world, Vector2 position)
            : base(world, position, new Vector2(MAX_SPEED_X, 0), new Vector2(SIZE_X, SIZE_Y), TextureBin.Get("Pixel"), true, true)
        {

        }

        public override void Update()
        {
            // preventing jumping when player falls off of ledge
            if (Velocity.Y <= 2f)
                CanJump = false;
            if (Input.ScreenTapped)
            {
                if (Input.TapPosition.X <= World.PLAYER_CAMERA_OFFSET && CanJump)
                    DoJump();
                else
                    FireGun(Input.TapPosition);
            }

            base.Update();
        }


        private void DoJump()
        {
            this.Velocity.Y = -MAX_SPEED_Y;
            CanJump = false;
        }

        protected override void HitFloor()
        {
            CanJump = true;

            base.HitFloor();
        }

        private void FireGun(Vector2 vector2)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spr)
        {
            base.Draw(spr);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombies
{
    public class Vampire : GameObject
    {
        const int SIZE_X = 48;
        const int SIZE_Y = 18;

        const int HEALTH = 1;

        const int DELAY_BEFORE_RUSH = 30;

        const float RUSH_SPEED = 7f;
        const float SWIRL_RUSH_SPEED = 5f;
        const int SWIRL_SPEED = 5; 

        int rushTimer = 0;
        EnemyPattern attackPattern;
        float minHeight, maxHeight;
        int swirlRotation = 0;

        public Vampire(World world, Vector2 position, EnemyPattern pattern)
            : base(world, position, Vector2.Zero, new Vector2(SIZE_X, SIZE_Y), TextureBin.Get("Pixel"), false, false, HEALTH)
        {
            attackPattern = pattern;
            if (position.Y < world.Player.Position.Y) 
            {
                maxHeight = Engine.ScreenResolution.Y - position.Y;
                minHeight = position.Y;
            }
            else
                maxHeight = position.Y;
                minHeight = (Engine.ScreenResolution.Y - position.Y)*2;
        }

        public override void Update()
        {
            switch (attackPattern)
            {
                case EnemyPattern.Swirling:
                    // swirl in a sine wave approaching the player
                    Position.X -= SWIRL_RUSH_SPEED;
                    swirlRotation += SWIRL_SPEED;
                    if (swirlRotation >= 360)
                        swirlRotation = 0;
                    Position.Y = (float)Math.Sin(MathHelper.ToRadians(swirlRotation)) * (maxHeight - minHeight) + minHeight;
                    break;
                case EnemyPattern.Rushing:
                    break;
                case EnemyPattern.Circling:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException("Invalid vampire attack pattern specified.");
            }

            base.Update();
            if (Health <= 0)
                world.Vampires.BufferRemove(this);
        }
    }
}

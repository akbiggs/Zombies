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
        const int SIZE_Y = 48;

        const int HEALTH = 1;

        const int DELAY_BEFORE_RUSH = 30;

        const float RUSH_SPEED = 10f;
        const float SWIRL_RUSH_SPEED = 5f;
        const int SWIRL_SPEED = 5;

        int rushTimer = 0;
        EnemyPattern attackPattern;
        float minHeight, maxHeight;
        int swirlRotation = 0;
        Vector2 targetPos = Vector2.Zero;
        Vector2 rushSpeed;

        public Vampire(World world, Vector2 position, EnemyPattern pattern)
            : base(world, position, Vector2.Zero, new Vector2(SIZE_X, SIZE_Y), new List<AnimationSet>
            {
                new AnimationSet("Main", TextureBin.Get("FlyerRed2"), 3, 16, 3, true, 0)
            }, "Main", false, false, HEALTH)
        {
            attackPattern = pattern;
            if (position.Y < world.Player.Position.Y)
            {
                maxHeight = Engine.ScreenResolution.Y - position.Y;
                minHeight = position.Y;
            }
            else
                maxHeight = position.Y;
            minHeight = (Engine.ScreenResolution.Y - position.Y) * 2;
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
                    // rush at the player after a short delay
                    if (targetPos == Vector2.Zero && rushTimer++ % DELAY_BEFORE_RUSH == 0)
                        targetPos = new Vector2(world.Player.Position.X + 200, world.Player.Position.Y);
                    else if (targetPos != Vector2.Zero)
                    {
                        Position = Position.PushTowards(targetPos, RUSH_SPEED * Vector2.One);
                    }
                    break;
                case EnemyPattern.Circling:
                    throw new NotImplementedException();
                default:
                    throw new InvalidOperationException("Invalid vampire attack pattern specified.");
            }

            base.Update();
        }

        public override void Die()
        {
            Explode();
            base.Die();
        }
    }
}

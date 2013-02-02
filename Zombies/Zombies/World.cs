using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zombies
{
    public class World
    {
        #region Constants
        float DEFAULT_ZOOM_LEVEL = 1f;
        public static int PLAYER_CAMERA_OFFSET = 250;
        #endregion

        public float Gravity = 0.75f;

        //protected Camera camera;
        public Vector2 Camera;

        public Player Player;
        public BufferedList<Block> Blocks = new BufferedList<Block>();
        public BufferedList<Zombie> Zombies = new BufferedList<Zombie>();

        int width, height;
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        public World(int width, int height)
        {
            Player = new Player(this, new Vector2(50, 50));

            this.width = width;
            this.height = height;

            // TODO: replace this with random block generation
            for (int i = 0; i < 8; i++)
                Blocks.Add(new Block(this, new Vector2(i * 225, Engine.ScreenResolution.Y - 50), new Vector2(200, 50)));

            //camera = new Camera(Player, Width, Height, DEFAULT_ZOOM_LEVEL);
        }

        public virtual void Update()
        {
            foreach (Block block in Blocks)
                block.Update();
            foreach (Zombie zombie in Zombies)
                zombie.Update();

            Player.Update();
            Camera = new Vector2(Math.Max(Player.Position.X - PLAYER_CAMERA_OFFSET, 0), Engine.ScreenResolution.Y / 2);

            //camera.Update(this);
        }

        public virtual void Draw(SpriteBatch spr)
        {
            spr.Begin();
            //spr.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.GetTransformation(spr.GraphicsDevice));
            spr.GraphicsDevice.Clear(Color.Gray);
            foreach (Block block in Blocks)
                block.Draw(spr);
            foreach (Zombie zombie in Zombies)
                zombie.Draw(spr);

            Player.Draw(spr);
            spr.End();
        }
    }
}

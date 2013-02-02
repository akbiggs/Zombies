using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Zombies
{
    public class World
    {
        #region Constants
        float DEFAULT_ZOOM_LEVEL = 1f;
        #endregion

        protected Camera camera;

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
            Player = new Player(new Vector2(50, 50));

            this.width = width;
            this.height = height;

            // TODO: replace this with random block generation
            for (int i = 0; i < 4; i++)
                Blocks.Add(new Block(this, new Vector2(i * 225, Engine.ScreenResolution.Y - 50), new Vector2(200, 50)));

            camera = new Camera(Player, Width, Height, DEFAULT_ZOOM_LEVEL);
        }

        public virtual void Update()
        {
            foreach (Block block in Blocks)
                block.Update();
            foreach (Zombie zombie in Zombies)
                zombie.Update();

            Player.Update();

            camera.Update(this);
        }
    }
}

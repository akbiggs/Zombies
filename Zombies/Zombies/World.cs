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
        public BufferedList<Laser> Lasers = new BufferedList<Laser>();
        public BufferedList<Zombie> Zombies = new BufferedList<Zombie>();
        public BufferedList<Vampire> Vampires = new BufferedList<Vampire>();

        public bool Failed;

        int width, height;
        public int Width
        {
            get { return width; }
        }
        public int Height
        {
            get { return height; }
        }

        public List<GameObject> Mobs
        {
            get 
            {
                List<GameObject> ret = new List<GameObject>();
                ret.AddRange(Zombies.Cast<GameObject>());
                ret.AddRange(Vampires.Cast<GameObject>());
                return ret;
            }
        }

        public World(int width, int height)
        {
            Player = new Player(this, new Vector2(50, 50));
            this.width = width;
            this.height = height;
            Failed = false;

            // TODO: replace this with random block generation
            Blocks.Add(new Block(this, new Vector2(0, Engine.ScreenResolution.Y - 50), new Vector2(2000, 50)));
            for (int i = 0; i < 5; i++)
            {
                Zombies.Add(new Zombie(this, new Vector2(500 + i * 100, Engine.ScreenResolution.Y - 120)));
                Vampires.Add(new Vampire(this, new Vector2(500 + i * 100, Engine.ScreenResolution.Y - 120), EnemyPattern.Swirling));
            }

            //camera = new Camera(Player, Width, Height, DEFAULT_ZOOM_LEVEL);
        }

        public virtual void Update()
        {
            foreach (Laser laser in Lasers)
                laser.Update();
            foreach (Block block in Blocks)
                block.Update();
            foreach (Zombie zombie in Zombies)
                zombie.Update();
            foreach (Vampire vampire in Vampires)
                vampire.Update();

            if (!Failed)
            {
                Camera = new Vector2(Math.Max(Player.Position.X - PLAYER_CAMERA_OFFSET, 0), Engine.ScreenResolution.Y / 2);
                Player.Update();
            } 
            else if (Input.ScreenTapped)
                Engine.ShouldReset = true;

            Lasers.ApplyBuffers();
            Blocks.ApplyBuffers();
            Zombies.ApplyBuffers();
            Vampires.ApplyBuffers();

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
            foreach (Vampire vampire in Vampires)
                vampire.Draw(spr);

            if (!Failed)
                Player.Draw(spr);
            spr.End();

            foreach (Laser laser in Lasers)
                laser.Draw(spr);
        }

        public void GameOver()
        {
            // TODO: Write game over.
            Failed = true;
        }
    }
}

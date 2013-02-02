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
        RenderTarget2D target = null;

        //protected Camera camera;
        TerrainGenerator generator;

        public Vector2 Camera;

        public Player Player;
        public BufferedList<Block> Blocks = new BufferedList<Block>();
        public BufferedList<Laser> Lasers = new BufferedList<Laser>();
        public BufferedList<Zombie> Zombies = new BufferedList<Zombie>();
        public BufferedList<Vampire> Vampires = new BufferedList<Vampire>();
        public BufferedList<Particle> Particles = new BufferedList<Particle>();

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

        int colorFoolery = 0;
        int colorChangeDirection = 1;
        public Color NextColor
        {
            get
            {

                colorFoolery += 3 * colorChangeDirection;
                if (colorFoolery > 255)
                {
                    colorFoolery = 255;
                    colorChangeDirection = -1;
                }
                else if (colorFoolery < 125 && colorChangeDirection == -1)
                {
                    colorFoolery = 125;
                    colorChangeDirection = 1;
                }
                return new Color(colorFoolery, 0, 0);
            }
        }

        public World(int width, int height)
        {
            generator = new TerrainGenerator(this);

            Player = new Player(this, new Vector2(50, 50));
            this.width = width;
            this.height = height;
            Failed = false;


            //camera = new Camera(Player, Width, Height, DEFAULT_ZOOM_LEVEL);
        }

        public virtual void Update()
        {
            generator.Update();

            foreach (Laser laser in Lasers)
                laser.Update();
            foreach (Block block in Blocks)
                block.Update();
            foreach (Zombie zombie in Zombies)
                zombie.Update();
            foreach (Vampire vampire in Vampires)
                vampire.Update();
            foreach (Particle particle in Particles)
                particle.Update();

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
            Particles.ApplyBuffers();

            //camera.Update(this);
        }

        public virtual void Draw(SpriteBatch spr)
        {
            spr.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            spr.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;

            if (target == null)
            {
                target = new RenderTarget2D(spr.GraphicsDevice, Engine.ScreenResolution.X, Engine.ScreenResolution.Y);
            }
            else
            {
                spr.GraphicsDevice.Textures[0] = null;
                target.SetData(new Color[Engine.ScreenResolution.X * Engine.ScreenResolution.Y]);
            }
            spr.GraphicsDevice.SetRenderTarget(target);
            spr.Begin();
            //spr.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, camera.GetTransformation(spr.GraphicsDevice));
            spr.GraphicsDevice.Clear(Color.Gray);
            foreach (Block block in Blocks)
                block.Draw(spr);
            foreach (Particle particle in Particles)
                particle.Draw(spr);
            spr.End();

            spr.GraphicsDevice.SetRenderTarget(null);
            spr.Begin();
            spr.Draw(target, Vector2.Zero, NextColor);
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

        public void Remove(GameObject gameObject)
        {
            if (gameObject is Player)
                Player = null;
            else if (gameObject as Laser != null)
                Lasers.BufferRemove((Laser)gameObject);
            else if (gameObject as Zombie != null)
                Zombies.BufferRemove((Zombie)gameObject);
            else if (gameObject as Vampire != null)
                Vampires.BufferRemove((Vampire)gameObject);
            else if (gameObject as Block != null)
                Blocks.BufferRemove((Block)gameObject);
            else
                throw new InvalidOperationException("Unhandled removal of object.");
        }

        public void Remove(Particle particle)
        {
            Particles.Remove(particle);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Zombies.Support;

namespace Zombies
{
    public class TerrainGenerator
    {
        World world;
        public List<TerrainPattern> NextPatterns = new List<TerrainPattern>();
        public float NextX = 0;

        public TerrainGenerator(World world)
        {
            this.world = world;
            if (Engine.FirstPlay)
                NextPatterns.Add(TerrainPattern.Tutorial);
            else
                NextPatterns.Add(TerrainPattern.Flat);
        }

        public void Update()
        {
            if (Math.Abs(NextX - world.Camera.X) < Engine.ScreenResolution.X + 300)
            {
                Generate(NextX);
                Random randomGen = new Random();
                for (int i = 0; i < 8; i++)
                {
                    TerrainPattern nextPattern = (TerrainPattern)Enum.Parse(typeof(TerrainPattern), randomGen.Next(1, 7).ToString(), true);
                    NextPatterns.Add(nextPattern);
                }
            }
        }

        public void Generate(float startX)
        {
            switch (NextPatterns.Pop(0))
            {
                case TerrainPattern.Tutorial:
                    world.Blocks.BufferAdd(new Block(world, new Vector2(startX, Engine.ScreenResolution.Y - 50), new Vector2(50, 200)));
                    NextX += 50;
                    break;
                case TerrainPattern.Flat:
                    world.Blocks.BufferAdd(new Block(world, new Vector2(startX, Engine.ScreenResolution.Y), new Vector2(200, 200)));
                    NextX += 200;
                    break;
                case TerrainPattern.ZombieFlat:
                    world.Blocks.BufferAdd(new Block(world, new Vector2(startX, Engine.ScreenResolution.Y), new Vector2(400, 100)));
                    for (int i = 0; i < 5; i++)
                        world.Zombies.BufferAdd(new Zombie(world, new Vector2(startX + Engine.ScreenResolution.X + 100 + i * 60, Engine.ScreenResolution.Y - 120)));
                    NextX += 400;
                    break;
                case TerrainPattern.ZombieHills:
                    for (int i = 0; i < 3; i++)
                    {
                        world.Blocks.BufferAdd(new Block(world, new Vector2(50 + startX + i * 225, Engine.ScreenResolution.Y),
                            new Vector2(200, 100 * (i+1))));
                        world.Zombies.BufferAdd(new Zombie(world, new Vector2(25 + startX + i * 80, Engine.ScreenResolution.Y - (100 * (i + 1)))));
                    }
                    NextX += 800;
                    break;
                case TerrainPattern.VampireFlat:
                    world.Blocks.BufferAdd(new Block(world, new Vector2(startX, Engine.ScreenResolution.Y - 50), new Vector2(400, 50)));
                    for (int i = 0; i < 5; i++)
                        world.Vampires.Add(new Vampire(world, new Vector2(startX + Engine.ScreenResolution.X + 100, 50), EnemyPattern.Rushing));
                    NextX += 400;
                    break;
                case TerrainPattern.Jumps:
                    break;
                case TerrainPattern.ZombieJumps:
                    break;
                case TerrainPattern.VampireJumps:
                    break;
                default:
                    break;
            }
        }
    }

    public enum TerrainPattern
    {
        Tutorial = 0,
        Flat,
        ZombieFlat,
        ZombieHills,
        ZombieCrazed,
        VampireFlat,
        Jumps,
        ZombieJumps,
        VampireJumps,
    }
}

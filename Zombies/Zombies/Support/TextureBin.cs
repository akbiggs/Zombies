using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Zombies
{
    public static class TextureBin
    {
        const int NUM_BUILDING_TEXTURES = 5;
        static Dictionary<String, Texture2D> texDic = new Dictionary<string, Texture2D>();
        static Dictionary<String, SpriteFont> fontDic = new Dictionary<string, SpriteFont>();
        static List<String> names = new List<string> { "Pixel", "Building2", "Building3", "Building4", "Building5", "RunGreenHat", "Zombie", 
            "FlyerRed2", };

        public static void LoadContent(ContentManager cm)
        {
            foreach (String name in names)
                texDic[name] = cm.Load<Texture2D>(name);
        }

        public static Texture2D Get(String name)
        {
            return texDic[name];
        }

        public static Texture2D GetRandomBuilding()
        {
            Random random = new Random();
            return texDic["Building" + (random.Next(0, NUM_BUILDING_TEXTURES) + 2).ToString()];
        }
    }
}

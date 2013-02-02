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
        static Dictionary<String, Texture2D> texDic = new Dictionary<string, Texture2D>();
        static List<String> names = new List<string> { "Pixel" };

        public static void LoadContent(ContentManager cm)
        {
            foreach (String name in names)
                texDic[name] = cm.Load<Texture2D>(name);
        }

        public static Texture2D Get(String name)
        {
            return texDic[name];
        }
    }
}

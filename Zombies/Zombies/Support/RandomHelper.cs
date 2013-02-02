using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zombies.Support
{
    public static class RandomHelper
    {
        static Random random = new Random();

        public static int RandomSign()
        {
            return random.Next(2) == 0 ? -1 : 1; 
        }
    }
}

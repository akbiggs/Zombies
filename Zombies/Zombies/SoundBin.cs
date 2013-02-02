using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Zombies
{
    public static class SoundBin
    {
        private static Dictionary<String, Song> songDic = new Dictionary<string, Song>();
        private static Dictionary<String, SoundEffect> soundDic = new Dictionary<string, SoundEffect>();

        public static void LoadSounds(ContentManager c)
        {
            //Load(c, "jump");
            //Load(c, "hurt");
            //Load(c, "laser");
            //Load(c, "death");
            //LoadSong(c, "gods");
            //LoadSong(c, "main");
        }

        private static void Load(ContentManager c, String name)
        {
            SoundBin.Add(name, c.Load<SoundEffect>(name));
        }

        private static void LoadSong(ContentManager c, String name)
        {
            SoundBin.Add(name, c.Load<Song>(name));
        }

        public static Song GetSong(String songName)
        {
            return songDic[songName];
        }

        public static SoundEffect Get(String name)
        {
            return soundDic[name];
        }

        public static void Play(String soundName)
        {
            if (soundDic.ContainsKey(soundName))
                soundDic[soundName].Play();
            else
                MediaPlayer.Play(songDic[soundName]);
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }

        public static void Add(String songName, Song song)
        {
            songDic.Add(songName, song);
        }

        public static void Add(String soundName, SoundEffect sound)
        {
            soundDic.Add(soundName, sound);
        }
    }
}

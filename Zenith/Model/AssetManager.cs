using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace Zenith
{
    public enum GameImage
    {
        None,
        PlayerShip,
        Enemy1,
        Enemy2,
        Enemy3,
        Boss1,
        Boss2,
        Boss3,
        Boss4,
        Boss5,
        HealthBar_0,
        HealthBar_1,
        HealthBar_2,
        HealthBar_3,
        HealthBar_4,
        HealthBar_5,
        HealthBar_6,
        HealthBar_7,
        HealthBar_8,
        HealthBar_9,
        HealthBar_10,
        Asteroid,
        LaserBlue,
        LaserGreen,
        LaserOrange,
        LaserRed,
        DamagePowerUp,
        HealthPowerUp
    }

    public enum GameSound
    {
        Laser,
        SmallExplosion,
    }

    public class AssetManager
    {
        public static AssetManager Instance { get; } = new AssetManager();

        private AssetManager()
        {
            images = new Texture2D[Enum.GetValues(typeof(GameImage)).Length];
            sounds = new SoundEffect[Enum.GetValues(typeof(GameSound)).Length];
        }

        ContentManager contentManager;
        Texture2D[] images;
        SoundEffect[] sounds;

        public ContentManager ContentManager { set { contentManager = value; } }
        public Texture2D[] Images { get { return images; } }

        public void LoadAssets(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);

            using (reader)
            {
                string s = reader.ReadLine();
                int line = 1;

                while (s != null)
                {
                    if (s.Length == 0 || s[0] == '#') goto dontCalculate;

                    string[] arguments = s.Split(' ');
                    if (arguments.Length != 3)
                    {
                        throw new Exception("Not a sufficient amount of arguments on line " + line);
                    }

                    if (arguments[0] == "image")
                    {
                        try
                        {
                            images[(int)Enum.Parse(typeof(GameImage), arguments[1])] = contentManager.Load<Texture2D>(arguments[2]);
                        }
                        catch
                        {
                            throw new Exception("'" + arguments[1] + "' is not a valid identifier for a game image. (On line " + line + ")");
                        }
                    }
                    else if (arguments[0] == "sound")
                    {
                        try
                        {
                            sounds[(int)Enum.Parse(typeof(GameSound), arguments[1])] = contentManager.Load<SoundEffect>(arguments[2]);
                        }
                        catch
                        {
                            throw new Exception("'" + arguments[1] + "' is not a valid identifier for a game image. (On line " + line + ")");
                        }
                    }
                    else
                    {
                        throw new Exception("Unknown asset type '" + arguments[0] + "' on line " + line);
                    }
                    dontCalculate:
                    s = reader.ReadLine();
                    ++line;
                }
            }
        }

        public void PlaySound(GameSound gameSound)
        {
            var i = sounds[(int)gameSound].CreateInstance();
            i.Play();
        }
        public void PlaySound(GameSound gameSound, float volume)
        {
            var i = sounds[(int)gameSound].CreateInstance();
            i.Volume = volume;
            i.Play();
        }

    }
}

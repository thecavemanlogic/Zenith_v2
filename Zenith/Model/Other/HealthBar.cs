//-----------------------------------------------------------
//File:   HealthBar.cs
//Desc:   Holds the class responsible for displays all Ship
//        health bars.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class is responsible for handling the display
    // of health bars for each ship object on the screen.
    class HealthBar : GameObject
    {
        // The ship that is going to have its health displayed.
        Ship host;

        // The vertical offset from the to be displayed at.
        float distance;

        // Updates the location of the health bar relative to the
        // host ship's postion. It will also update the health bar
        // index to reflect the ship's current health.
        public override void Loop()
        {
            distance = Math.Max(host.Size.X, host.Size.Y);
            position = host.Position - new Vector2(0, distance);
            destroy = host.Destroy;

            gameImage = (GameImage)((float)host.Health / host.MaxHealth * 10) + ((int)GameImage.HealthBar_0);
            if (gameImage <= 0) { gameImage = GameImage.HealthBar_0; }


            // imageIndex = (int)((double)host.Health / host.MaxHealth * 10);
            // if (imageIndex < 0) imageIndex = 0;
        }

        // Constructor
        public HealthBar(Ship host)
            : base(host.Position)
        {
            gameImage = GameImage.HealthBar_10;
            canSerialize = false;

            this.host = host;
            /*
            //~~~~~~~~~~~~~~~~~~~~ Health Bar Display ~~~~~~~~~~~~~~~~~~~~
            imageSources = new List<string> {
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_0.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_1.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_2.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_3.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_4.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_5.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_6.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_7.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_8.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_9.png"),
                Util.GetSpriteFolderPath("Health_Bar\\healthbar_10.png"),
            };
            */
            imageRotation = 0;
            collidable = false;
            type = GameObjectType.HealthBar;
            size = new Vector2(26 * 5, 4 * 5);
        }

        // ~~~~~~~~~~~~~~~~~~~~ Serialize ~~~~~~~~~~~~~~~~~~~~
        public override string Serialize()
        {
            return base.Serialize() + ',' + host.Position.ToString() + ',' + distance.ToString();
        }

        // ~~~~~~~~~~~~~~~~~~~~ Deserialize ~~~~~~~~~~~~~~~~~~~~
        public override void Deserialize(string saveInfo)
        {
            int index = IndexOfNthOccurance(saveInfo, ",", 12);

            string gameObjectSaveInfo = saveInfo.Substring(0, index);
            base.Deserialize(gameObjectSaveInfo);

            string[] healthBarSaveInfo = saveInfo.Substring(index + 1, saveInfo.Length - index - 1).Split(',');

            string[] xNy = healthBarSaveInfo[0].Split(':');
            Vector2 pos = new Vector2((float)Convert.ToDouble(xNy[0]), (float)Convert.ToDouble(xNy[1]));

            host = (Ship)World.Instance.Objects.Find(obj => (obj.Position == pos && obj.Collidable == true));
            distance = (float)Convert.ToDouble(healthBarSaveInfo[1]);
        }
    }
}

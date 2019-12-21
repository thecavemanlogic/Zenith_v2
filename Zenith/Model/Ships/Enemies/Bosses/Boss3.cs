//-----------------------------------------------------------
//File:   Boss3.cs
//Desc:   This file holds the class responsible for controlling
//        Boss3.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class controls Boss3. It also controls the movement for the object as well.
    class Boss3 : Enemy
    {
        // Fires the cannon as fast as possible (no laser will be fired if
        // the internal state of the cannon is not ready for a laser to be
        // fired). It also updates the movement of Boss3 to allow it to
        // sway back and forth.
        public override void ShipLoop()
        {
            cannon.Fire();
            ++clock;
            float goalY = (float)(Math.Cos(clock / 100f) + 1) / 2 * World.Instance.EndY - position.Y;
            AddForce(new Vector2(0, goalY) * 100);
            var offset = new Vector2(World.Instance.EndX * 0.75f - position.X, 0);
            AddForce(offset);

            angle = Vector.GetAngle(World.Instance.Player.Position - position);
        }

        // Constructor
        public Boss3(Vector2 position)
            : base(position)
        {
            gameImage = GameImage.Boss3;
            type = GameObjectType.Boss3;
            cannon = new Boss3Cannon(this);
            // imageSources = new List<string> { Util.GetShipSpriteFolderPath("large_grey_02.png") };
            size = new Vector2(256, 256);
            health = 4000;
            maxHealth = 4000;
            mass = 400;
            worth = 300;
        }
    }
}

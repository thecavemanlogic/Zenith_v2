//-----------------------------------------------------------
//File:   Boss2.cs
//Desc:   This file holds the class responsible for controlling
//        Boss2.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class controls Boss2. It also initializes the specialized cannon for Boss2
    // and controls the movement for the object as well.
    public class Boss2 : Enemy
    {
        // Fires the cannon as fast as possible (no laser will be fired if
        // the internal state of the cannon is not ready for a laser to be
        // fired). It also updates the movement of Boss2 to allow it to
        // sway back and forth.
        public override void ShipLoop() {
            cannon.Fire();
            ++clock;
            float goalY = (float)(Math.Cos(clock / 100f) + 1) / 2 * World.Instance.EndY - position.Y;
            AddForce(new Vector2(0, goalY) * 100);
            var offset = new Vector2(World.Instance.EndX * 0.75f - position.X, 0);
            AddForce(offset);

            angle = Vector.GetAngle(World.Instance.Player.Position - position);
        }

        // Constructor
        public Boss2(Vector2 position)
            : base(position) 
        {
            gameImage = GameImage.Boss2;
            type = GameObjectType.Boss2;
            cannon = new Boss2Cannon(this);
            // imageSources = new List<string> { Util.GetShipSpriteFolderPath("large_purple_01.png") };
            size = new Vector2(256, 256);
            health = 4000;
            maxHealth = 4000;
            mass = 400;
            worth = 200;
        }
    }
}

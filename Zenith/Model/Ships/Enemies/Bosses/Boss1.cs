//-----------------------------------------------------------
//File:   Boss1.cs
//Desc:   This file holds the class responsible for controlling
//        Boss1.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class controls Boss1. It also initializes the specialized cannon for Boss1
    // and controls the movement for the object as well.
    public class Boss1 : Enemy
    {
        // Fires the cannon as fast as possible (no laser will be fired if
        // the internal state of the cannon is not ready for a laser to be
        // fired). It also updates the movement of Boss1 to allow it to
        // sway back and forth.
        public override void ShipLoop()
        {
            cannon.Fire();
            ++clock;
            float goalY = (float)(Math.Cos(clock / 100.0) + 1) / 2 * World.Instance.EndY - position.Y;
            AddForce(new Vector2(0, goalY) * 100);
            var offset = new Vector2(World.Instance.EndX * 0.75f - position.X, 0);
            AddForce(offset);
            
            angle = Vector.GetAngle(World.Instance.Player.Position - position);
        }

        // Constructor
        public Boss1(Vector2 position)
            : base(position)
        {
            gameImage = GameImage.Boss1;

            angle = (float)Math.PI;
            type = GameObjectType.Boss1;
            size = new Vector2(256, 256);
            health = 4000;
            maxHealth = 4000;
            mass = 400;
            cannon = new Boss1Cannon(this);
            worth = 100;
        }
    }
}

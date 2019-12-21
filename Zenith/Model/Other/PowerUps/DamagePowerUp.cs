//-----------------------------------------------------------
//File:   DamagePowerUp.cs
//Desc:   Increases a ship's damage when collided with.
//Note:   This file is unused.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class is responsible for increasing a ship's
    // laser damage when collided with.
    public class DamagePowerUp : PowerUp
    {
        // Constructor
        public DamagePowerUp(Vector2 position)
            : base(position)
        {
            Damage = 50;
        }
    }
}

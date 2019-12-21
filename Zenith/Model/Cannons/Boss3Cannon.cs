//-----------------------------------------------------------
//File:   Boss3Cannon.cs
//Desc:   Serves as the cannon for Boss3.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

namespace Zenith
{
    public class Boss3Cannon : Cannon
    {
        // Constructor
        // Sets the fire pattern to wait 5 seconds before
        // releasing a continuous stream of lasers toward
        // the player. It also changes the color
        // of the projectile to make the lasers easily
        // seen by the player.
        public Boss3Cannon(Ship host)
            : base(host)
        {
            firePattern = new List<int> { 300 };

            for (int i = 0; i < 100; ++i)
            {
                firePattern.Add(0);
            }
            damage = 60 * World.Instance.Difficulty;
            // ProjectileColor = ProjectileColor.Red;
        }
    }
}

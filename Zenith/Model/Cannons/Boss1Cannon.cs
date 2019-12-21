//-----------------------------------------------------------
//File:   Boss1Cannon.cs
//Desc:   This class serves as the cannon used by Boss1 and
//        Boss4.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

namespace Zenith
{
    public class Boss1Cannon : Cannon
    {
        // Constructor
        // Sets the firepattern to a three-volley pattern
        // and the damage to 100. It also changes the color
        // of the projectile to make the lasers easily
        // seen by the player.
        public Boss1Cannon(Ship host)
            : base(host)
        {
            firePattern = new List<int> { 15, 15, 100 };
            damage = 100 * World.Instance.Difficulty;
            // projectileColor = ProjectileColor.Red;
        }
    }
}

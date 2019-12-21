//-----------------------------------------------------------
//File:   Boss2Cannon.cs
//Desc:   This class serves as the cannon for both Boss2 and
//        Boss5.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

namespace Zenith
{
    public class Boss2Cannon : Cannon
    {
        // Updates the fire pattern by decreasing it based on the
        // Boss's health.
        public override void Update()
        {
            if (reloadTime > 0)
            {
                double healthLeft = (double)Host.Health / Host.MaxHealth;
                int miniShot = (int)Math.Max(15 * healthLeft, 5);
                int gapTime = (int)Math.Max(100 * healthLeft, 60);

                damage = 200 - (int)(50 * healthLeft);

                FirePattern[0] = miniShot;
                FirePattern[1] = miniShot;
                FirePattern[2] = gapTime;
                --reloadTime;
            }
        }

        // Constructor
        // Sets the fire pattern to a volley of three and
        // sets the damage to 300. It also changes the color
        // of the projectile to make the lasers easily
        // seen by the player.
        public Boss2Cannon(Ship host)
            : base(host)
        {
            firePattern = new List<int> { 15, 15, 100 };
            damage = 200 * World.Instance.Difficulty;
            // ProjectileColor = ProjectileColor.Red;
        }
    }
}

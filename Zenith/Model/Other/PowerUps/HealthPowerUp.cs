using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    public class HealthPowerUp : PowerUp
    {
        // Constructor
        public HealthPowerUp(Vector2 position)
            : base (position)
        {
            gameImage = GameImage.HealthPowerUp;
            Health = true;
        }
    }
}

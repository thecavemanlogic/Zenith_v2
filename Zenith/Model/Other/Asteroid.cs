//-----------------------------------------------------------
//File:   Asteroid.cs
//Desc:   Holds the class controlling all asteroids.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class initializes all asteroids for 
    // preparation to be added to the World objects
    // collection.
    public class Asteroid : Enemy
    {
        // Does nothing when called so that
        // it will glide to a stop. We do not
        // need to update position or velocity
        // because they are handled by parent
        // class methods.
        public override void ShipLoop()
        {
        }

        // Constructor
        // Randomizes the velocity and sets the mass to the size squared.
        public Asteroid(Vector2 position, float size)
            : base(position)
        {
            gameImage = GameImage.Asteroid;

            this.size = new Vector2(size, size);
            mass = size * size;
            velocity.X = (-1 / size * 10) * 6000;
            velocity.Y = (float)(World.Instance.Random.NextDouble() * 2 - 1) * 8;
            type = GameObjectType.Asteroid;
            // imageSources = new List<string> { Util.GetSpriteFolderPath("Aster1.png") };
            worth = 20;
            bodyDamage = 20;
        }
    }
}

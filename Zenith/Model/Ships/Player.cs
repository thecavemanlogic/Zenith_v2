//-----------------------------------------------------------
//File:   Player.cs
//Desc:   Holds the code required to run the player ship.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class is responsible for reading the inputs from
    // the user and controlling the player ship to respond
    // appropriately.
    public class Player : Ship
    {
        // Defines the amount to accelerate the player ship by.
        private const float acceleration = 2000;

        // Reads the inputs from the PlayerController and adds the
        // correct accerlation or attempts to fire the cannon.
        public override void ShipLoop()
        {
            if (World.Instance.PlayerController.Up) AddForce(new Vector2(0, -acceleration));
            if (World.Instance.PlayerController.Down) AddForce(new Vector2(0, acceleration));
            if (World.Instance.PlayerController.Left) AddForce(new Vector2(-acceleration, 0));
            if (World.Instance.PlayerController.Right) AddForce(new Vector2(acceleration, 0));

            if (World.Instance.PlayerController.Fire) cannon.Fire();
        }

        // Constructor
        // Checks if the game is in cheat mode, then it sets the player's health accordingly.
        // It also sets Ship.onDeath to World's OnPlayerDeath method.
        public Player(Vector2 position)
            : base(position)
        {
            if (World.Instance.CheatsOn)
            {
                health = 0x7FFFFFFF;
                maxHealth = 0x7FFFFFFF;
            }
            else
            {
                health = 1000;
                maxHealth = 1000;
            }
            
            type = GameObjectType.Player;
            gameImage = GameImage.PlayerShip;
            cannon = new BasicCannon(this, 3);
            cannon.Accuracy = 0.25f;
            worth = 0;

            onDeath = World.Instance.EndGame;
        }
    }
}

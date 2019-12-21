//-----------------------------------------------------------
//File:   Enemy3.cs
//Desc:   This file holds the class dealing with the hardest
//        "small-tier" enemy of the game.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This enemy is a swarming enemy. It will
    // approach the player until it is within
    // a 200 unit radius. It will attempt to 
    // maintain this distance while it constantly
    // fires at the player.
    class Enemy3 : Enemy
    {
        // This method is in charge of maintaining a 200 unit
        // padding between the ship and the player. It aims the ship
        // towards the player and fires as fast as possible.
        public override void ShipLoop()
        {
            var playerOffset = World.Instance.Player.Position - position;
            cannon.Fire();

            if (playerOffset.Length() > 200)
            {
                Vector.SetLength(playerOffset, 500);
                AddForce(playerOffset);
            }
            else
            {
                Vector.SetLength(playerOffset, 500);
                AddForce(playerOffset * -1);
            }
            angle = Vector.GetAngle(World.Instance.Player.Position - position);
        }

        // Constructor
        /// <summary>
        /// Constructor for Enemy3
        /// </summary>
        /// <param name="position">The initial position of Enemy3</param>
        public Enemy3(Vector2 position)
            : base(position)
        {
            gameImage = GameImage.Enemy3;
            type = GameObjectType.Enemy3;
            imageRotation = (float)(Math.PI / 2);
            angle = (float)Math.PI;
            cannon = new BasicCannon(this, 120);
            worth = 50;
        }
    }
}

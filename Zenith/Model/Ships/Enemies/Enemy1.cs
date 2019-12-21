//-----------------------------------------------------------
//File:   Enemy1.cs
//Desc:   Holds the class that controls the most basic enemy
//        in the game.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This enemy is the easiest out of all the other
    // enemies. It actually doesn't even aim towards
    // the player. It just simply moves towards the
    // player and shoots.
    public class Enemy1 : Enemy
    {
        // Fires as towards the left of the screen
        // every 5 seconds and moves towards the
        // player with a force of 10 units.
        public override void ShipLoop() {
            
            cannon.Fire();
            MoveTo(World.Instance.Player.Position, 10);
        }

        // Constructor
        public Enemy1(Vector2 position)
           : base(position)
        {
            type = GameObjectType.Enemy1;
            gameImage = GameImage.Enemy1;

            // imageSources = new List<string> { Util.GetShipSpriteFolderPath("tankbase_01.png") };
            angle = (float)Math.PI;
            velocity.X = -50;
            this.position.X = World.Instance.EndX;
            cannon = new BasicCannon(this, 300);
            // cannon.ProjectileColor = ProjectileColor.Red;
            worth = 30;
        }
    }
}

//-----------------------------------------------------------
//File:   Enemy.cs
//Desc:   This holds the base information used for all enemies,
//        including the Enemy class and the EnemyState enum.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    public enum EnemyState
    {
        Sway,
        Ram,
        Flee,
        Pause
    }

    // Holds all the information needed for all
    // enemies.
    public class Enemy : Ship
    {
        // Shows the current state of the enemy.
        protected EnemyState state;

        // A numerical value that can be used for any purpose,
        // but is mainly used for timing events.
        protected int clock = 0;
        
        // A blank implementation of Ship.ShipLoop(). Only here
        // to quite the compiler's fussing.
        public override void ShipLoop() { }

        // Constructor
        public Enemy(Vector2 position)
            : base(position)
        {
            type = GameObjectType.Enemy;
            state = EnemyState.Sway;
        }
    }
}

//-----------------------------------------------------------
//File:   Wave2.cs
//Desc:   This class defines the enemies that will spawn in the second wave.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    class Wave2 : Wave
    {
        public override void Spawn()
        {
            for (int i = 0; i < difficulty + (level * 2); i++)
            {
                startingPos = new Vector2(World.Instance.Width, World.Instance.Random.Next(0, Convert.ToInt32(World.Instance.Height)));
                size = (float) World.Instance.Random.NextDouble() * 100 + 30;
         
                Enemy1 e1 = new Enemy1(startingPos);
                Asteroid a = new Asteroid(startingPos, size);
                Enemy2 e2 = new Enemy2(startingPos);
                AddEnemy(e1);
                AddEnemy(a);
                AddEnemy(e2);
            }
        }

       
    }
}

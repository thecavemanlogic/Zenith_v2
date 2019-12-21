//-----------------------------------------------------------
//File:   Wave3.cs
//Desc:   This class defines the enemies that will spawn in the third wave.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    class Wave3 : Wave
    {
        public override void Spawn()
        {
            waveCount = 0;
            for (int i = 0; i < difficulty + (level * 2); i++)
            {
                startingPos = new Vector2(World.Instance.Width, World.Instance.Random.Next(0, Convert.ToInt32(World.Instance.Height)));
                size = (float)World.Instance.Random.NextDouble() * 100 + 30;
                
                Enemy1 e1 = new Enemy1(startingPos);
                Enemy2 e2 = new Enemy2(startingPos);
                Enemy3 e3 = new Enemy3(startingPos);
                AddEnemy(e1);
                AddEnemy(e2);
                AddEnemy(e3);
                
            }
        }

    }
}

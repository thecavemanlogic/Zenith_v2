//-----------------------------------------------------------
//File:   Wave4.cs
//Desc:   This class defines the enemies that will spawn in the fourth wave.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    public class Wave4 : Wave
    {
        public override void Spawn()
        {
            for (int i = 0; i < difficulty + (level * 2); i++)
            {
                startingPos = new Vector2(World.Instance.Width, World.Instance.Random.Next(0, Convert.ToInt32(World.Instance.Height)));
 
                Enemy2 e2 = new Enemy2(startingPos);
                Enemy3 e3 = new Enemy3(startingPos);
                AddEnemy(e2);
                AddEnemy(e3);
            }
        }

    }
    
}

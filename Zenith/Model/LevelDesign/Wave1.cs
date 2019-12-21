//-----------------------------------------------------------
//File:   Wave1.cs
//Desc:   This class defines the enemies that will spawn in the first wave.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    public class Wave1 : Wave
    {

        public override void Spawn()
        {
            for (int i = 0; i < difficulty + (level * 2); i++)
            {

                startingPos = new Vector2(World.Instance.Width, World.Instance.Random.Next(0, Convert.ToInt32(World.Instance.Height)));
                size = (float) World.Instance.Random.NextDouble() * 100 + 30;

                Enemy1 e1 = new Enemy1(startingPos);
                AddEnemy(e1);
            }
        }

    }
}

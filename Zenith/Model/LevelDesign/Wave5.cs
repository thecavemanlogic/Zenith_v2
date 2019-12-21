//-----------------------------------------------------------
//File:   Wave5.cs
//Desc:   This class defines the enemies that will spawn in the fifth and final wave.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;

namespace Zenith
{
    class Wave5 : Wave
    {
        // This method is distinct from the Spawn methods because it spawns a boss determined by the current level.
        public override void Spawn()
        {
            var boss = World.Instance.SpawnBoss(level);
            AddEnemy(boss);
            // Defeating the final boss leads to finishing the game.
            if (level == 5) boss.OnDeath = World.Instance.OnGameFinish;
        }
    }
}

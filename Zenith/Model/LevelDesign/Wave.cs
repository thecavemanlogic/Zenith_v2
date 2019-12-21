//-----------------------------------------------------------
//File:   Wave.cs
//Desc:   This abstract class defines all the necessary methods
//        and variables in order for the Waves to progress and
//        Spawn enemies based on the level and difficulty.
//----------------------------------------------------------- 

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This abstract class defnines all the necessary methods and variables for the Waves.
    public abstract class Wave
    {
        protected int difficulty;
        protected int level;
        protected int waveCount = 0;
        protected Vector2 startingPos;
        protected float size;

        // Counts how many enemies in a wave.
        public int WaveCount { get { return waveCount; } set { waveCount = value; } }

        // Spawns a fixed number enemies at random positions near the edge of the screen.
        // This number and type of enemies are determined by the specific wave class (1-5).
        public virtual void Spawn() { }
        
        // Initializes waves based on difficulty and level
        public Wave()
        {
            this.difficulty = World.Instance.Difficulty;
            this.level = World.Instance.Level;
        }

        // A method to make adding enemies to the game easier.
        public void AddEnemy(Ship type)
        {
            type.OnDeath = World.Instance.DeathAction;
            waveCount++;
            World.Instance.AddObject(type);
        }
    }
}

//-----------------------------------------------------------
//File:   LevelManager.cs
//Desc:   This class keeps track of the Waves of enemies
//        that the player has to face as the game progresses.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;


// Steps. Change level manager so that it spawns enemies one after another in waves. Change waves so that they're randomized.
// Change waves so that they hold a list of enemies. Add those enemies to current wave. Update the game background according to the level number.


// Two ways to change the level manager: When it gets initialized and when it gets updated. Lets focus on when it gets updated.
// Lets use Update.




namespace Zenith
{

    public class LevelManager
    {
        // Instance variables

        // Controls the time between waves.
        private int spawnRate;

        // Keeps track of the time until the next wave.
        private int timeUntilNextWave;

        // Hold's the currentWave the player is facing.
        private Wave currentWave;

        // Used to start the Wave "Engine".
        private bool startingGame;

        // Properties

        public Wave CurrentWave { get { return currentWave; } set { currentWave = value; } }
        public bool StartingGame { set { startingGame = value; } }

        // Update makes sure that the right wave is spawning units at the right time.
        public void Update()
        {
            if (startingGame)
            {
                currentWave = CreateWave(World.Instance.CurrentWave);
                startingGame = false;
            }
            if (World.Instance.EnemiesLeftInWave > 0)
            {
                currentWave.WaveCount = World.Instance.EnemiesLeftInWave;
                World.Instance.EnemiesLeftInWave = 0;
            }
            if (currentWave.WaveCount == 0)
            {
                if (timeUntilNextWave > 0) --timeUntilNextWave;
                else
                {
                    currentWave = CreateWave(World.Instance.CurrentWave);
                    CurrentWave.Spawn();
                    timeUntilNextWave = spawnRate;
                }
            }
            
        }

        // Constructor for LevelManager. On First Load it assumes starting Game is true.
        public LevelManager()
        {
            startingGame = true;
            spawnRate = 100; 
            timeUntilNextWave = spawnRate;
        }

        // Used like a dictionary to create a wave corresponding to the int parameter.
        public Wave CreateWave(int nextWave)
        {
            switch(nextWave)
            {
                case 1:
                    return new Wave1();
                case 2:
                    return new Wave2();
                case 3:
                    return new Wave3();
                case 4:
                    return new Wave4();
                case 5:
                    return new Wave5();
            }
            return null;
        }
        
    }
}

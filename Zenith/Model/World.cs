using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Zenith
{
    public interface ISerialize
    {
        string Serialize();
        void Deserialize(string saveInfo);
    }

    public class World : ISerialize
    {

        public string Serialize() { return null; }
        public void Deserialize(string saveInfo) { }

        // Singleton Code

        // A singletone instance of World
        private static World instance = new World();

        public static World Instance { get { return instance; } }

        // Constructor
        private World()
        {
            gameTick = 0;
            level = 1;
            difficulty = 1;
            random = new Random();
            PlayerController = new GameController();
            EndX = 500;
            EndY = 500;
            StartX = 0;
            StartY = 0;

            objects = new List<GameObject>();
            collisionManager = new CollisionQuad(new Vector2(StartX, StartY), new Vector2(Width, Height), 0);
            collisionManager.Objects = objects;

            levelManager = new LevelManager();

            var r = new Rectangle();
            r.Contains(new Rectangle());

            // EndGame = () => ViewManager.TriggerEndGame();
        }

        // End of Singleton Code

        // Instance variables

        // A list of GameObjects that are active in Zenith
        private List<GameObject> objects;

        // Random object for all code in Zenith to use
        public Random random;

        // The amount of times Update() was called
        private int gameTick;

        // The base quadtree branch that handles collision
        private CollisionQuad collisionManager;

        // The name of the player
        private string playerName;

        // The current level the player is on
        private int level;

        // The score the player has currently
        private int score;

        // The precalculated time that will pass between each call to Update()
        private double deltaTime = 1.0 / 60.0;

        // The director of the executable (this is used to located images for sprites)
        private string directory = null;

        // This is a debugging variables that is used to count the number of collisions
        // that took place
        private int collisions = 0;

        // The difficulty of the game
        private int difficulty = 1;

        // The instance that is handling the different levels of the game
        private LevelManager levelManager;

        // Specifies whether cheat mode is on
        private bool cheatsOn = false;

        // Specifies whether the game is over
        private bool gameOver = false;

        // The wave the player is currently in
        private int currentWave = 1;

        // The amount of enemies required to destroy
        // before he can progress to the next wave
        private int enemiesLeftInWave = 0;

        // The method called when the player dies or
        // Boss5 is defeated
        private Action endGame;

        // Properties

        public float Width { get { return EndX - StartX; } }

        public float Height { get { return EndY - StartY; } }

        public float EndX { get; set; }

        public float EndY { get; set; }

        public float StartX { get; set; }

        public float StartY { get; set; }

        public float MidX { get { return (EndX - StartX) / 2; } }

        public float MidY { get { return (EndY - StartY) / 2; } }

        public Random Random { get { return random; } }

        public Ship Player { get; set; }

        public GameController PlayerController { get; set; }

        public string PlayerName { get { return playerName; } set { playerName = value; } }

        public int Level { get { return level; } set { level = value; } }

        public int Score { get { return score; } set { score = value; } }

        public int GameTick { get { return gameTick; } set { gameTick = value; } }

        public List<GameObject> Objects { get { return objects; } set { objects = value; } }

        public double DeltaTime { get { return deltaTime; } }

        public string Directory { get { return directory; } set { directory = value; } }

        public int Difficulty { get { return difficulty; } set { difficulty = value; } }

        public int Collisions { get { return collisions; } set { collisions = value; } }

        public LevelManager LevelManager { get { return levelManager; } }

        public bool CheatsOn { get { return cheatsOn; } }
        public bool GameOver { get { return gameOver; } set { gameOver = value; } }

        public int CurrentWave { get { return currentWave; } set { currentWave = value; } }

        public int EnemiesLeftInWave { get { return enemiesLeftInWave; } set { enemiesLeftInWave = value; } }

        public Action EndGame { get { return endGame; } set { endGame = value; } }

        // Methods

        // Sets the screen dimensions to the values given
        public void SetScreenDimensions(float startX, float startY, float endX, float endY)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            // collisionManager = new CollisionQuad(new Vector2(StartX, StartY), new Vector2(Width, Height), 0);
        }

        // Maps the absolute position on a physical screen to the virtual position
        // according to World's dimensions.
        public Vector2 GetScreenPosition(float x, float y)
        {
            return new Vector2((EndX - StartX) * x, (EndY - StartY) * y);
        }

        // Called every 1.60 of a second. It reacts to a player's call
        // to save and load. When the game is not paused, all GameObjects
        // will be updates and collision checks will take place. The
        // LevelManager is also called in this method.
        public void Update()
        {
            if (PlayerController.Save) Save("Zenith.txt");

            if (PlayerController.Load)
            {
                Load("Zenith.txt");
            }

            if (!PlayerController.Pause)
            {
                for (int i = 0; i < objects.Count; ++i)
                {
                    objects[i].Update();

                    if (objects[i].Destroy)
                    {
                        RemoveObject(objects[i]);
                        // fix index after removal
                        --i;
                    }
                }

                collisions = 0;
                collisionManager.CheckForCollisions();

                levelManager.Update();

                ++gameTick;
            }

        }

        // This method is called when Boss5 is defeated
        public void OnGameFinish()
        {
            if (Player.Health > 0) World.Instance.Score += (54000 - World.Instance.GameTick);
            // ViewManager.TriggerEndGame();
        }

        // This method allows the game to progress by keeping track of how many enemies are left in the wave.
        public void DeathAction()
        {
            LevelManager.CurrentWave.WaveCount--;
            if (LevelManager.CurrentWave.WaveCount == 0)
            {
                if (World.Instance.CurrentWave < 5)
                {
                    World.Instance.CurrentWave++;
                }
                else
                {
                    World.Instance.Level++;
                    World.Instance.CurrentWave = 1;
                }
            }

        }
        // This method adds an object to the GameObject list
        // and adds an accompanying Sprite.
        public void AddObject(GameObject gameObject)
        {
            objects.Add(gameObject);
            // ViewManager.AddSprite(gameObject);
        }

        // Removes an object from the GameObject list
        // and removes an accompanying Sprite.
        public void RemoveObject(GameObject gameObject)
        {
            objects.Remove(gameObject);
            // ViewManager.RemoveSprite(gameObject);
        }

        // Turns cheat mode on.
        public void EnableCheatMode()
        {
            cheatsOn = true;
        }

        // Turns cheat mode off.
        public void DisableCheatMode()
        {
            cheatsOn = false;
        }

        // Spawns a boss with a valid ID. Used by the final Wave class.
        public Ship SpawnBoss(int bossID)
        {
            Ship boss = null;
            var startingPosition = new Vector2(EndX, EndY / 2);

            switch (bossID)
            {
                case 1:
                    boss = new Boss1(startingPosition);
                    break;
                case 2:
                    boss = new Boss2(startingPosition);
                    break;
                case 3:
                    boss = new Boss3(startingPosition);
                    break;
                case 4:
                    boss = new Boss4(startingPosition);
                    break;
                case 5:
                    boss = new Boss5(startingPosition);
                    break;
            }
            return boss;
        }

        // This method creates the player and sets its initial properties
        public void CreatePlayer()
        {
            var p = new Player(new Vector2(90, EndY / 2));
            AddObject(p);
            Player = p;
            Vector.SetLength(p.Velocity, 0);
            p.OnDeath = OnGameFinish;
        }

        // This method resets the instance of World. 
        public void Reset()
        {
            playerName = "";
            level = 1;
            score = 0;
            gameTick = 0;
            currentWave = 1;
            enemiesLeftInWave = 0;
            cheatsOn = false;

            for (int i = objects.Count - 1; i > 0; i--)
            {
                objects[i].Destroy = true;
            }
        }

        // Reads a list of strings from the file specifed by filename and uses each
        // the comma seperated values in each line to populate the properties of each
        // game object type specified by the first value in the line. In addition, it sets
        // the non static death action for each game object according to its type.
        public void Load(string filename)
        {
            Reset();

            if (File.Exists(filename))
            {
                using (StreamReader reader = new StreamReader(filename, true))
                {
                    playerName = reader.ReadLine();
                    gameTick = Convert.ToInt32(reader.ReadLine());
                    level = Convert.ToInt32(reader.ReadLine());
                    score = Convert.ToInt32(reader.ReadLine());
                    currentWave = Convert.ToInt32(reader.ReadLine());
                    enemiesLeftInWave = Convert.ToInt32(reader.ReadLine());
                    cheatsOn = Convert.ToBoolean(reader.ReadLine());
                    while (reader.Peek() != -1)
                    {
                        string saveInfo = reader.ReadLine();
                        string objectType = saveInfo.Substring(0, saveInfo.IndexOf(","));
                        string objectInfo = saveInfo.Substring(saveInfo.IndexOf(",") + 1);
                        GameObject obj = CreateInstanceOf(objectType);
                        obj.Deserialize(objectInfo);
                        AddObject(obj);
                    }
                }
                foreach (GameObject obj in objects)
                {
                    if (obj is Boss5)
                    {
                        Boss5 b = obj as Boss5;
                        b.OnDeath = OnGameFinish;
                    }
                    else if (obj is Enemy)
                    {
                        Enemy e = obj as Enemy;
                        e.OnDeath = DeathAction;
                    }
                    else if (obj is Player)
                    {
                        Player p = obj as Player;
                        Player = p;
                        p.OnDeath = OnGameFinish;
                    }

                }
            }
        }



        // Saves the game as a text file named filename.
        // It does this by first deleting any text files under the same name,
        // creating a new file under filename, and then writing 
        // the serialized version of all the necessary world variables and 
        // game objects to the file and closing it.
        public void Save(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (StreamWriter writer = new StreamWriter(filename, true))
            {
                writer.WriteLine(playerName);
                writer.WriteLine(gameTick);
                writer.WriteLine(level);
                writer.WriteLine(score);
                writer.WriteLine(currentWave);
                writer.WriteLine(LevelManager.CurrentWave.WaveCount);
                writer.WriteLine(cheatsOn);
                foreach (GameObject obj in this.objects)
                {
                    if (!(obj is HealthBar))
                    {
                        writer.WriteLine(obj.Serialize());
                    }

                }
            }
        }

        // Used in the Load method in order to create instances of Game Objects, which can
        // then have their properties updated by the rest of the comma seperated values.
        public GameObject CreateInstanceOf(string objectType)
        {
            Vector2 tempVector2 = new Vector2(1, 1);
            switch (objectType)
            {
                case "Item":
                    return new Item(tempVector2);
                case "Asteroid":
                    return new Asteroid(tempVector2, 0);
                case "Laser":
                    return new Laser(tempVector2, tempVector2, 0, true);
                case "Enemy1":
                    return new Enemy1(tempVector2);
                case "Enemy2":
                    return new Enemy2(tempVector2);
                case "Enemy3":
                    return new Enemy3(tempVector2);
                case "Boss1":
                    return new Boss1(tempVector2);
                case "Boss2":
                    return new Boss2(tempVector2);
                case "Boss3":
                    return new Boss3(tempVector2);
                case "Boss4":
                    return new Boss4(tempVector2);
                case "Boss5":
                    return new Boss5(tempVector2);
                case "Player":
                    return new Player(tempVector2);
                case "HealthBar":
                    return new HealthBar(null);
            }
            return null;
        }
    }
}

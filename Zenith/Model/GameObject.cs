//-----------------------------------------------------------
//-----------------------------------------------------------
//File:   GameObject.cs
//Desc:   Holds the class that holds all objects in Zenith
//        and two enums to catagorize them all.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Zenith
{
    // Specific categories
    public enum GameObjectType
    {
        Unknown,
        Ship,
        Item,
        Asteroid,
        Laser,
        BackgroundElement,
        Enemy,
        Enemy1,
        Enemy2,
        Enemy3,
        Boss1,
        Boss2,
        Boss3,
        Boss4,
        Boss5,
        Player,
        HealthBar
    }

    // General categories
    public enum GameTag
    {
        None,
        Ship,
        Projectile,
        Item
    }

    // This class is responsible for all of the moving objects in Zenith.
    // It deals with the physics, animations, and logic that help make
    // the game come alive.
    public abstract class GameObject : ISerialize
    {
        // The specific type of the object
        protected GameObjectType type;

        // Specifies whether the object can collide with other objects
        protected bool collidable = true;

        // Specifies whether this object should be removed from World's
        // list of objects
        protected bool destroy = false;

        // The position, velocity, and size of the GameObject
        protected Vector2 position, velocity, size;

        protected Vector2 scale = Vector2.One;

        // The maximum speed any GameObject can have
        protected const float maxSpeed = 2000;

        // The value to be multiplied with velocity to create a friction effect
        // Unused
        protected float deceleration = 1;

        // The angle of the GameObject, in radians
        protected float angle = 0;


        // A list of images the GameObject can resemble
        // protected List<string> imageSources;

        // The offset rotation of the image compared to angle,
        // in radians. This is used to correct images in the file
        // so that the source images do not need to be altered.
        protected float imageRotation = (float)(Math.PI / 2);

        // The index into imageSources that is selecting
        // the current image to render in the View.
        // protected int imageIndex = 0;


        protected GameImage gameImage;

        // The mass of the object. This is used to:
        // 1.   Calculate the acceleration of an object
        //      given a force (using a = f / m).
        // 2.   Calculate momentum for collisions. The
        //      more mass an object has, the less it
        //      should be affected by other objects.
        protected float mass = 1;

        // The general type of the object (None, Ship, Projectile, or Item)
        protected GameTag tag = GameTag.None;

        // Specifies whether the object should be serialized or not.
        // This is used for the HealthBars and Sensors. Each of these
        // objects are automatically created whenever the correct
        // GameObject is initialized.
        protected bool canSerialize = true;

        // Properties

        public Vector2 Position { get { return position; } }

        public Vector2 Velocity { get { return velocity; } set { velocity = value; } }

        public Vector2 Size { get { return size; } }

        public GameObjectType Type { get { return type; } }

        public bool Collidable { get { return collidable; } }

        public bool Destroy { get { return destroy; } set { destroy = value; } }

        // public List<string> ImageSources { get { return imageSources; } }

        public float ImageRotation { get { return imageRotation; } set { imageRotation = value; } }

        // public int ImageIndex { get { return imageIndex; } set { imageIndex = value; } }

        public float Mass { get { return mass; } }

        public GameTag Tag { get { return tag; } }

        public float Angle { get { return angle; } }

        public bool CanSerialize { get { return canSerialize; } }

        public GameImage GameImage { get { return gameImage; } set { gameImage = value; } }

        public Vector2 Scale { get { return scale; } }


        // Methods

        // Method to be called whenever the object is found to
        // collide with another.
        public virtual void OnCollision(GameObject gameObject) { }

        // The method is called every 1/60th of a second. Used
        // to update the specific logic of the GameObject
        public abstract void Loop();

        // The method is called every 1/60th of a second. Used
        // to update the logic used by all GameObjects
        public void Update()
        {
            Loop();
            if (velocity.Length() > maxSpeed)
            {
                Vector.SetLength(velocity, (float)maxSpeed);
            }
            position += velocity * (float)World.Instance.DeltaTime;

            if (position.Y < World.Instance.StartY - 1)
            {
                position.Y = World.Instance.StartY - 1;
                velocity.Y = 0;
            }
            if (position.Y > World.Instance.EndY + 1)
            {
                position.Y = World.Instance.EndY + 1;
                velocity.Y = 0;
            }
            if (position.X < World.Instance.StartX - 1)
            {
                position.X = World.Instance.StartX - 1;
                velocity.X = 0;
            }
            if (position.X > World.Instance.EndX + 30)
            {
                position.X = World.Instance.EndX + 30;
                velocity.X = 0;
            }
        }

        // Accelerates the object in a given direction
        public void AddForce(Vector2 f)
        {
            this.velocity += f / (float)mass;
        }

        // Constructor
        public GameObject(Vector2 position)
        {
            this.position = position;
            velocity = new Vector2(0, 0);
            size = new Vector2(1, 1);
            type = GameObjectType.Unknown;
        }

        // This method turns all the necessary variables relating to a Game Object to strings so that the Game Object
        // can be saved.
        public virtual string Serialize()
        {
            /*string serializedImageSources = "";
            foreach (string image in imageSources)
            {
                serializedImageSources += image + ':';
            }*/

            return type.ToString() + ',' + collidable.ToString() + ',' + Destroy.ToString() +
                ',' + position.ToString() + ',' + velocity.ToString() + ',' + size.ToString() +
                ',' + deceleration.ToString() + ',' + angle.ToString() /* + ',' + serializedImageSources*/ +
                ',' + imageRotation.ToString() + ',' /*+ ImageIndex.ToString()*/ + ',' + mass.ToString() + ',' + tag.ToString();
        }


        // Deserialze takes a string of comma seperated values (with a few nested colon seperated values) and loads their values
        // into the Game Object properties.

        public virtual void Deserialize(string saveInfo)
        {
            // saveInfo includes everything but the gameObjectType
            string[] savedValues = saveInfo.Split(',');

            collidable = Convert.ToBoolean(savedValues[0]);
            Destroy = Convert.ToBoolean(savedValues[1]);

            // position, velocity, and size Vector2s
            string[] xNy1 = savedValues[2].Split(':');
            position = new Vector2((float)Convert.ToDouble(xNy1[0]), (float)Convert.ToDouble(xNy1[1]));
            string[] xNy2 = savedValues[3].Split(':');
            velocity = new Vector2((float)Convert.ToDouble(xNy2[0]), (float)Convert.ToDouble(xNy2[1]));
            string[] xNy3 = savedValues[4].Split(':');
            size = new Vector2((float)Convert.ToDouble(xNy3[0]), (float)Convert.ToDouble(xNy3[1]));

            // deacceleartion, and angle
            deceleration = (float)Convert.ToDouble(savedValues[5]);
            angle = (float)Convert.ToDouble(savedValues[6]);

            // imagesources, rotation, and index
            /*string[] Isources = savedValues[7].Split(':');
            foreach(string source in Isources)
            {
                imageSources.Add(source);
            }*/
            ImageRotation = (float)Convert.ToDouble(savedValues[7]);

            // mass, tag (double, GameTag)
            mass = (float)Convert.ToDouble(savedValues[9]);
            Enum.TryParse(savedValues[10], out GameTag tag);
        }

        // Got this method from here: https://stackoverflow.com/questions/186653/get-the-index-of-the-nth-occurrence-of-a-string
        // Returns the index of the nth occurance of a match within a string.
        public int IndexOfNthOccurance(string s, string match, int n)
        {
            int i = 1;
            int index = 0;

            while (i <= n && (index = s.IndexOf(match, index + 1)) != -1)
            {
                if (i == n)
                    return index;
                i++;
            }
            return -1;
        }
    }
}

//-----------------------------------------------------------
//File:   Laser.cs
//Desc:   Holds the class responsible for lasers in Zenith.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class is responsible for handling laser collisions
    // and destruction upon collision with a Ship object.
    class Laser : GameObject
    {
        // The amount of health the laser will remove from the Ship it
        // collides with.
        private int damage;

        // Boolean marking if the laser was launched from the player or not.
        private bool isFromPlayer;

        // Properties

        public bool IsFromPlayer
        {
            get { return isFromPlayer; }
        }

        public int Damage
        {
            get { return damage; }
        }

        // Methods

        // Checks if the object it collided with is a ship. If it has,
        // then it will destroy itself.
        public override void OnCollision(GameObject gameObject)
        {
            // only does damage to the opponenet
            if (gameObject.Tag == GameTag.Ship)
            {
                // Found here that C# has a xor operator
                // Source: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/boolean-logical-operators
                if (isFromPlayer ^ (gameObject is Player))
                {
                    Destroy = true;
                }
            }
        }

        // Checks if the laser is not within the outer bounds of the world.
        // If it is, then the laser will destroy itself.
        public override void Loop()
        {
            if (position.X < World.Instance.StartX ||
                position.X > World.Instance.EndX ||
                (position.Y < World.Instance.StartY && !IsFromPlayer) ||
                (position.Y > World.Instance.EndY && !IsFromPlayer)) Destroy = true;
        }

        // Constructor
        // Initializes the laser class by setting the required variables.
        // It also limits the size of the laser by forcing 100 to be the
        // max width and height.
        public Laser(Vector2 position, Vector2 velocity, int damage, bool isFromPlayer)
            : base(position)
        {
            gameImage = GameImage.LaserBlue;
            imageRotation = 0;

            this.isFromPlayer = isFromPlayer;
            this.velocity = velocity;
            this.damage = damage;

            int size2 = Math.Min(damage, 100);
            size = new Vector2(size2, size2);
            angle = Vector.GetAngle(velocity);

            type = GameObjectType.Laser;
            tag = GameTag.Projectile;
        }

        // This method turns all the necessary variables of Laser into strings in order to save them.
        public override string Serialize()
        {
            throw new NotImplementedException();
            // return base.Serialize() + ',' + damage.ToString() + ',' + isFromPlayer.ToString() + ',' + imageSources[0];
        }

        // This method takes a list of comma seperated values and sets the properties of laser accordingly.
        public override void Deserialize(string saveInfo)
        {
            int index = IndexOfNthOccurance(saveInfo, ",", 11);

            string gameObjectSaveInfo = saveInfo.Substring(0, index);
            base.Deserialize(gameObjectSaveInfo);

            string[] laserSaveInfo = saveInfo.Substring(index + 1, saveInfo.Length - index - 1).Split(',');
            damage = Convert.ToInt32(laserSaveInfo[0]);
            isFromPlayer = Convert.ToBoolean(laserSaveInfo[1]);
            // imageSources[0] = laserSaveInfo[2];
        }
    }
}

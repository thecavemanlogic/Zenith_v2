//-----------------------------------------------------------
//File:   Sensor.cs
//Desc:   Holds the class responsible for detecting objects
//        in a defined space.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // The purpose of this class is to detect GameObjects in a certain space
    // surrounding a host GameObject.
    class Sensor : GameObject
    {
        // The external method to be called once a object is "sensed."
        Action<GameObject> onSense;

        // The host object that has the sensor.
        GameObject host;

        // Updates the position to be equal to the 
        // position of the host and the destroy
        // variable to be the same as the host's.
        public override void Loop()
        {
            position = host.Position;
            destroy = host.Destroy;
        }

        // Here is the "sensing" method. Sensor is a GameObject, so
        // it can be collided with like all other objects in Zenith.
        // Whenever a GameObject is sensed that is not the host, then
        // it will call the onSense action with the given gameObject
        // as an argument.
        public override void OnCollision(GameObject gameObject)
        {
            if (gameObject != host) onSense(gameObject);
        }

        // Constructor
        // Radius is not an actual radius of a circle, but it is
        // half the length of each side for the sensor.
        public Sensor(GameObject host, Action<GameObject> callback, float radius)
            : base(host.Position)
        {
            size = new Vector2(radius * 2, radius * 2);
            this.host = host;
            onSense = callback;

            canSerialize = false;

            // imageSources = new List<string> { };

            World.Instance.AddObject(this);
        }
    }
}

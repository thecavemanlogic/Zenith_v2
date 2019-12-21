//-----------------------------------------------------------
//File:   Ship.cs
//Desc:   Defines the abstract Ship class.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class controls all Enemies, Bosses, Players, and Asteroids.
    // It also defines some common methods used all all as well.
    public abstract class Ship : GameObject
    {
        // The percentage to decrease the force applied after colliding by.
        const float collisionDamper = 0.50f;

        // instance variables

        // moved to Cannon.cs (kept here to conserve serialization)
        protected int reloadTime = 0;

        // moved to Cannon.cs (kept here to conserve serialization)
        protected int fireRate = 15;

        // The amount of health to remove from other ships that collide
        // with this one.
        protected int bodyDamage = 100;

        // kept here to conserve serialization
        protected double direction = 0;

        // moved to Cannon.cs (kept here to conserve serialization)
        protected double accuracy = 0.05;

        // moved to Cannon.cs (kept here to conserve serialization)
        protected int laserDamage = 40;

        // kept here to conserve serialization
        protected double laserSpeed = 400;

        // The current amount of health the ship
        // has.
        protected int health = 120;

        // The maximum possible amount of health
        // a ship can have.
        protected int maxHealth = 120;

        // The change in position of a ship to make
        // the shake happen.
        private Vector2 shakeOffset;

        // The game ticks left until a shake is over.
        private int shakeTime = 0;

        // The amount of game ticks that a shake should
        // last for.
        private int shakeDuration = 30;

        // The callback method that is called whenever the
        // ship is destroyed.
        protected Action onDeath;

        // The cannon that will be used by the player.
        protected Cannon cannon;

        // The amount of points added to the player's score after it is destroyed.
        protected int worth;
        
        // Properties

        public int Health { get { return health; } set { health = value; } }
        public int BodyDamage { get { return bodyDamage; } set { bodyDamage = value; } }
        public Vector2 ShakeOffSet { get { return shakeOffset; } }
        public Action OnDeath { get { return onDeath; } set { onDeath = value; } }
        public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
        public int Worth { get { return worth; } }

        // Methods

        // This collision check only will respond to 2 kinds of objects:
        // Ship:
        //      The current ship object will Shake and will bounce
        //      off the other ship. If one of the ships is a Player,
        //      then this.health will be decremented by the other
        //      ship's body damage.
        // Laser:
        //      If this ship is a Player and the laser is not from
        //      a player, or vice versa, then decrement health by
        //      the laser's Damage value.
        public override void OnCollision(GameObject gameObject)
        {
            var offset = (position - gameObject.Position);
            switch (gameObject.Tag)
            {
                case GameTag.Ship:
                    if ((this is Player) != (gameObject is Player))
                    {
                        var ship = (Ship)gameObject;
                        health -= ship.BodyDamage;
                        Shake();
                    }
                    AddForce(offset * ((gameObject.Velocity.Length()) * gameObject.Mass / mass) * collisionDamper);
                    break;
                case GameTag.Projectile:
                    var laser = (Laser)gameObject;

                    if (laser.IsFromPlayer != this is Player)
                    {
                        health -= laser.Damage;
                        AddForce(offset * (gameObject.Velocity.Length() * gameObject.Mass / mass) * collisionDamper);
                        Shake();
                    }
                    break;
            }
        }

        // Increment shakeTime to make the Ship
        // Shake.
        protected void Shake()
        {
            shakeTime = shakeDuration;
        }

        // Moves the ship towards a specified position given
        // an acceleration.
        public void MoveTo(Vector2 destination, float acceleration)
        {
            var f = (destination - position) - (velocity / 60);
            Vector.SetLength(f, mass * acceleration);
            AddForce(f);
        }

        public void Die()
        {
            destroy = true;

            if (World.Instance.Player.Health > 0)
            {
                World.Instance.Score += worth;
            }

            if (World.Instance.Random.NextDouble() < 0.2)
            {
                //var p = new DamagePowerUp(position);
                var p = PowerUp.GetRandomPowerUp(position);
                World.Instance.AddObject(p);
            }
            

            onDeath?.Invoke();
        }

        // Calls ShipLoop(). Also checks if the ship's health is
        // less than or equal to 0 and deals with that accordingly.
        // This method also handles the shaking of all ships, updates
        // the cannon, and continuously slows down all ships.
        public override void Loop()
        {
            ShipLoop();
            if (health <= 0)
            {
                Die();
                return;
            }
            if (shakeTime > 0)
            {
                position -= shakeOffset;

                float x = (float)(World.Instance.Random.NextDouble() * 2 - 1) * 4;
                float y = (float)(World.Instance.Random.NextDouble() * 2 - 1) * 4;
                shakeOffset = new Vector2(x, y);
                --shakeTime;

                position += shakeOffset;
            }
            else
            {
                Vector.SetLength(shakeOffset, 0);
            }
            if (position.X > World.Instance.EndX) AddForce(new Vector2(-500, 0));
            velocity *= 0.97f;

            cannon.Update();
        }

        // A method specifically called to update
        // all ships.
        public abstract void ShipLoop();

        // Applys a powerup's properties to the ship.
        public void ApplyPowerUp(PowerUp power)
        {
            cannon.Damage += power.Damage;
            if (power.Health) health = maxHealth;
            for (int i = 0; i < cannon.FirePattern.Count; ++i)
            {
                if (cannon.FirePattern[i] > 0) cannon.FirePattern[i] -= 1;
            }

            if (power.Damage != 0) size *= 1.20f;
        }

        // Constructor
        public Ship(Vector2 position)
            : base(position)
        {
            type = GameObjectType.Ship;
            size = new Vector2(48, 48);
            shakeOffset = new Vector2(0, 0);
            mass = 50;
            tag = GameTag.Ship;

            cannon = new Cannon(this);

            var h = new HealthBar(this);
            World.Instance.AddObject(h);
        }

        // This method turns all the necessary Ship variables into strings and turns them into a line of
        // comma seperated values so that they can be loaded in later.
        public override string Serialize()
        {
            return base.Serialize() + ',' + reloadTime.ToString() + ',' + fireRate.ToString() + ',' +
                bodyDamage.ToString() + ',' + direction.ToString() + ',' + accuracy.ToString() + ',' +
                laserDamage.ToString() + ',' + laserSpeed.ToString() + ',' + health.ToString() + ',' +
                maxHealth.ToString() + ',' + shakeOffset.ToString() + ',' + shakeTime.ToString() + ',' +
                shakeDuration.ToString() + ',' + cannon.ToString() + ',' + worth.ToString();
        }
        // This method loads in all the necessary variables for a ship from a line of comma seperated values.
        public override void Deserialize(string saveInfo)
        {

            int index = IndexOfNthOccurance(saveInfo, ",", 11);

            string gameObjectSaveInfo = saveInfo.Substring(0, index);
            base.Deserialize(gameObjectSaveInfo);

            string[] shipSaveInfo = saveInfo.Substring(index + 1, saveInfo.Length - index - 1).Split(',');

            reloadTime = Convert.ToInt32(shipSaveInfo[0]);
            fireRate = Convert.ToInt32(shipSaveInfo[1]);
            bodyDamage = Convert.ToInt32(shipSaveInfo[2]);

            direction = Convert.ToDouble(shipSaveInfo[3]);
            accuracy = Convert.ToDouble(shipSaveInfo[4]);
            laserDamage = Convert.ToInt32(shipSaveInfo[5]);
            laserSpeed = Convert.ToDouble(shipSaveInfo[6]);

            health = Convert.ToInt32(shipSaveInfo[7]);
            maxHealth = Convert.ToInt32(shipSaveInfo[8]);

            string[] xNy = shipSaveInfo[9].Split(':');
            shakeOffset = new Vector2((float)Convert.ToDouble(xNy[0]), (float)Convert.ToDouble(xNy[1]));
            shakeTime = Convert.ToInt32(shipSaveInfo[10]);
            shakeDuration = Convert.ToInt32(shipSaveInfo[11]);


            string[] cannonValues = shipSaveInfo[12].Split(':');

            cannon.Host = this;
            cannon.ReloadTime = Convert.ToInt32(cannonValues[0]);
            char[] charSeparators = new char[] { ';' };
            string[] firePatternValues = cannonValues[1].Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);
            foreach (string value in firePatternValues)
            {
                cannon.FirePattern.Add(Convert.ToInt32(value));
            }
            cannon.FireSequence = Convert.ToInt32(cannonValues[2]);
            cannon.Damage = Convert.ToInt32(cannonValues[3]);
            cannon.Accuracy = (float)Convert.ToDouble(cannonValues[4]);
            cannon.ProjectileSpeed = (float)Convert.ToDouble(cannonValues[5]);
            worth = Convert.ToInt32(shipSaveInfo[13]);
        }

    }
}

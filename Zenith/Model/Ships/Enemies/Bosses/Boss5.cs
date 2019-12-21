//-----------------------------------------------------------
//File:   Boss5.cs
//Desc:   This file holds the class responsible for controlling
//        Boss5.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class controls Boss5. It also controls the specialized
    // movement for the object as well as the internal state of Boss5.
    class Boss5 : Enemy
    {
        // The amount, in radians, at which to increment the boss' angle
        // every time Update is called.
        private float spinSpeed = 0.5f;

        // The sensor for Boss5. This allows the boss to avoid the player's
        // lasers.
        private Sensor sensor;

        // This is a force to be added to velocity as a result of "sensed"
        // lasers.
        private Vector2 avoid = new Vector2(0, 0);

        // This is a checkpoint to mark when to start the next phase of Boss5's
        // cycle. This value is a multiple of 1000, and is calculated given
        // this formula: nextDamageMarker = ((health / 1000) - 1) * 1000;
        // Whenever the boss' health reaches or goes below this value, the
        // current state is set to Ram and the formula is used to get the
        // next value of nextDamageMarker.
        private int nextDamageMarker;

        // This Vector2 marks where Boss5 should stand when it is in Flee mode.
        private Vector2 goal = new Vector2(0, 0);

        // Represents the amount of smaller enemy ships left on the screen to
        // destroy before the boss will return to its Sway state.
        private int minionsLeft = 0;

        // If the object is a laser from the player and the boss' state
        // is not Sway, then the laser will appear to bounce off the ship
        // by creating a new Enemy laser to replace the player's.
        public override void OnCollision(GameObject gameObject)
        {
            if (state != EnemyState.Sway && gameObject is Laser)
            {
                var laser = (Laser)gameObject;
                if (laser.IsFromPlayer)
                {
                    var velocity = laser.Position - position;
                    Vector.SetLength(velocity, laser.Velocity.Length());
                    var newLaser = new Laser(laser.Position, velocity, 40, false);
                    World.Instance.AddObject(newLaser);
                }
            }
            else
            {
                base.OnCollision(gameObject);
            }
        }

        // This is the callback method for whenever one of the boss' spawned
        // ships is destroyed. When minionsLeft == 0, then the boss will resume
        // its Sway state.
        public void OnMinionDeath()
        {
            --minionsLeft;
            if (minionsLeft == 0)
            {
                state = EnemyState.Sway;
                nextDamageMarker = ((health - 1) / 1000) * 1000;
            }
        }

        // This method is split up into 4 different states:
        // Sway:
        //      Like all other bosses, this state will make the boss
        //      sway vertically up and down. This is the only state
        //      that the boss is not invincible. Here is when the
        //      boss will periodically fire at the player.
        // Ram:
        //      In this state, the boss will constantly move towards
        //      towards the player for about 10 seconds.
        // Flee:
        //      Here, the boss will move towards the goal Vector2.
        // Pause:
        //      Once at the goal, the boss will spawn 10 enemies for
        //      the player to defeat before resuming to the Sway
        //      state.

        // Note:
        //      When the boss is invincible, then it will spin and
        //      deflect all of the lasers from the player, not
        //      taking any damage.
        public override void ShipLoop()
        {
            switch (state)
            {
                case EnemyState.Sway:
                    if (avoid.Length() > 1)
                    {
                        AddForce(avoid);
                        avoid /= 2;
                    }
                    else MoveTo(new Vector2(World.Instance.EndX * 0.75f, World.Instance.EndY / 2), 10);

                    angle = Vector.GetAngle(World.Instance.Player.Position - position);
                    cannon.Fire();

                    if (health <= nextDamageMarker)
                    {
                        state = EnemyState.Ram;
                        clock = 0;
                    }
                    break;
                case EnemyState.Ram:
                    angle += spinSpeed;
                    MoveTo(World.Instance.Player.Position, 10);
                    ++clock;
                    if (clock >= 600)
                    {
                        state = EnemyState.Flee;
                        goal = new Vector2(World.Instance.EndX, World.Instance.EndY / 2);
                    }
                    break;
                case EnemyState.Flee:
                    angle += spinSpeed;
                    MoveTo(goal, 10);
                    if ((goal - position).Length() < 100)
                    {
                        state = EnemyState.Pause;
                        clock = 0;
                        minionsLeft = 10;
                    }
                    break;
                case EnemyState.Pause:
                    angle += spinSpeed;
                    if (clock < 300 && clock % 30 == 0)
                    {
                        double angle = Math.PI * (World.Instance.Random.NextDouble() - 0.5) + Math.PI;
                        float x = (float)Math.Cos(angle) * 100;
                        float y = (float)Math.Sin(angle) * 100;
                        var enemy = new Enemy2(position - new Vector2(x, y));
                        enemy.OnDeath = OnMinionDeath;
                        World.Instance.AddObject(enemy);
                    }
                    ++clock;
                    break;
            }

        }

        // This method is called whenever the sensor receives a GameObject
        // in a "collision." This is used to detect lasers that were fired
        // from the player, so that the boss can avoid the lasers from
        // the player. Also, when the boss is in the top or bottom 50 units
        // from the edge of screen, it will receive a shove towards the
        // middle so that it does not get stuck in the corner.
        public void OnSense(GameObject gameObject)
        {
            if (gameObject is Laser)
            {
                var laser = (Laser)gameObject;
                if (laser.IsFromPlayer)
                {
                    var offset = position - laser.Position;
                    float dist = offset.Length();
                    Vector.SetLength(offset, 1400000000);
                    avoid = offset / (dist * dist);

                    if (avoid.Y < 0 && position.Y < 50)
                    {
                        avoid.Y = 100 * mass;
                    }
                    if (avoid.Y > 0 && position.Y > World.Instance.EndY - 50)
                    {
                        avoid.Y = -100 * mass;
                    }

                }
            }
        }

        // Constructor
        public Boss5(Vector2 position)
            : base(position)
        {
            gameImage = GameImage.Boss5;
            type = GameObjectType.Boss5;
            mass = 400;
            health = 4000;
            maxHealth = 4000;

            nextDamageMarker = ((health / 1000) - 1) * 1000;
            state = EnemyState.Sway;

            cannon = new Boss2Cannon(this);
            cannon.Damage = 180 * World.Instance.Difficulty;

            sensor = new Sensor(this, OnSense, 200);

            worth = 500;

            size = new Vector2(256, 256);
        }

        // ???
        public override string Serialize()
        {
            return base.Serialize() + ',' + spinSpeed.ToString() + ',' + avoid.ToString() + ',' +
                nextDamageMarker.ToString() + ',' + goal.ToString() + ',' + minionsLeft.ToString();
        }

        // ???
        public override void Deserialize(string saveInfo)
        {
            int index = IndexOfNthOccurance(saveInfo, ",", 24);

            string enemySaveInfo = saveInfo.Substring(0, index);
            base.Deserialize(enemySaveInfo);

            string[] boss5SaveInfo = saveInfo.Substring(index + 1, saveInfo.Length - index - 1).Split(',');

            spinSpeed = (float)Convert.ToDouble(boss5SaveInfo[0]);
            string[] xNy = boss5SaveInfo[1].Split(':');
            avoid = new Vector2((float)Convert.ToDouble(xNy[0]), (float)Convert.ToDouble(xNy[1]));
            nextDamageMarker = Convert.ToInt32(boss5SaveInfo[2]);
            string[] xNy1 = boss5SaveInfo[3].Split(':');
            goal = new Vector2((float)Convert.ToDouble(xNy1[0]), (float)Convert.ToDouble(xNy1[1]));
            minionsLeft = Convert.ToInt32(boss5SaveInfo[4]);

        }
    }
}

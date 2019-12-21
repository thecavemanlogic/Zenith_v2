//-----------------------------------------------------------
//File:   Boss4.cs
//Desc:   This file holds the class responsible for controlling
//        Boss4.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class controls Boss4. It also controls the specialized
    // movement for the object as well as the internal state of Boss4.
    public class Boss4 : Enemy
    {
        // The goal position to ram once the boss is in its
        // ramming state. This is set immediately before the
        // boss starts ramming the player.
        Vector2 goal;

        // This method is split up into three different states:
        // Sway, Pause, and Ram.
        // Sway:   
        //      this state will make the boss move vertically
        //      up and down for at least 400 game ticks.
        // Pause:   
        //      this state will move the boss to the right side
        //      of the screen vertically in the middle. This state
        //      has a duration of approximately 1.5 seconds.
        // Ram:
        //      this state will accelerate the boss in the locked
        //      position of the player received at the moment the
        //      Pause state ended
        public override void ShipLoop()
        {
            switch (state)
            {
                case EnemyState.Sway:
                    ++clock;
                    if (clock > 400 && World.Instance.Random.NextDouble() < 0.02)
                    {
                        state = EnemyState.Pause;
                        clock = 0;
                    }

                    cannon.Fire();

                    float goalY = (float)(Math.Cos((double)clock / 100) + 1) / 2 * World.Instance.EndY;
                    float goalX = World.Instance.EndX * 0.75f;

                    MoveTo(new Vector2(goalX, goalY), 200);

                    angle = Vector.GetAngle(World.Instance.Player.Position - position);
                    break;
                case EnemyState.Pause:
                    angle = Vector.GetAngle(World.Instance.Player.Position - position);
                    AddForce((new Vector2(World.Instance.EndX * 0.90f, World.Instance.EndY / 2) - position) * 50);

                    ++clock;
                    if (clock >= 90)
                    {
                        state = EnemyState.Ram;
                        goal = World.Instance.Player.Position;
                    }
                    Shake();
                    break;
                case EnemyState.Ram:
                    AddForce((goal - position) * 200);
                    
                    if ((goal - position).Length() < 50f)
                    {
                        state = EnemyState.Sway;
                    }
                    break;
            }
        }

        // Constructor
        public Boss4(Vector2 position)
            : base(position)
        {
            gameImage = GameImage.Boss4;
            type = GameObjectType.Boss4;
            cannon = new Boss1Cannon(this);
            cannon.Damage = 160 * World.Instance.Difficulty;
            // imageSources = new List<string> { Util.GetShipSpriteFolderPath("large_red_01.png") };
            bodyDamage = 40;
            size = new Vector2(256, 256);
            health = 4000;
            maxHealth = 4000;
            mass = 400;
            worth = 400;
        }

        // ???
        public override string Serialize()
        {
            return base.Serialize() + ',' + goal.ToString();
        }

        // ???
        public override void Deserialize(string saveInfo)
        {
            int index = IndexOfNthOccurance(saveInfo, ",", 24);

            string enemySaveInfo = saveInfo.Substring(0, index);
            base.Deserialize(enemySaveInfo);

            string[] boss4SaveInfo = saveInfo.Substring(index + 1, saveInfo.Length - index - 1).Split(',');

            string[] xNy = boss4SaveInfo[0].Split(':');
            goal = new Vector2((float)Convert.ToDouble(xNy[0]), (float)Convert.ToDouble(xNy[1]));
        }
    }
}

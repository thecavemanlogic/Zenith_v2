//-----------------------------------------------------------
//File:   .cs
//Desc:   
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith.Game_Object_Model.Other.Projectiles
{
    class Missile : GameObject
    {
        GameObject target;

        public override void Loop()
        {
            angle = Vector.GetAngle(target.Position - position);
            position += target.Position - position;
        }

        public Missile(Vector2 position, GameObject target)
            : base(position)
        {
            // imageSources = new List<string>() { "" };
            this.target = target;
        }
    }
}

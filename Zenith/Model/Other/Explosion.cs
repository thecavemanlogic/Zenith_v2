//-----------------------------------------------------------
//File:   Explosion.cs (Cancelled)
//Desc:   Manage the explosion object.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

namespace Zenith
{
    class Explosion : GameObject
    {
        int index = 6;
        int clock = 0;
        Ship host;

        public override void Loop()
        {
            position = host.Position; 
            // imageIndex = index;
            ++index;
            if (clock == 4)
            {
                destroy = true;
            }
            ++clock;
        }

        public Explosion(Ship host) : base(host.Position)
        {
            this.host = host;
            /*
            imageSources = new List<string>
            {
                Util.GetShipSpriteFolderPath("Explosion\\explosion-01.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-02.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-03.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-04.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-05.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-06.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-07.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-08.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-09.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-10.png"),
                Util.GetShipSpriteFolderPath("Explosion\\explosion-11.png")
            };
            */
        }
    }
}

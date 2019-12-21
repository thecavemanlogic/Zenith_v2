//-----------------------------------------------------------
//File:   BasicCannon.cs
//Desc:   Holds the class for BasicCannon, which shoots at a
//        consistent rate.
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

namespace Zenith
{
    // Serves as a simple cannon with a consistent, but
    // variable fire rate.
    public class BasicCannon : Cannon
    {
        // Constructor
        // Sets the fire pattern to a consistent
        // value given by the caller.
        public BasicCannon(Ship host, int fireRate)
            : base(host)
        {
            firePattern = new List<int> { fireRate };
        }
    }
}

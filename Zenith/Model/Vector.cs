using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Zenith
{
    class Vector
    {
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return new Vector2(
                (float)Math.Cos(angle) * magnitude,
                (float)Math.Sin(angle) * magnitude
                );
        }

        public static float GetAngle(Vector2 v)
        {
            return (float)Math.Atan2(v.Y, v.X);
        }

        public static void SetLength(Vector2 v, float length)
        {
            float l = length / v.Length();
            v.X *= l;
            v.Y *= l;
        }
    }
}

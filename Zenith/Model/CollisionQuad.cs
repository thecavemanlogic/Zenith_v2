//-----------------------------------------------------------
//File:   CollisionQuad.cs
//Desc:   Holds the class than handles all collisions in Zenith
//----------------------------------------------------------- 
using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    // This class controls all collisions in Zenith
    // using quadtree optimizations.
    class CollisionQuad
    {
        // Constants

        // Marks the maximum depth a quadtree can grow
        const int maxTier = 4;

        // Marks the minimum amount of objects required to
        // allow growing the quadtree.
        const int minObjectCount = 10;

        // Instance Variables

        // The upper-left corner of the area to cover
        private Vector2 origin;

        // The size of the area to cover
        private Vector2 size;

        // The list of objects to check for collisions
        private List<GameObject> objects;

        // The nested quads
        private CollisionQuad[] quads;

        // The tier of the current quad
        private int tier;

        // Properties

        public List<GameObject> Objects { get { return objects; } set { objects = value; } }

        // Methods

        // Creates 4 nested quadtree branches
        private void Split()
        {
            var newSize = size / 2;

            quads[0] = new CollisionQuad(origin, newSize, tier + 1);
            quads[1] = new CollisionQuad(origin + new Vector2(0, newSize.Y), newSize, tier + 1);
            quads[2] = new CollisionQuad(origin + new Vector2(newSize.X, 0), newSize, tier + 1);
            quads[3] = new CollisionQuad(origin + newSize, newSize, tier + 1);
        }

        // Divides the objects into the 4 nested quadtree branches. If an object
        // happens to lie on the border of two or more quadtree branches, then
        // its reference is passed onto all branches it lies in.
        private void DivideObjects()
        {
            for (int i = 0; i < objects.Count; ++i)
            {
                if (!objects[i].Collidable) continue;
                // if the object fits to the left of the quad
                if (objects[i].Position.X < origin.X)
                {
                    // quad[0]
                    if (objects[i].Position.Y < origin.Y + size.Y) quads[0].Objects.Add(objects[i]);
                    // quad[1]
                    if (objects[i].Position.Y > origin.Y + size.Y) quads[1].Objects.Add(objects[i]);
                }
                // if the object fits to the right of the quad
                else
                {
                    // quad[2]
                    if (objects[i].Position.Y < origin.Y + size.Y) quads[2].Objects.Add(objects[i]);
                    // quad[3]
                    if (objects[i].Position.Y > origin.Y + size.Y) quads[3].Objects.Add(objects[i]);
                }
            }
        }

        // Performs collisions checks on the objects it has in its List.
        // If minObjectCount is exceeded and the current tier is less
        // than maxTier, then the current quadtree is split and collisions
        // are checked with the child branches. Otherwise, collisions are
        // checked with the current quadtree.
        public void CheckForCollisions()
        {
            size = new Vector2(World.Instance.Width, World.Instance.Height);

            int objectCount = objects.Count;

            if (objectCount > minObjectCount && tier < maxTier)
            {
                Split();
                DivideObjects();
                quads[0].CheckForCollisions();
                quads[1].CheckForCollisions();
                quads[2].CheckForCollisions();
                quads[3].CheckForCollisions();
            }
            else
            {
                for (int i = 0; i < objectCount; ++i)
                {
                    if (!objects[i].Collidable) continue;
                    for (int j = i + 1; j < objectCount; ++j)
                    {
                        if (!objects[j].Collidable) continue;
                        if (objects[i].Position.X - objects[i].Size.X / 2 <= objects[j].Position.X + objects[j].Size.X / 2 &&
                            objects[i].Position.Y - objects[i].Size.Y / 2 <= objects[j].Position.Y + objects[j].Size.Y / 2 &&
                            objects[i].Position.X + objects[i].Size.X / 2 >= objects[j].Position.X - objects[j].Size.X / 2 &&
                            objects[i].Position.Y + objects[i].Size.Y / 2 >= objects[j].Position.Y - objects[j].Size.Y / 2)
                        {
                            objects[i].OnCollision(objects[j]);
                            objects[j].OnCollision(objects[i]);
                        }
                        ++World.Instance.Collisions;
                    }
                }
            }
        }

        // Constructor
        public CollisionQuad(Vector2 origin, Vector2 size, int tier)
        {
            this.tier = tier;
            this.origin = origin;
            this.size = size;
            objects = new List<GameObject>();
            quads = new CollisionQuad[4];
        }
    }
}

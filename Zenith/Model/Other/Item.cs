//---------------------------------------------------------------
//File:   Item.cs (cancelled)
//Desc:   Intended to add Items to Shop. But has been cancelled.
//---------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Zenith
{
    class Item : GameObject
    {
        // ~~~~~~~~~~~~~~~~~~~~ Types of Shop Items ~~~~~~~~~~~~~~~~~~~~
        // Selling: Item is available and selling
        // Sold: Item has already been bought and unavailable
        // IsSelected: Is selected, and will show description, cost, and name of item
        public enum shopItems
        {
            IsSelected,
            Selling,
            Sold
        }
        public override void Loop() { }

        public Item(Vector2 position)
            : base(position)
        {

        }

        // ~~~~~~~~~~~~~~~~~~~~ Money when start the game ~~~~~~~~~~~~~~~~~~~~
        protected int startMoney = 0;
    }
}

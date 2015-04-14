﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    abstract class Item
    {
        public Item(){}

        

        public virtual void Update(GameTime gameTime) {}

        public virtual void Draw(SpriteBatch sb) {}
    }
}

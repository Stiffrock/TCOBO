using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TCOBO
{
    abstract class MovableObject
    {
        public MovableObject(){}
        protected float maxLife;
        protected float percentLife;
        protected bool isHpBarVisible;

        

        public virtual void Update(GameTime gameTime) {}

        public virtual void Draw(SpriteBatch spriteBatch) {}


    }
}

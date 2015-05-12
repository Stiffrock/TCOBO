using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    abstract class Item
    {
        public Color itemColor;
        public Color defaultColor;
        public string info;
       
        public Texture2D itemTex;
        public Rectangle hitBox;
        public int stat;
        public Vector2 pos;
        public bool equip;
        public bool hand;
        public bool bagRange = true;

        public Item()
        {

        }

        public virtual void Update(GameTime gameTime) { }

        //public bool PickUp(int x, int y)
        //{
        //    if (hitBox.Contains(x, y))
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //public void Move(int x, int y)
        //{
        //    itemPos.X = x;
        //    itemPos.Y = y;

        //    hitBox.X = x;
        //    hitBox.Y = y;
        //}


        public virtual void Draw(SpriteBatch sb) { }
    }
}

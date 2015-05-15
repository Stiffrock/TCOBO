using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class Armor : Item
    {
        public Armor(int armor, Texture2D tex, Vector2 pos, string info)
            : base()
        {
            this.info = info;
            this.itemTex = tex;
            this.pos = pos;
            this.stat = armor;
            this.itemColor = Color.Black;
            this.hitBox = new Rectangle((int)pos.X, (int)pos.Y, itemTex.Width, itemTex.Height);
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)pos.X + itemTex.Width/3, (int)pos.Y+ itemTex.Height/3, itemTex.Width, itemTex.Height);
        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(itemTex, new Vector2(pos.X + itemTex.Width/5, pos.Y + itemTex.Height/5), new Rectangle(0, 0, itemTex.Width, itemTex.Height), itemColor);
          //  sb.Draw(itemTex, pos, new Rectangle(0, 0, itemTex.Width, itemTex.Height), defaultColor, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0f);

        }

    }
}

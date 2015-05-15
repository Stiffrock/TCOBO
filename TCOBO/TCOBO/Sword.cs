using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class Sword : Item
    {
       

        public Sword(int damage, Texture2D tex, Color color, Vector2 pos, string info)
            : base()
        {
            this.info = info;
            // this.itemTex = TextureManager.goldenSword;
            this.itemTex = tex;
            this.stat = damage;
            this.itemColor = color;
            this.defaultColor = color; 
            this.pos = pos;
            this.hitBox = new Rectangle((int)pos.X, (int)pos.Y, itemTex.Width, itemTex.Height);
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, itemTex.Width - itemTex.Width/5, itemTex.Height - itemTex.Height/5);
        }

         
        public override void Draw(SpriteBatch sb)
        {
           // sb.Draw(itemTex, pos, new Rectangle(0, 0, itemTex.Width, itemTex.Height), defaultColor);





            sb.Draw(itemTex, pos, new Rectangle(0, 0, itemTex.Width, itemTex.Height), defaultColor, 0, new Vector2(0, 0), 0.7f, SpriteEffects.None, 0f);
        }
        


    }
    
    }
 

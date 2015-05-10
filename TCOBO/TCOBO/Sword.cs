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
<<<<<<< HEAD
        public Color swordColor;

=======
>>>>>>> origin/Stoffe
        public Sword(ContentManager content, int damage, Texture2D tex, Color color, Vector2 pos)
            : base(content)
        {
           // this.itemTex = TextureManager.goldenSword;
            this.itemTex = tex;
            this.stat = damage;
            this.itemColor = color;
            this.pos = pos;
            this.hitBox = hitBox;
        }

        public override void Update(GameTime gameTime)
        {
            hitBox = new Rectangle((int)pos.X, (int)pos.Y, itemTex.Width, itemTex.Height);
        }


        public override void Draw(SpriteBatch sb)
<<<<<<< HEAD
        {          
            sb.Draw(itemTex, pos, new Rectangle(0,0,50,50) , itemColor);
=======
        {
            sb.Draw(itemTex, pos, new Rectangle(0, 0, 50, 50), Color.White);
>>>>>>> origin/Stoffe
        }

    }
}

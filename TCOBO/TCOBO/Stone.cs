using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class Stone : Item
    {
        private Texture2D stoneTex;
        private Vector2 stonePos;
        public Stone(ContentManager content)
        {
            stonePos = new Vector2(150, 150);
            stoneTex = content.Load<Texture2D>("stone");
        }

        public override void Update(GameTime gameTime)
        { 
        
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(stoneTex, stonePos, Color.White);
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class InventoryTile
    {
        public Vector2 pos;
        public Rectangle texture_rect;
        //Vector2 playerPos;
        //public bool isCollided = false;

        public bool Equiped;

        public InventoryTile(int x, int y, int tex_x, int tex_y)
        {
            this.pos = new Vector2(x, y);
            Equiped = false;

            //Vector2 playerPos
            //pos = new Vector2(playerPos.X, playerPos.Y);
            texture_rect = new Rectangle((int)pos.X, (int)pos.Y, 50, 50);

        }
      

        public void Draw(Texture2D texture, SpriteBatch spriteBatch)
        {
            
                spriteBatch.Draw(texture, pos, Color.White);
            
        }

    }
}

﻿using System;
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
    public class Tile
    {
        KeyboardState keyboardState, oldkeyboardState;
        MouseState mouse_state;
        MouseState old_mouse_state;
        float msModifier = 1;
        int tileSize = 50;
        public Texture2D image;
        public Vector2 origin;
        public Vector2 position;
        public string typeOfTile;
        public Rectangle bounds;

        public Tile(string typeOfTile, Vector2 position)
        {
            this.position = position;
            this.typeOfTile = typeOfTile;
            bounds = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
            if (typeOfTile == "Sand1")
            {
                msModifier = 0.7F;
              //  image = TextureManager.sand1;
                image = TextureManager.sandtype2;
               
            }
            else if (typeOfTile == "Sand2")
            {
                msModifier = 0.7F;
                image = TextureManager.sand1;
        
            }
            else if (typeOfTile == "Sand3")
            {
                msModifier = 0.7F;
                image = TextureManager.sand1;
       
            }
            else if (typeOfTile == "tree1")
            {
                msModifier = 0.7F;
              //  image = TextureManager.tree1;
                image = TextureManager.bushtile1;
            }
            else if (typeOfTile == "road1")
            {
                msModifier = 0.7F;
                //image = TextureManager.road1;
                image = TextureManager.grass1;
          
            }
        }


        public void Update(int dx, int dy, float zoom)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, Color.White);
        }
    }
}

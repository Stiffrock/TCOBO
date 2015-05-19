using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class Inventory
    {

        private Texture2D inventoryTex;

        public InventoryTile[,] grid;
        const int num_rows = 5;
        const int num_cols = 4;
        private bool showInventory = false;
        public Vector2 pos;
        public Rectangle hitBox;

        public Inventory(ContentManager content, Vector2 pos)
        {
            inventoryTex = content.Load<Texture2D>("inventorytile4");

            this.pos = pos;
            //this.sf = sf;
            hitBox = new Rectangle(950, 125, 250, 250);

            CreateGameGrid();
        }

        private void ShowInventory()
        {
            if (KeyMouseReader.KeyPressed(Keys.I))
            {
                showInventory = !showInventory;
            }
        }


        public void Update()
        {
            ShowInventory();
            //hitBox = new Rectangle((int)pos.X, (int)pos.Y, 150, 250);
        }

        

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                     if (showInventory)
	                {
                        grid[i, j].Draw(inventoryTex, spritebatch);		 
	                }
                }
            }
        }

        void CreateGameGrid()
        {
                grid = new InventoryTile[num_rows, num_cols];

            for (int i = 0; i < num_rows; i++)
            {
                for (int j = 0; j < num_cols; j++)
                {
                    grid[i, j] = new InventoryTile(i * 50 + 950, j * 50 + 125, 0, 0);

                    //i * 50 + 300, j * 50 + 300
                }
            }
        }
    }
}

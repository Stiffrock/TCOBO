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
    class ItemManager 
    {
        private Game1 game1;
        private Sword standardSword, goldenSword, blueSword, redSword;
        private Inventory inventory;
        private GraphicsDevice grahpics;
        private SpriteFont sf;
        private bool Showstats,IsInventoryshown,PickedUp;
        public bool swordEquip;
        public List<Item> ItemList = new List<Item>();
        public List<Item> InventoryList = new List<Item>();
        public List<Item> EquipList = new List<Item>();

        public ItemManager(Game1 game1)
        {
            this.game1 = game1;
            this.sf = game1.Content.Load<SpriteFont>("SpriteFont1");
            grahpics = game1.GraphicsDevice;

            standardSword = new Sword(game1.Content, 10, TextureManager.standardSword, Color.White, new Vector2(0,0));
            blueSword = new Sword(game1.Content, 20, TextureManager.blueSword, Color.LightBlue, new Vector2(0, 20));
            redSword = new Sword(game1.Content, 40, TextureManager.redSword, Color.Red, new Vector2(0, 40));
            goldenSword = new Sword(game1.Content, 100, TextureManager.goldenSword, Color.Gold, new Vector2(0, 60));
            inventory = new Inventory(game1.Content, new Vector2(200, 200));  
            ItemList.Add(standardSword);
            ItemList.Add(redSword);
            ItemList.Add(blueSword);
            ItemList.Add(goldenSword);
            PickedUp = false;
            Showstats = false;
            IsInventoryshown = false;
        }

       /* public Color changeWeaponColor(Color color)
        {
            //return color = sword.swordColor;
        }*/




        public void HandleInventory()
        {
            foreach (Item item in InventoryList)
            {
                if (item.hitBox.Contains(Mouse.GetState().X, Mouse.GetState().Y) && KeyMouseReader.LeftClick() && item.hand == false)
                {
                    // PickedUp = true;
                    item.hand = true;
                    return;
                }

                if (item.hand == true)
                {
                    foreach (InventoryTile tile in inventory.grid)
                    {
                        if (item.hitBox.Intersects(tile.texture_rect))
                        {
                            item.pos.X = tile.pos.X;
                            item.pos.Y = tile.pos.Y + 5;
                            if (KeyMouseReader.LeftClick())
                            {
                                item.hand = false;
                            }
                        }
                    }
                }
            }
        }

        public void equipItem()
        {
            foreach (Item item in InventoryList)
            {
                if (item.hitBox.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y) && KeyMouseReader.RightClick() && item.equip == false)
                {
                    //EquipList.Add(item);

                    item.equip = true;
                    return;
                }
                if (item.hitBox.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y) && KeyMouseReader.RightClick() && item.equip == true)
                {                   


                   // EquipList.Remove(item);
                    item.equip = false;
                    return;
                }
            }
        }


        

        public void MoveItem()
        {
            Vector2 mousePos = new Vector2(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y);

            foreach (Item item in InventoryList)
            {
                if (item.hand == true)
                {
                    item.pos.X = mousePos.X - 25;
                    item.pos.Y = mousePos.Y - 25;
                    item.hitBox.X = (int)mousePos.X - 50;
                    item.hitBox.Y = (int)mousePos.Y - 50;

                    if (item.hitBox.Intersects(inventory.hitBox))
                    {
                        item.bagRange = true;                      
                    }
                    else
                    {
                        item.bagRange = false;                
                    }
                }
            }
       }


       /* public void ShowStats()
        {
            if (sword.hitBox.Contains(Mouse.GetState().X,Mouse.GetState().Y))
            {
                Showstats = true;
            }
            else
            {
                Showstats = false;
            }
        }*/

        private void IsInventoryShown()
        {
            if (KeyMouseReader.KeyPressed(Keys.I))
            {
                IsInventoryshown = !IsInventoryshown;
            }
        }

        public void Update(GameTime gameTime)
        {
            standardSword.Update(gameTime);
            redSword.Update(gameTime);
            blueSword.Update(gameTime);
            goldenSword.Update(gameTime);
            equipItem();
            inventory.Update();
            MoveItem();
            IsInventoryShown();
            HandleInventory();

        }

        public void Draw(SpriteBatch sb)
        {
            inventory.Draw(sb);
            if (Showstats && IsInventoryshown)
            {
                sb.DrawString(sf, "This is a sword.", new Vector2(575, 350), Color.Black);
                sb.DrawString(sf, "Dmg + 3  Str + 3", new Vector2(575, 375), Color.Black);
            }

            if (IsInventoryshown)
            {
                foreach (Item item in InventoryList)
                {
                    item.Draw(sb);
                }
            }
            sb.End();
        }
    }
}

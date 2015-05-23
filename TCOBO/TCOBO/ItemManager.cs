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
        public Key redKey, blueKey, yellowKey;
        public Armor standardArmor;
        private Inventory inventory;
        private GraphicsDevice grahpics;
        private SpriteFont sf;
        public bool Showstats,IsInventoryshown,PickedUp;
        public List<Item> ItemList = new List<Item>();
        public List<Item> InventoryList = new List<Item>();
        public List<Item> EquipList = new List<Item>();
        SoundManager soundManager = new SoundManager();



        public Inventory GetGrid()
        {
            return inventory;
        }

        public ItemManager(Game1 game1)
        {
            this.game1 = game1;
            this.sf = game1.Content.Load<SpriteFont>("SpriteFont1");
            grahpics = game1.GraphicsDevice;
            standardSword = new Sword(10, TextureManager.standardSword, Color.White, new Vector2(0,0), "Standard sword");
            blueSword = new Sword(20, TextureManager.blueSword, Color.LightBlue, new Vector2(0, 20),"Magic Blue sword");
            redSword = new Sword(40, TextureManager.redSword, Color.SandyBrown, new Vector2(0, 40),"Rusty but vicious sword");
            goldenSword = new Sword(100, TextureManager.goldenSword, Color.Gold, new Vector2(0, 60),"The super duper golden\nmega rod of destruction");
            standardArmor = new Armor(5, TextureManager.standardArmor, new Vector2(0, 100),"Standard armor");
            redKey = new Key(TextureManager.RedKey, new Vector2(-200, 0), "Red Key");
            blueKey = new Key(TextureManager.BlueKey, new Vector2(-200, 20), "Blue Key");
            yellowKey = new Key(TextureManager.YellowKey, new Vector2(-200, 40), "Yellow Key");
            inventory = new Inventory(game1.Content, new Vector2(200, 200));  
            ItemList.Add(standardSword);
            ItemList.Add(redSword);
            ItemList.Add(blueSword);
            ItemList.Add(goldenSword);
            ItemList.Add(standardArmor);
            ItemList.Add(redKey);
            ItemList.Add(blueKey);
            ItemList.Add(yellowKey);
            PickedUp = false;
            Showstats = false;
            IsInventoryshown = false;
            soundManager.LoadContent(game1.Content);
        }

        public void HandleInventory()
        {
            foreach (Item item in InventoryList)
            {
                foreach (InventoryTile tile in inventory.grid)
                {
                    if (item.hitBox.Contains(Mouse.GetState().X, Mouse.GetState().Y) && KeyMouseReader.LeftClick() && !PickedUp && tile.texture_rect.Contains(Mouse.GetState().X, Mouse.GetState().Y))
                    {
                        item.hand = true;
                        PickedUp = true;
                        tile.hasItem = false;
                        return;
                    }

                    if (item.hand == true)
                    {                                            
                        if (item.hitBox.Intersects(tile.texture_rect))
                        {
                            if (tile.texture_rect.Contains(Mouse.GetState().X, Mouse.GetState().Y) && KeyMouseReader.LeftClick() && tile.hasItem == false) // Drop item to tile
                            {
                                item.pos.X = tile.pos.X + item.itemTex.Width / 5;
                                item.pos.Y = tile.pos.Y + item.itemTex.Height / 5;
                                item.hand = false;
                                tile.hasItem = true;
                                PickedUp = false;
                            }
                        }                       
                    }

                    

                }

                /*if (item.hand == true)
                {
                    foreach (InventoryTile tile in inventory.grid)
                    {
                        if (item.hitBox.Intersects(tile.texture_rect))
                        {
                            if (item.hitBox.Intersects(tile.texture_rect) && KeyMouseReader.LeftClick() && tile.hasItem == false) // Drop item to tile
                            {
                                item.pos.X = tile.pos.X + item.itemTex.Width / 5;
                                item.pos.Y = tile.pos.Y + item.itemTex.Height / 5;
                                item.hand = false;
                                tile.hasItem = true;
                                PickedUp = false;

                            }
                        }
                    }
                }*/
            }
        }
                

        public void equipItem()
        {
            foreach (Item item in InventoryList)
            {
                if (item.hitBox.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y) && KeyMouseReader.RightClick() && item.equip == false)
                {
                    item.equip = true;
                    return;
                }
                if (item.hitBox.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y) && KeyMouseReader.RightClick() && item.equip == true)
                {                   
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
                    item.pos.X = mousePos.X - item.itemTex.Width /2;
                    item.pos.Y = mousePos.Y - item.itemTex.Height /2;
                    item.hitBox.X = (int)mousePos.X - item.itemTex.Width;
                    item.hitBox.Y = (int)mousePos.Y - item.itemTex.Width;
                    PickedUp = true;

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


        private void IsInventoryShown()
        {
            if (KeyMouseReader.KeyPressed(Keys.I))
            {
                IsInventoryshown = !IsInventoryshown;
                soundManager.inventorySound.Play();
            }  
        }



        public void Update(GameTime gameTime)
        {
            foreach (Item item in ItemList)
            {
                item.Update(gameTime);
            }
            foreach (Item item in InventoryList)
            {
                item.Update(gameTime);
            }
   
            equipItem();
            inventory.Update();
            MoveItem();
            IsInventoryShown();
            HandleInventory();

        }

        public void Draw(SpriteBatch sb)
        {
            inventory.Draw(sb);

            if (IsInventoryshown)
            {
                foreach (Item item in InventoryList)
                {
                    item.Draw(sb);
                    if (item.hitBox.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y))
                    {
                        if (item is Sword)
                        {
                            sb.DrawString(TextureManager.uitext, item.info + "\n\nStr+ " + item.stat.ToString(), new Vector2(970, 350), Color.Black);
                        }
                        if (item is Armor)
                        {
                            sb.DrawString(TextureManager.uitext, item.info + "\n\nVit+ " + item.stat.ToString(), new Vector2(970, 350), Color.Black);
                        }
                        if (item is Key)
                        {
                            sb.DrawString(TextureManager.uitext, item.info, new Vector2(970, 350), Color.Black);
                        }
                    }
                }
            }
            sb.End();
        }
    }
}

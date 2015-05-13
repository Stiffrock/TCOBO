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
        private Sword standardSword, goldenSword, goldenSword1, goldenSword2, goldenSword3, blueSword, redSword;
        public Armor standardArmor;
        private Inventory inventory;
        private GraphicsDevice grahpics;
        private SpriteFont sf;
        public bool Showstats,IsInventoryshown,PickedUp;
        public bool swordEquip;
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
<<<<<<< HEAD
            goldenSword = new Sword(100, TextureManager.goldenSword, Color.Gold, new Vector2(0, 60),"The super duper golden mega rod");
            goldenSword1 = new Sword(100, TextureManager.goldenSword, Color.Gold, new Vector2(0, 100), "The super duper golden mega rod");
            goldenSword2 = new Sword(100, TextureManager.goldenSword, Color.Gold, new Vector2(0, 120), "The super duper golden mega rod");
            goldenSword3 = new Sword(100, TextureManager.goldenSword, Color.Gold, new Vector2(0, 140), "The super duper golden mega rod");
=======
            goldenSword = new Sword(100, TextureManager.goldenSword, Color.Gold, new Vector2(0, 60),"The super duper golden\nmega rod of destruction");
       
     
>>>>>>> origin/Stoffe
            
            standardArmor = new Armor(5, TextureManager.standardArmor, new Vector2(0, 100),"Standard armor");
            inventory = new Inventory(game1.Content, new Vector2(200, 200));  
            ItemList.Add(standardSword);
            ItemList.Add(redSword);
            ItemList.Add(blueSword);
            ItemList.Add(goldenSword);
            ItemList.Add(standardArmor);
<<<<<<< HEAD
            ItemList.Add(goldenSword1);
            ItemList.Add(goldenSword2);
            ItemList.Add(goldenSword3);
=======
>>>>>>> origin/Stoffe

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
                    if (tile.texture_rect.Intersects(item.hitBox))
                    {
                        if (item.hand)
                        {
                            tile.hasItem = false;
                        }
                        else
                        {
                            tile.hasItem = true;
                        }                    
                    }                                                      
                }
                    
                if (item.hitBox.Contains(Mouse.GetState().X, Mouse.GetState().Y) && KeyMouseReader.LeftClick())
                {
<<<<<<< HEAD
                    item.hand = true;
                 
=======
                    item.hand = true;                 
>>>>>>> origin/Stoffe
                    return;
                }

                if (item.hand == true)
                {
                    foreach (InventoryTile tile in inventory.grid)
                    {
<<<<<<< HEAD
                        if (item.hitBox.Intersects(tile.texture_rect))
                        {                         
                            item.pos.X = tile.pos.X;
                            item.pos.Y = tile.pos.Y + 5;                       
                                                                           
                            if (KeyMouseReader.LeftClick())
                            {
                                item.hand = false;
                                
                            }
=======
                        if (item.hitBox.Intersects(tile.texture_rect) && KeyMouseReader.LeftClick() )
                        {                         
                            item.pos.X = tile.pos.X;
                            item.pos.Y = tile.pos.Y + 5;
                            item.hand = false;                                             
>>>>>>> origin/Stoffe
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
                        
                        
                    }
                }
            }
            sb.End();
        }
    }
}

using System;
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
    class Main              
    {
        public Game1 game1; 
        private TestWorld testWorld;
        private ItemManager itemManager;
        private GraphicsDevice graphics;
        public Player player;
        private Attack attack;
        private Camera2D camera;        
        private SpriteFont spriteFont;
        private Color swordColor;
        private Color newSwordColor;
        private KeyMouseReader krm;
        private List<Enemy> enemyList;
        private List<Enemy> inrangeList;
        private Vector2 aimVector;
        private PlayerPanel board;
        private int row = 0, itemcount = 0;
        private Tuple<int, int, int, int, int, int> playerStats;
        private Tuple<float, float, float> effectiveStats;
        SoundManager soundManager = new SoundManager();

        //private Song statseffect;
        
        public Main(Game1 game1)
        {
            this.game1 = game1;
            graphics = game1.GraphicsDevice;
            krm = new KeyMouseReader();
            itemManager = new ItemManager(game1);
            player = new Player(game1.Content);
            testWorld = new TestWorld(game1.Content);
            camera = new Camera2D(game1.GraphicsDevice.Viewport, player);        
            enemyList = new List<Enemy>();
            inrangeList = new List<Enemy>();

            //Enemy STR, DEX, VIT, INT, EXPDROP
            enemyList.Add(new Enemy(new Vector2(300, 300), game1.Content, 1, 0, 100, 0, 10));
            enemyList.Add(new Enemy(new Vector2(-2000, 300), game1.Content, 5, 0, 75, 0, 500));
            enemyList.Add(new Enemy(new Vector2(-2794, -4474), game1.Content, 35, 0, 125, 0, 3000));
            attack = new Attack(player, game1.Content);
            testWorld.ReadLevel("map01");
            testWorld.SetMap();                 
            spriteFont = game1.Content.Load<SpriteFont>("SpriteFont1");
            board = new PlayerPanel(game1.Content, new Vector2(950, 0), spriteFont);

            soundManager.LoadContent(game1.Content);
            MediaPlayer.Play(soundManager.bgMusic);

        }
      
        public void Rotation()
        {
            Vector2 mousePosition;
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;
            Vector2 worldPosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.GetTransformation(graphics)));
            Vector2 ms = worldPosition;
            float xDistance = (float)ms.X - player.playerPos.X;
            float yDistance = (float)ms.Y - player.playerPos.Y;
            player.rotation = (float)Math.Atan2(yDistance, xDistance);

            double h = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
            float dn = (float)h;
            player.strikeVelocity = new Vector2(xDistance / dn * 70, yDistance / dn * 70);

            aimVector = new Vector2(xDistance, yDistance);

            player.aimRec = new Vector2(xDistance, yDistance);
            player.aimRec.Normalize();
            double recX = (double)player.aimRec.X * 40 * player.size;
            double recY = (double)player.aimRec.Y * 40 * player.size;
            player.attackHitBox = new Rectangle((int)(player.playerPos.X + recX - 25 * player.size), (int)(player.playerPos.Y + recY - 25 * player.size), (int)(50*player.size), (int)(50*player.size));
        }


        public void ClickStats()
        {
            if (player.newStat != 0 && itemManager.IsInventoryshown)
            {
                board.statColor = Color.Gold;
                board.showStatButton = true;

                if (board.StrButton.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y) )
                {
                    board.currentStrFont = board.MOStatFont;
                    if (KeyMouseReader.LeftClick())
                    {
                        player.Str += 1;
                        player.newStat -= 1;                       

                        soundManager.statSound.Play();
                    }                    
                }
                else
                {
                    board.currentStrFont = board.spriteFont;
                }
                if (board.DexButton.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y))
                {
                    board.currentDexFont = board.MOStatFont;
                    if (KeyMouseReader.LeftClick())
                    {
                        player.Dex += 1;
                        player.speed += 1;
                        player.max_speed += 3;
                        player.newStat -= 1;
                        soundManager.statSound.Play();
                    }
                }
                else
                {
                    board.currentDexFont = board.spriteFont;
                }

                if (board.VitButton.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y))
                {
                    board.currentVitFont = board.MOStatFont;
                    if (KeyMouseReader.LeftClick())
                    {
                        player.Vit += 1;
                        player.HP += 1;
                        player.newStat -= 1;
                        soundManager.statSound.Play();
                    }
                }
                else
                {
                    board.currentVitFont = board.spriteFont;
                }

                if (board.IntButton.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y))
                {
                    board.currentIntFont = board.MOStatFont;
                    if (KeyMouseReader.LeftClick())
                    {
                        player.Int += 1;
                        player.newStat -= 1;
                        soundManager.statSound.Play();
                    }
                }
                else
                {
                    board.currentIntFont = board.spriteFont;
                }                
            }
            else
            {
                board.currentStrFont = board.spriteFont;
                board.currentDexFont = board.spriteFont;
                board.currentVitFont = board.spriteFont;
                board.currentIntFont = board.spriteFont;
                board.statColor = Color.Black;
                board.showStatButton = false;
            }
        }

        public void detectItem()
        {
            foreach (Item item in itemManager.ItemList)
            {
       
                if (player.attackHitBox.Intersects(item.hitBox) && KeyMouseReader.LeftClick())
                {
                    for (int i = 0; i < itemManager.GetGrid().grid.GetLength(1); i++)
                    {
                        for (int j = 0; j < itemManager.GetGrid().grid.GetLength(0);)
                        {
                            if (itemManager.GetGrid().grid[j, i].hasItem == true)
                            {
                                j++;
                                
                            }
                            else
                            {
                                item.pos = itemManager.GetGrid().grid[j, i].pos;
                                itemManager.GetGrid().grid[j, i].hasItem = true;
                                itemManager.InventoryList.Add(item);
                                itemManager.ItemList.Remove(item);
                                item.bagRange = true;
                                return;                                
                            }                                                                         
                        }                       
                    }                
                }
            }
            foreach (Item item in itemManager.InventoryList)
            {
                if (!item.bagRange && KeyMouseReader.LeftClick() && !item.equip)
                {
                   
                    item.hand = false;
                    item.pos = player.playerPos;
                    itemManager.ItemList.Add(item);
                    itemManager.InventoryList.Remove(item);                   
                    break;
                }
            }


       
        }

        public void detectEquip()
        {
            foreach (Item item in itemManager.InventoryList)
            {
                if (item.hitBox.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y) && KeyMouseReader.RightClick() && itemManager.IsInventoryshown)
                {
                    Color itemCol = item.itemColor;
                    int statAdd = item.stat;
                   // int oldStatAdd = 0;

                    soundManager.equipSound.Play();

                    if (item is Sword && item.equip == true && player.swordinHand == true && itemManager.EquipList.Contains(item))
                    {
                        player.Str -= statAdd;

                        
                        item.defaultColor = itemCol;
                        player.colorswitch(itemCol);
                        player.swordinHand = false;
                        player.swordEquipped = false;
    

                        itemManager.EquipList.Remove(item);
                        return;
                    }

                    if (item is Sword && item.equip == false && player.swordinHand == false)
                    {
                       
                        item.defaultColor = Color.Green;
                        player.Str += statAdd;
                        player.colorswitch(itemCol);
                        player.swordinHand = true;
                        player.swordEquipped = true;
                 
                        itemManager.EquipList.Add(item);
                        return;
                    }


                    if (item is Armor && item.equip == true && itemManager.EquipList.Contains(item))
                    {
                        player.Vit -= statAdd;
                        player.armorEquip = false;                       
                        itemManager.EquipList.Remove(item);
                        return;
                    }
                    if (item is Armor && item.equip == false)
                    {
                        player.Vit += statAdd;
                        player.armorEquip = true;
                        itemManager.EquipList.Add(item);
                        return;
                    } 

                }
            }

        }
             

        private void detectEnemy()
        {
            if (player.HP > 0)
            foreach (Enemy enemy in enemyList)
            {
                if (enemy.hitBox.Intersects(player.attackHitBox))
                {
                    attack.inRange(enemy, aimVector);
                    inrangeList.Add(enemy);
                }                       
            }   
        }
        public void Update(GameTime gameTime)
        {
            if (itemManager.InventoryList.Count() != 0)
            {
                Console.WriteLine(itemManager.InventoryList[0].hitBox); 
            }
           
            if (itemManager.InventoryList.Count != 0)
            {
                Console.WriteLine(itemManager.InventoryList[0].hitBox);
            }
            Console.WriteLine(player.playerPos); // Boss spa
          

            detectEquip();
            detectItem();
            ClickStats();      
            itemManager.Update(gameTime);
            krm.Update();
            attack.Update(gameTime);
            player.Update(gameTime);
            player.Collision(gameTime, testWorld.tiles);
            detectEnemy();
            Rotation();
            playerStats = player.GetPlayerStats();
            effectiveStats = player.GetEffectiveStats();
            board.Update(playerStats, effectiveStats);
            camera.Update(gameTime);
            Collision();
            foreach (Enemy e in enemyList)
            {
                e.UpdateEnemy(gameTime, player, testWorld.tiles);         
            }         
        }

        public void Collision()
        {
            float x1;
            float y1;
            float x2;
            float y2;
            float radius1;
            float radius2;
            foreach (Enemy p in enemyList)
            {
                if (p.health < 0)
                    break;
                foreach (Enemy p2 in enemyList)
                {
                    if (p == p2)
                        break;
                    x1 = p.pos.X;
                    y1 = p.pos.Y;
                    x2 = p2.pos.X;
                    y2 = p2.pos.Y;
                    radius1 = p.playerSize / 2;
                    radius2 = p2.playerSize / 2;
                    if (Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)) < (radius1 + radius2))
                    {
                        float moveX = x2 - x1;
                        float moveY = y2 - y1;
                        double h = Math.Sqrt(moveX * moveX + moveY * moveY);
                        float dn = (float)h;

                        float knockback = 25f;
                        p2.velocity = new Vector2((moveX / dn * knockback), (moveY / dn * knockback));
                        p.velocity = new Vector2(-(moveX / dn * knockback), -(moveY / dn * knockback));
                    }
                }
                x1 = p.pos.X;
                y1 = p.pos.Y;
                x2 = player.GetPos().X;
                y2 = player.GetPos().Y;
                radius1 = p.playerSize / 2;
                radius2 = player.playerSize / 2;
                if (Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)) < (radius1 + radius2))
                {
                    float moveX = x2 - x1;
                    float moveY = y2 - y1;
                    double h = Math.Sqrt(moveX * moveX + moveY * moveY);
                    float dn = (float)h;

                    float knockback = 25f;
                    player.velocity = new Vector2((moveX / dn * knockback), (moveY / dn * knockback));
                    p.velocity = new Vector2(-(moveX / dn * knockback), -(moveY / dn * knockback));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {     
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                camera.transform);
            testWorld.Draw(spriteBatch);
          
          

            foreach (Enemy e in enemyList)
            {
                e.Draw(spriteBatch);
            }      
            foreach (Item item in itemManager.ItemList)
            {
                item.Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            testWorld.DrawDoodad(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            board.Draw(spriteBatch);
            itemManager.Draw(spriteBatch);
        }

        }        
    }




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
        private Enemy enemy;
        private SpriteFont spriteFont;
        private Color swordColor;
        private Color newSwordColor;
        private KeyMouseReader krm;
        private List<Enemy> enemyList;
        private List<Enemy> inrangeList;
        private Vector2 aimVector;
        private PlayerPanel board;
        private Tuple<int, int, int, int, int, int> playerStats;
        private Tuple<float, float, float> effectiveStats;

        
        public Main(Game1 game1)
        {
            this.game1 = game1;
            graphics = game1.GraphicsDevice;
            krm = new KeyMouseReader();
            itemManager = new ItemManager(game1);
            player = new Player(game1.Content);
            testWorld = new TestWorld(game1.Content);
            camera = new Camera2D(game1.GraphicsDevice.Viewport, player);
            enemy = new Enemy(game1.Content);            
            enemyList = new List<Enemy>();
            inrangeList = new List<Enemy>();
            enemyList.Add(enemy);
            attack = new Attack(player);
            testWorld.ReadLevel("Map01");
            testWorld.SetMap();                 
            spriteFont = game1.Content.Load<SpriteFont>("SpriteFont1");
            board = new PlayerPanel(game1.Content, new Vector2(950, 0), spriteFont);
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

            aimVector = new Vector2(xDistance, yDistance);

            player.aimRec = new Vector2(xDistance, yDistance);
            player.aimRec.Normalize();
            double recX = (double)player.aimRec.X * 40 * player.size;
            double recY = (double)player.aimRec.Y * 40 * player.size;
            player.attackHitBox = new Rectangle((int)(player.playerPos.X + recX - 25 * player.size), (int)(player.playerPos.Y + recY - 25 * player.size), (int)(50*player.size), (int)(50*player.size));
        }


 

        public void ClickStats()
        {
            if (player.newStat != 0)
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
                        player.newStat -= 1;
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
                if (player.attackHitBox.Intersects(item.hitBox)&& KeyMouseReader.LeftClick())
                {
                    item.pos = new Vector2(1050, 170);
                    itemManager.InventoryList.Add(item);
                    itemManager.ItemList.Remove(item);
                    item.bagRange = true;
                    break;
                }
            }
            foreach (Item item in itemManager.InventoryList)
            {
                if (!item.bagRange && KeyMouseReader.LeftClick() && !item.equip)
                {
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
                                  
                    if (item.hitBox.Contains(KeyMouseReader.MousePos().X, KeyMouseReader.MousePos().Y) && KeyMouseReader.RightClick())
                    {
                        Color itemCol = item.itemColor;
                     
                        int statAdd = item.stat;
                        int oldStatAdd = 0;

           


                        if (item.equip == false && itemManager.swordEquip == true)          //TODO fixxa equippen så den inte buggar runt emd färger. Och så att man kan byta utan att ta av vapen
                        {
                            foreach (Item sword in itemManager.EquipList)
                            {
                                if (sword is Sword)
                                {
                                    oldStatAdd = sword.stat;
                                    itemManager.EquipList.Remove(sword);
                                    break;
                                }
                            }                          
                            player.Str -= oldStatAdd;
                            player.Str += statAdd;
                            player.colorswitch(itemCol);
                            player.swordinHand = true;
                            player.swordEquipped = true;
                            itemManager.swordEquip = true;
                            itemManager.EquipList.Add(item);
                            return;
                        }






                        if (item is Sword && item.equip == true && itemManager.swordEquip == true)
                        {
                            
                            player.Str -= statAdd;
                            itemCol = Color.White;
                            player.colorswitch(itemCol);
                            player.swordinHand = false;
                            player.swordEquipped = false;
                            itemManager.swordEquip = false;
                            itemManager.EquipList.Remove(item);
                            return;
                        }
                        if (item.equip == false && itemManager.swordEquip == false)
                        {
                            player.Str += statAdd;                             
                            player.colorswitch(itemCol);
                            player.swordinHand = true;
                            player.swordEquipped = true;
                            itemManager.swordEquip = true;
                            return;
                        }
                        
                    }         
            }
            
        }
             

        private void detectEnemy()
        {
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
            enemy.UpdateEnemy(gameTime, player.GetPos());            
        }

        public void Draw(SpriteBatch spriteBatch)
        {     
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                camera.transform);
            testWorld.Draw(spriteBatch);           
            player.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            
            foreach (Item item in itemManager.ItemList)
            {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();
            spriteBatch.Begin();
            board.Draw(spriteBatch);
            itemManager.Draw(spriteBatch);
        }

        }        
    }




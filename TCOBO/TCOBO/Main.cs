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
        public Player scenePlayer;
        private Scene scene1;
        private Attack attack;
        private Camera2D camera;        
        private SpriteFont spriteFont;
        private Color swordColor;
        private Random rnd = new Random();
        private Color newSwordColor;
        private KeyMouseReader krm;
        private List<Enemy> enemyList;
        private List<Enemy> inrangeList;
        private Vector2 aimVector;
        private PlayerPanel board;
        private int row = 0, itemcount = 0, i = 0;
        private Tuple<int, int, int, int, int, int> playerStats;
        private Tuple<float, float, float> effectiveStats;
        private float deltaTime = 8000;
        SoundManager soundManager = new SoundManager();
        private bool cutScene = true, enemiesSpawned = false;
        public bool loss = false;
        private string activeTooltip;
        private List<string> ttList = new List<string>();
        //private Song statseffect;
        
        public Main(Game1 game1)
        {
            this.game1 = game1;
            
            graphics = game1.GraphicsDevice;
            krm = new KeyMouseReader();
            itemManager = new ItemManager(game1);
            player = new Player(game1.Content);
            scenePlayer = new Player(game1.Content);
            testWorld = new TestWorld(game1.Content);
            camera = new Camera2D(game1.GraphicsDevice.Viewport, player);        
            enemyList = new List<Enemy>();
            inrangeList = new List<Enemy>();
            scene1 = new Scene(this, scenePlayer);                      
            attack = new Attack(player, game1.Content);
            testWorld.ReadLevel("map01");
            testWorld.SetMap();                 
            spriteFont = game1.Content.Load<SpriteFont>("SpriteFont1");
            board = new PlayerPanel(game1.Content, new Vector2(950, 0), spriteFont);
            soundManager.LoadContent(game1.Content);
            MediaPlayer.Play(soundManager.bgMusic);
            MediaPlayer.Volume = 0.4f;
            MediaPlayer.IsRepeating = true;

        }

        public void spawnEnemies()
        {
            enemyList.Add(new Enemy(new Vector2(-200, -1300), game1.Content, 55, 1, 300, 0, 10, 1)); // Main boss
            enemyList.Add(new Enemy(new Vector2(-2240, 500), game1.Content, 25, 1, 150, 0, 10, 1)); // Red Key boss

            for (int i = 0; i < testWorld.enemyposList.Count; i++)          // Kan basicly helt ställa in svårighetsgrad här
            {
                int Str = rnd.Next(1, 20);
                int Dex = rnd.Next(2, 30);
                int Vit = rnd.Next(5, 40); // Måste va över 5
                int Exp = (Vit/2) + (Str/2);
                enemyList.Add(new Enemy(testWorld.enemyposList[i], game1.Content, Str, Dex, Vit, 0, Exp, 720));
                i += 80; // Bestämmer hur många fiender som spawnar, ju mindre värde desto tätare spawnar som
            }

        }
      
        public void Rotation()
        {
            Vector2 mousePosition;
            mousePosition.X = Mouse.GetState().X;
            mousePosition.Y = Mouse.GetState().Y;
            Vector2 worldPosition = Vector2.Transform(mousePosition, Matrix.Invert(camera.GetTransformation(graphics)));
            Vector2 ms = worldPosition;
            float xDistance = (float)ms.X - player.pos.X;
            float yDistance = (float)ms.Y - player.pos.Y;
            player.rotation = (float)Math.Atan2(yDistance, xDistance);
            double h = Math.Sqrt(xDistance * xDistance + yDistance * yDistance);
            float dn = (float)h;
            player.strikeVelocity = new Vector2(xDistance / dn * 70, yDistance / dn * 70);
            aimVector = new Vector2(xDistance, yDistance);
            player.aimRec = new Vector2(xDistance, yDistance);
            player.aimRec.Normalize();
            double recX = (double)player.aimRec.X * 40 * player.size;
            double recY = (double)player.aimRec.Y * 40 * player.size;
            player.attackHitBox = new Rectangle((int)(player.pos.X + recX - 25 * player.size), (int)(player.pos.Y + recY - 25 * player.size), (int)(50*player.size), (int)(50*player.size));
        }

        public void handleLoss()
        {
            if (player.dead)
            {
                if (KeyMouseReader.KeyPressed(Keys.Space))
                {
                    loss = true; 
                }
              
            }
        }

        public void handleTooltip(SpriteBatch spriteBatch)
        {
            string tooltip1 = "Tip: Walk over items to pick them up, press I to enter Inventory and Right click on item to equip it.";
            string tooltip2 = "Tip: Hold ALT to show HP bars";
            string tooltip3 = "Tip: Press Q to use HEAL spell";
            string tooltip4 = "Tip: You will level up by killing enemies, this will give you points that you can distribute in your inventorys statpanel";
            string tooltip5 = "Tip: Find hidden keys to unlock doors";
            ttList.Add(tooltip1);
            ttList.Add(tooltip2);
            ttList.Add(tooltip3);
            ttList.Add(tooltip4);
            ttList.Add(tooltip5);

            if (deltaTime > 8000 && i != 6)
            {
                activeTooltip = ttList[i];
                i += 1;
                deltaTime = 0;
            }

            if (i < 6)
            {
                spriteBatch.DrawString(TextureManager.uitext, activeTooltip, new Vector2(300, 200), Color.Silver);
            }
            
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
                        player.attackspeed += 0.1f;
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
                        player.HP += 5;
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
                        player.MANA += 1;
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
                float offsetX = item.itemTex.Width/5;
                float offsetY = item.itemTex.Height/5;          
       
                if (player.hitBox.Intersects(item.hitBox))
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
                                item.pos.X = itemManager.GetGrid().grid[j, i].pos.X + offsetX;
                                item.pos.Y = itemManager.GetGrid().grid[j, i].pos.Y + offsetY;   
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
                if (item.info == "Red Key")
                    player.hasRedKey = true;
                else if (item.info == "Blue Key")
                    player.hasBlueKey = true;
                else if (item.info == "Yellow Key")
                    player.hasYellowKey = true;


                if (!item.bagRange && KeyMouseReader.LeftClick() && !item.equip)
                {
                    if (item.info == "Red Key")
                        player.hasRedKey = false;
                    else if (item.info == "Blue Key")
                        player.hasBlueKey = false;
                    else if (item.info == "Yellow Key")
                        player.hasYellowKey = false;
                    item.hand = false;
                    itemManager.PickedUp = false;
                    item.pos = new Vector2(player.pos.X + 50, player.pos.Y);
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

            if (scene1.sceneOver)
            {
                cutScene = false;
            }
            if (!cutScene)
            {
                deltaTime += gameTime.ElapsedGameTime.Milliseconds;
                handleLoss();
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
            else
            {
                krm.Update();
                player.scene1 = true;
                scene1.Update(gameTime);             
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
                if (p.health <= 0)
                    continue;
                foreach (Enemy p2 in enemyList)
                {
                    if (p == p2)
                        break;
                    if (p2.health <= 0)
                        continue;
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
            if (!cutScene)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null,
                camera.transform);
                testWorld.Draw(spriteBatch);

         
                foreach (Enemy e in enemyList)
                {
                    e.DrawBlood(spriteBatch);
                }
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
                if (player.dead)
                {
                    spriteBatch.DrawString(TextureManager.uitext, "You are dead\nPress Space to restart\nPathetic...", new Vector2(player.pos.X - 50, player.pos.Y - 100), Color.White);
                }
                spriteBatch.End();
                spriteBatch.Begin();
                handleTooltip(spriteBatch);
         
                board.Draw(spriteBatch);
                itemManager.Draw(spriteBatch);
                if (!enemiesSpawned)
                {
                    spawnEnemies();
                    enemiesSpawned = true;
                    testWorld.initial = false;
                }
           

                
            }
            else
            {
                spriteBatch.Begin();
                scene1.Draw(spriteBatch);
                spriteBatch.End();
            }

        }

        }        
    }




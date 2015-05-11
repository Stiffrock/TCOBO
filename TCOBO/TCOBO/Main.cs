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
            testWorld.ReadLevel("map01");
            testWorld.SetMap();                 
            spriteFont = game1.Content.Load<SpriteFont>("SpriteFont1");
            board = new PlayerPanel(game1.Content, new Vector2(550, 0), spriteFont);
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
            double recX = (double)player.aimRec.X * 100;
            double recY = (double)player.aimRec.Y * 100;
            player.attackHitBox = new Rectangle(((int)player.playerPos.X - 40) + (int)recX, ((int)player.playerPos.Y - 40) + (int)recY, 100, 100);
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
            spriteBatch.Draw(TextureManager.bigtree, new Vector2(-750, 200), Color.White);
            spriteBatch.Draw(TextureManager.smalltree, new Vector2(-700, 400), Color.White);
            testWorld.Draw(spriteBatch);           
            player.Draw(spriteBatch);
            enemy.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            board.Draw(spriteBatch);
            itemManager.Draw(spriteBatch);
           
            


        }

        }        
    }




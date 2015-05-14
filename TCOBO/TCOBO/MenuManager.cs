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

    class MenuManager
    {
        public bool GameOn = false, drawTime = false;
        private SpriteFont menuTitle;
        private SpriteFont menuText;
        private KeyMouseReader krm;
        private Vector2 textpos;
        private string tcobo;
        private List<string> stringList = new List<string>();
        private Random rnd = new Random();
        private float deltaTime = 200f;
        private float time;
        private int textmoveY = 0;
        private int textmoveX = 0;
        private int textmoveY2 = 0;
        private int textmoveX2 = 0;
        private int textmoveY3 = 0;
        private int textmoveX3 = 0;
        private int textmoveY4 = 0;
        private int textmoveX4 = 0;
        


        public MenuManager(Game1 game1)
        {
            menuTitle = game1.Content.Load<SpriteFont>("MenuTitleFont");
            menuText = game1.Content.Load<SpriteFont>("MenuFont");
            krm = new KeyMouseReader();
            tcobo = "T\nh\ne\nr\ne\n \nc\na\nn\n \no\nn\nl\ny\n \nb\ne\n \no\nn\ne\n";
        }

        public void bgText(SpriteBatch spriteBatch)
        {
              for (int i = 0; i < 100; i++)
              {
                int textmove = rnd.Next(0, 10000);
                 spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + i * 15, textpos.Y + textmove), Color.Blue);
              }

         
            if (time >= deltaTime)
            {              
                textmoveY = rnd.Next(0, 1280);
                textmoveX = rnd.Next(0, 1280);
                textmoveY2 = rnd.Next(0, 1280);
                textmoveX2 = rnd.Next(0, 1280);
                textmoveY3 = rnd.Next(0, 1280);
                textmoveX3 = rnd.Next(0, 1280);
                textmoveY4 = rnd.Next(0, 1280);
                textmoveX4 = rnd.Next(0, 1280);
                time = 0;
            }
                spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + textmoveX, textpos.Y + textmoveY), Color.Blue);
                spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + textmoveX2, textpos.Y + textmoveY2), Color.Blue);
                spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + textmoveX3, textpos.Y + textmoveY3), Color.Blue);
                spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + textmoveX4, textpos.Y + textmoveY4), Color.Blue); 
            
            
             
        }

        public void Update(GameTime gameTime)
        {
  
            time += gameTime.ElapsedGameTime.Milliseconds;

            krm.Update();
            Console.WriteLine(stringList.Count);

            if (KeyMouseReader.LeftClick())
            {
                   GameOn = true;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            bgText(spriteBatch);
            spriteBatch.DrawString(menuTitle, "TCOBO ", new Vector2(540, 260), Color.Blue);
            spriteBatch.DrawString(menuText, "Click to start", new Vector2(540, 360), Color.Blue);

            spriteBatch.End();
        }

    }
}

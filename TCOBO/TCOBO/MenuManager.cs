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
        public bool GameOn = false, endSeq = false;
        private SpriteFont menuTitle;
        private SpriteFont menuText;
        private KeyMouseReader krm;
        private Vector2 textpos;
        private string tcobo, title;
        private List<string> stringList = new List<string>();
        private Random rnd = new Random();
        private float deltaTime = 10f, deltaTime2 = 20f, time, time2, textspeed, textCount = 40,
            endseqTimer = 200;
        private List<float> xposList = new List<float>();
        private List<float> yposList = new List<float>();
        private List<char> charlist = new List<char>();
        private List<Color> colorList = new List<Color>();
        private Color flowColor;
        private Vector2 coverPos = new Vector2(300,300);
        private Rectangle coverRec = new Rectangle(0,0,670,50);
        private int textmoveY = 0;
        private int textmoveX = 0;
        


        public MenuManager(Game1 game1)
        {
            menuTitle = game1.Content.Load<SpriteFont>("MenuTitleFont");
            menuText = game1.Content.Load<SpriteFont>("MenuFont");
            krm = new KeyMouseReader();
            flowColor = Color.Blue;
            tcobo = "T\nh\ne\nr\ne\n \nc\na\nn\n \no\nn\nl\ny\n \nb\ne\n \no\nn\ne\n";
        }



        public void endseqText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(menuTitle, "THERE CAN ONLY BE ONE", new Vector2(300, 300), Color.Blue);
            spriteBatch.Draw(TextureManager.bricktile1, coverPos, coverRec, Color.Black);
        }

        public void bgText(SpriteBatch spriteBatch)
        {
         /*   for (int i = 0; i < 100; i++)     Gammal textSpam utan timer bara fps cap
            {
                int textmove = rnd.Next(0, 10000);
                spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + i * 15, textpos.Y + textmove), Color.Blue);
            }*/

            if (KeyMouseReader.LeftClick())
            {
                endSeq = true;
            }

            if (time >= deltaTime)
            {               
                for (int i = 0; i < textCount; i++)
                {
                    textmoveY = rnd.Next(-200, 1000);
                    textmoveX = rnd.Next(0, 1280);
                    xposList.Add(textmoveX);
                    yposList.Add(textmoveY);                 
                }
              
                if (xposList.Count != 0 && yposList.Count != 0)
                {
                    
                    for (int i = 0; i < textCount; i++)
                    {          
                        spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + xposList[i], textpos.Y + yposList[i]), flowColor);
                    }
                }

                if (time >= deltaTime2 && textCount != 0 && yposList.Count != 0 && xposList.Count != 0)
                {
                    for (int i = 0; i < textCount; i++)
                    {
                        xposList.Remove(xposList[i]);
                        yposList.Remove(yposList[i]);
                    }  
                    time = 0;
                    if (endSeq)
                    {                    
                        textCount -= 0.5f;
                    }                   
                }
            }                                         
        }

        private void textposUpdate()
        {
            if (yposList.Count != 0)
            {
                float textspeed;
                for (int i = 0; i < 40; i++)
                {
                    textspeed = rnd.Next(25, 50);
                    yposList[i] += textspeed;
                }
            }

        }

        public void Update(GameTime gameTime)
        {
   
            if (textCount <= 0 )
            {
                GameOn = true;
            }
            if (endSeq)
            {
                coverRec.Width -= 10;
                coverPos.X += 10;
            }
  
            time += gameTime.ElapsedGameTime.Milliseconds;
            time2 += gameTime.ElapsedGameTime.Milliseconds;
            krm.Update();
            textposUpdate();
           
             
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            bgText(spriteBatch);
            if (endSeq)
            {
                endseqText(spriteBatch);
            }
            if (!endSeq)
            {
                spriteBatch.DrawString(menuTitle, "TCOBO ", new Vector2(540, 260), Color.Blue);
                spriteBatch.DrawString(menuText, "Click to start", new Vector2(540, 360), Color.Blue);               
            }
            spriteBatch.End();
        }

    }
}

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
    class Scene
    {
        Main main;
        Player scenePlayer;
        Vector2 bossPos;
        bool moveX, moveY, canMove = false, monolog1;
        SpriteFont sceneText;
        float deltaTime;
        string activeText;
        List<string> textList = new List<string>();
        int i = 0;
        public bool sceneOver = false, endSeq = false, screenBlink, redScreen= false;

        public Scene(Main main, Player scenePlayer)
        {
            this.main = main;
            this.scenePlayer = scenePlayer;
            scenePlayer.pos = new Vector2(500, 500);
            bossPos = new Vector2(50, 50);
            sceneText = TextureManager.uitext;
        }




        public void moveScript1()
        {
            if (bossPos.X <= 500)
            {
                moveX = true;
            }
            else
            {
                moveX = false;
            }
            if (bossPos.Y < 500 && !moveX)
            {
                moveY = true;
            }
            if (bossPos.Y >= 420)
            {
                moveY = false;
                monolog1 = true;
               

            }
        }

        public void textScript()
        {
            string text1 = "They raped your mother";
            string text2 = "They took your sister";
            string text3 = "They split your fathers skull like firewood";
            string text4 = "This is not a dream...";
            string text5 = "This wont go away...";
            string text6 = "You will never forget...";
            string text7 = "You will never forgive...";
            string text8 = "Slaughter them all";
            textList.Add(text1);
            textList.Add(text2);
            textList.Add(text3);
            textList.Add(text4);
            textList.Add(text5);
            textList.Add(text6);
            textList.Add(text7);
            textList.Add(text8);
           
        }

        public void chooseString()
        {
            if (!endSeq)
            {
                activeText = textList[i];

                if (deltaTime >= 2500)
                {
                    i += 1;
                    deltaTime = 0;
                    if (i >= 8)
                    {
                        endSeq = true;
                    }
                }
                
            }
            
        }


        public void cameraBounds()
        {
            if (scenePlayer.pos.X >= 1330)
            {
                scenePlayer.pos.X = 0;
            }
            if (scenePlayer.pos.X <= -50)
            {
                scenePlayer.pos.X = 1280;
            }
            if (scenePlayer.pos.Y >= 770)
            {
                scenePlayer.pos.Y = 0;
            }
            if (scenePlayer.pos.Y <= -50)
            {
                scenePlayer.pos.Y = 720;
            }

        }

        public void Update(GameTime gt)
        {
            if (KeyMouseReader.LeftClick() || KeyMouseReader.KeyPressed(Keys.Escape) || KeyMouseReader.KeyPressed(Keys.Space))
            {
                sceneOver = true;
            }
            if (endSeq)
            {
                screenBlink = true;

                if (scenePlayer.pos.X <= 650 || scenePlayer.pos.Y <= 150)
                {

                    scenePlayer.pos.X += 0.5f;
                    scenePlayer.pos.Y -= 0.5f;                    
                }
                else
                {

                   // sceneOver = true;
                    redScreen = true;
                }             
            }
            
            if (monolog1)
            {
                deltaTime += gt.ElapsedGameTime.Milliseconds;
            }
           
            textScript();
            chooseString();
            if (canMove)
            {
                scenePlayer.Update(gt);
            }
      
            cameraBounds();
            moveScript1();

            if (moveX)
            {
                bossPos.X += 2;
            }
            if (moveY)
            {
                bossPos.Y += 2;
            }
     
        }

        public void Draw(SpriteBatch sb)
        {
            sb.DrawString(sceneText, "Click to skip", new Vector2(0, 0), Color.White);  

            if (screenBlink && !redScreen)
            {
                if (deltaTime >= 200)
                {
                    sb.Draw(TextureManager.redScreen, new Vector2(0, 0), Color.Red);
                    deltaTime = 0;
                }
            }
            scenePlayer.Draw(sb);
            sb.Draw(main.player.playerTex[0], bossPos, Color.Silver);
            if (monolog1 && textList.Count != 0)
            {
                sb.DrawString(sceneText, activeText, new Vector2(450, 400), Color.White);             
            }
            if (redScreen)
            {

                sb.Draw(TextureManager.redScreen, new Vector2(0, 0), Color.Red);
                if (deltaTime >= 1000)
                {
                    sceneOver = true;
                }
            }
        }
    }
}

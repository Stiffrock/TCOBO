using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class PlayerPanel
    {
        private Texture2D boardTex;
        private Vector2 boardPos;
        private Rectangle hitBox;
        private bool showBoard = false;
        public Rectangle StrButton, DexButton, VitButton, IntButton;
        private Tuple<int, int, int, int, int, int> playerStats;
        private Tuple<float, float, float> effectiveStats;
        public SpriteFont spriteFont;
        public  SpriteFont MOStatFont;
        public Color statColor;
        public int Str, Dex, Vit, Int;
        public bool showStatButton = false;
        public SpriteFont currentStrFont, currentDexFont, currentVitFont, currentIntFont;
        SoundManager soundManager = new SoundManager();


        public PlayerPanel(ContentManager content, Vector2 pos, SpriteFont spriteFont)
        {
            this.boardTex = content.Load<Texture2D>("inventory5");
            MOStatFont = content.Load<SpriteFont>("MOStatFont");
            this.hitBox = new Rectangle((int)pos.X, (int)pos.Y, boardTex.Width, boardTex.Height);
            this.boardPos = pos;
            this.spriteFont = spriteFont;
            currentStrFont = spriteFont;
            currentDexFont = spriteFont;
            currentVitFont = spriteFont;
            currentIntFont = spriteFont;
            statColor = Color.Black;
            StrButton = new Rectangle(973, 40, 25, 25);
            DexButton = new Rectangle(1015, 40, 25, 25);
            VitButton = new Rectangle(1058, 40, 25, 25);
            IntButton = new Rectangle(1100, 40, 25, 25);

            soundManager.LoadContent(content);

        }

        private void ShowBoard()
        {
            if (KeyMouseReader.KeyPressed(Keys.I))
            {
                showBoard = !showBoard;
            }
        }

        public void Update(Tuple<int, int, int, int, int, int> playerStats, Tuple<float, float, float> effectiveStats)
        {            
            this.playerStats = playerStats;
            this.effectiveStats = effectiveStats;
            ShowBoard();
        }
          

        public void Draw(SpriteBatch spriteBatch)
        {
            float Dmg = effectiveStats.Item1;
            float SpellDmg = effectiveStats.Item2;
            float HP = effectiveStats.Item3;
            Str = playerStats.Item1;
            Dex = playerStats.Item2;
            Vit = playerStats.Item3;
            Int = playerStats.Item4;
            int Level = playerStats.Item5;
            int newStat = playerStats.Item6;

            //if (HP <= 2)
            //{
            //    MediaPlayer.Play(soundManager.deathSound);
            //}

            if (showBoard)
            {
                
                spriteBatch.Draw(boardTex, boardPos, Color.White);
                spriteBatch.DrawString(spriteFont, " Available Stats: " + newStat.ToString(), new Vector2(967, 20), Color.Black);

               // spriteBatch.DrawString(currentFont, " Str   " + "Dex   " + "Vit   " + "Int   " + "Lvl", new Vector2(567, 40), statColor);

                spriteBatch.DrawString(currentStrFont, "Str", new Vector2(973 ,36), Color.Black);
                spriteBatch.DrawString(currentDexFont, "Dex", new Vector2(1015, 36), Color.Black);
                spriteBatch.DrawString(currentVitFont, "Vit", new Vector2(1055, 36), Color.Black);
                spriteBatch.DrawString(currentIntFont, "Int", new Vector2(1098, 36), Color.Black);
                spriteBatch.DrawString(spriteFont, "Level", new Vector2(1135, 36), Color.Black);
                spriteBatch.DrawString(spriteFont, " " + Str.ToString() + "    " + Dex.ToString() + "    " + Vit.ToString() + "    " + Int.ToString() + "    " + Level.ToString(), new Vector2(967, 50), statColor);               


                spriteBatch.DrawString(currentStrFont, "Dmg: " + Dmg.ToString(), new Vector2(973, 70), Color.Black);
                spriteBatch.DrawString(currentDexFont, "Speed: " + Dex.ToString(), new Vector2(1100, 70), Color.Black);
                spriteBatch.DrawString(currentIntFont, "Mana: " + SpellDmg.ToString(), new Vector2(973, 80), Color.Black);
                spriteBatch.DrawString(currentVitFont, "Hp: " + HP.ToString(), new Vector2(1100, 80), Color.Black);


                
               // spriteBatch.DrawString(spriteFont, " Damage:" + Dmg.ToString() + "   Spell Damage:" + SpellDmg.ToString() + "\n" + " Hp:" + HP.ToString()+ "      Speed Bonus:" + Dex.ToString(), new Vector2(567, 70), Color.Black);
                if (showStatButton)
                {
                   // currentFont = MOStatFont;
                    spriteBatch.Draw(TextureManager.statBox, new Vector2(975, 48), new Rectangle(6, 6, 23, 23), Color.White);
                    spriteBatch.Draw(TextureManager.statBox, new Vector2(1017, 48), new Rectangle(6, 6, 23, 23), Color.White);
                    spriteBatch.Draw(TextureManager.statBox, new Vector2(1059, 48), new Rectangle(6, 6, 23, 23), Color.Gold);
                    spriteBatch.Draw(TextureManager.statBox, new Vector2(1100, 48), new Rectangle(6, 6, 23, 23), Color.Gold);  
                    
                }
                else
                {
                    //currentFont = spriteFont;
                }
        
            }



            
        }
    }
}

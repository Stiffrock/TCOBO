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
        public bool GameOn = false;
        private SpriteFont menuTitle;
        private SpriteFont menuText;
        private KeyMouseReader krm;
        private Vector2 textpos;
        private string tcobo;
        private List<string> stringList = new List<string>();
        private Random rnd = new Random();
        


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
                spriteBatch.DrawString(menuText, tcobo, new Vector2(textpos.X + i*15, textpos.Y + textmove), Color.Blue);
            }
        }

        public void Update(GameTime gameTime)
        {
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

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
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MenuManager menuManager;
        Main main;
        SoundManager soundManager = new SoundManager();
      
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
                
        }
        protected override void Initialize()
        {
            base.Initialize();
   
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            TextureManager.LoadContent(Content);
            soundManager.LoadContent(Content);
            menuManager = new MenuManager(this);
            main = new Main(this);
       
         
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (main.loss)
            {
                main.loss = false;
                main = null;
                menuManager = null;
                menuManager = new MenuManager(this);
                main = new Main(this);
               
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (!menuManager.GameOn)
            {
                menuManager.Update(gameTime);
            }

            if (menuManager.GameOn)
            {
                main.Update(gameTime);
            }

           

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            if (!menuManager.GameOn)
            {
                menuManager.Draw(spriteBatch);
            }
           

            if (menuManager.GameOn)
            {
                main.Draw(spriteBatch);
            }
            
        


            
          
        }
    }
}

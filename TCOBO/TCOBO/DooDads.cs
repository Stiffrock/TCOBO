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
using System.IO;

namespace TCOBO
{
    class DooDads
    {
        public Vector2 currentPos;
        public Texture2D currentTex;
        public string typeOfDoodad;

        public DooDads(string typeOfDoodad, Vector2 currentPos)
        {
            this.typeOfDoodad = typeOfDoodad;
            this.currentPos = currentPos;
            if (typeOfDoodad == "[DOODAD] Big Tree")
            {
                
                //  image = TextureManager.sand1;
                currentTex = TextureManager.bigtree;

            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentTex, currentPos, Color.White);
        }
    }

}

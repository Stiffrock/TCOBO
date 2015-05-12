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
    class TestWorld
    {

        private ContentManager content;
        protected Vector2 pos;
        StreamWriter sw;
        public List<Tile> tiles = new List<Tile>();
        public List<DooDads> dooDads = new List<DooDads>();
        public List<string> stringList = new List<string>();

        public TestWorld(ContentManager content)
        {
            this.content = content;
        }           
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            foreach(DooDads d in dooDads)
            {
                d.Draw(spriteBatch);
            }
        }

        public void SetMap()
        {
            tiles.Clear();
            int skip = 0;
            string typeoftile = "sand1";
            for (int i = 0; i < stringList.Count; i++)
            {
                if (skip == 0)
                {
                    typeoftile = stringList[i];
                    skip = 1;
                }
                else if (skip == 1)
                {
                    int j = 0;
                    string pos_x = "";
                    while (stringList[i][j] != ':')
                    {
                        pos_x = pos_x + stringList[i][j];
                        j++;
                    }
                    j++;
                    String pos_y = "";
                    while (stringList[i].Length > j)
                    {
                        pos_y = pos_y + stringList[i][j];
                        j++;
                    }
                    if(typeoftile.Length > 7)
                    {
                        if (typeoftile.Substring(0, 8) == "[DOODAD]")
                        {
                            dooDads.Add(new DooDads(typeoftile, new Microsoft.Xna.Framework.Vector2(float.Parse(pos_x), float.Parse(pos_y))));
                        }
                    }
                    else
                    tiles.Add(new Tile(typeoftile, new Microsoft.Xna.Framework.Vector2(float.Parse(pos_x), float.Parse(pos_y))));
                    skip = 0;
                }

            }
        }

        public bool ReadLevel(String name)
        {
            try
            {
                StreamReader sr;
                stringList = new List<String>();
                sr = new StreamReader(name + ".txt");
                while (!sr.EndOfStream)
                {
                    stringList.Add(sr.ReadLine());
                }
                sr.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

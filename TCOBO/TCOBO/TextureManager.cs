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
    public static class TextureManager
    {

        public static Texture2D sand1 { get; private set; }
        public static Texture2D tree1 { get; private set; }
        public static Texture2D road1 { get; private set; }
        public static Texture2D grasstile1 { get; private set; }
        public static Texture2D sandtile1 { get; private set; }
        public static Texture2D bushtile1 { get; private set; }
        public static Texture2D smalltree { get; private set; }
        public static Texture2D bigtree { get; private set; }
        public static Texture2D graveltile1 { get; private set; }
        public static Texture2D bricktile1 { get; private set; }
        public static Texture2D watertile1 { get; private set; }
        public static Texture2D standardSword { get; private set; }
        public static Texture2D redSword { get; private set; }
        public static Texture2D blueSword { get; private set; }
        public static Texture2D goldenSword { get; private set; }
        public static Texture2D statBox { get; private set; }
        public static Texture2D standardArmor { get; private set; }
        public static Texture2D blankHpBar { get; private set; }
        public static SpriteFont uitext { get; private set; }
        public static Texture2D redScreen { get; private set; }
        public static Texture2D RedKey { get; private set; }
        public static Texture2D RedWall { get; private set; }
        public static Texture2D BlueKey { get; private set; }
        public static Texture2D BlueWall { get; private set; }
        public static Texture2D YellowKey { get; private set; }
        public static Texture2D YellowWall { get; private set; }






        public static void LoadContent(ContentManager Content)
        {
            sand1 = Content.Load<Texture2D>("sand1");
            tree1 = Content.Load<Texture2D>("tree1");
            road1 = Content.Load<Texture2D>("road1");
            grasstile1 = Content.Load<Texture2D>("grasstile_type4");
            sandtile1 = Content.Load<Texture2D>("sandtile_type3");
            bushtile1 = Content.Load<Texture2D>("bushtile_type5");
            smalltree = Content.Load<Texture2D>("treetile_type50x50");
            bigtree = Content.Load<Texture2D>("treetile_blackoutline");
            graveltile1 = Content.Load<Texture2D>("gravel_type1");
            bricktile1 = Content.Load<Texture2D>("brick_type1");
            watertile1 = Content.Load<Texture2D>("watertile_type2");
            standardSword = Content.Load<Texture2D>("inventorysword_type1");
            blueSword = Content.Load<Texture2D>("inventorysword_type3");
            redSword = Content.Load<Texture2D>("inventorysword_type4");
            goldenSword = Content.Load<Texture2D>("inventorysword_type2");
            statBox = Content.Load<Texture2D>("statbox4");
            standardArmor = Content.Load<Texture2D>("armor_type1");
            blankHpBar = Content.Load<Texture2D>("blankHp");
            redScreen = Content.Load<Texture2D>("screenBlink");
            uitext = Content.Load<SpriteFont>("SpriteFont1");
            RedKey = Content.Load<Texture2D>("redkey");
            RedWall = Content.Load<Texture2D>("redwall");
            BlueKey = Content.Load<Texture2D>("bluekey");
            BlueWall = Content.Load<Texture2D>("bluewall");
            YellowKey = Content.Load<Texture2D>("yellowkey");
            YellowWall = Content.Load<Texture2D>("yellowwall");

        }

    }




}

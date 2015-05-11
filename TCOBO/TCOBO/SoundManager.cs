using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class SoundManager
    {
        public SoundEffect statseffect;

        public SoundManager()
        {
            statseffect = null;
        }

        public void LoadContent(ContentManager Content)
        {
            statseffect = Content.Load<SoundEffect>("StatsLjud");
        }
    }
}

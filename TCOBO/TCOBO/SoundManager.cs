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
        public SoundEffect statSound;
        public SoundEffect fightSound;
        public SoundEffect equipSound;
        public SoundEffect inventorySound;
        public Song bgMusic;
        public SoundEffect levelupSound;

        public SoundManager()
        {
            statSound = null;
            fightSound = null;
            equipSound = null;
            inventorySound = null;
            levelupSound = null;
            bgMusic = null;
        }

        public void LoadContent(ContentManager Content)
        {
            statSound = Content.Load<SoundEffect>("StatsLjud");
            fightSound = Content.Load<SoundEffect>("FightSound");
            equipSound = Content.Load<SoundEffect>("EquipSound");
            inventorySound = Content.Load<SoundEffect>("InventorySound");
            levelupSound = Content.Load<SoundEffect>("Level up sound");
            //bgMusic = Content.Load<Song>("TCOBO musik");
        }
    }
}

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
        public SoundEffect levelupSound;
        public SoundEffect bounceSound;
        public SoundEffect runningSound;
        public SoundEffect hitSound;

        public Song bgMusic;
        public Song deathSound;

        public SoundManager()
        {
            statSound = null;
            fightSound = null;
            equipSound = null;
            inventorySound = null;
            levelupSound = null;
            bounceSound = null;
            runningSound = null;
            hitSound = null;


            bgMusic = null;
            deathSound = null;
        }

        public void LoadContent(ContentManager Content)
        {
            statSound = Content.Load<SoundEffect>("StatsLjud");
            fightSound = Content.Load<SoundEffect>("FightSound1");
            equipSound = Content.Load<SoundEffect>("EquipSound");
            inventorySound = Content.Load<SoundEffect>("InventorySound");
            levelupSound = Content.Load<SoundEffect>("Level up sound");
            bounceSound = Content.Load<SoundEffect>("BouncingSound");
            runningSound = Content.Load<SoundEffect>("RunningSound");
            hitSound = Content.Load<SoundEffect>("bloodsplat");

            bgMusic = Content.Load<Song>("TCOBO musik");
            deathSound = Content.Load<Song>("Death sound");
        }
    }
}

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
    class Attack : Abilities
    {
        private Player player;
        private List<Enemy> inrangeList;
        private float write;

        SoundManager soundManager = new SoundManager();

        public Attack(Player player, ContentManager content)
        {
  
            this.player = player;

            soundManager.LoadContent(content);

            
            
        }

        public void inRange(Enemy enemy, Vector2 aimVector)
        {

            if (KeyMouseReader.LeftClick() == true && player.swordEquipped)
            {
                double deltaX = enemy.pos.X - player.playerPos.X;
                double deltaY =  enemy.pos.Y - player.playerPos.Y;

                soundManager.hitSound.Play();

                double h = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
                float dn = (float)h;
                enemy.velocity += new Vector2((float)deltaX / dn * 260, (float)deltaY / dn * 260);
                enemy.health -= (int)player.mDamage;
                if (enemy.health < 0)
                {
                    player.Exp += enemy.expDrop;
                }
            }
        }


        public override void Draw()
        {
           
           
        }
        public override void Update(GameTime gameTime)
        {
        }
    }
}

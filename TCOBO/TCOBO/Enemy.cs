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
    class Enemy : MovableObject 
    {
        public Vector2 pos;
        Rectangle attackHitBox;
        private ContentManager content;
        protected SpriteEffects Fx;
        protected float rotation;
        public Rectangle hitBox;
        public float speed = 230f, max_speed = 130, slow_speed = 85, slow_speed_2 = 200;
        private bool move, moveUp, moveDown, moveLeft, moveRight, strike, strike2;
        private List<Texture2D> tex = new List<Texture2D>();
        private List<Texture2D> swordTex = new List<Texture2D>();
        private List<Texture2D> blood = new List<Texture2D>();
        public Vector2 velocity, velocity2;
        private Vector2 acceleration;
        public float size = 1;
        Vector2 origin = new Vector2(80, 80);
        Color color;
        public int animaCount = 0;
        private float deltaTime = 0;
        float attackspeed = 5f;
        TimeSpan attack_timer;
        float attack_seconds;
        float spawnTime = 0;
        TimeSpan spawn_timer;
        float attackProgress = 0f;
        public float playerSize = 36, basePlayerSize = 36;
        public Rectangle boundsTop, boundsBot, boundsLeft, boundsRight;
        Texture2D strikeTexSword1, strikeTexPlayer1, strikeTexSword2, strikeTexPlayer2, deathTex;
        bool drawBlood = false;
        public int
        Str = 10, Dex = 10,
        Vit = 10, Int = 10, health, expDrop;
        public bool dead = false;
        bool spawn = false;
        Random rnd;
        List<ParticleEngine> particleEngine = new List<ParticleEngine>();
        Vector2 aimRec;

        SoundManager soundManager = new SoundManager();

        public Enemy(Vector2 pos, ContentManager content, int Str, int Dex, int Vit, int Int, int expDrop, int spawnTime)
        {
            this.spawnTime = spawnTime;
            if (spawnTime > 0)
                spawn = true;
            this.Str = Str;
            this.Dex = Dex;
            speed += Dex;
            max_speed += Dex*2;
            
            this.Vit = Vit;
            this.Int = Int;
            this.expDrop = expDrop;
            attack_seconds = 1.5f;
            attack_timer = TimeSpan.FromSeconds(attack_seconds);
            rnd = new Random();
            health = Vit*5;
            this.content = content;
            this.pos = pos;
            hitBox = new Rectangle((int)pos.X-15, (int)pos.Y-15, 30, 30);
            Fx = SpriteEffects.None;
            color = new Color(0, 0, 255 - (Str * 10));
            for (int i = 1; i < 22; i++)
            {
                tex.Add(content.Load<Texture2D>("player" + i));
            }
            for (int i = 1; i < 22; i++)
            {
                swordTex.Add(content.Load<Texture2D>("sword" + i));
            }
          /*  for (int i = 1; i < 7; i++)
            {
                blood.Add(content.Load<Texture2D>("fire" + i));
            }*/
            blood.Add(TextureManager.blood2);
            strikeTexSword1 = content.Load<Texture2D>("faststrikeSword4.1");
            strikeTexPlayer1 = content.Load<Texture2D>("faststrikePlayer1");
            strikeTexSword2 = content.Load<Texture2D>("faststrikeSword5");
            strikeTexPlayer2 = content.Load<Texture2D>("faststrikePlayer2");
            deathTex = content.Load<Texture2D>("Death");

            soundManager.LoadContent(content);
        }
        public void HuntPlayer(Player player, GameTime gameTime)
        {
            float distance = Vector2.Distance(player.pos, pos);
            if (distance < 400 + (Vit *10))
            {
                move = true;
                if (player.pos.X > pos.X) {
                    moveRight = true;
                    moveLeft = false;
                }
                else if (player.pos.X < pos.X) {
                    moveLeft = true;
                    moveRight = false;
                }

                if (player.pos.Y > pos.Y)
                {
                    moveDown = true;
                    moveUp = false;
                }
                else if (player.pos.Y < pos.Y)
                {
                    moveUp = true;
                    moveDown = false;
                }


                float xDistance = (float)player.pos.X - pos.X;
                float yDistance = (float)player.pos.Y - pos.Y;

                aimRec = new Vector2(xDistance, yDistance);
                aimRec.Normalize();
                double recX = (double)aimRec.X * 40 * size;
                double recY = (double)aimRec.Y * 40 * size;
                attackHitBox = new Rectangle((int)(pos.X + recX - 25 * size), (int)(pos.Y + recY - 25 * size), (int)(50 * size), (int)(50 * size));


                if (attack_timer.TotalSeconds > 0)
                    attack_timer = attack_timer.Subtract(gameTime.ElapsedGameTime);
                else
                {
                    if (player.hitBox.Intersects(attackHitBox))
                    {
                        moveDown = false;
                        moveUp = false;
                        moveLeft = false;
                        moveRight = false;
                        move = false;

                        if (!strike && !strike2)
                        {
                            strike = true;
                            if (!player.shieldUp)
                            {
                                player.HP -= Str;
                                soundManager.hitSound.Play();
                            } 
                            else if (player.shieldUp)
                                soundManager.ShieldHitSound.Play();
                        }
                        else if (strike)
                        {

                            if (1 == rnd.Next(1, 7))
                            {
                                strike2 = true;
                                if (!player.shieldUp)
                                {
                                    player.HP -= Str;
                                    soundManager.hitSound.Play();
                                }
                                else if (player.shieldUp)
                                    soundManager.ShieldHitSound.Play();
                            }

                            attack_seconds = rnd.Next(0, 4);
                            attack_timer = TimeSpan.FromSeconds(attack_seconds);
                        }
                        
                    }
                }

                

                

            }
            else
            {
                moveDown = false;
                moveUp = false;
                moveLeft = false;
                moveRight = false;
                move = false;
            }
        }

        public void UpdateEnemy(GameTime gameTime, Player player, List<Tile> tiles)
        {
            UpdateParticle(gameTime);
            float distance = Vector2.Distance(player.pos, pos);
            if (health > 0 && distance < 400 + (Vit * 2))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                {
                    isHpBarVisible = true;
                }
                else 
                    isHpBarVisible = false;

                float tempVit = Vit;
                size = 1 + ((tempVit-10) / 30);
                Fx = SpriteEffects.None;
                //HuntPlayer(playerPos);
                hitBox.X = (int)pos.X;
                hitBox.Y = (int)pos.Y;
                Movement(gameTime);
                Rotation(player.GetPos());
                HuntPlayer(player, gameTime);
                handleAnimation(gameTime);
                Collision(gameTime, tiles);
                
            }
            else if (spawn == true)
            {
                if (distance > 1000 + (Vit * 10))
                {
                    if (spawn_timer.TotalSeconds > 0)
                        spawn_timer = spawn_timer.Subtract(gameTime.ElapsedGameTime);
                    else
                    {
                        dead = false;
                        health = Vit * 5;
                        spawn_timer = TimeSpan.FromSeconds(spawnTime);
                    }
                }
            }

        }

        public void Rotation(Vector2 playerPos)
        {
            float xDistance = (float)playerPos.X - pos.X;
            float yDistance = (float)playerPos.Y - pos.Y;
            rotation = (float)Math.Atan2(yDistance, xDistance);
        }

        public void StartParticleEffect()
        {
            ParticleEngine NPE = new ParticleEngine(blood, new Vector2(pos.X, pos.Y), Vit);
            NPE.bloodTimer = TimeSpan.FromSeconds(NPE.bloodTime);
            NPE.drawBlood = true;
            particleEngine.Add(NPE);
        }

        public void UpdateParticle(GameTime gameTime)
        {
            foreach (ParticleEngine PE in particleEngine) {
                if (PE.drawBlood)
                {
                    if (PE.bloodTimer.TotalSeconds > 0)
                        PE.bloodTimer = PE.bloodTimer.Subtract(gameTime.ElapsedGameTime);
                    else
                    {
                        PE.drawBlood = false;
                        PE.bloodTimer = TimeSpan.FromSeconds(PE.bloodTime);
                    }
                    PE.EmitterLocation = new Vector2(pos.X, pos.Y);
                    PE.Update();
                }
                else if (!PE.drawBlood)
                {
                    PE.KillParticles();
                    if (PE.particles.Count == 0)
                    {
                        particleEngine.Remove(PE);
                        break;
                    }
                       
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
           
            if (health > 0)
            {
                if (strike)
                {
                    spriteBatch.Draw(strikeTexSword1, pos, null, new Color(235,10,10), rotation, origin, size, SpriteEffects.None, 0f);
                    spriteBatch.Draw(strikeTexPlayer1, pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
                else if (strike2)
                {
                    spriteBatch.Draw(strikeTexSword2, pos, null, new Color(235, 10, 10), rotation, origin, size, SpriteEffects.None, 0f);
                    spriteBatch.Draw(strikeTexPlayer2, pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(swordTex[animaCount], pos, null, new Color(235, 10, 10), rotation, origin, size, SpriteEffects.None, 0f);
                    spriteBatch.Draw(tex[animaCount], pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
            }
            else
            {
                //spriteBatch.Draw(deathTex, pos, null, Color.White, rotation, origin, size, SpriteEffects.None, 0f);
            }

            if (isHpBarVisible && health > 0)
            {
                float tempVit = Vit;
                percentLife = health / (tempVit * 5);
                if (percentLife < 1.0f)
                {
                    spriteBatch.Draw(TextureManager.blankHpBar, new Rectangle((int)pos.X - hitBox.Width / 2,
                        ((int)pos.Y - 4) - hitBox.Height / 2, hitBox.Width, 4), Color.Red); // ritar över en röd bar över den gröna
                }
                spriteBatch.Draw(TextureManager.blankHpBar, new Rectangle((int)pos.X - hitBox.Width / 2,
                    ((int)pos.Y - 4) - hitBox.Height / 2, (int)(hitBox.Width * percentLife), 4), Color.Black);
            }


            //spriteBatch.Draw(TextureManager.sand1, hitBox, Color.Black);
            //spriteBatch.Draw(TextureManager.bricktile1, attackHitBox, Color.Black);
            //spriteBatch.Draw(TextureManager.sand1, boundsTop, Color.White);
            //spriteBatch.Draw(TextureManager.sand1, boundsBot, Color.White);
            //spriteBatch.Draw(TextureManager.sand1, boundsLeft, Color.White);
            //spriteBatch.Draw(TextureManager.sand1, boundsRight, Color.White);
        }

        public void DrawBlood(SpriteBatch sb)
        {
            foreach (ParticleEngine PE in particleEngine)
            {
                PE.Draw(sb);
            }
            
        }

        public void handleAnimation(GameTime gameTime)
        {
            deltaTime += gameTime.ElapsedGameTime.Milliseconds;
            if (strike == true)
            {
                if (deltaTime >= 1)
                {
                    deltaTime = 0;
                    attackProgress += attackspeed;
                    if (attackProgress > 100)
                    {
                        strike = false;
                        animaCount = 0;
                        attackProgress = 0;
                    }
                }
            }
            else if (strike2 == true)
            {
                if (deltaTime >= 1)
                {
                    deltaTime = 0;
                    attackProgress += attackspeed;
                    if (attackProgress > 100)
                    {
                        strike2 = false;
                        animaCount = 0;
                        attackProgress = 0;
                    }
                }
            }
            else if (move == true)
            {
                if (deltaTime >= 60)
                {
                    deltaTime = 0;
                    animaCount++;
                    if (animaCount > 19)
                        animaCount = 0;
                }
            }
        }

        public void Movement(GameTime gameTime)
        {
            if (moveLeft)
            {
                acceleration.X = -speed;
            }
            else if (moveRight)
            {
                acceleration.X = speed;
            }
            else
            {
                acceleration.X = 0;
                if (velocity.X > 0)
                {
                    if (velocity.X - slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds <= 0)
                        velocity.X = 0;
                    else
                        velocity.X -= slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (velocity.X < 0)
                {
                    if (velocity.X + slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds >= 0)
                        velocity.X = 0;
                    else
                        velocity.X += slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (moveUp)
            {
                acceleration.Y = -speed;
            }
            else if (moveDown)
            {
                acceleration.Y = speed;
            }
            else
            {
                acceleration.Y = 0;
                if (velocity.Y > 0)
                {
                    if (velocity.Y - slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds <= 0)
                        velocity.Y = 0;
                    else
                        velocity.Y -= slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                }
                else if (velocity.Y < 0)
                {
                    if (velocity.Y + slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds >= 0)
                        velocity.Y = 0;
                    else
                        velocity.Y += slow_speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (velocity.X > max_speed)
                velocity.X = max_speed;
            else if (velocity.X < -max_speed)
                velocity.X = -max_speed;
            if (velocity.Y > max_speed)
                velocity.Y = max_speed;
            else if (velocity.Y < -max_speed)
                velocity.Y = -max_speed;


            velocity += Vector2.Multiply(acceleration, (float)gameTime.ElapsedGameTime.TotalSeconds);
            pos += Vector2.Multiply(velocity, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Collision(GameTime gameTime, List<Tile> tiles)
        {
            hitBox = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 10), (int)(pos.Y - playerSize / 2 + playerSize / 10), (int)(playerSize - playerSize / 5), (int)(playerSize - playerSize / 5));
            playerSize = basePlayerSize * size;
            boundsTop = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 5), (int)(pos.Y - playerSize / 2 + playerSize / 10), (int)(playerSize - (playerSize / 2.5f)), (int)(playerSize / 8));
            boundsBot = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 5), (int)((pos.Y + playerSize / 2 - playerSize / 4f)), (int)(playerSize - (playerSize / 2.5f)), (int)(playerSize / 8));
            boundsLeft = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 8), (int)(pos.Y - playerSize / 2 + playerSize / 4.5f), (int)(playerSize / 8), (int)(playerSize - playerSize / 2));
            boundsRight = new Rectangle((int)(pos.X + playerSize / 2 - playerSize / 4), (int)(pos.Y - playerSize / 2 + playerSize / 4.5f), (int)(playerSize / 8), (int)(playerSize - playerSize / 2));
            foreach (Tile t in tiles)
            {

                if (t.collisionEnabled)
                {
                    if (t.bounds.Intersects(boundsLeft))
                    {
                        if (velocity.X < 0)
                            velocity.X = (velocity.X * -0.8f) + 10;
                        else
                            velocity.X = 10;
                        velocity.Y = velocity.Y * 0.9f;
                        break;

                    }
                    if (t.bounds.Intersects(boundsRight))
                    {
                        if (velocity.X < 0)
                            velocity.X = -10;
                        else
                            velocity.X = (velocity.X * -0.8f) - 10;
                        velocity.Y = velocity.Y * 0.9f;
                        break;
                    }
                    if (t.bounds.Intersects(boundsBot))
                    {
                        if (velocity.Y < 0)
                            velocity.Y = -10;
                        else
                            velocity.Y = (velocity.Y * -0.8f) - 10;
                        velocity.X = velocity.X * 0.9f;
                        break;

                    }
                    if (t.bounds.Intersects(boundsTop))
                    {
                        if (velocity.Y < 0)
                            velocity.Y = (velocity.Y * -0.8f) + 10;
                        else
                            velocity.Y = 10;
                        velocity.X = velocity.X * 0.9f;
                        break;
                    }

                }
            }
        }
    }
}

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
    class Player : MovableObject 
    {
        public Vector2 pos, origin, aimRec;
        private ContentManager content;
        public Color swordColor, newColor;
        public Rectangle srcRec, attackHitBox;
        public float deltaTime, Exp = 0, mDamage = 1, sDamage = 1, HP = 10;
        private List<Texture2D> levelUpTex = new List<Texture2D>();
        public float rotation = 0f;
        public int 
            animaCount = 0, Level = 1, Str = 10, Dex = 10, 
            Vit = 10, Int = 10, maxLvl = 101, newStat = 5;
        private Color color;
        public float speed = 230f, max_speed = 130, slow_speed = 85, slow_speed_2 = 200;
        public bool swordEquipped = true, swordinHand = false, armorEquip = false;
        public Vector2 velocity, velocity2;
        private Vector2 acceleration;
        private Tuple<int, int, int, int, int, int> playerStats;
        private Tuple<float, float, float> effectiveStats;
        private bool move, moveUp, moveDown, moveLeft, moveRight, strike, strike2;
        public List<Texture2D> playerTex = new List<Texture2D>();
        private List<Texture2D> swordTex = new List<Texture2D>();
        private List<Texture2D> heal = new List<Texture2D>();
        public Texture2D strikeTexSword1, strikeTexPlayer1, strikeTexSword2, strikeTexPlayer2, deathTex, shieldTex;
        private List<float> levelList = new List<float>();
        public Rectangle boundsTop, boundsBot, boundsLeft, boundsRight, hitBox;
        public float attackspeed = 5f;
        float attackProgress = 0f;
        public float playerSize = 36, basePlayerSize = 36;
        public float size;
        public bool healing = false, scene1 = false, dead = false;
        public int MANA;
        float manaTicDelay = 1.5f;
        TimeSpan manaTimer;
        public bool hasRedKey, hasBlueKey, hasYellowKey, hasGreenKey;
        public Vector2 strikeVelocity;
        public bool shieldUp = false;
        public Rectangle drawRec;
        List<ParticleEngine> particleEngine = new List<ParticleEngine>();
        SoundManager soundManager = new SoundManager();
        SoundEffectInstance instance;
      
        public Vector2 GetPos()
        {
            return pos;
        }

        public Tuple<int, int, int, int, int, int> GetPlayerStats()
        {
            return playerStats;
        }
        public Tuple<float, float, float> GetEffectiveStats()
        {
            return effectiveStats;
        }


        public Player(ContentManager content)
        {
            this.content = content;
          //  swordColor = Color.White;
            pos = new Vector2(-145, -50);
            //attackHitBox = new Rectangle(0, 0, 0, 0);
            srcRec = new Rectangle(0, 0, 100, 100);
            origin = new Vector2(80, 80);
            color = new Color(255, 30, 30, 255);
            size = 1 + ((Vit-10) / 30);
            
            HP = Vit * 5;
            MANA = Int*10;
            LoadPlayerTex();
            HandleLevel();

            soundManager.LoadContent(content);
            instance = soundManager.ShieldSound.CreateInstance();
            instance.IsLooped = true;
            instance.Volume = 1f;
        }

        public void HandleLevel()
        {            
            for (int i = 1; i < maxLvl; i++)
            {
                float newLevel = i*10;
                levelList.Add(newLevel);                
            }         
        }

        public void HandleLevelUp()
        {
            if (Level <= 99)
            {
                float needExp = levelList[Level];

                if (Exp > needExp)
                {
                    Level += 1;
                    newStat += 5;
                    Exp = 0;
                    StartParticleEffect();

                    soundManager.levelupSound.Play();

                }
            }
        }

        public void HandlePlayerStats(GameTime gameTime) // Bör göra all stat förändring här
        {
            playerStats = Tuple.Create<int, int, int, int, int, int>(Str, Dex, Vit, Int, Level, newStat);
            effectiveStats = Tuple.Create<float, float, float>(mDamage, MANA, HP);
            
            mDamage = Str * 0.5f;
            sDamage = Int * 0.5f;

            if (manaTimer.TotalSeconds > 0)
                manaTimer = manaTimer.Subtract(gameTime.ElapsedGameTime);
            else
            {
                MANA += Int/5;
                if (MANA > Int*10)
                    MANA = Int*10;
                manaTimer = TimeSpan.FromSeconds(manaTicDelay);
            }

            if (KeyMouseReader.KeyPressed(Keys.D5))
            {
                Exp += 5;
            }
        }

        private void LoadPlayerTex()
        {
            for (int i = 1; i < 22; i++)
            {
                playerTex.Add(content.Load<Texture2D>("player" + i));
            }
            for (int i = 1; i < 22; i++)
            {
                swordTex.Add(content.Load<Texture2D>("sword" + i));
            }
            for (int i = 1; i < 6; i++)
            {
                heal.Add(content.Load<Texture2D>("heal" + i));
            }
            for (int i = 1; i < 7; i++)
            {
                levelUpTex.Add(content.Load<Texture2D>("fire" + i));
            }
            
            strikeTexSword1 = content.Load<Texture2D>("faststrikesword4");
            strikeTexPlayer1 = content.Load<Texture2D>("faststrikePlayer1");
            strikeTexSword2 = content.Load<Texture2D>("faststrikeSword5");
            strikeTexPlayer2 = content.Load<Texture2D>("faststrikePlayer2");
            deathTex = content.Load<Texture2D>("deadPlayer2");
            shieldTex = content.Load<Texture2D>("shield");
            
        }

        private void Movement(GameTime gameTime)
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
            pos += Vector2.Multiply(velocity2, (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (velocity2.Y > 0)
            {
                if (velocity2.Y - slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds <= 0)
                    velocity2.Y = 0;
                else
                    velocity2.Y -= slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            }
            else if (velocity2.Y < 0)
            {
                if (velocity2.Y + slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds >= 0)
                    velocity2.Y = 0;
                else
                    velocity2.Y += slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (velocity2.X > 0)
            {
                if (velocity2.X - slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds <= 0)
                    velocity2.X = 0;
                else
                    velocity2.X -= slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (velocity2.X < 0)
            {
                if (velocity2.X + slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds >= 0)
                    velocity2.X = 0;
                else
                    velocity2.X += slow_speed_2 * (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                    attackProgress+= attackspeed;
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
                    attackProgress+= attackspeed;
                    if (attackProgress > 100)
                    {
                        strike2 = false;
                        animaCount = 0;
                        attackProgress = 0;
                    }
                }
            }
            else if (healing == true)
            {
                if (deltaTime >= 130)
                {
                    deltaTime = 0;
                    animaCount++;
                    if (animaCount > 4)
                    {
                        healing = false;
                        HP += Int;
                        if (HP > Vit * 5)
                            HP = Vit * 5;
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

        public void playerDirection()
        {
            moveLeft = false;
            moveRight = false;
            moveUp = false;
            moveDown = false;
            move = false;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                move = true;
                moveRight = true;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                move = true;
                moveLeft = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                move = true;
                moveUp = true;
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                move = true;
                moveDown = true;
            }
                
            
        }

        private void handleAction(GameTime gameTime)
        {   

            if (KeyMouseReader.LeftClick() == true && swordEquipped == true && strike == false && strike2 == false && swordinHand && !healing)
            {
                strike = true;
                animaCount = 0;
                velocity += strikeVelocity;
                soundManager.fightSound.Play();
                
            }
            else if (KeyMouseReader.LeftClick() == true && swordEquipped == true && strike == true && strike2 == false && swordinHand && !healing)
            {
                strike = false;
                strike2 = true;
                animaCount = 0;
                attackProgress = 0;
                velocity += strikeVelocity;
                soundManager.fightSound.Play();
            }

            if (KeyMouseReader.mouseState.RightButton == ButtonState.Pressed && MANA > 0)
            {
                instance.Play();
                shieldUp = true;
                MANA--;
            }
            else
            {
                instance.Stop();
                shieldUp = false;
            }
                
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                if (!strike && !strike2 && !healing && MANA >= 40)
                {
                    animaCount = 0;
                    healing = true;
                    MANA -= 40;
                }
            }
            //spell
        }

        public void StartParticleEffect()
        {
            ParticleEngine NPE = new ParticleEngine(levelUpTex, new Vector2(pos.X, pos.Y), 60, false);
            NPE.bloodTimer = TimeSpan.FromSeconds(NPE.bloodTime);
            NPE.drawBlood = true;
            particleEngine.Add(NPE);
        }

        public void UpdateParticle(GameTime gameTime)
        {
            foreach (ParticleEngine PE in particleEngine)
            {
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
            }

        }
        

        public void Collision (GameTime gameTime, List<Tile> tiles) 
         {
            playerSize = basePlayerSize * size;
            hitBox = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 10), (int)(pos.Y - playerSize / 2 + playerSize / 10), (int)(playerSize - playerSize / 5), (int)(playerSize - playerSize / 5));

            boundsTop = new Rectangle((int)(pos.X - playerSize/2 + playerSize / 5), (int)(pos.Y - playerSize/2 + playerSize/10), (int)(playerSize - (playerSize / 2.5f)), (int)(playerSize / 8));
            boundsBot = new Rectangle((int)(pos.X - playerSize/2 + playerSize / 5), (int)((pos.Y + playerSize / 2 - playerSize / 4f)), (int)(playerSize - (playerSize / 2.5f)), (int)(playerSize / 8));
            boundsLeft = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 8), (int)(pos.Y - playerSize / 2 + playerSize / 4.5f), (int)(playerSize / 8), (int)(playerSize - playerSize / 2));
            boundsRight = new Rectangle((int)(pos.X + playerSize / 2 - playerSize / 4), (int)(pos.Y - playerSize / 2 + playerSize / 4.5f), (int)(playerSize / 8), (int)(playerSize - playerSize / 2));
            foreach (Tile t in tiles)
            {
                if (t.typeOfTile == "redwall" && hasRedKey == true)
                    t.collisionEnabled = false;
                else if (t.typeOfTile == "bluewall" && hasBlueKey == true)
                    t.collisionEnabled = false;
                else if (t.typeOfTile == "yellowwall" && hasYellowKey == true)
                    t.collisionEnabled = false;
                else if (t.typeOfTile == "greenwall" && hasGreenKey == true)
                    t.collisionEnabled = false;

                if (t.typeOfTile == "redwall" && hasRedKey == false)
                    t.collisionEnabled = true;
                else if (t.typeOfTile == "bluewall" && hasBlueKey == false)
                    t.collisionEnabled = true;
                else if (t.typeOfTile == "yellowwall" && hasYellowKey == false)
                    t.collisionEnabled = true;
                else if (t.typeOfTile == "greenwall" && hasGreenKey == false)
                    t.collisionEnabled = true;

                if (t.collisionEnabled)
                {
                    if (t.bounds.Intersects(boundsLeft))
                    {
                        soundManager.bounceSound.Play();
                        if (velocity.X < 0)
                            velocity.X = (velocity.X * -0.8f) + 10;
                        else
                            velocity.X = 10;
                        velocity.Y = velocity.Y * 1.1f;
                        break;

                    }
                    if (t.bounds.Intersects(boundsRight))
                    {
                        soundManager.bounceSound.Play();
                        if (velocity.X < 0)
                            velocity.X = -10;
                        else
                            velocity.X = (velocity.X * -0.8f) - 10;
                        velocity.Y = velocity.Y * 1.1f;
                        break;
                    }
                    if (t.bounds.Intersects(boundsBot))
                    {
                        soundManager.bounceSound.Play();
                        if (velocity.Y < 0)
                            velocity.Y = -10;
                        else
                            velocity.Y = (velocity.Y * -0.8f) - 10;
                        velocity.X = velocity.X * 1.1f;
                        break;

                    }
                    if (t.bounds.Intersects(boundsTop))
                    {
                        soundManager.bounceSound.Play();
                        if (velocity.Y < 0)
                            velocity.Y = (velocity.Y * -0.8f) + 10;
                        else
                            velocity.Y = 10;
                        velocity.X = velocity.X * 1.1f;
                        break;
                    }
                }
            }
        }

        public void colorswitch(Color newCol)
        {
            swordColor = newCol;

        }
        public void stopMove()
        {
            if (KeyMouseReader.KeyPressed(Keys.Space))
            {
                velocity = new Vector2(0, 0);
                velocity2 = new Vector2(0, 0);
            }
        }

        //public void PlaySound()
        //{
        //    MediaPlayer.Play(soundManager.deathSound);
            
        //}


        public override void Update(GameTime gameTime)
        {
            drawRec = new Rectangle((int)pos.X - 1000, (int)pos.Y - 1000, 2000, 2000);
            effectiveStats = Tuple.Create<float, float, float>(mDamage, MANA, HP);
            if (HP > 0)
            {
                UpdateParticle(gameTime);
                if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                {
                    isHpBarVisible = true;
                }
                else isHpBarVisible = false;

                float tempVit = Vit;
                size = 1 + ((tempVit - 10) / 30);
                stopMove();
                HandleLevelUp();
                HandlePlayerStats(gameTime);
                playerDirection();
                Movement(gameTime);
                handleAction(gameTime);
                handleAnimation(gameTime);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {       
            //spriteBatch.Draw(TextureManager.sand1, boundingBox, Color.Black);
            if (HP > 0)
            {
                if (shieldUp)
                    spriteBatch.Draw(shieldTex, pos, null, new Color(155,20,20), rotation, origin, size, SpriteEffects.None, 0f);

                if (swordEquipped && !(strike || strike2 || healing))
                    spriteBatch.Draw(swordTex[animaCount], pos, null, swordColor, rotation, origin, size, SpriteEffects.None, 0f);

                if (strike)
                {
                    spriteBatch.Draw(strikeTexSword1, pos, null, swordColor, rotation, origin, size, SpriteEffects.None, 0f);
                    spriteBatch.Draw(strikeTexPlayer1, pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
                else if (strike2)
                {
                    spriteBatch.Draw(strikeTexSword2, pos, null, swordColor, rotation, origin, size, SpriteEffects.None, 0f);
                    spriteBatch.Draw(strikeTexPlayer2, pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
                else if (healing)
                {
                    spriteBatch.Draw(heal[animaCount], pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(playerTex[animaCount], pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }

                if (armorEquip)
                {
                    spriteBatch.Draw(TextureManager.standardArmor, new Vector2(pos.X, pos.Y), null, Color.Black, rotation, new Vector2(15, 15), size, SpriteEffects.None, 0f);
                }
           
            }
            else
            {
                dead = true;
                MediaPlayer.Play(soundManager.deathSound);
            
            }
            
            if(isHpBarVisible && HP > 0)
            {
                float tempVit = Vit;
                percentLife = HP / (tempVit * 5);
                if (percentLife < 1.0f)
                {
                    spriteBatch.Draw(TextureManager.blankHpBar, new Rectangle((int)pos.X - hitBox.Width / 2,
                        ((int)pos.Y - 4) - hitBox.Height / 2, hitBox.Width, 4), Color.Red); // ritar över en röd bar över den gröna
                }
                spriteBatch.Draw(TextureManager.blankHpBar, new Rectangle((int)pos.X - hitBox.Width / 2,
                    ((int)pos.Y - 4) - hitBox.Height / 2, (int)(hitBox.Width * percentLife), 4), Color.Black);
            }
            

          

            //Show attackHitBox
           // spriteBatch.Draw(TextureManager.bricktile1, attackHitBox, Color.Black);

            //spriteBatch.Draw(TextureManager.bricktile1, hitBox, Color.Black);



            //spriteBatch.Draw(TextureManager.sand1, boundsTop, Color.Black);
            //spriteBatch.Draw(TextureManager.sand1, boundsBot, Color.Black);
            //spriteBatch.Draw(TextureManager.sand1, boundsLeft, Color.Black);
            //spriteBatch.Draw(TextureManager.sand1, boundsRight, Color.Black);
        }

        public void DrawLevelEffect(SpriteBatch sb)
        {
            foreach (ParticleEngine PE in particleEngine)
            {
                PE.Draw(sb);
            }

        }
    }
}

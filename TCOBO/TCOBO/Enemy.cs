using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        float attackProgress = 0f;
        public float playerSize = 36, basePlayerSize = 36;
        public Rectangle boundsTop, boundsBot, boundsLeft, boundsRight;
        Texture2D strikeTexSword1, strikeTexPlayer1, strikeTexSword2, strikeTexPlayer2, deathTex;
        public int
        Str = 1, Dex = 10,
        Vit = 10, Int = 10, health, expDrop;
        bool dead = false;
        Random rnd;

        Vector2 aimRec;

        SoundManager soundManager = new SoundManager();

        public Enemy(Vector2 pos, ContentManager content, int Str, int Dex, int Vit, int Int, int expDrop)
        {
            this.Str = Str;
            this.Dex = Dex;
            this.Vit = Vit;
            this.Int = Int;
            this.expDrop = expDrop;
            attack_seconds = 1.5f;
            attack_timer = TimeSpan.FromSeconds(attack_seconds);
            rnd = new Random();
            health = Vit*2;
            this.content = content;
            this.pos = pos;
            hitBox = new Rectangle((int)pos.X-15, (int)pos.Y-15, 30, 30);
            Fx = SpriteEffects.None;
            color = new Color(30, 30, 235);
            for (int i = 1; i < 22; i++)
            {
                tex.Add(content.Load<Texture2D>("player" + i));
            }
            strikeTexSword1 = content.Load<Texture2D>("faststrikeSword1");
            strikeTexPlayer1 = content.Load<Texture2D>("faststrikePlayer1");
            strikeTexSword2 = content.Load<Texture2D>("faststrikeSword2");
            strikeTexPlayer2 = content.Load<Texture2D>("faststrikePlayer2");
            deathTex = content.Load<Texture2D>("Death");

            soundManager.LoadContent(content);
        }
        public void HuntPlayer(Player player, GameTime gameTime)
        {
            float distance = Vector2.Distance(player.playerPos, pos);
            if (distance < 400 + (Vit *10))
            {
                move = true;
                if (player.playerPos.X > pos.X) {
                    moveRight = true;
                    moveLeft = false;
                }
                else if (player.playerPos.X < pos.X) {
                    moveLeft = true;
                    moveRight = false;
                }

                if (player.playerPos.Y > pos.Y)
                {
                    moveDown = true;
                    moveUp = false;
                }
                else if (player.playerPos.Y < pos.Y)
                {
                    moveUp = true;
                    moveDown = false;
                }


                float xDistance = (float)player.playerPos.X - pos.X;
                float yDistance = (float)player.playerPos.Y - pos.Y;

                aimRec = new Vector2(xDistance, yDistance);
                aimRec.Normalize();
                double recX = (double)aimRec.X * 40 * size;
                double recY = (double)aimRec.Y * 40 * size;
                attackHitBox = new Rectangle((int)(pos.X + recX - 25 * size), (int)(pos.Y + recY - 25 * size), (int)(50 * size), (int)(50 * size));



                    if (player.hitBox.Intersects(attackHitBox))
                    {
                        moveDown = false;
                        moveUp = false;
                        moveLeft = false;
                        moveRight = false;
                        move = false;

                        //soundManager.hitSound.Play();

                        if (attack_timer.TotalSeconds > 0)
                            attack_timer = attack_timer.Subtract(gameTime.ElapsedGameTime);
                        else
                        {
                            if (!strike && !strike2)
                            {
                                strike = true;
                                player.HP -= Str;
                            }
                            else if (strike)
                            {
                                player.HP -= Str;
                                strike2 = true;
                                attack_seconds = rnd.Next(1, 3);
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
            if (health > 0)
            {

                float tempVit = Vit;
                size = tempVit / 10;
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

        }

        public void Rotation(Vector2 playerPos)
        {
            float xDistance = (float)playerPos.X - pos.X;
            float yDistance = (float)playerPos.Y - pos.Y;
            rotation = (float)Math.Atan2(yDistance, xDistance);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (health > 0)
            {
                if (strike)
                {
                    spriteBatch.Draw(strikeTexSword1, pos, null, Color.Azure, rotation, origin, size, SpriteEffects.None, 0f);
                    spriteBatch.Draw(strikeTexPlayer1, pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
                else if (strike2)
                {
                    spriteBatch.Draw(strikeTexSword2, pos, null, Color.Azure, rotation, origin, size, SpriteEffects.None, 0f);
                    spriteBatch.Draw(strikeTexPlayer2, pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
                else
                {
                    spriteBatch.Draw(tex[animaCount], pos, null, color, rotation, origin, size, SpriteEffects.None, 0f);
                }
            }
            else
            {
                spriteBatch.Draw(deathTex, pos, null, Color.White, rotation, origin, size, SpriteEffects.None, 0f);
            }

            spriteBatch.Draw(TextureManager.sand1, hitBox, Color.Black);
            spriteBatch.Draw(TextureManager.bricktile1, attackHitBox, Color.Black);
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
            boundsTop = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 10), (int)(pos.Y - playerSize / 2), (int)(playerSize - (playerSize / 5)), (int)(playerSize / 10));
            boundsBot = new Rectangle((int)(pos.X - playerSize / 2 + playerSize / 10), (int)((pos.Y + playerSize / 2 - playerSize / 10)), (int)(playerSize - (playerSize / 5)), (int)(playerSize / 10));
            boundsLeft = new Rectangle((int)(pos.X - playerSize / 2), (int)(pos.Y - playerSize / 2 + playerSize / 10), (int)(playerSize / 10), (int)(playerSize - playerSize / 5));
            boundsRight = new Rectangle((int)(pos.X + playerSize / 2 - playerSize / 10), (int)(pos.Y - playerSize / 2 + playerSize / 10), (int)(playerSize / 10), (int)(playerSize - playerSize / 5));
            foreach (Tile t in tiles)
            {
                if (t.collisionEnabled)
                {
                    if (t.bounds.Intersects(boundsLeft))
                    {
                        if (velocity.X < 0)
                            velocity.X = (velocity.X * -2) + max_speed / 10;
                        else
                            velocity.X = max_speed / 10;
                        velocity.Y = velocity.Y * 1.1f;
                        break;

                    }
                    if (t.bounds.Intersects(boundsRight))
                    {
                        if (velocity.X < 0)
                            velocity.X = -max_speed / 10;
                        else
                            velocity.X = (velocity.X * -2) - max_speed / 10;
                        velocity.Y = velocity.Y * 1.1f;
                        break;
                    }
                    if (t.bounds.Intersects(boundsBot))
                    {
                        if (velocity.Y < 0) //om påväg uppåt
                            velocity.Y = -max_speed / 10;
                        else
                            velocity.Y = (velocity.Y * -2) - max_speed / 10;
                        velocity.X = velocity.X * 1.1f;
                        break;

                    }
                    if (t.bounds.Intersects(boundsTop))
                    {
                        if (velocity.Y < 0) //om påväg neråt
                            velocity.Y = (velocity.Y * -2) + max_speed / 10;
                        else
                            velocity.Y = max_speed / 10;
                        velocity.X = velocity.X * 1.1f;
                        break;
                    }

                }
            }
        }
    }
}

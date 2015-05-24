using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCOBO
{
    class ParticleEngine
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        public List<Particle> particles;
        private List<Texture2D> textures;
        public bool drawBlood = false;
        public TimeSpan bloodTimer;
        public float bloodTime = 0.4f;
        bool blood = false;
        float Vit;

        public ParticleEngine(List<Texture2D> textures, Vector2 location, int Vit, bool blood)
        {
            this.blood = blood;
            EmitterLocation = location;
            this.textures = textures;
            this.Vit = Vit;
            this.particles = new List<Particle>();
            random = new Random();
        }

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
         //   Texture texture = TextureManager.blood2;
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    (5+random.Next(1, (int)(Vit/3)))* (float)(random.NextDouble() * 2 - 1),
                    (5+random.Next(1, (int)(Vit/3))) * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color;
            if (blood) 
            {
                color = new Color(
                0.1f + (float)random.NextDouble(),
                0,
                0);
            }
            else 
            {
                color = new Color(
                (float)random.NextDouble(),
                (float)random.NextDouble(),
                (float)random.NextDouble());
            }
            float size = (float)random.NextDouble();


            int ttl = 100 + random.Next((int)1500);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Update()
        {
            int total;
            if (blood)
                total = (int)(Vit * 0.8f);
            else
                total = 5;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateNewParticle());
            }
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
            }
        }

        public void KillParticles()
        {
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].TTL--;
                particles[particle].Position += particles[particle].Velocity;
                particles[particle].Velocity *= 0.94f;
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }

    }
}

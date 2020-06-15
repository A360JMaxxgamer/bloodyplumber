using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    public class PuddleOfBlood : IDisposable
    {
        public List<Blood> m_listOfBlood;
        private int m_lifeTime;
        private int m_birthTime;
        private bool m_active;
        private int speed;

        public virtual void Initialize( Animation bloodPixel, int speed)
        {
            Random xRandom = new Random();
            m_listOfBlood = new List<Blood>();
            m_birthTime = 0;
            m_lifeTime = 4000;
            m_active = false;
            Vector2 position = Vector2.Zero;
            this.speed = speed;
            for(int i = 1; i <10; i++)
            {
                Blood tmp = new Blood();

                
                float random = (float)(xRandom.NextDouble() * xRandom.Next(6,8));
                int yrandom = xRandom.Next(-50, 50);
                float alpha = (float)(xRandom.Next(5,10))/10;
                int tmpSpeed = calcSpeed(speed, random);
                Animation tmpBlood = new Animation();


                tmpBlood.Initialize(bloodPixel.getAnimationStrip(), position.X + (speed / (i)), position.Y + (speed / (i)),alpha, bloodPixel.getAlphaReducing(), 8, 8, 1, 10000, false, new Vector2(1, 1));
                tmp.Initialize(position.X + yrandom * (speed / 30), position.Y + yrandom*(speed / 30), tmpSpeed * random, 10, 5, 1, tmpBlood, 1920,20);
                m_listOfBlood.Add(tmp);
            }
        }

        public virtual void Initialize(Vector2 currentposition, GameTime gameTime, int projectilSpeed)
        {
            checkDirection(projectilSpeed);
            Random xRandom = new Random();
            m_birthTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
            foreach(Blood blood in m_listOfBlood)
            {
                float random = (float)(xRandom.NextDouble() * xRandom.Next(6, 8));
                int yrandom = xRandom.Next(-25, 25);
                float alpha = (float)(xRandom.Next(5, 10)) / 10;
                int tmpSpeed = calcSpeed(speed, random);
                blood.Initialize(currentposition.X + yrandom * (speed / UIConstants.bloodFaktor), currentposition.Y + yrandom * (speed / UIConstants.bloodFaktor), tmpSpeed*random, 10, tmpSpeed);
            }
            m_active = true;
        }

        public virtual void Update(GameTime gameTime, Player player)
        {
            if (m_active)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds < m_birthTime + m_lifeTime)
                    foreach (Blood blood in m_listOfBlood)
                    {
                        blood.Update(gameTime, player);
                    }
                else
                {
                    m_active = false;
                    foreach (Blood blood in m_listOfBlood)
                    {
                        blood.deactive();
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (m_active)
            {
                foreach (Blood blood in m_listOfBlood)
                {
                    blood.Draw(spriteBatch);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }



        public int calcSpeed (int speed, float multi)
        {
            int tmp = UIConstants.m_bloodSpeedFaktor*speed;
            tmp = (int)(speed * multi) / 10;
            return tmp;
        }

        public bool getActivity()
        {
            return m_active;
        }

        public void checkDirection(int projectileSpeed)
        {
            if(projectileSpeed>=0 && speed < 0)
            {
                speed *= -1;
            }

             if(projectileSpeed<0 && speed >= 0)
             {
                 speed *= -1;
             }

        }
    }
}

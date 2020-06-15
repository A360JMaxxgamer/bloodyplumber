using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParticleEmitter;

namespace BloodyPlumber
{
    class Boogeyman : IDisposable
    {
        private Texture2D m_texture;
        private int x_speed, y_speed;
        private Vector2 f_position;
        private int y_Start;
        private ParticleSystemSettings m_ermitterSettings;
        private ParticleEmitter.ParticleSystem m_ermitter;

        public void Initialize(Texture2D texture, int x_Start, int y_Start, ScreenManager screenmanager)
        {
            f_position = new Vector2(x_Start,y_Start);
            this.y_Start = y_Start;
            m_texture = texture;
            x_speed = 10;
            y_speed = 2;

            #region Ermitter
            m_ermitterSettings = new ParticleSystemSettings();
            m_ermitterSettings.ParticleTextureFileName = "ParticleStar";
            m_ermitterSettings.IsBurst = false;
            m_ermitterSettings.SetLifeTimes(1.0f, 3.5f);
            m_ermitterSettings.SetScales(0.1f, 0.8f);
            m_ermitterSettings.ParticlesPerSecond = 200.0f;
            m_ermitterSettings.InitialParticleCount = (int)(m_ermitterSettings.ParticlesPerSecond * m_ermitterSettings.MaximumLifeTime);
            m_ermitterSettings.SetDirectionAngles(0, 360);
            m_ermitterSettings.emitterOn = true;
            m_ermitterSettings.color = Color.White;

            m_ermitter = new ParticleEmitter.ParticleSystem(screenmanager.Game, m_ermitterSettings, screenmanager.imageFileSystem.smokeParticle);
            m_ermitter.OriginPosition = f_position;
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            f_position.X += x_speed;
            f_position.Y += y_speed;

            checkYSpeed();
            m_ermitter.Update(gameTime);
            m_ermitter.OriginPosition=f_position+ new Vector2(m_texture.Width/2, m_texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_texture,f_position,Color.White);
            m_ermitter.Draw(spriteBatch);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            m_ermitter.Dispose();

        }

        private void checkYSpeed()
        {
            if(f_position.Y > (y_Start + 50))
            {
                y_speed = -2;
            }
            if (f_position.Y < (y_Start - 50))
            {
                y_speed = 2;
            }
        }
    }
}

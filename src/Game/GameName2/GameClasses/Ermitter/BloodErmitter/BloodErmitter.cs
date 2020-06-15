using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ParticleEmitter;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    public class BloodErmitter
    {
        private ParticleSystem m_ermitter;
        private ParticleSystemSettings m_settings;

        private int m_startedLastBlood;
        private int m_ermitterDuration;
        private ScreenManager screenManager;

        public BloodErmitter(ScreenManager manager)
        {
            screenManager = manager;
            m_settings = new ParticleSystemSettings();
            m_settings.emitterOn = false;
            m_settings.IsBurst = true;
            m_settings.SetLifeTimes(0.2f, 0.5f);
            m_settings.SetScales(0.2f, 1.0f);
            m_settings.ParticlesPerSecond = 250.0f;
            m_settings.SetSpeeds(100, 500);
            m_settings.InitialParticleCount = (int)(m_settings.ParticlesPerSecond * m_settings.MaximumLifeTime)* 3;

            m_ermitter = new ParticleSystem(screenManager.Game,m_settings, screenManager.imageFileSystem.redPixel);
            m_ermitterDuration = 250;
            m_startedLastBlood = 0;
        }

        public void Update(GameTime gameTime)
        {
            m_ermitter.Update(gameTime);
            m_settings.EndBurst = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_ermitter.Draw(spriteBatch);
        }

        private void checkErmitterBurst(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > m_startedLastBlood + m_ermitterDuration)
            {
                m_settings.EndBurst = true;
                m_settings.emitterOn = false;
            }
        }

        public void startBlood(GameTime gameTime, Vector2 position)
        {
            m_settings.emitterOn = true;
            m_settings.IsBurst = true;
            m_settings.EndBurst = false;
            m_ermitter.OriginPosition = position;
            m_startedLastBlood = (int)gameTime.TotalGameTime.TotalMilliseconds;
        }
    }
}

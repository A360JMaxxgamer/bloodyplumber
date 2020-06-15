using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParticleEmitter;

namespace BloodyPlumber
{
    public class Boss : Enemy
    {
        private bool m_isVisible;
        private Lifebar m_lifebar;
        protected BloodErmitter m_bloodErmitter;
        protected ScreenManager screenManager;
        protected bool m_fighting;

        public virtual void Initialize(float f_xStartPosition, float f_yStartPosition, float f_xStartVelocity, float f_yStartVelocity, float speed, short health, Animation startAnimation, int screenWidth, ScreenManager manager, String name)
        {
            base.Initialize(f_xStartPosition, f_yStartPosition, f_xStartVelocity, f_yStartVelocity, speed, health, startAnimation, screenWidth);
            screenManager = manager;
            m_isVisible = false;
            m_Health = 30;
            m_fighting = false;
            m_bloodErmitter = new BloodErmitter(screenManager);
            m_lifebar = new Lifebar(m_Health, screenManager, name);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            
            if(!m_isVisible)
                checkVisible();
            if (m_isVisible)
                checkActivity();
            if(m_isVisible)
                m_Animation.Update(gameTime, f_Position.X, f_Position.Y);
            checkHealth();
            m_lifebar.Update(m_Health);
            m_bloodErmitter.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (m_isVisible && m_fighting)
            {
                m_Animation.Draw(spriteBatch, m_playerAnimationMirror);
                m_lifebar.Draw(spriteBatch);
            }
            if (m_isVisible && !m_fighting)
                m_Animation.Draw(spriteBatch, SpriteEffects.None);
            m_bloodErmitter.Draw(spriteBatch);
        }

        private void checkVisible()
        {
            if (f_Position.X >= 0 || f_Position.X < UIConstants.screenWidth + m_Animation.getFrameWidth())
            {
                m_isVisible = true;
                m_Animation.setAnimationActive(true);
            }
        }

        public bool isFighting()
        {
            return m_fighting;
        }

        public bool isVisible()
        {
            return m_isVisible;
        }

        public virtual void checkHealth()
        {
            if (m_Health == 0)
                m_alive = false;
        }

        public void setVisible(bool b)
        {
            m_isVisible = b;
        }

        public void gotShot(GameTime gameTime)
        {
            m_Health--;
            Vector2 pos = new Vector2();
            if (m_playerAnimationMirror == SpriteEffects.None)
                pos.X = f_Position.X;
            else
                pos.X = f_Position.X + m_Animation.getFrameWidth() / 2;
            pos.Y = f_Position.Y + m_Animation.getFrameHeight() / 2;
            m_bloodErmitter.startBlood(gameTime, pos);
        }
        

    }
}

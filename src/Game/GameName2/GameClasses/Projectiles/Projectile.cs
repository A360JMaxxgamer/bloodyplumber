//wird die unterschiedlichen projektile repräsentieren

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;

namespace BloodyPlumber
{
    public class Projectile : IDisposable
    {
        #region Attributes
        private Animation m_projectileIAnimation;            //Die Animation des Projektils
        private Vector2 f_projectilePosition;                //Die Position des Projektils
        private int m_projectileSpeed;                       //Geschwindigkeit des Schuss
        private SpriteEffects m_animationMirror;             //Spiegelung der Animation falls nach links geschossen wird
        private bool m_active;
        private Vector2 scale;
        #endregion

        public void Initialize(Vector2 scale)
        {
            m_projectileIAnimation = new Animation();

            this.scale = scale;
          

            m_active = false;
        }

        public void Initialize(Texture2D texture,SpriteEffects effect, Vector2 position, int speed)
        {

            m_animationMirror = effect;
            m_projectileIAnimation.Initialize(texture, position.X, position.Y, 1.0f, 0.0f,texture.Width, texture.Height, 1, 100, true, scale);       
            f_projectilePosition = position;
            m_projectileSpeed = speed;
            m_active = true;
        }

        public void Update(GameTime gameTime)
        {
            /*Die Update Methode muss die Position des Projektils aktualisieren. Diese neuen Daten muss es an die Animation weitergeben und das Kollisions-Rechteck muss ebenfalls
             aktualisiert werden. */
            if (m_active)
            {
                f_projectilePosition.X += m_projectileSpeed;
                m_projectileIAnimation.setAnimationActive(true);
                m_projectileIAnimation.Update(gameTime, f_projectilePosition.X, f_projectilePosition.Y);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (m_active)
            {
                m_projectileIAnimation.Draw(spriteBatch, m_animationMirror);
                //spriteBatch.Draw(m_projectileIAnimation.getAnimationStrip(), f_projectilePosition, Color.White);
            }
   
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.SuppressFinalize(m_projectileIAnimation);
        }

        #region helper
        public Animation getProjectileAnimation()
        {
            return m_projectileIAnimation;
        }

         public float getXPosition()
        {
            return f_projectilePosition.X;
        }

        public int getSpeed()
        {
            return m_projectileSpeed; 
        }

        public Vector2 getPosition()
        {
            return f_projectilePosition;
        }

        public void deactivate()
        {
            m_active = false;
        }

        public bool getActivity()
        {
            return m_active;
        }
        #endregion




    }
}

//Repräsentiert die Gegner in dem Spiel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    public class Enemy : Object
    {
        [XmlElement]
        public bool m_alive;           //m_alive = lebt der Gegner 
        [XmlElement]
        public bool m_active;           //m_active= soll er sich bewegen / ist er bald im Bild zusehen
        public Animation m_deathAnimation;       //Animation für den Tod
        [XmlElement]
        public int m_id;                        // Nummer für den selben Gegnertyp
        [XmlElement]
        public bool m_endFlag;


        protected bool m_left, m_right, m_top, m_down;

        public virtual void Initialize(Animation animation)
        {
            base.m_Animation = new Animation();
            m_alive = true;                         //Nur zu testzwecken
            base.m_Animation.Initialize(animation.getAnimationStrip(), base.f_Position.X, base.f_Position.Y, 1.0f, 0.0f, animation.getFrameWidth(), animation.getFrameHeight(),
                                                                  animation.getFrameCount(), animation.getFrameTime(), animation.isLooping(), animation.getScale());
            f_yVelocity = -5;
            m_left = m_right = m_top = m_down = false;

            m_Health = 1;
        }

        public  virtual void Update(GameTime gameTime, bool isFloorMoving, float floorMovingspeed)
        {

            checkActivity();
            updateMovement();
            checkSpriteMirror();

           
                float tmpX = calculateXSpeed(isFloorMoving, floorMovingspeed);
                m_Animation.shiftRectangle((int)tmpX, (int)f_yVelocity, m_playerAnimationMirror);

                f_Position.X += tmpX;
            if(m_active)
            {
                f_Position.Y += (f_yVelocity + m_gravity);


                m_Animation.Update(gameTime, f_Position.X, f_Position.Y);
            }

            if (f_Position.X < UIConstants.m_disposeEnemyPosition)
                Dispose();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_active)
                base.Draw(spriteBatch);
        }


        #region helper
        public void setPosition(Vector2 pos)
        {

            base.f_Position = pos;
        }
      
        public virtual void checkActivity()
        {
            if (base.f_Position.X < UIConstants.m_initializePositionEnemy && !m_active)
            {
                m_active = true;
                base.m_Animation.setAnimationActive(true);
                base.f_xVelocity = -base.f_Speed;
                f_yVelocity = 0;
            }
        }

        public float calculateXSpeed(bool isFloorMoving, float floorMovingspeed)
        {
            if (isFloorMoving)
            {
                return f_xVelocity - floorMovingspeed;
            }
            else
                return f_xVelocity;
        }
        #endregion

        public void setActive(bool b)
        {
            m_active = b;
        }

        public void updateIntersects(bool left, bool right, bool top, bool down)
        {
            m_left = left;
            m_right = right;
            m_top = top;
            m_down = down;
        }

        public  virtual void updateMovement()
        {
            if (m_left)
            {
                f_xVelocity = f_Speed;
            }
            
            if (m_right)
            {
                f_xVelocity = -f_Speed;
            }

            if (m_down)
            {
                f_yVelocity = -5;
            }

            if (!m_down)
            {
                f_xVelocity = f_xVelocity * (-1);             
            }

            if (f_xVelocity >= 0)
            {
                m_playerAnimationMirror = SpriteEffects.FlipHorizontally;
            }
            else
            {
                m_playerAnimationMirror = SpriteEffects.None;
            }

        }

        protected void checkSpriteMirror()
        {
            if (f_xVelocity >= 0)
            {
                m_playerAnimationMirror = SpriteEffects.FlipHorizontally;
            }
            else
            {
                m_playerAnimationMirror = SpriteEffects.None;
            }
        }


        public virtual void gotShot()
        {
            m_Health--;
        }
    }
}

//Repräsentiert die Gegner in dem Spiel

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;



namespace BloodyPlumberLevelEditor
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
        [XmlElement]
        public String m_fileName;          

        public void Initialize(float f_xStartPosition, float f_yStartPosition, float f_xStartVelocity, float f_yStartVelocity, float speed, short health, Animation startAnimation, int screenWidth,int id, bool endFlag)
        {
            base.Initialize(f_xStartPosition, f_yStartPosition, f_xStartVelocity, f_yStartVelocity, speed, health, startAnimation, screenWidth);
            m_active = false;
            m_alive = true;
            m_id = id;
            m_endFlag = endFlag;
            m_fileName = startAnimation.getAnimationStrip().Name;
        }

        public override void Update(GameTime gameTime)
        {
            if (!base.m_playerAnimation.getActiv())
                base.m_playerAnimation.setAnimationActive(true);
            base.m_playerAnimation.Update(gameTime, base.f_Position.X, base.f_Position.Y);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
                base.Draw(spriteBatch);
        }


        #region helper
        public void setPosition(Vector2 pos)
        {
            base.f_Position = pos;
        }

        public void setActive(bool b)
        {
            m_active = b;
            base.getAnimation().setAnimationActive(b);
        }

        public void checkActivity()
        {
            if (base.f_Position.X < 2000 && ! m_active)
            {
                m_active = true;
                base.getAnimation().setAnimationActive(true);
            }
            if (base.f_Position.X >= 2000 && m_active)
            {
                m_active = false;
                base.getAnimation().setAnimationActive(false);
            }
        }
        #endregion


    }
}



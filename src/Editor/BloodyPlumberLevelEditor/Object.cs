using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace BloodyPlumberLevelEditor
{
    public class Object
    {

        #region Attributes
        [XmlElement]
        public Vector2 f_Position;
        [XmlElement]
        public float f_xVelocity, f_yVelocity;           //Werte für die  Geschwindigkeiten in die Richtungen
        [XmlElement]
        public short m_Health;                                  //Wert für die Spielerenergi
        [XmlElement]
        public Animation m_playerAnimation;                           //Die Animation für den Spieler
        [XmlElement]
        public float f_Speed;                                   //Die Geschwindigkeit mit der der Spieler sich bewegt
        [XmlElement]
        public int m_gravity;                                          //Erdanziehungskraft
        [XmlElement]
        public SpriteEffects m_playerAnimationMirror;                  //guckt der Spieler nach rechts muss nicht gespiegelt werde, guckt er nach links muss horizontal gespiegelt werden

        protected int m_screenWidth;                                  //Wird benötigt um die Liste der Projektile sauber zu halten;
        protected bool m_movementCollisionLeftSide;                           //Gibt an ob der Spieler den linken BildschirmRand erreicht hat
        protected bool m_movementCollisionRightSide;                           //Gibt an ob der Spieler den linken BildschirmRand erreicht hat
        protected bool m_movementCollisionTopSide;                           //Gibt an ob der Spieler den linken BildschirmRand erreicht hat
        protected bool m_movementCollisionBottomSide;                           //Gibt an ob der Spieler den linken BildschirmRand erreicht hat

        #endregion

        public virtual void Initialize(float f_xStartPosition, float f_yStartPosition, float f_xStartVelocity, float f_yStartVelocity, float speed, short health, Animation startAnimation, int screenWidth)
        {

            f_Position.X = f_xStartPosition;
            f_Position.Y = f_yStartPosition;
            f_xVelocity = f_xStartVelocity;
            f_yVelocity = f_yStartVelocity;
            f_Speed = speed;

            m_Health = health;
            m_playerAnimation = startAnimation;

            m_gravity = (int)speed;

            m_movementCollisionLeftSide = m_movementCollisionRightSide = m_movementCollisionTopSide = m_movementCollisionBottomSide = false;

            m_playerAnimationMirror = SpriteEffects.None;

            m_screenWidth = screenWidth;
        }

        public virtual void Update(GameTime gameTime)
        {
            f_Position.X += f_xVelocity;
            f_Position.Y += (f_yVelocity + m_gravity);


            m_playerAnimation.Update(gameTime, f_Position.X, f_Position.Y);
        }

        
        public virtual  void Draw(SpriteBatch spriteBatch)
        {
            m_playerAnimation.Draw(spriteBatch, m_playerAnimationMirror);
        }





        public virtual short getHealth()
        {
            return m_Health;
        }

        public virtual Animation getAnimation()
        {
            return m_playerAnimation;
        }

        public virtual void setHealth(short health)
        {
            m_Health = health;
        }

        public void setAnimationMirror(SpriteEffects effect)
        {
            m_playerAnimationMirror = effect;
        }
    }
}

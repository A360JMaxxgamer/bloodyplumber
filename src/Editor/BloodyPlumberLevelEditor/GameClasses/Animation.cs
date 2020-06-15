using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BloodyPlumberLevelEditor
{
    public class Animation
    {
        #region Attributes
        protected Texture2D m_animationSpriteSrip;         //Das Image auf dem die einzelnen Bilder der Animation sind

        protected Vector2 f_animationPosition;             //Position des der Animation

        protected int m_animationElapsedGameTime;          //Die Zeit die das Spiel schon läuft seit dem letzten Frame wechsel, muss im Update aktualisiert werden!!!
        protected int m_animationFrameTime;                //Die Dauer die ein frame angezeigt wird
        protected int m_animationFrameCount;               //Die Anzahl der Frames des SpriteStrips
        protected int m_animationCurrentFrame;             //Aktueller Frame , also der gerade angezeigt wird
        protected int m_animationFrameHeight;              //Die Höhe eines Frames
        protected int m_animationFrameWidth;               //Die Breite eines Frames

        protected bool m_animationIsActive;                //Läuft die Animation gerade;
        protected bool m_animationIsLooping;               //Wiederholt sich die Animation

        protected Rectangle m_animationSourceRectangle;       //Rectangle das später in der Draw Methode benötigt wird

        protected Vector2 f_animationScale;                  //Wert um den die Animation skaliert wird

        protected Rectangle m_animationCollisionRectangle; //Kollisions Rectangle

        protected Rectangle left, right, top, bottom;

        #endregion

        #region Kernschleife-Methodes
        //Die Methode mit dem ein Objekt vom Typ Animation initialisiert wird und dessen Startwerte eingestellt werden
        public virtual void Initialize(Texture2D sprite, float f_xPosition, float f_yPosition, int frameWidth, int frameHeight, int frameCount, int frameTime, bool looping, Vector2 scale)
        {
            m_animationSpriteSrip = sprite;
            f_animationPosition.X = f_xPosition;
            f_animationPosition.Y = f_yPosition;
            m_animationFrameHeight = frameHeight;
            m_animationFrameWidth = frameWidth;
            m_animationFrameCount = frameCount;
            m_animationFrameTime = frameTime;
            m_animationIsLooping = looping;

            m_animationElapsedGameTime = 0;
            m_animationCurrentFrame = 0;

            //Animation wird als inaktiv initialisiert
            m_animationIsActive = false;

            m_animationSourceRectangle = new Rectangle();

            f_animationScale = scale;

            //Kollisions Rechteck berechnen
            top = new Rectangle();
            bottom = new Rectangle();
            left = new Rectangle();
            right = new Rectangle();
            m_animationSourceRectangle = new Rectangle(m_animationCurrentFrame * m_animationFrameWidth, 0, m_animationFrameWidth, m_animationFrameHeight);

            
        }

        //Die Methode die in regelmäßigen Zeitintervallen aufgerufen wird um die Animation zu aktualisieren
        public virtual void  Update(GameTime gameTime, float f_xPosition, float f_yPosition)
        {
            #region Allgemeines Update
            //Die Standortdaten werden aktualisiert
            f_animationPosition.X = f_xPosition;
            f_animationPosition.Y = f_yPosition;
            
            
            //Update wird abgebrochen wenn die Animation inaktiv ist
            if (!m_animationIsActive)
                return;
            if (m_animationIsLooping)
            {
                //Die vergangene Zeit wird aktualisiert
                m_animationElapsedGameTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

                //Sobald die verstrichene Zeit(m_animationeElapsedGameTime) größer ist als die Frame-Zeit(m_animationFrameTime) wird der Frame gewechselt
                if (m_animationElapsedGameTime > m_animationFrameTime)
                {
                    m_animationCurrentFrame++;

                    //Ist der aktuelle Frame(m_animationCurrentFrame) gleich der maximalen Anzahl Frames (m_animationFrameCount), wird der aktuelle Frame auf null gesetzt
                    if (m_animationCurrentFrame == m_animationFrameCount)
                    {
                        m_animationCurrentFrame = 0;
                        //Ist looping(m_animationIsLooping) deaktiviert wird die Animation deaktiviert
                    }

                    //Verstrichene Zeit auf Null zurück setzen
                    m_animationElapsedGameTime = 0;
                }
            }
            #endregion


            //Berechnung des korrekten Frames des Strips
            m_animationSourceRectangle = new Rectangle(m_animationCurrentFrame * m_animationFrameWidth, 0, m_animationFrameWidth, m_animationFrameHeight);

            //Kollisions Rectangle Updaten
           /* top = new Rectangle((int)f_animationPosition.X + (int)(f_animationScale.X * 10), (int)f_animationPosition.Y - 5, m_animationFrameWidth - 20, 5);
            
            bottom = new Rectangle((int)f_animationPosition.X - (int)(f_animationScale.X * 2), (int)f_animationPosition.Y+m_animationFrameHeight, m_animationFrameWidth+(int)(f_animationScale.X*4),5);

            left = new Rectangle((int)f_animationPosition.X - (int)(f_animationScale.X * 5), (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10), 5, m_animationFrameHeight - (int)(f_animationScale.Y * 20));

            right = new Rectangle((int)f_animationPosition.X + m_animationFrameWidth + (int)(f_animationScale.X * 5), (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10), 5, m_animationFrameHeight - (int)(f_animationScale.Y * 20));*/
        }

        //Die Methode die die Animation zeichnet
        public virtual void  Draw(SpriteBatch spriteBatch, SpriteEffects effect)
        {

            //Sofern die Animation aktiv(m_animationIsActive) ist
            if (m_animationIsActive)
            {
                //spriteBatch.Draw(m_animationSpriteSrip, f_animationPosition, m_animationSourceRectangle , Color.White);       Draw Methode ohne Spiegelung und Skalierung
                spriteBatch.Draw(m_animationSpriteSrip, f_animationPosition, m_animationSourceRectangle, Color.White, 0, Vector2.Zero, f_animationScale, effect, 0);
            }
        }
        #endregion

        #region Helper
        public void setAnimationActive(bool active)
        {
            m_animationIsActive = active;
        }

        public void setCurrentFrame(int frame)
        {
            m_animationCurrentFrame = frame;
        }

        public void setAnimationLooping(bool value)
        {
            m_animationIsLooping = value;
        }

        public Texture2D getAnimationStrip()
        {
            return m_animationSpriteSrip;
        }

        public Rectangle getSourceRectangle()
        {
            return m_animationSourceRectangle;
        }

        public int getFrameWidth()
        {
            return m_animationFrameWidth;
        }

        public int getFrameHeight()
        {
            return m_animationFrameHeight;
        }

        public Vector2 getScale()
        {
            return f_animationScale;
        }

        public int getFrameCount()
        {
            return m_animationFrameCount;
        }

        public int getFrameTime()
        {
            return m_animationFrameTime;
        }

        public Vector2 getPosition()
        {
            return f_animationPosition;
        }

        public bool getActiv()
        {
            return m_animationIsActive;
        }
        

        public void setCollision(Rectangle rec)
        {
            m_animationCollisionRectangle = rec;
        }

        public virtual Rectangle getLeft()
        {
            return left;
        }

        public virtual Rectangle getBottom()
        {
            return bottom;
        }

        public virtual Rectangle getTop()
        {
            return top;
        }

        public virtual Rectangle getRight()
        {
            return right;
        }

        public virtual void shiftRectangle(int shiftX, int shiftY)
        {
            top = new Rectangle((int)f_animationPosition.X + (int)(f_animationScale.X * 10) + shiftX, (int)f_animationPosition.Y - 5+ shiftY, m_animationFrameWidth - 20, 5);

            bottom = new Rectangle((int)f_animationPosition.X - (int)(f_animationScale.X * 2) + shiftX, (int)f_animationPosition.Y + m_animationFrameHeight+5 + shiftY, m_animationFrameWidth + (int)(f_animationScale.X * 4), 5);

            left = new Rectangle((int)f_animationPosition.X - (int)(f_animationScale.X * 5) + shiftX, (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10) + shiftY, 5, m_animationFrameHeight - (int)(f_animationScale.Y * 20));

            right = new Rectangle((int)f_animationPosition.X + m_animationFrameWidth + (int)(f_animationScale.X * 5) + shiftX, (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10) + shiftY, 5, m_animationFrameHeight - (int)(f_animationScale.Y * 20));
        }
        #endregion

    }
}

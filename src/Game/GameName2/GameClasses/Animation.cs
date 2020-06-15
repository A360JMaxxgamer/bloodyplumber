//Diese klasse erstellt aus den PNG Bildern eine Animation.
//Indem sie nach jedem Tick das nächste teilbild aus der grafik anzeigt


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BloodyPlumber
{
    public class Animation : IDisposable
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

        private float f_rotation;

        private float f_alphaChannel;

        private float f_reducingAlpha;


        #endregion

        #region Kernschleife-Methodes
        //Die Methode mit dem ein Objekt vom Typ Animation initialisiert wird und dessen Startwerte eingestellt werden
        public virtual void Initialize(Texture2D sprite, float f_xPosition, float f_yPosition, float startAlpha, float reducingAlpha, int frameWidth, int frameHeight, int frameCount, int frameTime, bool looping, Vector2 scale)
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

            f_rotation = 0.0f;

            //Kollisions Rechteck berechnen
            top = new Rectangle();
            bottom = new Rectangle();
            left = new Rectangle();
            right = new Rectangle();

            f_alphaChannel = startAlpha;
            f_reducingAlpha = reducingAlpha;
        }

        //Die Methode die in regelmäßigen Zeitintervallen aufgerufen wird um die Animation zu aktualisieren
        public virtual void  Update(GameTime gameTime, float f_xPosition, float f_yPosition)
        {
            #region Allgemeines Update
            //Die Standortdaten werden aktualisiert
            f_animationPosition.X = f_xPosition;
            f_animationPosition.Y = f_yPosition;

            f_alphaChannel -= f_reducingAlpha;
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

        }

        //Die Methode die die Animation zeichnet
        public virtual void  Draw(SpriteBatch spriteBatch, SpriteEffects effect)
        {

            //Sofern die Animation aktiv(m_animationIsActive) ist
            if (m_animationIsActive)
            {
                spriteBatch.Draw(m_animationSpriteSrip, f_animationPosition, m_animationSourceRectangle, Color.White* f_alphaChannel, f_rotation, Vector2.Zero, f_animationScale, effect, 0);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, SpriteEffects effect, Vector2 move)
        {

            //Sofern die Animation aktiv(m_animationIsActive) ist
            if (m_animationIsActive)
            {
                spriteBatch.Draw(m_animationSpriteSrip, f_animationPosition+move, m_animationSourceRectangle, Color.White * f_alphaChannel, f_rotation, Vector2.Zero, f_animationScale, effect, 0);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual void DrawCopy(SpriteBatch spriteBatch, SpriteEffects effect, Vector2 position)
        {
            //Sofern die Animation aktiv(m_animationIsActive) ist
            if (m_animationIsActive)
            {
                spriteBatch.Draw(m_animationSpriteSrip, position, m_animationSourceRectangle, Color.White * f_alphaChannel, f_rotation, Vector2.Zero, f_animationScale, effect, 0);
            }
        }
        #endregion

        #region Helper
        public Rectangle getRectangle()
        {
            return new Rectangle((int)f_animationPosition.X, (int)f_animationPosition.Y, m_animationFrameWidth, m_animationFrameHeight);
        }


        public void setRotation(float rotation)
        {
            f_rotation = rotation;
        }
        public void setAnimationActive(bool active)
        {
            m_animationIsActive = active;
        }

        public void setCurrentFrame(int frame)
        {
            m_animationCurrentFrame = frame;
        }

        public void setStrip(Texture2D strip)
        {
            m_animationSpriteSrip = strip;
        }

        public void setAnimationLooping(bool value)
        {
            m_animationIsLooping = value;
        }


        #region Kann nach dem verbessern der ShiftMethode entfernt werden
        public void setLeft(int x, int y, int width, int height)
        {
            left.X = x;
            left.Y = y;
            left.Width = width;
            left.Height = height;
        }

        public void setRight(int x, int y, int width, int height)
        {
            right.X = x;
            right.Y = y;
            right.Width = width;
            right.Height = height;
        }

        public void setBottom(int x, int y, int width, int height)
        {
            bottom.X = x;
            bottom.Y = y;
            bottom.Width = width;
            bottom.Height = height;
        }
        #endregion

        public Texture2D getAnimationStrip()
        {
            return m_animationSpriteSrip;
        }

        public int getFrameWidth()
        {
            return m_animationFrameWidth;
        }

        public int getFrameHeight()
        {
            return m_animationFrameHeight;
        }

        public int getCurrentFrame()
        {
            return m_animationCurrentFrame;
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

        public float getAlphaReducing()
        {
            return f_reducingAlpha;
        }

        public bool isLooping()
        {
            return m_animationIsLooping;
        }

        

        public void setCollision(Rectangle rec)
        {
            m_animationCollisionRectangle = rec;
        }

        public virtual Rectangle getLeft()
        {
            return left;
        }

        public void setAlphaReducing(float reducing)
        {
            f_reducingAlpha = reducing;
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

        public void resetAlpha()
        {
            f_alphaChannel = 1;
        }

        public virtual void shiftRectangle(int shiftX, int shiftY, SpriteEffects effect)
        {
            
            if (effect == SpriteEffects.None)
            {
                top.X= (int)f_animationPosition.X + (int)(f_animationScale.X * 10) + shiftX;
                top.Y= (int)f_animationPosition.Y - 5 + shiftY;
                top.Width =  m_animationFrameWidth - 20;
                top.Height = 5;

                bottom.X =(int)f_animationPosition.X + (int)(f_animationScale.X * 5) + shiftX + 25;
                bottom.Y =(int)f_animationPosition.Y + m_animationFrameHeight + 5 + shiftY;
                bottom.Width = m_animationFrameWidth - 50;
                bottom.Height =5;

                left.X =(int)f_animationPosition.X - (int)(f_animationScale.X * 10) + shiftX;
                left.Y = (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10) + shiftY;
                left.Width = 5;
                left.Height = m_animationFrameHeight - (int)(f_animationScale.Y * 20);

                right.X = (int)f_animationPosition.X + m_animationFrameWidth + (int)(f_animationScale.X * 5) + shiftX;
                right.Y = (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10) + shiftY;
                right.Width = 5;
                right.Height = m_animationFrameHeight - (int)(f_animationScale.Y * 20);
            }
            else
            {
                top.X =(int)f_animationPosition.X + (int)(f_animationScale.X * 10) + shiftX;
                top.Y = (int)f_animationPosition.Y - 5 + shiftY;
                top.Width = m_animationFrameWidth - 20;
                top.Height = 5;

                bottom.X = (int)f_animationPosition.X + (int)(f_animationScale.X * 5) + shiftX + 25;
                bottom.Y = (int)f_animationPosition.Y + m_animationFrameHeight + 5 + shiftY;
                bottom.Width =m_animationFrameWidth-50;
                bottom.Height = 5;

                left.X = (int)f_animationPosition.X - (int)(f_animationScale.X * 10) + shiftX+10;
                left.Y =  (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10) + shiftY;
                left.Width = 5;
                left.Height = m_animationFrameHeight - (int)(f_animationScale.Y * 20);

                right.X = (int)f_animationPosition.X + m_animationFrameWidth + (int)(f_animationScale.X * 5) + shiftX;
                right.Y = (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 10) + shiftY;
                right.Width = 5;
                right.Height = m_animationFrameHeight - (int)(f_animationScale.Y * 20);
            }


          

        }
        #endregion

    }
}

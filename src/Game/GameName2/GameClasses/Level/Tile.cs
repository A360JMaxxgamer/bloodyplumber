using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    public class Tile : IDisposable
    {
        private Texture2D m_tileSourceImage;            // Das Bild aus dem die Tiles geladen werden
        
       
        private Vector2 f_tileScale;                   // Vector der das Tile skaliert
        private int m_tileSpeed;                         //Geschwindigkeit zum bewegen des Hintergrunds
        private Rectangle m_tileCollisionRectangle;      //Rectangle zu der Kollisionsabfrage
        
        [XmlElement]
        public Rectangle sourceRectangle;              // Das Rectangle das den Bildausschnitt bestimmt (Tile)
        private Vector2 f_tilePosition;                  // Position des Tiles
        [XmlElement]
        public int m_tileWidth;                         //Breite des Tiles
        [XmlElement]
        public int m_tileHeight;                       //Höhe des Tiles
        [XmlElement]
        public int m_tileXStart;                       //X-Start auf dem Sourceimage
        [XmlElement]
        public int m_tileYStart;                       //Y-Start auf dem Sourceimage
        [XmlElement]
        public int m_tileNumber;
        private int m_gridWith;
        [XmlElement]
        public int m_xData;                            //Der X Wert in welcher Box sich das Tilebefindet
        [XmlElement]
        public int m_yData;                            //Der X Wert in welcher Box sich das Tilebefindet
        [XmlElement]
        public bool m_destroyable;
        [XmlElement]
        public bool m_deadly;
        [XmlElement]
        public bool m_powerup;        
        [XmlElement]
        public bool m_noRectangle;                      //sagt ob durch das Tile gelaufen werden kann
        
        private bool m_isActive;                        //Gibt an ob sich das Tile im Bildschirm befindet und gezeichnet werden muss
       

       
        



        public void Initialize(Texture2D texture,  Vector2 scale, int gridXWidth, int gridYWidth, int tileSpeed)
        {
            m_tileSourceImage = texture;
            f_tilePosition = new Vector2(m_xData * (gridXWidth), m_yData * (gridYWidth));
            m_gridWith = gridXWidth;
            m_tileSpeed = tileSpeed;
            f_tileScale = scale;
            m_tileCollisionRectangle = new Rectangle((int)f_tilePosition.X, (int)f_tilePosition.Y, m_gridWith, m_gridWith);
            m_isActive = false;
           

        }

        //Die Position und das Kollisions-Rechteck werden geupdatet
        public void Update(GameTime gameTime)
        {

            f_tilePosition.X += m_tileSpeed;
            m_tileCollisionRectangle.X = (int)f_tilePosition.X ;
            m_tileCollisionRectangle.Y =(int)f_tilePosition.Y;
            m_tileCollisionRectangle.Width = m_gridWith;
            m_tileCollisionRectangle.Height = m_gridWith;


            if (f_tilePosition.X < UIConstants.activeTilePosition)
                m_isActive = true;
            
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (m_isActive)
                spriteBatch.Draw(m_tileSourceImage, f_tilePosition, sourceRectangle, Color.White, 0, Vector2.Zero, f_tileScale, SpriteEffects.None, 0);
            
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            
        }

        #region Helper
       
        public Rectangle getCollisionRectangle()
        {
            return m_tileCollisionRectangle;
        }

        public void setSpeed(int speed )
        {
            m_tileSpeed = speed;
        }

        public Vector2 getPosition()
        {
            return f_tilePosition;
        }

        public bool getActive()
        {
            return m_isActive;
        }
        public Texture2D getSourceImage()
        {
            return m_tileSourceImage;
        }

        public bool getNoRectangle()
        {
            return m_noRectangle;
        }


        #endregion
    }
}

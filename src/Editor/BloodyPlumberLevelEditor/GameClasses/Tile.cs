using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace BloodyPlumberLevelEditor
{
    public class Tile
    {
        private Texture2D m_tileSourceImage;            // Das Bild aus dem die Tiles geladen werden
        [XmlElement]
        public Rectangle sourceRectangle;              // Das Rectangle das den Bildaisschnitt bestimmt (Tile)
        private Vector2 f_tilePosition;                  // Position des Tiles
        private Vector2 f_tileScale;                   // Vector der das Tile skaliert
        private int m_tileSpeed;                         //Geschwindigkeit zum bewegen des Hintergrunds
        private Rectangle m_tileCollisionRectangle;      //Rectangle zu der Kollisionsabfrage
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
        [XmlElement]
        public bool m_destroyable;
        [XmlElement]
        public bool m_deadly;
        [XmlElement]
        public bool m_noRectangle;
        [XmlElement]
        public bool m_powerup;
        [XmlElement]
        public int m_xData;                            //Der X Wert in welcher Box sich das Tilebefindet
        [XmlElement]
        public int m_yData;                            //Der X Wert in welcher Box sich das Tilebefindet
        [XmlElement]
        public String m_fileName;  
        public bool[] m_infos;

        public void Initialize(Texture2D texture, int xStartingPoint, int yStartingPoint, int width, int height, float f_xPosition, float f_yPosition, Vector2 scale, int tileNumber, bool[] infos)
        {
            m_infos = new bool[infos.Count()];
            for (int i = 0; i < infos.Count(); i++)
                m_infos[i] = infos[i];

            m_tileSourceImage = texture;
            m_tileXStart = xStartingPoint;
            m_tileYStart = yStartingPoint;
            //Das Rechteck welches die Position des Teils auf dem Image angibt
            sourceRectangle = new Rectangle(m_tileXStart, m_tileYStart, width, height);
            m_tileWidth = width;
            m_tileHeight = height;
            f_tilePosition.X = f_xPosition;
            f_tilePosition.Y = f_yPosition;
            f_tileScale = scale;
            m_tileNumber = tileNumber;
            m_xData = (int)f_tilePosition.X / m_tileWidth;
            m_yData = (int)f_tilePosition.Y / m_tileHeight;

            //Kollisions-Rechteck wird erzeugt und konfiguriert
            m_tileCollisionRectangle = new Rectangle((int)f_tilePosition.X, (int)f_tilePosition.Y, m_tileWidth, m_tileHeight);
            m_tileSpeed = 0;
            m_destroyable = infos[0];
            m_deadly = infos[1];
            m_powerup = infos[2];
            m_noRectangle = infos[3];
            m_fileName = texture.Name;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_tileSourceImage, f_tilePosition, sourceRectangle , Color.White, 0, Vector2.Zero, f_tileScale, SpriteEffects.None,0);
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

        public void setPosition(Vector2 position)
        {
            f_tilePosition = position;
            m_xData = (int)Math.Round(f_tilePosition.X / (m_tileWidth*f_tileScale.X));
            m_yData = (int)Math.Round(f_tilePosition.Y / (m_tileHeight*f_tileScale.Y));
        }

        public Texture2D getImage()
        {
            return m_tileSourceImage;
        }

        public int getXStart()
        {
            return m_tileXStart;
        }

        public int getYStart()
        {
            return m_tileYStart;
        }

        public int getTileWidth()
        {
            return m_tileWidth;
        }

        public int getTileHeight()
        {
            return m_tileHeight;
        }

        public Vector2 getScale()
        {
            return f_tileScale;
        }

        public Vector2 getPosition()
        {
            return f_tilePosition;
        }
        #endregion

        public override string ToString()
        {
            return ("X Koordinate: "+ f_tilePosition.X);
        }

        public int getNumber()
        {
            return m_tileNumber;
        }
    }
}
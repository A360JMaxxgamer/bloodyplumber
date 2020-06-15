using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BloodyPlumberLevelEditor
{
    class User
    {
        private Texture2D m_tileImageSource;        //Das Bild aus dem die Tile Bilder bezogen werden
        private Level m_level;                    //Screen auf den das Raster liegt
        private List<Tile> m_availableTiles;        //Tiles die zum bauen des Levels zur Verfügung stehen
        private int m_currentTile;                  //Welches Teil wird gerade verwendet
        private int m_currentEnemy;
        private Vector2 f_tileScale;                //Die Skalierung für die Tiles
        private int m_tilesPerRow;                  //Wie viele Tiles sind in jeder Reihe des Bildes
        private int m_rows;
        private int m_tileWidth;
        private int m_tileHeight;

        private List<Enemy> m_availableEnemys;

        public void Initialize(Texture2D source, Level screen, int tileWidth, int tileHeight, Vector2 scale, List<Enemy> enemyList)
        {
            m_tileImageSource = source;
            m_level = screen;
            m_tileWidth = tileWidth;
            m_tileHeight = tileHeight;

            m_tilesPerRow = m_tileImageSource.Width / tileWidth;
            m_rows = m_tileImageSource.Height / tileHeight;
            m_currentTile = 0;
            m_availableTiles = new List<Tile>();

            bool[] infos = new bool[4];
            for (int i = 0; i < infos.Count(); i++)
                infos[i] = false;

            f_tileScale = scale;
            int tileNumber = 0;
            
            for(int i = 0; i < m_rows; i++)
            {
                if (i == 2)
                    infos[0] = true;
                else
                    infos[0] = false;
                if( i == 3)
                    infos[1] = true;
                else
                    infos[1] = false;
                if (i == 4)
                    infos[2] = true;
                else
                    infos[2] = false;
                if (i > 4)
                    infos[3] = true;
                else
                    infos[3] = false;
                for(int j = 0 ; j < m_tilesPerRow; j++)
                {
                    Tile tmp = new Tile();
                    tmp.Initialize(m_tileImageSource,(j*m_tileWidth),(i*m_tileHeight),m_tileWidth,m_tileHeight, 1800f,100f, f_tileScale, tileNumber, infos);
                    m_availableTiles.Add(tmp);
                    tileNumber++;
                }
            }
            m_availableEnemys = enemyList;
            m_currentEnemy = 0;
            m_availableEnemys.ElementAt(m_currentEnemy).setActive(true);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_availableTiles.ElementAt(m_currentTile).Draw(spriteBatch);
            m_availableEnemys.ElementAt(m_currentEnemy).Draw(spriteBatch);
        }

        public void setCurrentTile(int tileNumber)
        {
            m_currentTile = tileNumber;
        }

        public Tile getCurrentTile()
        {
            return m_availableTiles.ElementAt(m_currentTile);
        }

        public Texture2D getSourceImage()
        {
            return m_tileImageSource;
        }

        public Vector2 getTileSize()
        {
            return new Vector2(m_tileWidth, m_tileHeight);
        }

        public int getTilesPerRow()
        {
            return m_tilesPerRow;
        }

        public int getRows()
        {
            return m_rows;
        }

        public Level getLevel()
        {
            return m_level;
        }

        public Enemy getCurrentEnemy()
        {
            return m_availableEnemys.ElementAt(m_currentEnemy);
        }

        public void nextEnemy()
        {
            if (m_availableEnemys.Count - 1 > m_currentEnemy)
            {
                m_availableEnemys.ElementAt(m_currentEnemy).setActive(false);
                m_currentEnemy++;
                m_availableEnemys.ElementAt(m_currentEnemy).setActive(true);
            }
        }

        public void previousEnemy()
        {
            if (0 < m_currentEnemy)
            {
                m_availableEnemys.ElementAt(m_currentEnemy).setActive(false);
                m_currentEnemy--;
                m_availableEnemys.ElementAt(m_currentEnemy).setActive(true);
            }
        }

        public void nextTile()
        {
            if (m_availableTiles.Count-1 > m_currentTile)
                m_currentTile++;
        }

        public void previousTile()
        {
            if (0< m_currentTile)
                m_currentTile--;
        }

    }
}

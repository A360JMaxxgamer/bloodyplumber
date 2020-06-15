
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Xml.Serialization;
using System.Windows;
using System.Xml;
using System.Runtime.Serialization;


namespace BloodyPlumberLevelEditor 

{
    public class Level 
    {
        [XmlElement]
        public int m_rows;          //Die Zeilen/Kästchen die der Viewport haben soll
        [XmlElement]
        public int m_columns;          //Die Spalten/Kästchen die der Viewport haben soll
        [XmlElement]
        public int m_tilesWidth;       //Breite der Kästchen
        [XmlElement]
        public int m_tilesHeight;      //Höhe der Kästchen;
        
        private bool m_gridActive;          //gibt an ob das Grid gemalt werden soll
        [XmlElement]
        public List<Tile> m_listOfTiles; //Die Liste in der die Tiles gespeichert werden
        [XmlElement]
        public List<Rectangle> m_listOfLeftRectangles;      //Liste der linken Rectangles 
        [XmlElement]
        public List<Enemy> m_listOfEnemies; //Die Liste in der die Enemy gespeichert werden
        [XmlElement]
        public List<Coin> m_listOfCoins; //Die Liste in der die Enemy gespeichert werden
        
        private int m_movements;

        private bool m_endFlagPlaced;

        private Texture2D m_pixel;      //Pixel zum malen der Linien
        private Texture2D m_coins;
        private Vector2 m_screenSolution;
        private Texture2D m_minimap;
        private bool m_minimapActive;

        private  int[,] arrayBlockedBoxes;

        public void Initialize(Texture2D[] textures,int rows, int columns, Vector2 screenSolution)
        {
            m_pixel = textures[0];
            m_coins = textures[1];
            m_rows = rows;
            m_columns = columns;
            m_tilesWidth = (int)(screenSolution.X / rows);
            m_tilesHeight = m_tilesWidth;
            m_screenSolution = screenSolution;
        
            m_listOfTiles = new List<Tile>();
            m_gridActive = true;
            m_movements = 0;

            m_listOfEnemies = new List<Enemy>();

            m_listOfCoins = new List<Coin>();
            
            m_listOfLeftRectangles = new List<Rectangle>();

            arrayBlockedBoxes = new int [10000 , 50];
            m_endFlagPlaced = false;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in m_listOfEnemies)
                enemy.Update(gameTime);
            foreach (Coin coin in m_listOfCoins)
                coin.Update(gameTime);
            m_listOfTiles.Sort(ComparisonByXPosition);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            if(m_gridActive)
                DrawGrid(spriteBatch);

            for (int i = 0; i < m_listOfTiles.Count; i++)
                m_listOfTiles.ElementAt(i).Draw(spriteBatch);
            for (int j = 0; j < m_listOfEnemies.Count; j++)
                m_listOfEnemies.ElementAt(j).Draw(spriteBatch);
            for (int j = 0; j < m_listOfCoins.Count; j++)
                m_listOfCoins.ElementAt(j).Draw(spriteBatch);
            if(m_minimapActive)
                spriteBatch.Draw(m_minimap,new Vector2(200,200), new Rectangle(0, 0,10000, 1080), Color.White, 0f, Vector2.Zero, 0.15f, SpriteEffects.None, 0);
        }







        #region Helper

        public void setMinimap(RenderTarget2D rt)
        {
            m_minimap = rt;
        }

        public bool getMinimapActivity()
        {
            return m_minimapActive;
        }

        public void setMiniMapActive(bool b)
        {
            m_minimapActive = b;
        }

        //Magic Number
        public int getLevelWidth()
        {
            if(m_listOfTiles.Count >0)
                if (m_listOfTiles.ElementAt(m_listOfTiles.Count - 1).getPosition().X > 1920)
                    return (int)m_listOfTiles.ElementAt(m_listOfTiles.Count - 1).getPosition().X;
                else
                    return 1920;
            else
                return 1920;
        }
        public void DrawGrid(SpriteBatch spriteBatch)
        {
            for(int i = 1; i < m_rows; i++)
            {
                for(int j = 1; j <m_columns ; j++)
                {
                    DrawLine(new Vector2((i * m_tilesWidth), 0), new Vector2((i * m_tilesWidth), m_screenSolution.Y), Color.White, spriteBatch);
                    DrawLine(new Vector2(0, j* m_tilesHeight), new Vector2(m_screenSolution.X,m_tilesHeight*j), Color.White, spriteBatch);
                }
            }
        }

        public void DrawLine(Vector2 p1, Vector2 p2, Color color, SpriteBatch spriteBatch)
        {
            float distance = Vector2.Distance(p1, p2);

            float angle = (float)Math.Atan2((double)(p2.Y - p1.Y), (double)(p2.X - p1.X));

            spriteBatch.Draw(m_pixel, p1, null, color, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 1);
        }

        public void addTile(int xPosition, int yPosition, Tile currentTile)
        {
            int x = xPosition / m_tilesWidth;
            int y = yPosition / m_tilesHeight;
            Vector2 position = new Vector2(x*m_tilesWidth,y*m_tilesHeight);
            x += m_movements;
            if(canBePlaced(x,y,new Vector2(1,1)))
            {
                Tile tmp = new Tile();
                tmp.Initialize(currentTile.getImage(), currentTile.getXStart(), currentTile.getYStart(), currentTile.getTileWidth(), currentTile.getTileHeight(), position.X, position.Y, currentTile.getScale(),currentTile.getNumber(), currentTile.m_infos);
                tmp.setPosition(position);
                m_listOfTiles.Add(tmp);
                arrayBlockedBoxes[x, y ] = 1;
            }       
        }

        public void addEnemy(int xPosition, int yPosition, Enemy currentEnemy, bool endFlag)
        {
            int x = xPosition / m_tilesWidth;
            int y = yPosition / m_tilesHeight;
            Vector2 position = new Vector2(x*m_tilesWidth,y*m_tilesHeight);
            Vector2 needTile = needTiles(currentEnemy);
            x += m_movements;
            if(canBePlaced(x,y, needTile))
            {
                if (!endFlag || endFlag && !m_endFlagPlaced)
                {
                    Enemy tmp = new Enemy();
                    Animation tmpAnimation = new Animation();
                    tmpAnimation.Initialize(currentEnemy.getAnimation().getAnimationStrip(), position.X, position.Y, currentEnemy.getAnimation().getFrameWidth(), currentEnemy.getAnimation().getFrameHeight(), currentEnemy.getAnimation().getFrameCount(), currentEnemy.getAnimation().getFrameTime(), true, new Vector2(1, 1));
                    tmp.Initialize(position.X, position.Y, currentEnemy.f_xVelocity, currentEnemy.f_xVelocity, currentEnemy.f_Speed, currentEnemy.m_Health, tmpAnimation, 1920, currentEnemy.m_id, endFlag);
                    m_listOfEnemies.Add(tmp);

                    for (int i = 0; i < needTile.X; i++)
                        for (int j = 0; j < needTile.Y; j++)
                            arrayBlockedBoxes[x + i, y + j] = 2;
                    if (endFlag)
                        m_endFlagPlaced = true;
                }
                else
                {
                    Debug.WriteLine("Enemy kann nicht gesetzt werden (ENDFLAG!)");
                }
            }
            else
            {
                Debug.WriteLine("Enemy kann nicht gesetzt werden");
            }
           
        }

        public void addCoin(int xPosition, int yPosition)
        {
            int x = xPosition / m_tilesWidth;
            int y = yPosition / m_tilesHeight;
            Vector2 position = new Vector2(x * m_tilesWidth, y * m_tilesHeight);
            x += m_movements;
            if (canBePlaced(x, y, new Vector2(1, 1)))
            {
                Coin tmpCoin = new Coin();
                tmpCoin.Initialize(position, m_coins, 14, 50,new Vector2(1,1));
                m_listOfCoins.Add(tmpCoin);
                arrayBlockedBoxes[x, y] = 3;
            }
        }

        public void delete(int xPosition, int yPosition)
        {
            int x = xPosition / m_tilesWidth;
            int y = yPosition / m_tilesHeight;

            if(arrayBlockedBoxes[x+m_movements,y] == 1)
            {
                for( int i = 0 ; i < m_listOfTiles.Count(); i++)
                    if(x*m_tilesWidth == m_listOfTiles.ElementAt(i).getPosition().X && y *m_tilesWidth == m_listOfTiles.ElementAt(i).getPosition().Y)
                    {
                        m_listOfTiles.RemoveAt(i);
                        arrayBlockedBoxes[x + m_movements, y] = 0;
                    }
            }
            if(arrayBlockedBoxes[x+m_movements,y] == 2)
            {
                for (int i = 0; i < m_listOfEnemies.Count(); i++)
                    if (x * m_tilesWidth == m_listOfEnemies.ElementAt(i).f_Position.X && y * m_tilesWidth == m_listOfEnemies.ElementAt(i).f_Position.Y)
                    {
                        
                        Vector2 needTile = needTiles(m_listOfEnemies.ElementAt(i));
                        m_listOfEnemies.RemoveAt(i);
                        for (int k = 0; k< needTile.X; k++)
                            for (int j = 0; j < needTile.Y; j++)
                            {
                                arrayBlockedBoxes[x + k + m_movements, y + j] = 0;
                            }
                        
                    }
            }
            if (arrayBlockedBoxes[x + m_movements, y] == 3)
            {
                for (int i = 0; i < m_listOfCoins.Count(); i++)
                    if (x * m_tilesWidth == m_listOfCoins.ElementAt(i).getPosition().X && y * m_tilesWidth == m_listOfCoins.ElementAt(i).getPosition().Y)
                    {
                        m_listOfCoins.RemoveAt(i);
                        arrayBlockedBoxes[x + m_movements, y] = 0;
                    }
            }
        }

        

        public void changeGridActive()
        {
            if (m_gridActive)
                m_gridActive = false;
            else
                m_gridActive = true;
        }

        public void moveScreenLeft()
        {
            
            for (int i = 0; i < m_listOfTiles.Count; i++)
            {
                m_listOfTiles.ElementAt(i).setPosition(new Vector2(m_listOfTiles.ElementAt(i).getPosition().X - m_tilesWidth, m_listOfTiles.ElementAt(i).getPosition().Y));
            }

            for( int j = 0 ; j< m_listOfEnemies.Count; j++)
            {
                m_listOfEnemies.ElementAt(j).setPosition(new Vector2(m_listOfEnemies.ElementAt(j).getAnimation().getPosition().X - m_tilesWidth, m_listOfEnemies.ElementAt(j).getAnimation().getPosition().Y));
            }
            for (int i = 0; i < m_listOfCoins.Count; i++)
            {
                m_listOfCoins.ElementAt(i).moveLeft();
            }
                m_movements++;
        }

        public void moveScreenRight()
        {
            if (m_movements > 0)
            {
                for (int i = 0; i < m_listOfTiles.Count; i++)
                {
                    m_listOfTiles.ElementAt(i).setPosition(new Vector2(m_listOfTiles.ElementAt(i).getPosition().X + m_tilesWidth, m_listOfTiles.ElementAt(i).getPosition().Y));
                }

                for (int j = 0; j < m_listOfEnemies.Count; j++)
                {
                    m_listOfEnemies.ElementAt(j).setPosition(new Vector2(m_listOfEnemies.ElementAt(j).getAnimation().getPosition().X + m_tilesWidth, m_listOfEnemies.ElementAt(j).getAnimation().getPosition().Y));
                }


                for (int i = 0; i < m_listOfCoins.Count; i++)
                {
                    m_listOfCoins.ElementAt(i).moveRight();
                }
                m_movements--;
            }

        }

        private static int ComparisonByXPosition(Tile a, Tile b)
        {
            if (a.getPosition().X == b.getPosition().X)
                return 0;
            if (a.getPosition().X > b.getPosition().X)
                return 1;
            else 
                return 0;
        }

        public void backToStart()
        {
            while(m_movements>0)
            {
                moveScreenRight();
            }
            m_listOfTiles.Sort(ComparisonByXPosition);
            
        }

        public Vector2 getScreenSolution()
        {
            return m_screenSolution;
        }

        public Vector2 needTiles(Object ob)
        {
            
            int x = (int)Math.Ceiling(ob.getAnimation().getFrameWidth()/(double)m_tilesWidth);
            int y =  (int)Math.Ceiling(ob.getAnimation().getFrameHeight()/(double)m_tilesHeight);
            return new Vector2(x, y);
        }

        public bool canBePlaced(int x, int y, Vector2 needTiles)
        {
            bool returnBool = true;
            for (int i = 0; i < needTiles.X; i++)
                for (int j = 0; j < needTiles.Y; j++)
                    if (arrayBlockedBoxes[x + i, y + j] != 0)
                        returnBool = false;
            return returnBool;      
        }

        #endregion
    }
 
}

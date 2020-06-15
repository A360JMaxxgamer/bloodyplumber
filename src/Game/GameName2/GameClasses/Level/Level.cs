//Steht für ein Level im Spiel und liest die xml files aus
//die mit dem Leveleditor erstellt wurden

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;

namespace BloodyPlumber
{
    public class Level : IDisposable
    {
        [XmlElement]
        public int m_rows;          //Die Zeilen/Kästchen die der Viewport haben soll
        [XmlElement]
        public int m_columns;          //Die Spalten/Kästchen die der Viewport haben soll
        [XmlElement]
        public int m_tilesWidth;       //Breite der Kästchen
        [XmlElement]
        public int m_tilesHeight;      //Höhe der Kästchen;
        [XmlElement]
        public List<Tile> m_listOfTiles; //Die Liste in der die Tiles gespeichert werden
        [XmlElement]
        public List<Enemy> m_listOfEnemies; //Die Liste in der die Enemy gespeichert werden
        [XmlElement]
        public List<Coin> m_listOfCoins; //Die Liste in der die Enemy gespeichert werden

        public List<PuddleOfBlood> m_puddleOfBloodList;

        public List<PuddleofMud> m_puddleOfMudList;

        private List<Chest> m_listOfChest;

        private List<IPowerUps> m_possiblePowerUps;
        
        private int m_currentPuddleOfBlood, m_currentMud;

        private ScreenManager screenManager;

        public void Initialize(Texture2D[] textures, Vector2 scale,int tileSpeed,Player player, Animation bloodAnimation, Animation mudAnimation, List<IPowerUps> powerUps, ScreenManager manager)
        {
            screenManager = manager;
            m_puddleOfBloodList = new List<PuddleOfBlood>();
            m_listOfChest = new List<Chest>();
            m_possiblePowerUps = powerUps;
            foreach (Tile tile in m_listOfTiles)
            {
                tile.Initialize(textures[0], scale, m_tilesWidth, m_tilesHeight, tileSpeed);
                if (tile.m_powerup)
                    m_listOfChest.Add(new Chest(getItem(), tile.m_xData, tile.m_yData));
            }

            foreach (Coin coin in m_listOfCoins)
                coin.Initialize(textures[1], 14, 50, scale, player, 100);

           

            initializeBlood(bloodAnimation);
            initializeMud(mudAnimation);
            m_currentPuddleOfBlood = 0;
            m_currentMud = 0;
        }

        public void Update(GameTime gameTime, Player player)
        {
            checkIfFloorIsMoving(player);


            foreach (PuddleOfBlood puddle in m_puddleOfBloodList)
                puddle.Update(gameTime, player);
            foreach (PuddleofMud mud in m_puddleOfMudList)
                mud.Update(gameTime, player);
            updateTileList(gameTime);
            foreach (Enemy enemy in m_listOfEnemies)
            {
                if (enemy.m_endFlag)
                {
                    Boss b = (Boss) enemy;
                    if (b.isVisible() && b.isFighting())
                        enemy.Update(gameTime);
                    else
                        enemy.Update(gameTime, player.getMoveFloor(), player.f_Speed);
                }
                else

                    enemy.Update(gameTime, player.getMoveFloor(), player.f_Speed);
            }
            for(int i = 0; i < m_listOfCoins.Count(); i++)
            {
                m_listOfCoins.ElementAt(i).Update(gameTime);
                if(m_listOfCoins.ElementAt(i).canBeDeleted(gameTime))
                {
                    m_listOfCoins.ElementAt(i).Dispose();
                    m_listOfCoins.RemoveAt(i);
                    i++;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in m_listOfTiles)
                tile.Draw(spriteBatch);
            foreach (Enemy enemy in m_listOfEnemies)
                enemy.Draw(spriteBatch);
            foreach (PuddleOfBlood puddle in m_puddleOfBloodList)
                puddle.Draw(spriteBatch);
            foreach (PuddleofMud mud in m_puddleOfMudList)
                mud.Draw(spriteBatch);
            foreach (Coin coin in m_listOfCoins)
                coin.Draw(spriteBatch);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void checkIfFloorIsMoving(Player player)
        {

            if (player.getXPlayerPosition() + player.getXPlayerVelocity() <= 0)
                player.setXPosition(0f); 

            if(player.getMoveFloor())
            {
                foreach (Tile tile in m_listOfTiles)
                    tile.setSpeed(-(int)player.getPlayerSpeed());
            }
            else
                foreach (Tile tile in m_listOfTiles)
                    tile.setSpeed(0);
        }

        //Müsste für verschiedene Bildschirmauflösungen angepasst werden
        public void CheckPlayerPosition(Player player)
        {
            if (player.getXPlayerPosition() + player.getXPlayerVelocity() <= 0)
                player.setXPosition(0f);

            if (player.getXPlayerPosition() + player.getXPlayerVelocity() >= UIConstants.playerMaxXPosition)                   // ScreenWidth muss als Variabel noch eingefügt werden
            {
                foreach (Tile tile in m_listOfTiles)
                    tile.setSpeed(-(int)player.getPlayerSpeed());
                player.setMoveFloor(true);
            }

            if (player.getXPlayerPosition() + player.getXPlayerVelocity() < UIConstants.playerMaxXPosition)                    // ScreenWidth muss als Variabel noch eingefügt werden
            {
                foreach (Tile tile in m_listOfTiles)
                    tile.setSpeed(0);
                player.setMoveFloor(false);
            }
        }

        public void createBlood(Vector2 position, GameTime gameTime, int projectileSpeed)
        {
            m_puddleOfBloodList.ElementAt(m_currentPuddleOfBlood).Initialize(position, gameTime, projectileSpeed);
            m_currentPuddleOfBlood++;
            if (m_currentPuddleOfBlood == m_puddleOfBloodList.Count)
                 m_currentPuddleOfBlood = 0;
        }

        public void createMud(Vector2 position, GameTime gameTime, int projectileSpeed)
        {
            m_puddleOfMudList.ElementAt(m_currentMud).Initialize(position, gameTime, projectileSpeed);
            m_currentMud++;
            if (m_currentMud == m_puddleOfMudList.Count)
                m_currentMud = 0;
        }

        public void updateTileList(GameTime gameTime)
        {
            List<int> toDelete = new List<int>();               //Liste der Stellen aus m_TileList die gelöscht werden müssen
            for(int i = 0; i < m_listOfTiles.Count; i++)
            {
                m_listOfTiles.ElementAt(i).Update(gameTime);
                if (m_listOfTiles.ElementAt(i).getPosition().X < UIConstants.tileDisposePosition)                
                {
                    
                    m_listOfTiles.ElementAt<Tile>(i).Dispose();
                    toDelete.Add(i);
                }
            }
            for( int j = toDelete.Count-1 ; j >=0 ; j--)
            {
                m_listOfTiles.RemoveAt(toDelete.ElementAt(j));
            }
        }

        public void initializeBlood(Animation bloodpixel)
        {
            for(int i = 0 ; i< 8; i++)
            {
                PuddleOfBlood tmp = new PuddleOfBlood();
                tmp.Initialize(bloodpixel, 20);
                m_puddleOfBloodList.Add(tmp);
            }
        }

        public void initializeMud(Animation mud)
        {
            for (int i = 0; i < 4; i++)
            {
                PuddleofMud tmp = new PuddleofMud();
                tmp.Initialize(mud, 20);
                m_puddleOfMudList.Add(tmp);
            }
        }

        public void collectChest(int x, int y, Player player)
        {
            foreach (Chest c in m_listOfChest)
            {
                if (c.getX() == x && c.getY() == y)
                {
                    c.collected(player);
                    break;
                }
            }
        }

        private IPowerUps getItem()
        {
            Random r = new Random();
            int number = r.Next(0, m_possiblePowerUps.Count - 1);
            IPowerUps p = m_possiblePowerUps.ElementAt(number).clone();
            return p;
        }

    }
}

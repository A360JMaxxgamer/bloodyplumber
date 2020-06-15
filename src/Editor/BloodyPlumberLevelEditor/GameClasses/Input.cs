using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using System.Xml.Serialization;
using System.Windows;
using System.Xml;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Windows.Input;


namespace BloodyPlumberLevelEditor
{
    class Input
    {
        private MouseState m_currentMouseState;       //Aktueller Status der Maus
        private MouseState m_previousMouseState;      //vorheriger Mausstatus

        private User m_user;                          //user
        private static Level m_level;                      //Screen der manipuliert wird
        private TileWindow m_tileWindow;              //Das Fenster zu der Tile auswahl
        private StatusScreen m_statusScreen;

        private Toolbar m_toolbar;
        private bool m_minimapActive, m_previousminimapActive;

        public enum States
        {
            SetTile,
            SetEnemy ,
            SelectEnemy,
            Delete ,
            SelectTile ,
            SetEndFlag,
            SetCoin,
            ViewMiniMap
        };
        States currenState;


        KeyboardState m_currentKeyboardState;
        KeyboardState m_previousKeyboardState;
        public void Initialize(User user, Level screen, StatusScreen statusScreen, Toolbar toolbar)
        {
            m_toolbar = toolbar;
            m_user = user;
            m_level = screen;
            m_currentMouseState = Mouse.GetState();
            m_previousMouseState = m_currentMouseState;
            m_currentKeyboardState = Keyboard.GetState();
            m_previousKeyboardState = m_currentKeyboardState;

            m_tileWindow = new TileWindow();
            m_tileWindow.Initialize(m_user);
            currenState = States.SetTile;

            m_statusScreen = statusScreen;
        }

        public bool Update(GameTime gameTime)
        {
            m_toolbar.Update(gameTime);
            m_previousMouseState = m_currentMouseState;
            m_currentMouseState = Mouse.GetState();

            m_previousKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();

            isToolbarActive();


            if(m_currentMouseState.LeftButton == ButtonState.Pressed  && currenState != States.ViewMiniMap)
            {
                if(mouseClickonToolbar())
                {
                    currenState = (States)m_toolbar.getState(m_currentMouseState.X, m_currentMouseState.Y);
                }
                else
                {
                    if (currenState == States.SelectTile && m_tileWindow.m_collisionRectangle.Intersects(new Rectangle(m_currentMouseState.X, m_currentMouseState.Y, 1, 1)))
                        m_user.setCurrentTile(m_tileWindow.getSelectedTile(new Vector2(m_currentMouseState.X, m_currentMouseState.Y)));
                    else if(currenState == States.SelectTile )
                    {
                        m_level.addTile(m_currentMouseState.X, m_currentMouseState.Y, m_user.getCurrentTile());
                    }
                    if (currenState == States.SetTile)
                    {
                        m_level.addTile(m_currentMouseState.X, m_currentMouseState.Y, m_user.getCurrentTile());
                    }

                    if (currenState == States.SetEnemy)
                    {
                        m_level.addEnemy(m_currentMouseState.X, m_currentMouseState.Y, m_user.getCurrentEnemy(), false);
                    }

                    if (currenState == States.SetEndFlag)
                    {
                        m_level.addEnemy(m_currentMouseState.X, m_currentMouseState.Y, m_user.getCurrentEnemy(), true);
                    }
                    if (currenState == States.SetCoin)
                    {
                        m_level.addCoin(m_currentMouseState.X, m_currentMouseState.Y);
                    }
                    if (currenState == States.Delete)
                    {
                        m_level.delete(m_currentMouseState.X, m_currentMouseState.Y);
                    }
                }
            }


            //Grid an oder ausschalten
            if(m_currentKeyboardState.IsKeyDown(Keys.G) && m_currentKeyboardState.IsKeyDown(Keys.G) != m_previousKeyboardState.IsKeyDown(Keys.G))
            {
                m_level.changeGridActive();
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.N) && m_currentKeyboardState.IsKeyDown(Keys.N) != m_previousKeyboardState.IsKeyDown(Keys.N))
            {
                if(currenState == States.SetTile || currenState == States.SelectTile)
                    m_user.nextTile();
                if (currenState == States.SetEnemy|| currenState == States.SelectEnemy)
                    m_user.nextEnemy();
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.P) && m_currentKeyboardState.IsKeyDown(Keys.P) != m_previousKeyboardState.IsKeyDown(Keys.P))
            {
                if (currenState == States.SetTile || currenState == States.SelectTile)
                    m_user.previousTile();
                if (currenState == States.SetEnemy || currenState == States.SelectEnemy)
                    m_user.previousEnemy();
            }
            //Aktivieren und deaktivieren des Gegner setzten
            if(m_currentKeyboardState.IsKeyDown(Keys.E) && m_currentKeyboardState.IsKeyDown(Keys.E) != m_previousKeyboardState.IsKeyDown(Keys.E))
            {
                currenState = States.SetEnemy;
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.D) && m_currentKeyboardState.IsKeyDown(Keys.D) != m_previousKeyboardState.IsKeyDown(Keys.D))
            {
                currenState = States.Delete;
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.T) && m_currentKeyboardState.IsKeyDown(Keys.T) != m_previousKeyboardState.IsKeyDown(Keys.T))
            {
                currenState = States.SetTile;
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.S) && m_currentKeyboardState.IsKeyDown(Keys.S) != m_previousKeyboardState.IsKeyDown(Keys.S))
            {
                currenState = States.SelectTile;
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.F) && m_currentKeyboardState.IsKeyDown(Keys.F) != m_previousKeyboardState.IsKeyDown(Keys.F))
            {
                currenState = States.SetEndFlag;
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.C) && m_currentKeyboardState.IsKeyDown(Keys.C) != m_previousKeyboardState.IsKeyDown(Keys.C))
            {
                currenState = States.SetCoin;
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.M) && m_currentKeyboardState.IsKeyDown(Keys.M) != m_previousKeyboardState.IsKeyDown(Keys.M))
            {
                if (currenState != States.ViewMiniMap)
                {
                    currenState = States.ViewMiniMap;
                    return true;
                }
                else
                    return false;
            }

            //Den Bildschirm nach links verschieben
            if (m_currentKeyboardState.IsKeyDown(Keys.Left) && currenState != States.ViewMiniMap )
            {
                m_level.moveScreenRight();
            }

            //Bildschirm nach rechts verschieben
            if (m_currentKeyboardState.IsKeyDown(Keys.Right) && currenState != States.ViewMiniMap)
            {
                m_level.moveScreenLeft();
            }

            //Xml schreiben
            if (m_currentKeyboardState.IsKeyDown(Keys.O) && m_currentKeyboardState.IsKeyDown(Keys.O) != m_previousKeyboardState.IsKeyDown(Keys.O))
            {
                //m_screen.writeTxt();
                Save();
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.L) && m_currentKeyboardState.IsKeyDown(Keys.L) != m_previousKeyboardState.IsKeyDown(Keys.L))
            {
                
                Load();
            }

            if (currenState != States.ViewMiniMap)
                m_level.setMiniMapActive(false);

            return false;
        }


        public void Draw(SpriteBatch spritebatch)
        {
            if(currenState == States.SelectTile)
                 m_tileWindow.Draw(spritebatch);
            //m_statusScreen.Draw(spritebatch, currenState.ToString());
            m_toolbar.Draw(spritebatch);
        }



        public async void Save()
        {
            m_level.backToStart();
            // Serialize the session state synchronously to avoid asynchronous access to shared
            // state
            MemoryStream sessionData = new MemoryStream();
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            serializer.Serialize(sessionData, m_level);

            // Get an output stream for the SessionState file and write the state asynchronously

            StorageFile file = await DownloadsFolder.CreateFileAsync("Test.xml",CreationCollisionOption.GenerateUniqueName);
            //StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Hallo.xml", CreationCollisionOption.ReplaceExisting);
            using (Stream fileStream = await file.OpenStreamForWriteAsync())
            {
                sessionData.Seek(0, SeekOrigin.Begin);
                await sessionData.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
        }

        public  void  Load()
        {
            XmlReader reader = XmlReader.Create("Hallo.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            Debug.WriteLine("" + serializer.CanDeserialize(reader));
            Level level = (Level)serializer.Deserialize(reader);
        }

        private void isToolbarActive()
        {
            if (m_currentMouseState.X > m_toolbar.getScreenWidth() - 20)
            {
                if (!m_toolbar.isActive())
                {
                    m_toolbar.activate();
                }
            }
            else 
            if (!m_toolbar.getRectangle().Contains(m_currentMouseState.X, m_currentMouseState.Y) && !m_toolbar.isMoving())
            {
                m_toolbar.deactivate();
            }
        }

        private bool mouseClickonToolbar()
        {
            if( m_toolbar.getRectangle().Contains(m_currentMouseState.X, m_currentMouseState.Y))
                return true;
            else 
                return false;
        }

        public void DrawString(SpriteBatch spriteBatch)
        {
            if (currenState == States.SelectTile)
                ;
        }
    }
}

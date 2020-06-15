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


namespace BloodyPlumberLevelEditor
{
    class Input
    {
        private MouseState m_currentMouseState;       //Aktueller Status der Maus
        private MouseState m_previousMouseState;      //vorheriger Mausstatus

        private User m_user;                          //user
        private static Level m_level;                      //Screen der manipuliert wird
        private TileWindow m_tileWindow;              //Das Fenster zu der Tile auswahl

        private bool m_placeEnemy;


        KeyboardState m_currentKeyboardState;
        KeyboardState m_previousKeyboardState;
        public void Initialize(User user, Level screen)
        {
            
            m_user = user;
            m_level = screen;
            m_currentMouseState = Mouse.GetState();
            m_previousMouseState = m_currentMouseState;
            m_currentKeyboardState = Keyboard.GetState();
            m_previousKeyboardState = m_currentKeyboardState;

            m_tileWindow = new TileWindow();
            m_tileWindow.Initialize(m_user);

            m_placeEnemy = false;
        }

        public void Update(GameTime gameTime)
        {
            m_previousMouseState = m_currentMouseState;
            m_currentMouseState = Mouse.GetState();

            m_previousKeyboardState = m_currentKeyboardState;
            m_currentKeyboardState = Keyboard.GetState();
            


            if(m_currentMouseState.LeftButton == ButtonState.Pressed && m_currentMouseState.LeftButton != m_previousMouseState.LeftButton )
            {
                if (!m_tileWindow.isWindowActiv()&& ! m_placeEnemy)
                {
                    m_level.addTile(m_currentMouseState.X, m_currentMouseState.Y, m_user.getCurrentTile());
                }

                if(m_tileWindow.m_collisionRectangle.Contains(m_currentMouseState.X, m_currentMouseState.Y) && !m_placeEnemy)
                    m_user.setCurrentTile(m_tileWindow.getSelectedTile(new Vector2(m_currentMouseState.X, m_currentMouseState.Y)));

                if(m_placeEnemy)
                {
                    m_level.addEnemy(m_currentMouseState.X, m_currentMouseState.Y, m_user.getCurrentEnemy());
                }
            }


            //Grid an oder ausschalten
            if(m_currentKeyboardState.IsKeyDown(Keys.G) && m_currentKeyboardState.IsKeyDown(Keys.G) != m_previousKeyboardState.IsKeyDown(Keys.G))
            {
                m_level.changeGridActive();
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.N) && m_currentKeyboardState.IsKeyDown(Keys.N) != m_previousKeyboardState.IsKeyDown(Keys.N))
            {
                m_user.nextTile();
            }

            if (m_currentKeyboardState.IsKeyDown(Keys.P) && m_currentKeyboardState.IsKeyDown(Keys.P) != m_previousKeyboardState.IsKeyDown(Keys.P))
            {
                m_user.previousTile();
            }
            //Aktivieren und deaktivieren des Gegner setzten
            if(m_currentKeyboardState.IsKeyDown(Keys.E) && m_currentKeyboardState.IsKeyDown(Keys.E) != m_previousKeyboardState.IsKeyDown(Keys.E))
            {
                if(m_placeEnemy)
                {
                    m_placeEnemy = false;
                }
                else
                {
                    m_placeEnemy = true;
                    m_tileWindow.setActive(false);
                }
            }


            //Nächstes Tile auswählen
            if(m_currentMouseState.RightButton == ButtonState.Pressed && m_currentMouseState.RightButton != m_previousMouseState.RightButton)
            {
                if(m_tileWindow.isWindowActiv())
                     m_tileWindow.setActive(false);
                else
                    m_tileWindow.setActive(true);
            }


            //Den Bildschirm nach links verschieben
            if (m_currentKeyboardState.IsKeyDown(Keys.Left) && m_currentKeyboardState.IsKeyDown(Keys.Left) != m_previousKeyboardState.IsKeyDown(Keys.Left))
            {
                m_level.moveScreenLeft();
            }

            //Bildschirm nach rechts verschieben
            if (m_currentKeyboardState.IsKeyDown(Keys.Right) && m_currentKeyboardState.IsKeyDown(Keys.Right) != m_previousKeyboardState.IsKeyDown(Keys.Right))
            {
                m_level.moveScreenRight();
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
        }


        public void Draw(SpriteBatch spritebatch)
        {
            m_tileWindow.Draw(spritebatch);
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
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Hallo.xml", CreationCollisionOption.ReplaceExisting);
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
    }
}

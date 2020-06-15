using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BloodyPlumberLevelEditor
{
    class TileWindow
    {
        private Texture2D m_imageSource;        //Die Bilder auf dem die Tiles enthalten sind
        private bool m_isWindowActiv;           //Bool Wert ob das Fenster gerade angezeigt wird
        private User m_currentUser;             //verweis auf den User und desen Tiles
        private Vector2 f_position;             //Die Position wo das Auswahlfenster abgebildet werden soll
        public Rectangle m_collisionRectangle;
        
        
        public void Initialize(User user)
        {
            m_currentUser = user;
            m_isWindowActiv = true;
            m_imageSource = m_currentUser.getSourceImage();
            f_position = new Vector2(100, 100);
            m_collisionRectangle = new Rectangle((int)f_position.X, (int)f_position.Y, m_imageSource.Width, m_imageSource.Height);
        }

        public void Draw (SpriteBatch spriteBatch)
        {
            if (m_isWindowActiv)
            {
                spriteBatch.Draw(m_imageSource, f_position, Color.White);
            }
        }

        public void setActive(bool active)
        {
            m_isWindowActiv = active;
        }

        public int getSelectedTile(Vector2 click)
        {
            int x =(int) (click.X -f_position.X)/(int) m_currentUser.getTileSize().X;
            int y = (int)(click.Y -f_position.Y) / (int)m_currentUser.getTileSize().Y;

            return (x + (y * m_currentUser.getTilesPerRow()));
        }

        public bool isWindowActiv()
        {
            return m_isWindowActiv;
        }

    }
}

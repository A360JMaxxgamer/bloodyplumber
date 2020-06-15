using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    public class Flyinghead
    {
        private Rectangle m_rectangle;
        private int m_xSpeed;
        private int m_ySpeed;
        private SpriteEffects m_spriteEffect;
        private Texture2D m_textureHead;
        private ScreenManager m_manager;

        public Flyinghead(ScreenManager manager)
        {
            m_manager = manager;
            m_textureHead = m_manager.imageFileSystem.flyingHead;
            Random r = new Random();
            m_xSpeed = r.Next(10, 20);
            m_ySpeed = r.Next(10, 20);
            m_rectangle = new Rectangle(100, 100, 0,0);
            m_spriteEffect = SpriteEffects.None;
            
        }

        public void Update()
        {
            m_rectangle.X += m_xSpeed;
            m_rectangle.Y += m_ySpeed;
            checkHeadPosition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_rectangle.X += m_xSpeed;
            m_rectangle.Y += m_ySpeed;
            spriteBatch.Draw(m_textureHead, m_rectangle, Color.White);
        }

        public void startInLoadingScreen()
        {
            m_textureHead = m_manager.imageFileSystem.flyingHead;
            m_rectangle.Width = m_textureHead.Width;
            m_rectangle.Height = m_textureHead.Height;
        }

        public void checkHeadPosition()
        {
           
            if (m_rectangle.X + m_xSpeed + m_textureHead.Width >= UIConstants.screenWidth)
            {
                m_spriteEffect = SpriteEffects.FlipHorizontally;
                m_xSpeed *= -1;
            }
            if (m_rectangle.X + m_xSpeed <= 0)
            {
                m_spriteEffect = SpriteEffects.None;
                m_xSpeed *= -1;
            }
            if (m_rectangle.Y + m_ySpeed + m_textureHead.Height >= UIConstants.screenHeight)
                m_ySpeed *= -1;
            if (m_rectangle.Y + m_ySpeed <= 0)
                m_ySpeed *= -1;
        }
    }
}

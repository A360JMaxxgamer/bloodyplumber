using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    class Lifebar
    {
        private int m_totalHealth;
        private int m_currentHealth;
          
        private Rectangle m_lifeBar;
        private ScreenManager m_screenManager;
        private int m_width;
        private static int height = 20;
        private Vector2 textPosition;
        private String text;

        public Lifebar(int totalHealth, ScreenManager screenmanager, String name)
        {
            m_width = 1920 / 2;
            m_lifeBar = new Rectangle();
            m_lifeBar.Y = 1080 - height;
            m_lifeBar.Height = height;
            m_currentHealth = totalHealth;
            m_totalHealth = totalHealth;
            m_screenManager = screenmanager;
            text = name;
            textPosition = new Vector2();
            textPosition.Y = m_lifeBar.Y - 50;

        }

        public void Update(int health)
        {
            m_currentHealth = health;
            calculateLifebar();
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.DrawString(m_screenManager.Font, text, textPosition, Color.Red);
            spritebatch.Draw(m_screenManager.imageFileSystem.redPixel,m_lifeBar, Color.White);
        }

        public void calculateLifebar()
        {
            m_lifeBar.Width = (m_currentHealth * m_width) / m_totalHealth;
            m_lifeBar.X = 1920 - m_lifeBar.Width;
        }


    }
}

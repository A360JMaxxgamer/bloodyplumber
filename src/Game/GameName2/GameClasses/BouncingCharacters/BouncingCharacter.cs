using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    class BouncingCharacter
    {
        private String m_char;
        private Vector2 f_position;
        private int m_maximum, m_minimum;
        private int m_currentVelocity;
        private Color m_color;


        public BouncingCharacter(Vector2 position, String c, int maximum, int minimum, int yvelocity)
        {
            f_position = position;
            m_char = c;
            m_maximum = maximum;
            m_minimum = minimum;
            m_currentVelocity = yvelocity;
            m_color = Color.Red;
        }

        public void Update()
        {
            if (f_position.Y > m_maximum || f_position.Y < m_minimum)
                m_currentVelocity *= (-1);

            f_position.Y += m_currentVelocity;
        }

        public void setColor(Color c)
        {
            m_color = c;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {

            //spriteBatch.DrawString(font, m_char, f_position, Color.Red);
            spriteBatch.DrawString(font, m_char, f_position, m_color, 0.0f, Vector2.Zero, 2, SpriteEffects.None, 1);
        }
    }
}

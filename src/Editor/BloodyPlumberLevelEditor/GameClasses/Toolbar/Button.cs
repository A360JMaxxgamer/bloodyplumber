using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BloodyPlumberLevelEditor
{
    class Button
    {
        private Texture2D m_image;
        private Vector2 f_position, f_startPosition, f_speed;
        private bool m_active, m_isMovingIn;
        private Rectangle m_collision;
        private int m_number;

        public void Initialize(Texture2D image, float xPosition, float yPosition, Vector2 speed, int number)
        {
            m_image = image;
            f_startPosition = new Vector2(xPosition, yPosition);
            f_position = new Vector2(xPosition, yPosition);
            f_speed = speed;
            m_active = m_isMovingIn = false;
            m_collision = new Rectangle((int)f_position.X, (int)f_position.Y, m_image.Width, m_image.Height);
            m_number = number;
        }

        public void Update(GameTime gameTime, bool active, bool moving)
        {
            m_active = active;
            m_isMovingIn = moving;
            checkActivity();
            updateRectangle();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (m_active)
                spriteBatch.Draw(m_image, f_position, Color.White);
        }

        private void updateRectangle()
        {
            m_collision.X = (int)f_position.X;
            m_collision.Y = (int)f_position.Y;
        }

        public Rectangle getRectangle()
        {
            return m_collision;
        }

        public int getNumber()
        {
            return m_number;
        }

        private void checkActivity()
        {
            if (m_active)
            {
                if (m_isMovingIn)
                    f_position += f_speed;
            }
            else
            {
                f_position = f_startPosition;
                m_isMovingIn = false;
            }
        }

    }
}

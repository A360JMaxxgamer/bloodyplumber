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
    class Toolbar
    {
        private Vector2 f_startPosition, f_position, f_speed;
        private Texture2D m_background;
        private bool m_isMovingIn, m_isActive;
        private int m_screenWidth, m_screenHeight;
        private List<Button> m_buttons;
        private Rectangle m_collision;

        public enum States
        {
            SetTile,
            SetEnemy ,
            Delete ,
            SelectTile ,
            SetEndFlag,
            SetCoin
        };

        public void Initialize(float xPosition,  float yPosition, float xSpeed, float ySpeed, Texture2D[] textures, int screenWidth, int screenHeight)
        {
            f_startPosition = new Vector2(xPosition,yPosition);
            f_position = new Vector2(xPosition, yPosition);
            f_speed    = new Vector2(xSpeed, ySpeed);
            m_background = textures[0];
            m_screenWidth = screenWidth;
            m_screenHeight = screenHeight;
            m_isActive = false;
            m_isMovingIn = false;
            m_collision = new Rectangle((int)f_position.X, (int)f_position.Y, m_background.Width, m_background.Height);
            m_buttons = new List<Button>();
            initializeButtons(textures);
        }

        public void Update(GameTime gameTime)
        {
            checkActivity();
            updateRectangle();
            foreach (Button button in m_buttons)
                button.Update(gameTime, m_isActive, m_isMovingIn);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (m_isActive)
            {
                spriteBatch.Draw(m_background, f_position, Color.White);
                foreach (Button button in m_buttons)
                    button.Draw(spriteBatch);
            }
        }



        private  int clickedButton(int xClick, int yClick)
        {
            foreach(Button button in m_buttons)
            {
                if(button.getRectangle().Contains(xClick, yClick))
                    return button.getNumber();
            }   
            return 0;
        }
        public States getState(int xClick, int yClick)
        {
            int value = clickedButton(xClick, yClick);
            if (value == 1)
                return States.SetTile;
            if (value == 2)
                return States.SetEnemy;
            if (value == 3)
                return States.Delete;
            if (value == 4)
                return States.SelectTile;
            if (value == 5)
                return States.SetCoin;
            return States.SetTile;
        }
        private void checkActivity()
        {
            if (m_isActive)
            {
                if(m_isMovingIn)
                    f_position += f_speed;
                
                if (m_isMovingIn && f_position.X < m_screenWidth - m_background.Width)
                {
                    m_isMovingIn = false;
                }
            }
            else
            {
                f_position = f_startPosition;
                m_isMovingIn = false;
            }
        }
        private void initializeButtons(Texture2D[] textures)
        {
            for(int i = 1; i < textures.Count() ; i++)
            {
                Button button = new Button();
                button.Initialize(textures[i], f_startPosition.X+( m_background.Width-textures[i].Width), f_startPosition.Y + i*(100), f_speed, i);
                m_buttons.Add(button);
            }
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

        public int getScreenWidth()
        {
            return m_screenWidth;
        }

        public bool isActive()
        {
            return m_isActive;
        }
        public void activate()
        {
            m_isActive = true;
            m_isMovingIn = true;
        }

        public void deactivate()
        {
            m_isActive = false;
            m_isMovingIn = false;
        }

        public bool isMoving()
        {
            return m_isMovingIn;
        }
    }
}

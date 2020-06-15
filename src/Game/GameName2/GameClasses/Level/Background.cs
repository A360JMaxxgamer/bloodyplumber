using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    class Background : IDisposable
    {
        private Texture2D m_backgroundImage;
        private Texture2D m_backgroundImage2;

        //Zwei Positionsvektoren damit wir zwei Bilder hintereinander haben um immer eines aktiv im Bild zu haben
        private Vector2 f_positionOne;
        private Vector2 f_positionTwo;
        
        private Vector2 f_scale;
        private int m_speed;
        private Player m_player;
        private bool m_alwaysMove;                      //Soll es sich bewegen obwohl der Spieler still steht
        private bool m_gameBackground;

        public void Initialize(Texture2D image, Texture2D image2, Vector2 position, int speed, Player player, bool isAlwaysMoving, bool gameBackground)
        {
            m_backgroundImage = image;
            m_backgroundImage2 = image2;
            m_speed = speed;
            m_player = player;
            m_alwaysMove = isAlwaysMoving;
            m_gameBackground = gameBackground;
            f_positionOne = position;
            f_positionTwo = new Vector2(f_positionOne.X + m_backgroundImage.Width, f_positionOne.Y);
        }

        public void Update(GameTime gameTime)
        {
            calculateAccleration();
            checkPosition();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Erstes Bild zeichnen
            spriteBatch.Draw(m_backgroundImage, f_positionOne, Color.White);        
            //Zweites Bild zeichnen
            spriteBatch.Draw(m_backgroundImage2, f_positionTwo, Color.White);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.SuppressFinalize(m_backgroundImage);
            GC.SuppressFinalize(m_backgroundImage2);
        }

        //Berechnet die Beschleunigung in Abhängigkeit von m_alwaysMove und der Bewegung des Spielers
        public void calculateAccleration()
        {
            if (m_alwaysMove)
            {
                if (m_player.getMoveFloor())
                {
                    f_positionOne.X -= m_speed + m_player.getPlayerSpeed();
                    f_positionTwo.X -= m_speed + m_player.getPlayerSpeed();
                }
                else
                {
                    f_positionOne.X -= m_speed;
                    f_positionTwo.X -= m_speed;
                }

            }
            else
            {
                //Bewegung des Spielers nach rechts
                if (m_player.getMoveFloor() && m_gameBackground)
                {
                    f_positionOne.X -= m_speed;
                    f_positionTwo.X -= m_speed;
                }
            }
        }
        //Überprüft ob ein Bild wieder hinten in der Schlange angestellt werden muss
        public void checkPosition()
        {
            if (f_positionOne.X + m_backgroundImage.Width < 0)
                f_positionOne.X = f_positionTwo.X + m_backgroundImage.Width;
            if (f_positionTwo.X + m_backgroundImage.Width < 0)
                f_positionTwo.X = f_positionOne.X + m_backgroundImage.Width;
        }
    }
}

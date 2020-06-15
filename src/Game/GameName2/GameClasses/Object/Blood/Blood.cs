using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    public class Blood : Object
    {

        private Vector2[] m_listOfVectors;
        public Vector2 currentposition;
        private Vector2 updateVector;                       //Wird in der Update Methode immer wieder überschrieben um die Vectore zu aktualisieren
        private int m_doublications;
        private bool m_active;

        public void Initialize(float f_xStartPosition, float f_yStartPosition, float f_xStartVelocity, float f_yStartVelocity, float speed, short health, Animation startAnimation, int screenWidth, int doublications)
        {
            updateVector = Vector2.Zero;
            base.Initialize(f_xStartPosition, f_yStartPosition, f_xStartVelocity, f_yStartVelocity, speed, health, startAnimation, screenWidth);
            m_Animation.setAnimationActive(true);
            m_listOfVectors = new Vector2[doublications];
            currentposition = new Vector2();
            Random xRandom = new Random();
            for (int i = 0; i < doublications; i++)
            {
                int x = xRandom.Next(-300, 300);
                int y = xRandom.Next(-100, 100);
                m_listOfVectors[i] = new Vector2(f_Position.X + x * (speed / UIConstants.bloodFaktor), f_Position.Y + y * (speed / UIConstants.bloodFaktor));
            }
            m_doublications = doublications;
            m_active = false;
               
        }
       
        public void Initialize(float f_xStartPosition, float f_yStartPosition ,float f_xStartVelocity, float f_yStartVelocity , float speed)
        {
            Random xRandom = new Random();
            currentposition.X = f_xStartPosition;
            currentposition.Y = f_yStartPosition;
            f_xVelocity = f_xStartVelocity;
            f_yVelocity = f_yStartVelocity;
            for (int i = 0; i < m_doublications; i++)
            {
                int x = xRandom.Next(-50, 200);
                int y = xRandom.Next(-20, 50);
                m_listOfVectors[i].X = currentposition.X + x * (speed / UIConstants.bloodFaktor);
                m_listOfVectors[i].Y = currentposition.Y + y * (speed / UIConstants.bloodFaktor);
            }
            m_active = true;
            m_Animation.resetAlpha();

        }

        public virtual void Update(GameTime gameTime, Player player)
        {
            if (m_active)
            {
                if (!player.getMoveFloor())
                {
                    currentposition.X += f_xVelocity;
                    currentposition.Y += (f_yVelocity + m_gravity);
                }
                if(player.getMoveFloor() && f_xVelocity == 0)
                {
                    currentposition.X -= player.f_Speed;
                    currentposition.Y += (f_yVelocity + m_gravity);
                }
                if (player.getMoveFloor() && f_xVelocity != 0)
                {
                    currentposition.X += f_xVelocity - player.f_Speed;
                    currentposition.Y += (f_yVelocity + m_gravity);
                }

                m_Animation.Update(gameTime, currentposition.X, currentposition.Y);

                m_Animation.setLeft((int)currentposition.X - 2, (int)currentposition.Y + 1, 2, 1);
                m_Animation.setRight((int)currentposition.X + 2 + m_Animation.getFrameWidth(), (int)currentposition.Y + 1, 2, 1);
                m_Animation.setBottom((int)currentposition.X + 2, (int)currentposition.Y + 2 + m_Animation.getFrameWidth(), 2, 1);


                for (int i = 0; i < m_listOfVectors.Count(); i++)
                {
                    if (!player.getMoveFloor())
                    {
                        updateVector.X = m_listOfVectors.ElementAt(i).X + f_xVelocity;
                        updateVector.Y = m_listOfVectors.ElementAt(i).Y + f_yVelocity + m_gravity;
                        m_listOfVectors.SetValue(updateVector, i);

                    }
                    if (player.getMoveFloor() && f_xVelocity == 0)
                    {
                        updateVector.X = m_listOfVectors.ElementAt(i).X - player.f_Speed;
                        updateVector.Y = m_listOfVectors.ElementAt(i).Y + f_yVelocity + m_gravity;
                        m_listOfVectors.SetValue(updateVector, i);
                    }
                    if (player.getMoveFloor() && f_xVelocity != 0)
                    {
                        updateVector.X = m_listOfVectors.ElementAt(i).X + f_xVelocity - player.f_Speed;
                        updateVector.Y = m_listOfVectors.ElementAt(i).Y + f_yVelocity + m_gravity;
                        m_listOfVectors.SetValue(updateVector, i);
                    }
                
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (m_active)
            {
                m_Animation.Draw(spriteBatch, m_playerAnimationMirror);
                foreach (Vector2 vec in m_listOfVectors)
                {
                    m_Animation.DrawCopy(spriteBatch, m_playerAnimationMirror, vec);
                }
            }
        }


        public void deactive()
        {
            m_active = false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BloodyPlumber
{
    public class Crocodile : Boss
    {
        enum States { Shooting, Running, Jumping, Standing }
        private States m_currentState;
        private States m_previousState;
        private int m_maxRunDistance;
        private int m_alreadyRan;
        private int m_jumpHeight;
        private int m_jumpMaximum;
        private bool m_startedJump;
        private int m_lastShot;
        private int m_shootingIntervall;
        private int m_shootsPerState;
        private int m_countShotsCurrentState;
        private List<Projectile> m_listOfProjectiles;
        private int m_currentProjectile;
        private int m_startingHealth;
       

        public override void Initialize(float f_xStartPosition, float f_yStartPosition, float f_xStartVelocity, float f_yStartVelocity, float speed, short health, Animation startAnimation, int screenWidth, ScreenManager manager, String name)
        {
            base.Initialize(f_xStartPosition, f_yStartPosition, f_xStartVelocity, f_yStartVelocity, speed, health, startAnimation, screenWidth, manager, name);
            m_currentState = States.Standing;
            m_previousState = States.Standing;
            m_maxRunDistance = 500;
            m_alreadyRan = 0;
            m_jumpHeight = 500;
            m_lastShot = 0;
            m_shootingIntervall = 1000;
            m_shootsPerState = 1;
            m_countShotsCurrentState = 0;
            initializeProjectiles();
            m_endFlag = true;
            m_alive = true;
            m_startedJump = false;
            m_startingHealth = m_Health;
            m_Animation.setAnimationLooping(false);
        }

        public override void Update(GameTime gameTime, bool isFloorMoving, float floorMovingspeed)
        {
            base.Update(gameTime, isFloorMoving, floorMovingspeed);
            base.Update(gameTime);
            m_Animation.shiftRectangle((int)f_xVelocity, (int)f_yVelocity, m_playerAnimationMirror);
            updateMovement();
            checkFighting();
        }

        public override void Update(GameTime gameTime)
        {
            if (!m_startedJump)
                updateMovement();
            m_Animation.shiftRectangle((int)f_xVelocity, (int)f_yVelocity, m_playerAnimationMirror);
            f_Position.X += f_xVelocity;
            f_Position.Y += (f_yVelocity + m_gravity);
            updateState();
            if (m_currentState == States.Running)
                updateRunningState();
            else
            {
                if (m_currentState == States.Jumping)
                    updateJumpingState();
                else
                {
                    if (m_currentState == States.Shooting)
                        updateShootingState(gameTime);
                }
            }
            updateProjectiles(gameTime);
            checkSides();
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            drawProjectiles(spriteBatch);
        }

        private void updateState()
        {
            m_previousState = m_currentState;
        }

        private void updateRunningState()
        {
            m_alreadyRan += Math.Abs((int)f_xVelocity);
            
            if (m_alreadyRan > m_maxRunDistance)
            {
                m_alreadyRan = 0;
                jump();
            }
            //Überprüft ob der Boss links am Bildschirm angekommen ist
            if (f_Position.X + f_xVelocity < 0)
            {
                f_xVelocity = 0;
                m_playerAnimationMirror = SpriteEffects.FlipHorizontally;
                shoot();
            }
            //Überprüft ob der Boss rechts am Bildschirm angekommen ist
            if (f_Position.X + f_xVelocity + m_Animation.getFrameWidth() > UIConstants.screenWidth)
            {
                f_xVelocity = 0;
                m_playerAnimationMirror = SpriteEffects.None;
                shoot();
            }
        }

        private void updateJumpingState()
        {
            if (f_Position.Y <= m_jumpMaximum || !m_startedJump)
            {
                f_yVelocity += 2f;
                m_startedJump = false;
            }
        }

        private void updateShootingState(GameTime gameTime)
        {
            if (m_countShotsCurrentState < m_shootsPerState)
            {
                if (m_lastShot + m_shootingIntervall < gameTime.TotalGameTime.TotalMilliseconds)
                {
                    shootProjectile();
                    m_lastShot = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    m_countShotsCurrentState++;
                }
            }
            else
                run();
        }

        private void run()
        {
            if (m_playerAnimationMirror == SpriteEffects.None)
                f_xVelocity = -f_Speed;
            else
                f_xVelocity = f_Speed;
            m_currentState = States.Running;
            m_alreadyRan = 0;
            m_Animation.setAnimationLooping(true);
        }

        private void shoot()
        {
            m_currentState = States.Shooting;
            m_countShotsCurrentState = 0;
            m_Animation.setAnimationLooping(false);
            m_Animation.setCurrentFrame(0);
        }

        private void jump()
        {
            m_currentState = States.Jumping;
            m_startedJump = true;
            f_yVelocity = UIConstants.jumpVelocity;
            m_jumpMaximum = (int)f_Position.Y - m_jumpHeight;
            m_Animation.setAnimationLooping(false);
            m_Animation.setCurrentFrame(3);
        }

        private void initializeProjectiles()
        {
            m_listOfProjectiles = new List<Projectile>();
            Vector2 projectileScale = new Vector2(1,1);
            for (int i = 0; i < 10; i++)
            {
                Projectile p = new Projectile();
                p.Initialize(projectileScale);
                m_listOfProjectiles.Add(p);
            }
            m_currentProjectile = 0;
        }

        private void shootProjectile()
        {
            if (m_playerAnimationMirror == SpriteEffects.None)
            {
                m_listOfProjectiles.ElementAt(m_currentProjectile).Initialize(
                    screenManager.imageFileSystem.crocProjectile, m_playerAnimationMirror,
                    f_Position + new Vector2(0, 110), -UIConstants.shootSpeed);
                calculateCurrentProjectile();
            }
            else
            {
                m_listOfProjectiles.ElementAt(m_currentProjectile).Initialize(
                    screenManager.imageFileSystem.crocProjectile, m_playerAnimationMirror,
                    f_Position + new Vector2(m_Animation.getFrameWidth(),110), UIConstants.shootSpeed);
                calculateCurrentProjectile();
            }
        }

        private void calculateCurrentProjectile()
        {
            if (m_currentProjectile -1  > m_listOfProjectiles.Count)
                m_currentProjectile = 0;
            else
                m_currentProjectile++;
        }

        private void updateProjectiles(GameTime gameTime)
        {
            foreach (Projectile p in m_listOfProjectiles)
                p.Update(gameTime);
        }

        private void drawProjectiles(SpriteBatch spriteBatch)
        {
            foreach (Projectile p in m_listOfProjectiles)
                p.Draw(spriteBatch);
        }

        public  void checkFighting()
        {
            if (f_Position.X < UIConstants.screenWidth - m_Animation.getFrameWidth())
            {
                m_fighting = true;
                screenManager.endBossActive = true;
                run();
                
            }
        }

        public override void updateMovement()
        {
            if (m_down)
            {
                f_yVelocity = -5;
            }

            if (m_down && m_currentState == States.Standing  && m_fighting)
               shoot();
            if (m_down && m_currentState == States.Jumping && !m_startedJump)
                run();
        }

        public override void checkHealth()
        {
            base.checkHealth();
            if (calculatePercentage(m_startingHealth, m_Health) < 75)
            {
                m_shootsPerState = 2;
                f_Speed = 11;
                m_maxRunDistance = 400;
            }
            if (calculatePercentage(m_startingHealth, m_Health) < 30)
            {
                m_shootsPerState = 3;
                f_Speed = 14;
                m_maxRunDistance = 200;
            }
        }

        protected int calculatePercentage(int total, int part)
        {
            return (part * 100) / total;
        }

        protected void checkSides()
        {
            if (f_Position.X + m_Animation.getFrameWidth() > 1920)
                f_Position.X = 1920 - m_Animation.getFrameWidth();
            if (f_Position.X < 0)
                f_Position.X = 0;
            if (m_down)
            {
                int i = (int) f_Position.Y / 80;
                f_Position.Y = i * 80;
            }
        }

        public List<Projectile> getProjectiles()
        {
            return m_listOfProjectiles;
        }


    }
}

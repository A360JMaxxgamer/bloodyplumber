//Steht für den Player in diesem fall mario
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Diagnostics;
using ParticleEmitter;
namespace BloodyPlumber
{
    public class Player : Object
    {
        #region Attributes

        private  Weapon m_playerCurrentWeapon;
        private bool m_playerIsJumping;                                //Gibt wieder ob der Spieler gerade einen Sprung ausführt     
        private int m_playerJumpHeight;                                 //Sprunghöhe
        private int m_playerJumpMaximum;                                //Bis wo er springt
        private bool m_playerStartedJump;                               //Hat der Spieler einen Jump gestartet

        private bool m_movefloor;                                       //Muss der Boden bewegt werden
        private bool m_active;

        private SoundEffect m_jumpWuhu;

        private List<Projectile> m_projectileList;
        private int m_currenProjectile;
        private Vector2 m_shootVector;

        private ParticleSystemSettings m_ermitterSettings;
        private ParticleEmitter.ParticleSystem m_ermitter;
        
        
        private bool m_canShoot;                                   //Bool Variabel ob geschossen werden darf
        private int m_lastShot;                                    //Letzter Schuss
        private int m_score;

        private ScreenManager screenManager;
        private Mouth m_mouth;
        bool leftCollision, rightCollision;
        

        #endregion

        #region Kernschleifen-Methodes 
        //Die Methode mit dem ein Objekt vom Typ Spieler initialisiert wird und dessen Startwerte eingestellt werden
        public void Initialize(float f_xStartPosition, float f_yStartPosition, float f_xStartVelocity, float f_yStartVelocity, float speed, short health,Animation startAnimation, int screenWidth, ScreenManager screenmanager)
        {
            base.Initialize(f_xStartPosition, f_yStartPosition, f_xStartVelocity, f_yStartVelocity, speed, health, startAnimation, screenWidth);
            m_mouth = new Mouth();
            m_mouth.Initialize(screenmanager.imageFileSystem.mouth, 48, 50);
            
            
            m_playerIsJumping = false;
            m_playerStartedJump = false;
            m_playerJumpHeight = 250;
            m_jumpWuhu = screenmanager.audioFileSystem.wuhu;


            screenManager = screenmanager;
            m_movefloor = false;

            m_projectileList = new List<Projectile>();

            m_canShoot = true;
            m_lastShot = 0;
            m_playerAnimationMirror = SpriteEffects.None;


            m_score = 0;
            m_currenProjectile = 0;
            m_shootVector = new Vector2();

           leftCollision = rightCollision = false;

            initializeProjectiles(150, new Vector2(1, 1));
            m_active = true;

            #region Ermitter
            m_ermitterSettings = new ParticleSystemSettings();
            m_ermitterSettings.IsBurst = false;
            m_ermitterSettings.SetLifeTimes(0.5f, 1.0f);
            m_ermitterSettings.SetScales(0.1f, 0.6f);
            m_ermitterSettings.ParticlesPerSecond = 15.0f;
            m_ermitterSettings.InitialParticleCount = (int)(m_ermitterSettings.ParticlesPerSecond * m_ermitterSettings.MaximumLifeTime);
            m_ermitterSettings.SetDirectionAngles(0, 180);
            m_ermitterSettings.color = Color.White;
            m_ermitter = new ParticleEmitter.ParticleSystem(screenmanager.Game, m_ermitterSettings, screenmanager.imageFileSystem.DustParticle);
            m_ermitter.OriginPosition = f_Position;
            m_ermitterSettings.emitterOn = false;
            #endregion
        }

        //Die Methode die in regelmäßigen Zeitintervallen aufgerufen wird um die Spieler Positionen und ggf. Animationen anzupassen
        public override void Update(GameTime gameTime)
        {
            if (m_active)
            {
                checkProjectilePosition(gameTime);
                lastShot(gameTime);

                m_Animation.shiftRectangle((int)getTMPX(), (int)f_yVelocity, m_playerAnimationMirror);
                base.Update(gameTime);

                if (screenManager.endBossActive)
                    m_movefloor = false;

                if (m_playerIsJumping)
                {

                    if (f_Position.Y <= m_playerJumpMaximum || !m_playerStartedJump)
                    {
                        f_yVelocity += 2f;
                        m_playerStartedJump = false;
                    }
                }
                updateWeaponRotation();
                m_playerCurrentWeapon.Update(gameTime, f_Position, m_playerAnimationMirror);
                if ((f_xVelocity > 0 && !m_playerIsJumping))
                {
                    m_ermitterSettings.emitterOn = true;
                    m_ermitterSettings.SetDirectionAngles(140, 180);
                    m_ermitterSettings.SetSpeeds(50, 100);
                }
                else
                {
                    if ((f_xVelocity < 0 && !m_playerIsJumping))
                    {
                        m_ermitterSettings.SetDirectionAngles(0, 40);
                        m_ermitterSettings.emitterOn = true;
                        m_ermitterSettings.SetSpeeds(50, 100);
                    }
                    else
                    {
                        if (m_movefloor && !m_playerIsJumping)
                        {
                            m_ermitterSettings.emitterOn = true;
                            m_ermitterSettings.SetDirectionAngles(140, 180);
                            m_ermitterSettings.SetSpeeds(150, 200);
                        }


                        else
                        {
                            m_ermitterSettings.emitterOn = false;
                        }
                        
                    }
                }
                    


                m_ermitter.Update(gameTime);
                m_ermitter.OriginPosition = f_Position + new Vector2(m_Animation.getFrameWidth() / 2f, m_Animation.getFrameHeight());
                m_mouth.Update(gameTime, f_Position, m_playerAnimationMirror);
            }

            foreach (Projectile projectile in m_projectileList)
                projectile.Update(gameTime);
        }

        //Die Methode um den Spieler zu zeichnen
        public override void Draw(SpriteBatch spriteBatch)
        {

            for(int i = 0 ; i < m_projectileList.Count ; i++)
            {
                m_projectileList.ElementAt(i).Draw(spriteBatch);
            }
            if (m_active)
            {
                base.Draw(spriteBatch);
                //Projektile Drawen
                m_playerCurrentWeapon.Draw(spriteBatch);
                m_ermitter.Draw(spriteBatch);
                m_mouth.Draw(spriteBatch, m_playerAnimationMirror);
            }
        }


        #endregion

        #region Helper

        //Wenn der Knopf zum springen gedrückt wird , wird diese Methode ausgeführt um den Vorgang zu starten
        public void setWeapon(Weapon w)
        {
            m_playerCurrentWeapon.setAnimationActive(false);
            m_playerCurrentWeapon = w;
            m_playerCurrentWeapon.setAnimationActive(true);
        }

        public void setStartWeapon(Weapon w)
        {
            m_playerCurrentWeapon = w;
            m_playerCurrentWeapon.setAnimationActive(true);
        }
        
        public void playerJump(GameTime gameTime)
        {
            m_playerIsJumping = true;
            m_playerStartedJump = true;

            m_Animation.setAnimationLooping(false);
            m_Animation.setCurrentFrame(2);

            m_playerJumpMaximum = (int)f_Position.Y - m_playerJumpHeight;

            Random r = new Random();
            int i = r.Next(0, 10);
            if (i == 5)
            {
                m_mouth.speak((int)m_jumpWuhu.Duration.TotalMilliseconds+(int)gameTime.TotalGameTime.TotalMilliseconds);
                m_jumpWuhu.Play();
            }
        }

        public void addScore(int plus)
        {
            m_score += plus;
        }

        public int getScore()
        {
            return m_score;
        }

        #region Schießen

        public void initializeProjectiles(int count, Vector2 scale)
        {
            for(int i = 0; i < count ; i++)
            {
                Projectile tmp = new Projectile();
                tmp.Initialize(scale);
                m_projectileList.Add(tmp);
            }
        }

        //Überprüft wie lange der letzte Schuss her ist und aktiviert das Schießen ggf wieder
        public void lastShot(GameTime gameTime)
        {
            if ((int)gameTime.TotalGameTime.TotalMilliseconds > (m_lastShot + m_playerCurrentWeapon.getTimeBetweenShots()) && !m_movementCollisionRightSide && !m_movementCollisionLeftSide) 
            {
                m_canShoot = true;
            }
            else
            {
                m_canShoot = false;
            }
        }

        public void updateWeaponRotation()
        {
           

                if (m_movementCollisionRightSide && (m_playerAnimationMirror == SpriteEffects.None))
                {
                    m_playerCurrentWeapon.updateRotation(1);
                    
                }
                else
                {
                    m_playerCurrentWeapon.updateRotation(0);
                    
                }





                if (m_movementCollisionLeftSide && (m_playerAnimationMirror == SpriteEffects.FlipHorizontally))
                {
                    m_playerCurrentWeapon.updateRotation(2);

                }
                else if ((m_playerAnimationMirror == SpriteEffects.FlipHorizontally))
                    m_playerCurrentWeapon.updateRotation(0);

            
        }

        //Wird die Schuss-Taste gedrückt, wird diese Methode aufgerufen und löst den Schuss aus. Sie erzeugt ein Objekt "Projectile", initialisiert dieses und fügt es der Projektilliste hinzu
        public void shoot(GameTime gameTime)
        {
            m_canShoot = false;
            m_lastShot = (int)gameTime.TotalGameTime.TotalMilliseconds;
            int shootSpeed;
            if(m_playerAnimationMirror == SpriteEffects.None)
            {
                shootSpeed = UIConstants.shootSpeed;
            }
            else
            {
                shootSpeed = -UIConstants.shootSpeed;
            }

            if (m_playerAnimationMirror == SpriteEffects.None)
            {
                m_shootVector.X = m_playerCurrentWeapon.getPosition().X+ m_playerCurrentWeapon.getAnimationWidth();
                m_shootVector.Y = m_playerCurrentWeapon.getPosition().Y+5;
                //Magic Number
                m_projectileList.ElementAt(m_currenProjectile).Initialize(m_playerCurrentWeapon.getCurrentProjectile(), m_playerAnimationMirror, m_shootVector, shootSpeed);
            }
            else
            {
                m_shootVector.X = m_playerCurrentWeapon.getPosition().X;
                m_shootVector.Y = m_playerCurrentWeapon.getPosition().Y+5;
                //Magic Number
                m_projectileList.ElementAt(m_currenProjectile).Initialize(m_playerCurrentWeapon.getCurrentProjectile(), m_playerAnimationMirror, m_shootVector, shootSpeed);
            }
            m_projectileList.ElementAt(m_currenProjectile).getProjectileAnimation().setAnimationActive(true);
            m_playerCurrentWeapon.shot();
            m_currenProjectile++;
            if(m_currenProjectile == m_projectileList.Count)
                m_currenProjectile = 0;

        }



        public void checkProjectilePosition(GameTime gameTime)
        {

            for (int i = 0; i < m_projectileList.Count; i++)
            {
                if (m_projectileList.ElementAt(i).getXPosition() < UIConstants.disoposeProjectileLeft || m_projectileList.ElementAt(i).getXPosition() > UIConstants.disoposeProjectileRight)                   // Magic Number !!!!
                {
                    m_projectileList.ElementAt(i).deactivate();
                }
            }
        }

        public void deactivate()
        {
            m_active = false;
        }

       

        #endregion

        #region Getter-Methodes
        public bool getActiv()
        {
            return m_active;
        }
        public List<Projectile> getProjectileList()
        {
            return m_projectileList;
        }

        public bool getMovementCollisionLeftSide()
        {
            return m_movementCollisionLeftSide;
        }

        public bool getMovementCollisionRightSide()
        {

            return m_movementCollisionRightSide;
        }

        public bool getMovementCollisionTopSide()
        {
            return m_movementCollisionTopSide;
        }

        public bool getMovementCollisionBottomSide()
        {
            return m_movementCollisionBottomSide;
        }

        public float getXPlayerVelocity()
        {
            return f_xVelocity;
        }

        public float getYPlayerVelocity()
        {
            return f_yVelocity;
        }

        public float getTMPX()
        {
            if (m_playerAnimationMirror == SpriteEffects.None)
            {
                return f_Speed;

            }
            else
            {
                return -f_Speed;
            }
        }

        public bool canShoot()
        {
            return m_canShoot;
        }

        public int getGravity()
        {
            return m_gravity;
        }

        public bool getMoveFloor()
        {
            return m_movefloor;
        }

        public bool playerIsJumping()
        {
            return m_playerIsJumping;
        }

        public bool playerStartedJump()
        {
            return m_playerStartedJump;
        }

        public float getXPlayerPosition()
        {
            return f_Position.X;
        }

        public float getYPlayerPosition()
        {
            return f_Position.Y;
        }

        public Weapon getCurrentPlayerWeapon()
        {
            return m_playerCurrentWeapon;
        }


        public float getPlayerSpeed()
        {
            return f_Speed;
        }

        public SpriteEffects getAnimationMirror()
        {
            return m_playerAnimationMirror;
        }

        
        #endregion

        #region Setter-Methodes
        public void setActive(bool val)
        {
            m_active = val;
        }
        public void setPlayerWeapon(Weapon weapon)
        {
            m_playerCurrentWeapon = weapon;
        }

        public void setPlayerStartedJump(bool value)
        {
            m_playerStartedJump = value;
        }

        public void setPlayerIsJumping(bool value)
        {
            m_playerIsJumping = value;
        }

        public void setMoveFloor(bool value)
        {
            m_movefloor = value;
        }

        public void speak(int duration)
        {
            m_mouth.speak(duration);
        }


        #endregion
        #endregion
    }
}

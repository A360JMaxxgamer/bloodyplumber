/// Mit dieser klasse können die unterschiedlichen waffen implementiert werden
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;


namespace BloodyPlumber
{
    public class Weapon : IPowerUps
    {
        #region Attributes
        private Vector2 f_weaponPosition;                           //Positionen der Waffe
        private Animation m_weaponAnimation;                        //Die zugehörige Animation der Waffe
        private Texture2D m_weaponProjectile;                      //Das aktuelle Projektil der Waffe als Grafikstrip
        private SpriteEffects m_animationMirror;                    //ggf. Spiegelung an der Y-Achse
        private Vector2 m_translation;
        private int m_timeBetweenShots;
        private SoundEffect m_shotSound;
        private Player m_player;
        private int m_ammo;
        private int m_shotAmmo;
        private ScreenManager m_manager;
        #endregion

        #region Kernschleife-Methodes
        //Die Methode mit dem ein Objekt vom Typ Waffe initialisiert wird und dessen Startwerte eingestellt werden
        public void Initialize(float f_xPosition, float f_yPosition, Animation animation, Texture2D projectile, int millisecondsBetweenShots, SoundEffect shotSound, int ammo, ScreenManager manager)
        {
            f_weaponPosition.X = f_xPosition;
            f_weaponPosition.Y = f_yPosition;
            m_weaponAnimation = animation;
            m_weaponProjectile = projectile;
            //SpriteEffects der die Informationen der Spiegelung enthält
            m_animationMirror = SpriteEffects.None;
            m_timeBetweenShots = millisecondsBetweenShots;
            m_shotSound = shotSound;
            m_ammo = ammo;
            m_translation = new Vector2();
            m_manager = manager;
            m_shotAmmo = m_ammo;
        }

        //Die Methode die in regelmäßigen Zeitintervallen aufgerufen wird um die Waffe bzw. ihre Animation(SpriteEffects) zu aktualisieren
        public void Update(GameTime gameTime, Vector2 currentPosition,SpriteEffects effect)
        {
            if (effect == SpriteEffects.None)
            {
                f_weaponPosition.X = currentPosition.X + 28 + m_translation.X;
                f_weaponPosition.Y = currentPosition.Y + 70 + m_translation.Y; 
            }
            else
            {
                f_weaponPosition.X = currentPosition.X - 60 + m_translation.X;
                f_weaponPosition.Y = currentPosition.Y + 70 + m_translation.Y;
            }
            m_weaponAnimation.Update(gameTime, f_weaponPosition.X, f_weaponPosition.Y);
            m_animationMirror = effect;
            checkAmmo();
        }

        //Die Methode um die Waffe zu zeichnen
        public void Draw(SpriteBatch spriteBatch)
        {
            m_weaponAnimation.Draw(spriteBatch, m_animationMirror);
        }
        #endregion

        #region Helper

        private void checkAmmo()
        {
            if(m_shotAmmo <= 0)
            {
                m_manager.powerupSystem.laser.refill();
                m_player.setWeapon(m_manager.powerupSystem.laser);
            }
        }

        public int getAmmo()
        {
            return m_shotAmmo;
        }

        public void shot()
        {
            m_shotSound.Play();
            m_shotAmmo--;
        }
        
        public void refill()
        {
            m_shotAmmo = m_ammo;
        }
        public void picked(Player player)
        {
            player.setWeapon(this);
            m_player = player;
            m_shotAmmo = m_ammo;
        }

        public Vector2 getPosition()
        {
            return f_weaponPosition;
        }

        public int getTimeBetweenShots()
        {
            return m_timeBetweenShots;
        }


        public Texture2D getCurrentProjectile()
        {
            return m_weaponProjectile;
        }

        public int getAnimationWidth()
        {
            return m_weaponAnimation.getFrameWidth();
        }

        public void setAnimationActive(bool b)
        {
            m_weaponAnimation.setAnimationActive(b);
        }

        public void updateRotation(int i)
        {
            /* i=0=keine Rotation 
             * i=1=intersects rechte seite
             * i=2=intersects linke Seite*/
            if (i == 0)
            {
                m_weaponAnimation.setRotation(0.0f);
                m_translation.X = 0;
                m_translation.Y = 0;
            }
            if (i == 1)
            {
                m_weaponAnimation.setRotation(4.8f);
                m_translation.X = 0;
                m_translation.Y = 10;
            }
            if (i == 2)
            {
                m_weaponAnimation.setRotation(-4.8f);
                m_translation.X = 130;
                m_translation.Y = -130;

            }

            

        }

        public IPowerUps clone()
        {
            Weapon w = (Weapon)this.MemberwiseClone();
            return w;
        }
        #endregion
    }
}

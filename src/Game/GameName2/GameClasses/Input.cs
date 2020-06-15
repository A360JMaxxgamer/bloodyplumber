//Verarbeitet den Keyboardinput des Spiels
//Input über touch muss ergänzt werden

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    class Input
    {
        #region Attribute
        private KeyboardState m_keyboardCurrentState;           //Aktueller Status der Tastatur

        private KeyboardState m_keyboardPreviousState;          //Vorheriger Status der Tastatur

        private Player m_player;                                //Der Spieler der gesteuert wird

        private Level m_level;

        private ScreenManager screenManager;
        private Keys left, right, jump ,shoot;

        #endregion

        public void Initialize(Player player, Level level, ScreenManager screenmanager)
        {
            m_keyboardCurrentState = new KeyboardState();
            m_keyboardCurrentState = Keyboard.GetState();
            m_keyboardPreviousState = new KeyboardState();
            m_keyboardPreviousState = m_keyboardCurrentState;
            m_player = player;
            screenManager = screenmanager;
            m_level = level;
            left = screenmanager.keys.left;
            right = screenmanager.keys.right;
            jump = screenmanager.keys.jump;
            shoot = screenmanager.keys.shoot;
        }

        public void Update(GameTime gametime)
        {
            m_keyboardPreviousState = m_keyboardCurrentState;
            m_keyboardCurrentState = Keyboard.GetState();

            #region Kein Knopf gedrückt / Keine Bewegung
            //Wenn gar kein Knopf gedrückt wird
            if (m_keyboardCurrentState.IsKeyUp(left) && m_keyboardCurrentState.IsKeyUp(right))
            {
                m_player.setXVelocity(0f);
                m_player.setAnimationLooping(false);
                if (!m_player.playerIsJumping())
                    m_player.getAnimation().setCurrentFrame(0);
                m_player.setMoveFloor(false);


            }
            #endregion

            #region Pressed A / Links laufen
            //Wenn  A gedrückt wird
            if (m_keyboardCurrentState.IsKeyDown(left ) && m_keyboardCurrentState.IsKeyUp(right))
            {
                if (!m_player.getMovementCollisionLeftSide())
                {
                    m_player.setXVelocity(-m_player.getPlayerSpeed());
                    m_player.setMoveFloor(false);

                    //Animation anpassen
                    if (!m_player.playerIsJumping())
                    {
                        m_player.setAnimationLooping(true);
                    }

                    m_player.setAnimationMirror(SpriteEffects.FlipHorizontally);
                }

                if(m_player.getMovementCollisionLeftSide())
                {
                    m_player.setXVelocity(0f);
                    m_player.setMoveFloor(false);

                    //Animation anpassen
                    m_player.setAnimationLooping(false);
                    m_player.getAnimation().setCurrentFrame(0);
                    m_player.setAnimationMirror(SpriteEffects.FlipHorizontally);
                    
                }
            }
            #endregion

            #region Pressed D / Rechts laufen
            //Wenn D gedrückt wird
            if (m_keyboardCurrentState.IsKeyDown(right) && m_keyboardCurrentState.IsKeyUp(left))
            {
                if ((!m_player.getMoveFloor() && !m_player.getMovementCollisionRightSide() && m_player.f_Position.X + m_player.f_xVelocity <= UIConstants.playerMaxXPosition )||(
                    !m_player.getMoveFloor() && !m_player.getMovementCollisionRightSide() && screenManager.endBossActive && m_player.f_Position.X + m_player.f_xVelocity < 1920- m_player.m_Animation.getFrameWidth()))
                {

                    m_player.setXVelocity(m_player.getPlayerSpeed());
                    m_player.setMoveFloor(false);
                    if (!m_player.playerIsJumping())
                         m_player.setAnimationLooping(true);
                    m_player.setAnimationMirror(SpriteEffects.None);
                }

                if (!m_player.getMoveFloor() && !m_player.getMovementCollisionRightSide() && m_player.f_Position.X + m_player.f_xVelocity > UIConstants.playerMaxXPosition && !screenManager.endBossActive)
                {

                    m_player.setXVelocity(0);
                    m_player.setMoveFloor(true);
                    if (!m_player.playerIsJumping())
                        m_player.setAnimationLooping(true);
                    m_player.setAnimationMirror(SpriteEffects.None);
                }
                if(m_player.getMoveFloor() && m_player.getMovementCollisionRightSide() || m_player.getMovementCollisionRightSide())
                {
                    m_player.setXVelocity((0f));
                    m_player.setAnimationLooping(false);
                    m_player.setAnimationMirror(SpriteEffects.None);
                    m_player.setMoveFloor(false);
                }
                if (m_player.getMoveFloor() && !m_player.getMovementCollisionRightSide() && m_player.f_Position.X + m_player.f_xVelocity <= UIConstants.playerMaxXPosition && !screenManager.endBossActive)
                {

                    m_player.setXVelocity(0f);
                    if (!m_player.playerIsJumping())
                         m_player.setAnimationLooping(true);
                    m_player.setAnimationMirror(SpriteEffects.None);
                }

                if (!m_player.getMovementCollisionRightSide() && m_player.f_Position.X + m_player.f_xVelocity > UIConstants.playerMaxXPosition  && !screenManager.endBossActive)
                {
                    m_player.setMoveFloor(true);
                    m_player.setXVelocity(0f);
                    if (!m_player.playerIsJumping())
                         m_player.setAnimationLooping(true);
                    m_player.setAnimationMirror(SpriteEffects.None);
                }
            }

#endregion

            #region Schuss / Enter
            //Wenn Enter gedrückt wird und es vor nicht gedrückt war , wird ein Schuss ausgelöst , sofern diese Möglichkeit aktiviert ist
            if (m_keyboardCurrentState.IsKeyDown(shoot) && m_player.canShoot())
            {
                m_player.shoot(gametime);
            }

            #endregion

            #region Sprung
            if (m_player.playerIsJumping() == false)
            {
                if (m_keyboardCurrentState.IsKeyDown(jump) && !m_keyboardPreviousState.IsKeyDown(jump) && m_player.getYPlayerVelocity() == 0)
                {
                    m_player.playerJump(gametime);
                }
            }
            if(m_player.playerStartedJump())
            {
                if (m_keyboardCurrentState.IsKeyDown(jump))
                {
                    m_player.setYVelocity(UIConstants.jumpVelocity);
                }
                else
                {
                    m_player.setPlayerStartedJump(false);
                }
            }

            #endregion


        }

    }
}

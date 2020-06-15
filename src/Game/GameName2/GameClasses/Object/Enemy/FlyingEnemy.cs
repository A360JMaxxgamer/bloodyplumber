using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyPlumber
{
    public class FlyingEnemy : Enemy
    {
        public override void Initialize(Animation animation)
        {
            base.Initialize(animation);
            f_yVelocity = -5;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool isFloorMoving, float floorMovingspeed)
        {
            checkActivity();
            updateMovement();
            checkSpriteMirror();
            float tmpX = calculateXSpeed(isFloorMoving, floorMovingspeed);
            m_Animation.shiftRectangle((int)tmpX, (int)f_yVelocity, m_playerAnimationMirror);

            f_Position.X += tmpX;


            m_Animation.Update(gameTime, f_Position.X, f_Position.Y);

            if (f_Position.X < UIConstants.m_disposeEnemyPosition)
                Dispose();
        }
        public override void updateMovement()
        {
            if (m_left)
            {
                f_xVelocity = f_Speed;
            }

            if (m_right)
            {
                f_xVelocity = -f_Speed;
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BloodyPlumber
{
    class Mouth
    {
        private Animation m_animation;
        private Vector2 f_moved;
        private int m_deactivateTime;
        private bool m_active;
        private Vector2 f_position;

        public void Initialize(Animation animation, int xSpacing, int ySpacing)
        {
            m_animation = animation;
            f_moved = new Vector2(xSpacing,ySpacing);
            f_position = new Vector2(0,0);
            m_active = false;
            m_deactivateTime = 0;
        }

        public void Update(GameTime gameTime, Vector2 currentPlayerPosition, SpriteEffects effect)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds > m_deactivateTime)
                m_active = false;
            if (effect == SpriteEffects.FlipHorizontally)
                f_position = f_moved + currentPlayerPosition + new Vector2(-20, 0);
            else
                f_position = f_moved + currentPlayerPosition;
            m_animation.Update(gameTime, f_position.X, f_position.Y);
            checkAnimation();
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects effect)
        {
            if (m_active)
                m_animation.Draw(spriteBatch, effect);
        }

        public void speak(int speakTime)
        {
            m_deactivateTime = speakTime;
            m_active = true;
        }

        public void checkAnimation()
        {
            m_animation.setAnimationActive(m_active);
        }
    }
}

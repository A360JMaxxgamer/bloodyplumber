//Animiert den Spieler mit hilfe der baseklasse Animation

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyPlumber
{
    public class PlayerAnimation : Animation
    {
        public override void Initialize(Texture2D sprite, float f_xPosition, float f_yPosition, float startAlpha, float alphaReducing, int frameWidth, int frameHeight, int frameCount, int frameTime, bool looping, Vector2 scale)
        {
            base.Initialize(sprite, f_xPosition, f_yPosition, startAlpha, alphaReducing, frameWidth, frameHeight, frameCount, frameTime, looping, scale);

          
        }

        public override void Update(GameTime gameTime, float f_xPosition, float f_yPosition)
        {
            base.Update(gameTime, f_xPosition, f_yPosition);
            
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteEffects effect)
        {
            base.Draw(spriteBatch, effect);
        }

        public override void shiftRectangle(int shiftX, int shiftY, SpriteEffects effect)
        {

            if (effect == SpriteEffects.None)
            {
                top = new Rectangle((int)f_animationPosition.X + (int)(f_animationScale.X * 20) + shiftX, (int)f_animationPosition.Y - 5 + shiftY, m_animationFrameWidth - 30, 5);

                bottom = new Rectangle((int)f_animationPosition.X + (int)(f_animationScale.X * 5) + shiftX + 25, (int)f_animationPosition.Y + m_animationFrameHeight + 5 + shiftY, 50, 5);

                left = new Rectangle((int)f_animationPosition.X - (int)(f_animationScale.X * 10) + shiftX, (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 40) + shiftY, 5, m_animationFrameHeight - (int)(f_animationScale.Y * 80));

                right = new Rectangle((int)f_animationPosition.X + m_animationFrameWidth + (int)(f_animationScale.X * 5) + shiftX, (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 40) + shiftY, 5, m_animationFrameHeight - (int)(f_animationScale.Y * 80));
            }
            else
            {
                top = new Rectangle((int)f_animationPosition.X + (int)(f_animationScale.X * 20) + shiftX, (int)f_animationPosition.Y - 5 + shiftY, m_animationFrameWidth - 30, 5);

                bottom = new Rectangle((int)f_animationPosition.X + (int)(f_animationScale.X * 5) + shiftX + 25, (int)f_animationPosition.Y + m_animationFrameHeight + 5 + shiftY, 60, 5);

                left = new Rectangle((int)f_animationPosition.X - (int)(f_animationScale.X * 10) + shiftX + 10, (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 40) + shiftY, 5, m_animationFrameHeight - (int)(f_animationScale.Y * 80));

                right = new Rectangle((int)f_animationPosition.X + m_animationFrameWidth + (int)(f_animationScale.X * 5) + shiftX, (int)(f_animationPosition.Y) + (int)(f_animationScale.Y * 40) + shiftY, 5, m_animationFrameHeight - (int)(f_animationScale.Y * 80));
            }
        }


    }
}

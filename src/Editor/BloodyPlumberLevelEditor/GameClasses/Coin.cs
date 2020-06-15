using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace BloodyPlumberLevelEditor
{
    public class Coin
    {
        [XmlElement]
        public bool m_active;
        [XmlElement]
        public Vector2 f_position;
        public Animation m_Animation;

        public void Initialize(Vector2 position, Texture2D picture, int animationFrameCount, int animationFrameTime, Vector2 scale)
        {
            f_position = position;
            m_Animation = new Animation();
            
            m_Animation.Initialize(picture, f_position.X, f_position.Y, 80, 80, animationFrameCount, animationFrameTime, true, scale);
            m_active = true;
            m_Animation.setAnimationActive(true);
        }

        public void Update(GameTime gameTime)
        {
            m_Animation.Update(gameTime, f_position.X, f_position.Y);
            if (m_active)
                m_Animation.setAnimationActive(true);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_Animation.Draw(spriteBatch, SpriteEffects.None);
        }

        public void moveLeft()
        {
            f_position.X -= 80;
        }

        public void moveRight()
        {
            f_position.X += 80;
        }

        public Vector2 getPosition()
        {
            return f_position;
        }
    }
}

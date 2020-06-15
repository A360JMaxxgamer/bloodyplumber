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

namespace BloodyPlumber
{
    public class Coin : IDisposable
    {
        [XmlElement]
        public bool m_active;
        [XmlElement]
        public Vector2 f_position;   
        public Animation m_Animation;
        private int m_speed;
        private bool m_collected;
        private int  m_collectedTime;
        private int m_points;
        public Player m_player;

        public void Initialize( Texture2D picture, int animationFrameCount, int animationFrameTime, Vector2 scale, Player player, int points)
        {
            m_Animation = new Animation();
            m_player = player;
            m_speed = 0;
            m_collected = false;
            m_points = points;
            m_Animation.Initialize(picture, f_position.X, f_position.Y, 1, 0, 80, 80, animationFrameCount, animationFrameTime, true, scale);
        }

        public void Update(GameTime gameTime)
        {
            if (f_position.X < UIConstants.m_initializePositionCoin)
                m_Animation.setAnimationActive(true);
            else
                m_Animation.setAnimationActive(false);

            if (m_player.getMoveFloor())
                f_position.X -= m_player.getPlayerSpeed();
            f_position.Y += m_speed;

            m_Animation.Update(gameTime, f_position.X, f_position.Y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            m_Animation.Draw(spriteBatch, SpriteEffects.None);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.SuppressFinalize(m_Animation);
        }

        public void collected(GameTime gameTime)
        {
            m_speed = -5;
            m_Animation.setAlphaReducing(0.05f);
            m_collected = true;
            m_collectedTime = gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool isCollectd()
        {
            return m_collected;
        }

        public bool canBeDeleted(GameTime gameTime)
        {
            if (m_collected)
                if (m_collectedTime + UIConstants.m_coinMoveUp < gameTime.ElapsedGameTime.Milliseconds)
                    return true;
                else
                    return false;
            else
                return false;
        }

        public int getPoints()
        {
            return m_points;
        }

    }
}

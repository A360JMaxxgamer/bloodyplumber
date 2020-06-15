using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace BloodyPlumber
{
    class ButtonsDraw : IDisposable
    {
        private Texture2D m_left, m_right, m_shoot, m_jump;             //Texturen der Knöpfe
        private Vector2 f_left, f_right, f_shoot, f_jump;               //Positionen der Knöpfe
        

        public void Initialize(Texture2D left, Texture2D right, Texture2D shoot, Texture2D jump, Vector2 leftV, Vector2 rightV, Vector2 shootV, Vector2 jumpV)
        {
            m_left = left;
            m_right = right;
            m_shoot = shoot;
            m_jump = jump;
            f_left = leftV;
            f_right = rightV;
            f_shoot = shootV;
            f_jump = jumpV;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_left, f_left, Color.White);
            spriteBatch.Draw(m_right, f_right, Color.White);
            spriteBatch.Draw(m_shoot, f_shoot, Color.White);
            spriteBatch.Draw(m_jump, f_jump, Color.White);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }


}

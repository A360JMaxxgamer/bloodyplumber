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

namespace BloodyPlumberLevelEditor
{
    class StatusScreen
    {
        Vector2 f_position;
        SpriteFont m_font;

        public void Initialize(Vector2 position, SpriteFont font)
        {
            f_position = position;
            m_font = font;
        }

        public void Draw(SpriteBatch spriteBatch, String text)
        { 
                spriteBatch.DrawString(m_font,text, f_position, Color.White);
        }
    }
}

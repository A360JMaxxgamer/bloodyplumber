using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BloodyPlumber
{
    public class BouncingText
    {
        private ScreenManager m_manager;
        private List<BouncingCharacter> m_characters;
        private Vector2 f_position;
        private int m_max;
        private int m_min;
        private int m_velocity;

        public BouncingText(String text,Vector2 position, int min, int max, int velocity, ScreenManager manager)
        {
            m_manager = manager;
            m_characters = new List<BouncingCharacter>();
            f_position = position;
            m_max = max;
            m_min = min;
            m_velocity = velocity;

            fillCharacters(text);
        }

        public void Update()
        {
            foreach (BouncingCharacter b in m_characters)
                b.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BouncingCharacter b in m_characters)
                b.Draw(spriteBatch, m_manager.Font);
        }

        public void newText(String text)
        {
            m_characters.Clear();
            fillCharacters(text);
        }

        public void setColor(Color c)
        {
            foreach (BouncingCharacter b in m_characters)
                b.setColor(c);
        }

        private void fillCharacters(String t)
        {
            char[] character = t.ToCharArray();
            Random r = new Random();
            for (int i = 0; i < character.Length; i++)
            {
               if(!character.ElementAt(i).Equals(" "))
               {
                   int vel = 0;
                   while (vel == 0)
                   {
                       vel = r.Next(-5, 5);
                   }

                   BouncingCharacter b = new BouncingCharacter(f_position + new Vector2(i * 80, 0), character.ElementAt(i).ToString(), m_max, m_min, vel);
                   m_characters.Add(b);
               }
            }
        }
    }
}

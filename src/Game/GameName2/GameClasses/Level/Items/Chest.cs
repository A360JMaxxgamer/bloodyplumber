using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyPlumber
{
    public class Chest
    {
        private IPowerUps m_currentItem;    //das IPowerUp welches in der Truhe liegt und aktiviert wird wenn man sie einsammelt
        private int m_x, m_y;               //repräsentiert den x und y des Quadrates wo die Truhe plaziert ist

        public Chest(IPowerUps item, int x, int y)
        {
            m_currentItem = item;
            m_x = x;
            m_y = y;
        }

        public int getX()
        {
            return m_x;
        }

        public int getY()
        {
            return m_y;
        }

        public void collected(Player player)
        {
            m_currentItem.picked(player);
        }
    }
}

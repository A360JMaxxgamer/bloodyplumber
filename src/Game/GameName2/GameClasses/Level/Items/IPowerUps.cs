using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BloodyPlumber
{
    public interface IPowerUps
    {
        void picked(Player p);

        IPowerUps clone();
    }
}

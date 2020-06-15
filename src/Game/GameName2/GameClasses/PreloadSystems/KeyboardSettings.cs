using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Input;

using System.Threading.Tasks;
using System.Diagnostics;

namespace BloodyPlumber
{
    public class KeyboardSettings
    {
        public Keys left, right, jump, shoot;

        public KeyboardSettings()
        {
            left = Keys.A;
            right = Keys.D;
            jump = Keys.J;
            shoot = Keys.K;
        }
    }
}

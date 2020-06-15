using System;

namespace BloodyPlumber
{
    
    static class Program
    {
        [STAThread]
        static void Main()
        
        {
            using (var game = new BloodyPlumber())
                game.Run();
        }
    }
}

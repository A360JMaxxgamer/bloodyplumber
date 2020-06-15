using System;

namespace BloodyPlumber
{
    
    static class Program
    {
        //Einstiegspunkt des Programms
        static void Main(string[] args)
        
        {
           var factory = new MonoGame.Framework.GameFrameworkViewSource<BloodyPlumber>();
           Windows.ApplicationModel.Core.CoreApplication.Run(factory);

            
            
        }
    }
}

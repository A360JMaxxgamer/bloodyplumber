#region File Description

#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
#endregion

//Klasse um mehrere Spieler einzubinden
namespace BloodyPlumber
{
   
    class PlayerIndexEventArgs : EventArgs
    {
       
        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }


        
        public PlayerIndex PlayerIndex
        {
            get { return playerIndex; }
        }

        PlayerIndex playerIndex;
    }
}

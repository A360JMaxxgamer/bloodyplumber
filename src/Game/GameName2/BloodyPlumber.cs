using System;
using System.Windows;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;


/* Hier ist der Einstiegspunkt für das Spiel
- Der ScreenManager wird gestartet und ein GraphiscManager
- Die Root Directory für unser Content wird gelinkt
- Die Framerate wird initialisiert
- Es werden die nötigen Componenten zum screenManager hinzugefügt
 */

namespace BloodyPlumber
{
    public class BloodyPlumber : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager m_graphics;
        ScreenManager screenManager;

        public BloodyPlumber()
        {

            m_graphics = new GraphicsDeviceManager(this);
            screenManager = new ScreenManager(this, 1920, 1080);
            Content.RootDirectory = "Assets";


            //Frame rate des Spiels 30fps
            TargetElapsedTime = TimeSpan.FromSeconds(1 / 30.0);

            Components.Add(screenManager);

            //neue screens hinzufügen
            BackgroundScreen bscreen = new BackgroundScreen(screenManager);
            screenManager.AddScreen(bscreen,null);
            screenManager.AddScreen(new MainMenuScreen(screenManager, bscreen,false),null);
 
        }

        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            
            
            base.LoadContent();
        }
    }
}



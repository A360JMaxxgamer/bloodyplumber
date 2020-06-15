// Diese Klasse zeigt die Menüeinträge an und verarbeitet die eingaben

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
#endregion

namespace BloodyPlumber
{
    class OptionsScreen : MenuScreen
    {
        ScreenManager screenManager;

        MenuEntry touchOptionMenuEntry;
        MenuEntry leftControlOptionEntry;
        MenuEntry rightControlOptionEntry;
        MenuEntry jumpControlOptionEntry;
        MenuEntry shootControlOptionEntry;
        BackgroundScreen bscreen;

        bool touchState;

        public OptionsScreen(ScreenManager manager,bool touch, BackgroundScreen b)
            : base(String.Empty)
        {
            screenManager = manager;
            IsPopup = true;

            touchState = touch;

            // Erstellen der menüeinträge
            touchOptionMenuEntry = new MenuEntry("Touch Disable");
            leftControlOptionEntry = new MenuEntry("Moving left("+ screenManager.keys.left.ToString()+")");
            rightControlOptionEntry = new MenuEntry("Movint right("+ screenManager.keys.right.ToString()+")");
            jumpControlOptionEntry = new MenuEntry("Jump("+ screenManager.keys.jump.ToString()+")");
            shootControlOptionEntry = new MenuEntry("Shoot("+ screenManager.keys.shoot.ToString()+")");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            bscreen = b;
            
            // menu eventhandler werden angelegt
            touchOptionMenuEntry.Selected += TouchOptionMenuEntrySelected;
            leftControlOptionEntry.Selected += MovingLeftControlSelected;
            rightControlOptionEntry.Selected += MovingRightControlSelected;
            jumpControlOptionEntry.Selected += JumpControlSelected;
            shootControlOptionEntry.Selected += ShootControlSelected;
            exitMenuEntry.Selected += OnCancel;
            


            // Einträge werden zu liste hinzugefügt
            MenuEntries.Add(touchOptionMenuEntry);
            MenuEntries.Add(leftControlOptionEntry);
            MenuEntries.Add(rightControlOptionEntry);
            MenuEntries.Add(jumpControlOptionEntry);
            MenuEntries.Add(shootControlOptionEntry);
            MenuEntries.Add(exitMenuEntry);

            if (touchState == false)
            {
                touchOptionMenuEntry.setText("Touch Disabled");
            }
            else
            {
                touchOptionMenuEntry.setText("Touch Enabled");               
            }


        }

        protected override void UpdateMenuEntryLocations()
        {
            base.UpdateMenuEntryLocations();

            foreach (var entry in MenuEntries)
            {
                Vector2 position = entry.Position;

                position.Y += 60;

                entry.Position = position;
            }
        }      

        void TouchOptionMenuEntrySelected(object sender, EventArgs e)
        {
            

            if (touchState == false)
            {
                touchOptionMenuEntry.setText("Touch Enabled");
                touchState = true;
                
            }
            else 
            {
                touchOptionMenuEntry.setText("Touch Disabled");
                touchState = false;
            }

        }

        void MovingLeftControlSelected(object sender, EventArgs e)
        {
            bscreen.titleText.newText("Push a button");
            ExitScreen();
            screenManager.AddScreen(new KeySelectScreen(screenManager, touchState, bscreen, "left"), null);
        }

        void MovingRightControlSelected(object sender, EventArgs e)
        {
            bscreen.titleText.newText("Push a button");
            ExitScreen();
            screenManager.AddScreen(new KeySelectScreen(screenManager, touchState, bscreen, "right"), null);
        }

        void JumpControlSelected(object sender, EventArgs e)
        {
            bscreen.titleText.newText("Push a button");
            ExitScreen();
            screenManager.AddScreen(new KeySelectScreen(screenManager, touchState, bscreen, "jump"), null);
        }

        void ShootControlSelected(object sender, EventArgs e)
        {
            bscreen.titleText.newText("Push a button");
            ExitScreen();
            screenManager.AddScreen(new KeySelectScreen(screenManager, touchState, bscreen, "shoot"), null);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            bscreen.titleText.newText("Bloody Plumber");
            ExitScreen();
            screenManager.AddScreen(new MainMenuScreen(screenManager,bscreen,touchState),null);

        }
    }
}

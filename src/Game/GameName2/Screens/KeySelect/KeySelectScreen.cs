using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;

namespace BloodyPlumber
{
    class KeySelectScreen : MenuScreen
    {
        ScreenManager screenManager;
        BackgroundScreen bscreen;
        String actionCommand;
        KeyboardState m_currentState;
        bool touchstate;
        bool getBack;
        public KeySelectScreen(ScreenManager manager, bool touch, BackgroundScreen b, String actionToChoose)
            : base(String.Empty)
        {
            m_currentState = new KeyboardState();
            m_currentState = Keyboard.GetState();
            screenManager = manager;
            IsPopup = true;
            touchstate = touch;
            bscreen = b;
            actionCommand = actionToChoose;
            getBack = false;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            if (Keyboard.GetState().GetPressedKeys().Count() == 1 && m_currentState != Keyboard.GetState() )
            {
                if (actionCommand == "left")
                    screenManager.keys.left = Keyboard.GetState().GetPressedKeys().ElementAt(0);
                if (actionCommand == "right")
                    screenManager.keys.right = Keyboard.GetState().GetPressedKeys().ElementAt(0);
                if (actionCommand == "jump")
                    screenManager.keys.jump = Keyboard.GetState().GetPressedKeys().ElementAt(0);
                if (actionCommand == "shoot")
                    screenManager.keys.shoot = Keyboard.GetState().GetPressedKeys().ElementAt(0);
                if (!getBack)
                {
                    OnCancel();
                    getBack = true;
                }
            }
        }

        protected void OnCancel()
        {
            bscreen.titleText.newText("Options");
            ExitScreen();
            screenManager.AddScreen(new OptionsScreen(screenManager, touchstate, bscreen), null);
        }
    }
}

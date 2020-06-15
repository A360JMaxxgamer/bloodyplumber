using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;


namespace BloodyPlumber
{
    class GameOverScreen : GameScreen
    {

        ScreenManager screenManager;
        Helper helper;
        List<TouchLocation> listOfTouchLocations;
        bool b_touchState;
        int myTick;

        Boogeyman boogey;

        public GameOverScreen(ScreenManager manager,Helper h,bool b)
        {
            screenManager = manager;
            helper = h;
            listOfTouchLocations = new List<TouchLocation>();
            b_touchState = b;
            myTick = 0;

            boogey = new Boogeyman();
            boogey.Initialize(screenManager.imageFileSystem.boogeyMan, 0- screenManager.imageFileSystem.boogeyMan.Width/2, screenManager.Viewport.Height / 2, screenManager);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (myTick == 25)
                screenManager.audioFileSystem.ninini.Play();
            myTick++;
            boogey.Update(gameTime);
            TouchPanel.EnabledGestures = GestureType.None;
            helper.updateInput(listOfTouchLocations);
            if (myTick == 150)
            {
                BackgroundScreen bscreen = new BackgroundScreen(screenManager);
                screenManager.AddScreen(bscreen, null);
                screenManager.AddScreen(new MainMenuScreen(screenManager,  bscreen,b_touchState), null);
                boogey.Dispose();
                ExitScreen();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            screenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, ScreenManager.Scale);

            screenManager.SpriteBatch.DrawString(screenManager.Font, "Gameover", new Vector2(UIConstants.screenWidth / 2, UIConstants.screenHeight / 2), Color.White);

            boogey.Draw(screenManager.SpriteBatch);
            screenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}

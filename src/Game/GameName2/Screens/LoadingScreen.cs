using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BloodyPlumber
{
    class LoadingScreen : GameScreen
    {


        bool loadingIsSlow;
        bool otherScreensAreGone;

        GameScreen[] screensToLoad;



        Flyinghead head;

        BouncingText text;


        /// <summary>
        ///Privater Konstruktur, Loadingscreen wird über die Methode Load aufgerufen
        /// </summary>
        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad, BouncingText bouncingText, Flyinghead flyinghead)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);

            TransitionOnTime = TimeSpan.FromSeconds(0.5);



            head = flyinghead;
            head.startInLoadingScreen();
            text = bouncingText;
        }


        /// <summary>
        /// Aktiviert den Loadingscreen
        /// </summary>
        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                PlayerIndex? controllingPlayer, Flyinghead head, BouncingText text,
                                params GameScreen[] screensToLoad)
        {
            // Alle bisher aktiven gamescreens werden deaktiviert
            foreach (GameScreen screen in screenManager.GetScreens())
                screen.ExitScreen();

            // erstellt einen aktiven loadingscreen
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                                            loadingIsSlow,
                                                            screensToLoad, text, head);

            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }





        /// <summary>
        /// Aktualisiert den Loadingscreen
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            text.Update();
            head.Update();
            // Wenn alle gamescreen deaktiviert sind beginnt das laden
            if (otherScreensAreGone)
            {
                ScreenManager.RemoveScreen(this);

                foreach (GameScreen screen in screensToLoad)
                {
                    if (screen != null)
                    {
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                        ExitScreen();
                    }
                }

                // Gametimer wird zurückgesetzt
                ScreenManager.Game.ResetElapsedTime();
            }
        }


        /// <summary>
        /// Zeichnet den Loadingscreen
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            
            if ((ScreenState == ScreenState.Active) &&
                (ScreenManager.GetScreens().Length == 1))
            {
                otherScreensAreGone = true;
            }

            if (loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

                // Zeichnen des Textes
                spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, ScreenManager.Scale);
                head.Draw(spriteBatch);
                text.Draw(spriteBatch);
                spriteBatch.End();
            }

        }

   

    }
}

// Diese Klasse zeigt die Menüeinträge an und verarbeitet die eingaben

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
#endregion

namespace BloodyPlumber
{
    class MainMenuScreen : MenuScreen
    {
        ScreenManager screenManager;
        bool touchState;
        public BackgroundScreen bscreen;
        private Flyinghead head;
        private BouncingText text;

        #region Initialization
        public MainMenuScreen(ScreenManager manager,BackgroundScreen b, bool touch)
            : base(String.Empty)
        {
            screenManager = manager;
            touchState = touch;
            screenManager.Game.IsMouseVisible = true;
            IsPopup = true;

            // Erstellen der menüeinträge
            MenuEntry startGameMenuEntry = new MenuEntry("Play");
            MenuEntry optionGameMenuEntry = new MenuEntry("Options");
            MenuEntry highscoreGameMenu = new MenuEntry("Highscores");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            

            // menu eventhandler werden angelegt
            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            optionGameMenuEntry.Selected += OptionGameMenuSelected;
            highscoreGameMenu.Selected += HighscoreGameMenuSelected;
            exitMenuEntry.Selected += OnCancel;

            // Einträge werden zu liste hinzugefügt
            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(optionGameMenuEntry);
            MenuEntries.Add(highscoreGameMenu);
            MenuEntries.Add(exitMenuEntry);

            bscreen = b;
            head = new Flyinghead(screenManager);

            text = new BouncingText("Loading", new Vector2(750,500), 450,550, 5, screenManager);
        }
        #endregion

        #region Overrides
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
        #endregion

        #region Event Handlers for Menu Items
        
        void StartGameMenuEntrySelected(object sender, EventArgs e)
        {
            //ScreenManager.AddScreen(new MainMapScreen(screenManager,touchState), null); falls weg über karte

            screenManager.Game.IsMouseVisible = false;
            LoadingScreen.Load(screenManager, true, null, head, text, new GameplayScreen(screenManager, 1,touchState,head,text));
            screenManager.audioFileSystem.stopMarioSounds();
            screenManager.audioFileSystem.s_letsgo.Play();
            ExitScreen();

        }

        public void OptionGameMenuSelected(object sender, EventArgs e)
        {
            screenManager.audioFileSystem.stopMarioSounds();
            screenManager.audioFileSystem.s_options.Play();
            bscreen.titleText.newText("Options");
            ScreenManager.AddScreen(new OptionsScreen(screenManager,touchState, bscreen), null);
            ExitScreen();
            
        }

        void HighscoreGameMenuSelected(object sender, EventArgs e)
        {
            screenManager.audioFileSystem.stopMarioSounds();
            screenManager.audioFileSystem.s_highscore.Play();
            bscreen.titleText.newText("Highscore");
            ExitScreen();
            screenManager.AddScreen(new Highscore(screenManager,bscreen,touchState), null);
           
        }


        protected override void OnCancel(PlayerIndex playerIndex)
        {
            ScreenManager.Game.Exit();
        }

        #endregion
    }
}

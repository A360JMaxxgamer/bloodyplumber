

#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
#endregion

namespace BloodyPlumber
{
    /// <summary>
    /// Basisklasse für alle Menuklassen
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        // anzahl pixel über und unter menü eintrag zum ausfüllen
        const int menuEntryPadding = 35;

        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        string menuTitle;

        #endregion

        #region Properties


        
        protected IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        #endregion

        #region Initialization


        
        public MenuScreen(string menuTitle)
        {
         
            EnabledGestures = GestureType.Tap;

            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        #endregion

        #region Handle Input

        
        protected virtual Rectangle GetMenuEntryHitBounds(MenuEntry entry)
        {
            
            return new Rectangle(
                0,
                (int)entry.Position.Y - menuEntryPadding,
                ScreenManager.Viewport.Width,
                entry.GetHeight(this) + (menuEntryPadding * 2));
        }

        
        
        public override void HandleInput(InputState input)
        {
            
            PlayerIndex player;
            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                OnCancel(player);
            }

            if (input.IsMenuDown(ControllingPlayer))
            {
                if (selectedEntry < menuEntries.Count - 1)
                    selectedEntry += 1;
            }

            if (input.IsMenuUp(ControllingPlayer))
            {
                if (selectedEntry > 0)
                    selectedEntry -= 1;
            }

            if (input.IsMenuSelect(ControllingPlayer, out player))
            {
                if(menuEntries.Count>0)
                 menuEntries[selectedEntry].OnSelectEntry(player);
            }

            if (input.MouseGesture.HasFlag(MouseGestureType.LeftClick))
            {

                Point clickLocation = new Point((int)input.CurrentMousePosition.X, (int)input.CurrentMousePosition.Y);
               
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = menuEntries[i];

                    if (GetMenuEntryHitBounds(menuEntry).Contains(clickLocation))
                    {
                        
                        OnSelectEntry(i, PlayerIndex.One);
                    }
                }
            }

            if (input.MouseGesture.HasFlag(MouseGestureType.Move))
            {

                Point clickLocation = new Point((int)input.CurrentMousePosition.X, (int)input.CurrentMousePosition.Y);
                
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = menuEntries[i];

                    if (GetMenuEntryHitBounds(menuEntry).Contains(clickLocation))
                    {
                        
                        selectedEntry = i;
                    }
                }
            }

            
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    
                    Point tapLocation = new Point((int)gesture.Position.X, (int)gesture.Position.Y);

                    
                    for (int i = 0; i < menuEntries.Count; i++)
                    {
                        MenuEntry menuEntry = menuEntries[i];

                        if (GetMenuEntryHitBounds(menuEntry).Contains(tapLocation))
                        {
                            
                            OnSelectEntry(i, PlayerIndex.One);
                        }
                    }
                }
            }
        }


        
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }


        
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw


       
        protected virtual void UpdateMenuEntryLocations()
        {
            
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // wo sollen die menüeinträge hingezeichnet werden
            Vector2 position = new Vector2(0f, ScreenManager.Viewport.Height/4.0f);

            // aktualisierend er menüeinträge
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // horizontale zentrierungg
                position.X = ScreenManager.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                
                menuEntry.Position = position;

                // neuer eintrag ist die größe des eintrags plus padding
                position.Y += menuEntry.GetHeight(this) + (menuEntryPadding * 2);
            }
        }

        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);
                
                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        

        
        public override void Draw(GameTime gameTime)
        {
            
            UpdateMenuEntryLocations();

            ;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, ScreenManager.Scale);

           
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Zeichnet den Menü Title zentriert
            Vector2 titlePosition = new Vector2(ScreenManager.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion
    }
}

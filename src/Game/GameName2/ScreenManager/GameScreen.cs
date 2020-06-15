

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;
#endregion

namespace BloodyPlumber
{
    //beschreibt in welchem zustand sich der gamescreen befindet
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }


    /// <summary>
    ///Ein Gamescreen entählt die Spiellogik mit der Interaktionsschleifen Update Draw
    /// </summary>
    public abstract class GameScreen : IDisposable
    {
        #region Properties


        /// <summary>
        /// Falls dieser wert true ist handelt es sich um einen Gamescreen der ein Popup ist
        /// In diesem Fall verschwindet der darunter liegende Screen nicht
        /// </summary>
        public bool IsPopup
        {
            get { return isPopup; }
            protected set { isPopup = value; }
        }

        bool isPopup = false;


        /// <summary>
        /// Wie lange es dauert bist der screen überblendet
        /// </summary>
        public TimeSpan TransitionOnTime
        {
            get { return transitionOnTime; }
            protected set { transitionOnTime = value; }
        }

        TimeSpan transitionOnTime = TimeSpan.Zero;


        /// <summary>
        /// wie lange es dauert bis der screen ausgeblendet wird
        /// </summary>
        public TimeSpan TransitionOffTime
        {
            get { return transitionOffTime; }
            protected set { transitionOffTime = value; }
        }

        TimeSpan transitionOffTime = TimeSpan.Zero;


        /// <summary>
        /// aktuelle position der überblendung
        /// </summary>
        public float TransitionPosition
        {
            get { return transitionPosition; }
            protected set { transitionPosition = value; }
        }

        float transitionPosition = 1;


        /// <summary>
        /// alpha wert der überblendung
        /// </summary>
        public float TransitionAlpha
        {
            get { return 1f - TransitionPosition; }
        }


        /// <summary>
        /// Welcher status
        /// </summary>
        public ScreenState ScreenState
        {
            get { return screenState; }
            protected set { screenState = value; }
        }

        ScreenState screenState = ScreenState.TransitionOn;


        /// <summary>
        /// Es gibt zwei gründe warum ein screen verschwindet
        /// Entweder er geht in den Hintergrund oder er soll geschlossen werden
        /// Wenn dieser Wert true ist, wird der screen nach der überblendung geschlossen
        /// </summary>
        public bool IsExiting
        {
            get { return isExiting; }
            protected internal set { isExiting = value; }
        }

        bool isExiting = false;

        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }


        bool otherScreenHasFocus;

        public ScreenManager ScreenManager
        {
            get { return screenManager; }
            internal set { screenManager = value; }
        }

        ScreenManager screenManager;

        /// <summary>
        /// Falls mehrere Spieler vorhanden sind, kontrollieren wir hier
        /// welcher welchen screen bedienen kann
        /// </summary>
        public PlayerIndex? ControllingPlayer
        {
            get { return controllingPlayer; }
            internal set { controllingPlayer = value; }
        }

        PlayerIndex? controllingPlayer;

        /// <summary>
        /// verarbeitet die gesten
        /// </summary>
        public GestureType EnabledGestures
        {
            get { return enabledGestures; }
            protected set
            {
                enabledGestures = value;

                
                if (ScreenState == ScreenState.Active)
                {
                    TouchPanel.EnabledGestures = value;
                }
            }
        }

        GestureType enabledGestures = GestureType.None;


        #endregion

        #region Initialization


        /// <summary>
        /// Läd den grafischen inhalt
        /// </summary>
        public virtual void LoadContent() { }


        /// <summary>
        /// löscht den grafischen inhalt
        /// </summary>
        public virtual void UnloadContent() { }


        #endregion

        #region Update and Draw


        
        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if (isExiting)
            {
                
                screenState = ScreenState.TransitionOff;

                if (!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if (coveredByOtherScreen)
            {
            
                if (UpdateTransition(gameTime, transitionOffTime, 1))
                {
              
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
               
                if (UpdateTransition(gameTime, transitionOnTime, -1))
                {
                   
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    
                    screenState = ScreenState.Active;
                }
            }
        }


        /// <summary>
        /// hilft bei die überblendung
        /// </summary>
        bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            
            float transitionDelta;

            if (time == TimeSpan.Zero)
                transitionDelta = 1;
            else
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);

          
            transitionPosition += transitionDelta * direction;

           
            if (((direction < 0) && (transitionPosition <= 0)) ||
                ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }

            
            return true;
        }


      
        public virtual void HandleInput(InputState input) { }


       
        public virtual void Draw(GameTime gameTime) { }


        #endregion

        #region Public Methods

        public void ExitScreen()
        {
            if (TransitionOffTime == TimeSpan.Zero)
            {
                
                ScreenManager.RemoveScreen(this);
            }
            else
            {
               
                isExiting = true;
            }
            Dispose();
            GC.Collect();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        #endregion

        #region Helper Methods
        /// <summary>
        /// Läd den Content
        /// </summary>
        /// <typeparam name="T">Art des Contents.</typeparam>
        /// <param name="assetName">Name des Contents ohne die .xnb endung.</param>
        /// <returns></returns>
        public T Load<T>(string assetName)
        {
            return ScreenManager.Game.Content.Load<T>(assetName);
        }
        #endregion
    }
}

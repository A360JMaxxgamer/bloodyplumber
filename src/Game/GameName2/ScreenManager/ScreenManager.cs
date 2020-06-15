#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System.IO;
//using System.IO.IsolatedStorage;
#endregion

namespace BloodyPlumber
{
    /// <summary>
    /// Der screenmanager verwaltet mehrere screens
    /// </summary>
    public class ScreenManager : DrawableGameComponent
    {
        #region Fields
        
        List<GameScreen> screens = new List<GameScreen>();
        List<GameScreen> screensToUpdate = new List<GameScreen>();

        public bool endBossActive = false;
        InputState input = new InputState();

        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D blankTexture;

        public KeyboardSettings keys;

        public AudioFiles audioFileSystem;

        public ImageFiles imageFileSystem;

        public PowerupSystem powerupSystem;

        bool isInitialized;

        bool traceEnabled;

        private int virtualWidth;
        private int virtualHeight;
        private bool updateMatrix = true;
        private Matrix scaleMatrix = Matrix.Identity;
        private GraphicsDeviceManager DeviceManager;

        #endregion

        #region Properties


        /// <summary>
        /// Standart spritebatch der von alleen screens verwendet wird
        /// </summary>
        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }


        /// <summary>
        /// Standart font der von allen menü einträgen genutzt werden kann
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
        }


        /// <summary>
        /// Wenn true können alle screens ausgegeben werden
        /// </summary>
        public bool TraceEnabled
        {
            get { return traceEnabled; }
            set { traceEnabled = value; }
        }

        #endregion

        #region Skalierung

        public Viewport Viewport
        {
            get
            {
                return new Viewport(0, 0, virtualWidth, virtualHeight);
            }
        }

        public Vector2 InputTranslate
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
            }
        }

        public Matrix Scale
        {
            get
            {
                if (updateMatrix)
                {
                    CreateScaleMatrix();
                    updateMatrix = false;
                }
                return scaleMatrix;
            }
        }

        public Matrix InputScale
        {
            get
            {
                return Matrix.Invert(Scale);
            }
        }
        #endregion



        #region Initialization


        /// <summary>
        /// Konstruktor für einen neue screenmanager
        /// </summary>
        public ScreenManager(Game game, int virtualWidth, int virtualHeight)
            : base(game)
        {
            audioFileSystem = new AudioFiles();
            audioFileSystem.Initialize();

            imageFileSystem = new ImageFiles();
            imageFileSystem.Initialize();

            keys = new KeyboardSettings();


            powerupSystem = new PowerupSystem();
            powerupSystem.Initialize(this);
            // variablen für die virtuelle umgebung
            this.virtualHeight = virtualHeight;
            this.virtualWidth = virtualWidth;

            this.DeviceManager = (GraphicsDeviceManager)game.Services.GetService(typeof(IGraphicsDeviceManager));

            //gesten werden aktiviert
            TouchPanel.EnabledGestures = GestureType.None;
            
            input.ScreenManager = this;
        }


        /// <summary>
        /// initialisiert den screenmanager
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            isInitialized = true;
        }


        /// <summary>
        /// Läd Grafische komponenten
        /// </summary>
        protected override void LoadContent()
        {
           
            ContentManager content = Game.Content;
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("SpriteFont1");

            audioFileSystem.LoadContent(this);
            imageFileSystem.LoadContent(this);
            powerupSystem.LoadContent();
        
            foreach (GameScreen screen in screens)
            {
                screen.LoadContent();
            }
        }

        protected override void UnloadContent()
        {
          
            foreach (GameScreen screen in screens)
            {
                screen.UnloadContent();
            }
        }

        #region helper für skalierung
        protected void CreateScaleMatrix()
        {
            scaleMatrix = Matrix.CreateScale(
                           (float)GraphicsDevice.Viewport.Width / virtualWidth,
                           (float)GraphicsDevice.Viewport.Height / virtualHeight,
                           1f);
        }

        protected void FullViewport()
        {
            Viewport vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = DeviceManager.PreferredBackBufferWidth;
            vp.Height = DeviceManager.PreferredBackBufferHeight;
            GraphicsDevice.Viewport = vp;
        }

        protected float GetVirtualAspectRatio()
        {
            return (float)virtualWidth / (float)virtualHeight;
        }

        protected void ResetViewport()
        {
            float targetAspectRatio = GetVirtualAspectRatio();
       
            int width = DeviceManager.PreferredBackBufferWidth;
            int height = (int)(width / targetAspectRatio + .5f);
            bool changed = false;

            if (height > DeviceManager.PreferredBackBufferHeight)
            {
                height = DeviceManager.PreferredBackBufferHeight;
     
                width = (int)(height * targetAspectRatio + .5f);
                changed = true;
            }

          
            Viewport viewport = new Viewport();

            viewport.X = (DeviceManager.PreferredBackBufferWidth / 2) - (width / 2);
            viewport.Y = (DeviceManager.PreferredBackBufferHeight / 2) - (height / 2);
            viewport.Width = width;
            viewport.Height = height;
            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (changed)
            {
                updateMatrix = true;
            }

            DeviceManager.GraphicsDevice.Viewport = viewport;
        }

        protected void BeginDraw()
        {
        
            FullViewport();

            GraphicsDevice.Clear(Color.Black);
    
            ResetViewport();
           
            GraphicsDevice.Clear(Color.Black);
        }
        #endregion

        #endregion

        #region Update and Draw


        /// <summary>
        /// Jeder screen kann seine logic ausführen
        /// </summary>
        public override void Update(GameTime gameTime)
        {
          
            input.Update();

            
            screensToUpdate.Clear();

            foreach (GameScreen screen in screens)
                screensToUpdate.Add(screen);

            bool otherScreenHasFocus = !Game.IsActive;
            bool coveredByOtherScreen = false;

          
            while (screensToUpdate.Count > 0)
            {
            
                GameScreen screen = screensToUpdate[screensToUpdate.Count - 1];

                screensToUpdate.RemoveAt(screensToUpdate.Count - 1);

              
                screen.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

                if (screen.ScreenState == ScreenState.TransitionOn ||
                    screen.ScreenState == ScreenState.Active)
                {
                    
                    if (!otherScreenHasFocus)
                    {
                        screen.HandleInput(input);

                        otherScreenHasFocus = true;
                    }

               
                    if (!screen.IsPopup)
                        coveredByOtherScreen = true;
                }
            }

        
            if (traceEnabled)
                TraceScreens();

        }


        /// <summary>
        /// Zeigt eine liste aller screens
        /// </summary>
        void TraceScreens()
        {
            List<string> screenNames = new List<string>();

            foreach (GameScreen screen in screens)
                screenNames.Add(screen.GetType().Name);

        }

     
        /// <summary>
        /// jeder screen soll gezeichnet werden
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            BeginDraw();
            foreach (GameScreen screen in screens)
            {
                if (screen.ScreenState == ScreenState.Hidden)
                    continue;

                screen.Draw(gameTime);
            }
        }


        #endregion

        #region Public Methods


        /// <summary>
        /// Neuer screen wird zum screenmanager hinzugefügt
        /// </summary>
        public void AddScreen(GameScreen screen, PlayerIndex? controllingPlayer)
        {
            screen.ControllingPlayer = controllingPlayer;
            screen.ScreenManager = this;
            screen.IsExiting = false;

       
            if (isInitialized)
            {
                screen.LoadContent();
            }

            screens.Add(screen);


            TouchPanel.EnabledGestures = screen.EnabledGestures;
        }


        
        public void RemoveScreen(GameScreen screen)
        {
           
            if (isInitialized)
            {
                screen.UnloadContent();
            }

            screens.Remove(screen);
            screensToUpdate.Remove(screen);

          
            if (screens.Count > 0)
            {
                TouchPanel.EnabledGestures = screens[screens.Count - 1].EnabledGestures;
            }
        }


      
        public GameScreen[] GetScreens()
        {
            return screens.ToArray();
        }

        public String countScreens()
        {
            String s = "";
            s += "Anzahl Screens: " + screens.Count + "\n";

            for (int i = 0; i < screens.Count; i++)
            {
                s += screens[i].ToString() + "\n";
            }
            return s;

        }



        public void FadeBackBufferToBlack(float alpha)
        {
            Viewport viewport = GraphicsDevice.Viewport;

            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Scale);

            spriteBatch.Draw(blankTexture,
                             new Rectangle(0, 0, viewport.Width, viewport.Height),
                             Color.Black * alpha);

            spriteBatch.End();
        }


        #endregion
    }
}
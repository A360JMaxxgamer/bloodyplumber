using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Windows.Storage;
using System.IO;
using System.Text;
using System;
using System.Diagnostics;
using System.Collections.Generic;


namespace BloodyPlumberLevelEditor
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Level screen;
        StatusScreen statusScreen;
        Vector2 screenSolution;
        Vector2 rowsAndColumns;         //Beinhaltet die Zeilen und Spalten als x und y wert
        User user;
        Input userInput;
        Vector2 tileBound;              //Beinhaltet Tile Width und Height
        Texture2D tileSource;

        Texture2D pixel;
        Texture2D coin;

        List<Enemy> enemyList;
        Texture2D flyingBombStrip;
        Animation flyingBombAnimation;
        Texture2D turtleStrip;
        Animation turtleAnimation;
        Texture2D mushroomStrip;
        Animation mushroomStripAnimation;
        SpriteFont font;

        Texture2D[] textures;
        
        Toolbar myBar;
        Texture2D[] toolbartextures;

        int enemySpawnX, enemySpawnY;

        bool renderMiniMap;

        Texture2D minimap;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Assets";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsMouseVisible = true;
            screen = new Level();
            screenSolution = new Vector2();
            rowsAndColumns = new Vector2(24, 14);
            // TODO: Add your initialization logic here
            screenSolution.X = GraphicsDevice.Viewport.Width;
            screenSolution.Y = GraphicsDevice.Viewport.Height;
            statusScreen = new StatusScreen();
            user = new User();
            userInput = new Input();

            tileBound = new Vector2(80, 80);

            enemyList = new List<Enemy>();

            turtleAnimation = new Animation();
            flyingBombAnimation = new Animation();
            mushroomStripAnimation = new Animation();
            textures = new Texture2D[5];

            myBar = new Toolbar();
            toolbartextures = new Texture2D[6];

            enemySpawnX = 1000;
            enemySpawnY = 100;
 
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pixel = Content.Load<Texture2D>("pixelRed.png");
            coin = Content.Load<Texture2D>("Coin.png");

            toolbartextures[0] = Content.Load<Texture2D>("Toolbar\\toolbarBackground.png");
            toolbartextures[1] = Content.Load<Texture2D>("Toolbar\\SetTile.png");
            toolbartextures[2] = Content.Load<Texture2D>("Toolbar\\SetEnemy.png");
            toolbartextures[3] = Content.Load<Texture2D>("Toolbar\\Delete.png");
            toolbartextures[4] = Content.Load<Texture2D>("Toolbar\\SelectTile.png");
            toolbartextures[5] = Content.Load<Texture2D>("Toolbar\\SetCoin.png");

            myBar.Initialize(1920, 100, -2, 0, toolbartextures, 1920, 1080);


            textures[0] = pixel;
            textures[1] = coin;
            screen.Initialize(textures, (int)rowsAndColumns.X, (int)rowsAndColumns.Y, screenSolution);
            tileSource = Content.Load<Texture2D>("grassTileSet.png");

            turtleStrip =Content.Load<Texture2D>("turtleAnimation.png");
            turtleAnimation.Initialize(turtleStrip, enemySpawnX, enemySpawnY, 65, 90, 4, 100, true, new Vector2(1, 1));

            flyingBombStrip = Content.Load<Texture2D>("bomb.png");
            flyingBombAnimation.Initialize(flyingBombStrip, enemySpawnX, enemySpawnY, 180, 100, 4, 100, true, new Vector2(1, 1));

            mushroomStrip = Content.Load<Texture2D>("mushroom_strip.png");
            mushroomStripAnimation.Initialize(mushroomStrip, enemySpawnX, enemySpawnY, 70, 70, 5, 100, true, new Vector2(1, 1));
            fillEnemyList();

            //font = Content.Load<SpriteFont>("retro");
            statusScreen.Initialize(new Vector2(10, 10), null);

            user.Initialize(tileSource, screen, (int)tileBound.X, (int)tileBound.Y, getTileScale(), enemyList);
            userInput.Initialize(user, screen,statusScreen, myBar);
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            renderMiniMap =userInput.Update(gameTime);
            screen.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if(renderMiniMap)
                RenderMiniMap(gameTime, _spriteBatch);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            DrawScreen(gameTime);
            _spriteBatch.End();
            
        }

        public void RenderMiniMap(GameTime gameTime, SpriteBatch spriteBatch)
        {
            RenderTarget2D renderTarget;
            renderTarget = new RenderTarget2D(GraphicsDevice,10000, 1080);
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.WhiteSmoke);


            spriteBatch.Begin();


            screen.Draw(_spriteBatch);

            minimap = (Texture2D)renderTarget;

            base.Draw(gameTime);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            screen.setMinimap(renderTarget);
            screen.setMiniMapActive(true);
            //renderTarget.Dispose();
            GraphicsDevice.Clear(Color.CornflowerBlue);
        }


        public void DrawScreen(GameTime gameTime)
        {


            screen.Draw(_spriteBatch);
            user.Draw(_spriteBatch);
            userInput.Draw(_spriteBatch);



        }
        public Vector2 getTileScale()
        {
            float gridTileWidth = GraphicsDevice.Viewport.Width / rowsAndColumns.X;

            return new Vector2(gridTileWidth / tileBound.X, gridTileWidth / tileBound.Y);
        }

        public void fillEnemyList()
        {
            //Enemy eins
            Enemy turtle = new Enemy();
            turtle.Initialize( enemySpawnX, enemySpawnY, 0, 0, 5, 1, turtleAnimation, 1920,1, false);

            Enemy flyingBomb = new Enemy();
            flyingBomb.Initialize(enemySpawnX, enemySpawnY, 0, 0, 5, 1, flyingBombAnimation, 1920, 2, false);

            Enemy mushroom = new Enemy();
            mushroom.Initialize(enemySpawnX, enemySpawnY, 0, 0, 5, 1, mushroomStripAnimation, 1920, 3, false);
            
            enemyList.Add(turtle);
            enemyList.Add(flyingBomb);
            enemyList.Add(mushroom);

        }


       
    }
}

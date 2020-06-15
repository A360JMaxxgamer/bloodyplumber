using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Windows.Storage;
using System.IO;
using System.Text;
using System;
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
        Vector2 screenSolution;
        Vector2 rowsAndColumns;         //Beinhaltet die Zeilen und Spalten als x und y wert
        User user;
        Input userInput;
        Vector2 tileBound;              //Beinhaltet Tile Width und Height
        Texture2D tileSource;

        Texture2D pixel;
        

        List<Enemy> enemyList;
        Texture2D turtleStrip;
        Animation turtleAnimation;
        

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

            user = new User();
            userInput = new Input();

            tileBound = new Vector2(80, 80);

            enemyList = new List<Enemy>();

            turtleAnimation = new Animation();

             
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
            screen.Initialize(pixel, (int)rowsAndColumns.X, (int)rowsAndColumns.Y, screenSolution);
            tileSource = Content.Load<Texture2D>("grassTileSet.png");

            
            turtleStrip =Content.Load<Texture2D>("turtle.png");
            turtleAnimation.Initialize(turtleStrip, 2000, 2000, 62, 50, 1, 100, true, new Vector2(1, 1));
            fillEnemyList();

            user.Initialize(tileSource, screen, (int)tileBound.X, (int)tileBound.Y, getTileScale(), enemyList);
            userInput.Initialize(user, screen);
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
            userInput.Update(gameTime);
            screen.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            screen.Draw(_spriteBatch);

            user.Draw(_spriteBatch);
            userInput.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
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

            turtle.Initialize( 2000, 2000, 0, 0, 5, 1, turtleAnimation, 1920,1);
            enemyList.Add(turtle);

        }
    }
}

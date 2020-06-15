using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Diagnostics;
using Windows.Storage;


namespace BloodyPlumber
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;

        #region Attributes
        Vector2 f_scaling;        // X Wert der die Bilder und Animationen skaliert
        //Player-Objekte
        #region Player
        private Player player;
        private Texture2D playerSpriteStrip;
        private Animation playerAnimation;
        private Input playerInput;
        #endregion

        //Level-Objekte
        #region Boden
        private Texture2D tileSource;           //Bild aus dem die Tiles bezogen werden
        private Floor floor;
        private int backGroundSpeed;            //Geschwindigkeit des Hintergrundes
        #endregion

        Tile testTile;

        #endregion
        //Weapon-Objekte
        Weapon startWeapon;
        Animation startWeaponAnimation;
        Texture2D projectileTest;
        Level testLevel;


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

            startWeapon = new Weapon();
            startWeaponAnimation = new Animation();
           

            #region Player
            playerAnimation = new Animation();
            playerInput = new Input();
            player = new Player();
            player.Initialize(200.0f, 200.0f, 0.0f, 0.0f, 5f, 5, startWeapon, playerAnimation, GraphicsDevice.Viewport.Width);
            playerInput.Initialize(player);
            

            #endregion
            #region
            floor = new Floor();
            backGroundSpeed = 3;
            #endregion

            testTile = new Tile();

            setScale();

            testLevel = new Level();
            testLevel.Initialize();
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
            // TODO: use this.Content to load your game content here
            playerSpriteStrip = Content.Load<Texture2D>("Animation\\mariobros320x160.png");
            playerAnimation.Initialize(playerSpriteStrip, player.getXPlayerPosition(), player.getYPlayerPosition(), 80, 120, 4, 100, true, f_scaling);
            playerAnimation.setAnimationActive(true);

            //Lädt das Tile-Set
            tileSource = Content.Load<Texture2D>("fulltileset.png");
            floor.Initialize(tileSource,f_scaling);

            testTile.Initialize(tileSource, 0, 0, 16, 16, 500, 550, f_scaling);
            floor.addTile(testTile);

            //Waffen und Projektil
            projectileTest = Content.Load<Texture2D>("projektil.png");
            startWeaponAnimation.Initialize(projectileTest, player.getXPlayerPosition(), player.getYPlayerPosition(), projectileTest.Width, projectileTest.Height, 1, 1000, true, f_scaling);
            startWeapon.Initialize(player.getXPlayerPosition(), player.getYPlayerPosition(), startWeaponAnimation, projectileTest);

            
 
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
            checkPlayerPosition();
            playerInput.Update(gameTime);

            player.Update(gameTime);
            if (player.getMoveFloor())
            {
                updateTiles(gameTime);
            }
            floorCollision();
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // TODO: Add your drawing code here
            player.Draw(_spriteBatch);              //Der Spieler Charakter wird gezeichnet
            floor.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }


        #region Helper

        /*Diese Methode überprüft, ob der Spieler mit einem Boden Element aus der Liste aus Floor kolidiert. 
         Dabei werden die Geschwindigkeit in Y-Richtung entsprechend verändert.
         Verbesserungen die notwendig sind:
         *- Kollisions Erkennung zwischen Boden, Rand und Oberfläche*/
        public void floorCollision()
        {
            bool intersect = false;
            for (int i = 0; i < floor.countTiles(); i++)
            {

                bool collision = player.getCollisionRectangle().Intersects(floor.getTile(i).getCollisionRectangle());       //Temporäre Variabel die überprüft ob eine Kollision stattfindet
                if(collision)
                {
                    intersect = true;
                    break;
                }
                
                if(collision)
                {
                    intersect = true;
                    break;
                } 
            }

            if ( intersect && !player.playerStartedJump())
            {
                player.setYPlayerVelocity(-(player.getPlayerSpeed()));
                player.setPlayerIsJumping(false);
            }
            if(!intersect && !player.playerIsJumping())
            {
                player.setYPlayerVelocity(0f);
            }
        }

        /*Die Methode überprüft ob der Spieler in der Mitte des Bildschirmes ist. Wenn ja, dann wird die Bewegung 
         * des Spielers in X-Richtung null gesetzt und der Hintergrund beginnt sich zu bewegen, um die Illusion
         * zu erzeugen das der Spieler sich weiterhin bewegt*/
        public void checkPlayerPosition()
        {
            if(player.getXPlayerPosition() >= ( GraphicsDevice.Viewport.Width/2))
            {
                player.setMoveFloor(true);
                for (int i = 0; i < floor.countTiles(); i++)
                    floor.getTile(i).setSpeed(-backGroundSpeed);
            }

            if(player.getXPlayerPosition() < 0)
            {
                player.setReachedLeftSide(true) ;
            }
        }
        /*Updatet jedes Teil
         * Verbesserung:
         * -Eventuell auslagern in Floor Klasse
         * */
        public void updateTiles(GameTime gameTime)
        {   
            for(int i = 0 ; i < floor.countTiles(); i++)
            {
                floor.getTile(i).Update(gameTime);
            }
        }

        public void setScale()
        {
            float tmpScale = (float)GraphicsDevice.Viewport.Width / 1920f;
            tmpScale += 0.5f;
            tmpScale = (int)tmpScale * 10;
            tmpScale = tmpScale / 10;
            f_scaling.X = tmpScale;

            tmpScale = (float)GraphicsDevice.Viewport.Height / 1080f;
            tmpScale += 0.5f;
            tmpScale = (int)tmpScale * 10;
            tmpScale = tmpScale / 10;
            f_scaling.Y = tmpScale;
        }
        
        #endregion

    }
}

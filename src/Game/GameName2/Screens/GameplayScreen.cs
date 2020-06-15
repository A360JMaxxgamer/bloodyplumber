// Hauptklasse des Spiels

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using ParticleEmitter;

namespace BloodyPlumber
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameplayScreen  : GameScreen
    {
        
        //Allgemeine Member
        int m_gameSpeed;
        Collision collision;
        ScreenManager screenManager;
        Helper helper;
        private Texture2D[] m_textures;
        
        //Audio
        private AudioFiles audioFileSystem;
        private ImageFiles imageFileSystem;
       

        //Player-Objekte

        private Player player;
        private Texture2D playerSpriteStrip;
        private PlayerAnimation playerAnimation;
        private Input playerInput;


        //Level-Objekte

        private Texture2D tileSource;           //Bild aus dem die Tiles bezogen werden
        private Level levelEins;
        int m_levelId;

        //Weapon-Objekte
        Weapon startWeapon;
        Animation startWeaponAnimation;
        

        //Background Objeket von 1(hinten) bis x(vorne)
        Background m_Background1;
        Background m_Background2;
        Background m_Background3;
        Background m_Background4;
        Background m_Background5;

        Texture2D t_Background1;
        Texture2D t_Background2;
        Texture2D t_Background2_2;
        Texture2D t_Background3;
        Texture2D t_Background3_3;
        Texture2D t_Background4;
        Texture2D t_Background4_4;
        
        //Input
        List<Rectangle> listOfRectangles;
        Rectangle r_forward,r_backwards,r_shoot;
        List<TouchLocation> listOfTouchLocations;
        TouchPanelCapabilities tc;
        bool b_touchState;

        //Inputtextures
       Texture2D left, right, shoot, jump;        

       
        
        //Soll über Helper Klasse mit den verschiedenen Enemy-Animationen befüllt werden um sie dann je nach Namen der Enemies zuzuweisen
        List<Animation> m_enemyAnimationList;

        //Buttons
        ButtonsDraw m_buttons;

        Highscore highscore;

        //GametTime
       private float myTime;

        //Sonstiges
       private bool deathApplied;
       private BouncingText bouncingText;
       private Flyinghead flyinghead;
       private bool victoryApplied;
       private bool bossThemeStarted;
       private int m_countdown;

        public GameplayScreen(ScreenManager manager, int id,bool touchState, Flyinghead head, BouncingText text)
        {
            //Allgemeine Initialisierungen
            screenManager = manager;
            screenManager.endBossActive = false;
            collision = new Collision();

            m_levelId = id;
            m_gameSpeed = 10;

            bouncingText = text;
            flyinghead = head;
            
            //Spielelemente
            startWeapon = new Weapon();
            player = new Player();
            playerInput = new Input();
            
            levelEins = new Level();
            m_enemyAnimationList = new List<Animation>();

            //Level
            m_Background1 = new Background();
            m_Background2 = new Background();
            m_Background3 = new Background();
            m_Background4 = new Background();
            m_Background5 = new Background();


            //Input
            listOfRectangles = new List<Rectangle>();
            r_backwards = new Rectangle(0,1080-160,80,160);
            r_forward = new Rectangle(90, 1080-160, 80, 160);
            r_shoot = new Rectangle(1600, 700, 320, 380);
            listOfTouchLocations = new List<TouchLocation>();
            tc = TouchPanel.GetCapabilities();
            b_touchState = touchState;
            helper = new Helper(screenManager);
            
            if(b_touchState == true)
            m_buttons = new ButtonsDraw();

            m_textures = new Texture2D[5];

            manager.audioFileSystem.menuTheme.Stop();
            audioFileSystem = screenManager.audioFileSystem;
            imageFileSystem = screenManager.imageFileSystem;
            
            
            deathApplied = false;
            screenManager.audioFileSystem.s_levelOneTheme.IsLooped = true;
            victoryApplied = false;
            bossThemeStarted = false;
            
            //Zählt runter bei null = Tod
            m_countdown = 300;
            
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public override void LoadContent()
        {
            
            m_textures[0] = screenManager.imageFileSystem.level1_TileStrip;
            m_textures[1] = screenManager.imageFileSystem.redCoin;

            
            //Leveldesign
            if (m_levelId == 1)
            {
                t_Background1 = screenManager.imageFileSystem.blue_background;
                t_Background2 = screenManager.imageFileSystem.mountain1;
                t_Background2_2 = screenManager.imageFileSystem.mountain2;
                t_Background3 = screenManager.imageFileSystem.cloud1;
                t_Background3_3 = screenManager.imageFileSystem.cloud2;
                t_Background4 = screenManager.imageFileSystem.tree1;
                t_Background4_4 = screenManager.imageFileSystem.tree2;


            }




            playerAnimation = screenManager.imageFileSystem.playerAnimation;
            playerAnimation.setAnimationActive(true);

            m_Background1.Initialize(t_Background1, t_Background1,Vector2.Zero, 0, player, false, true);
            m_Background2.Initialize(t_Background2,t_Background2_2, Vector2.Zero,2, player, false, true);
            m_Background3.Initialize(t_Background3,t_Background3_3, Vector2.Zero,3, player, true, true);
            m_Background4.Initialize(t_Background4,t_Background4_4,new  Vector2(0,320),5,player, false, true);
            m_Background5.Initialize(t_Background3, t_Background3_3, new Vector2(500, 0), 1, player, true, true);

            
            helper.LoadContent();
            player.Initialize(100.0f, 700.0f, 0.0f, 0.0f, m_gameSpeed, 1, playerAnimation, ScreenManager.Viewport.Width, screenManager);
            player.setStartWeapon(screenManager.powerupSystem.laser);

            levelEins = helper.LoadLevel(m_textures, helper.getTileScale(), player,m_levelId, screenManager.imageFileSystem.bloodAnimation, screenManager.imageFileSystem.mudAnimation);
        


            collision.Initialize(levelEins, player,screenManager.imageFileSystem.bloodAnimation , true, audioFileSystem.marioKillSounds);

            playerInput.Initialize(player, levelEins, screenManager);

            //Input
            listOfRectangles.Add(r_forward);
            listOfRectangles.Add(r_backwards);
            listOfRectangles.Add(r_shoot);

            //Buttons
            if (b_touchState == true)
            {
                left = screenManager.imageFileSystem.left;
                right = screenManager.imageFileSystem.right;
                shoot = screenManager.imageFileSystem.shoot;
                jump = screenManager.imageFileSystem.jump;


                m_buttons.Initialize(left, right, shoot, jump, new Vector2(35, 1000), new Vector2(185, 1000), new Vector2(1655, 1000), new Vector2(1815, 1000));
            }
            
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        public override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            if (myTime % 30 == 0)
                m_countdown--;

            gameOver();
            
            if(myTime == 59 && !deathApplied)
                screenManager.audioFileSystem.s_levelOneTheme.Play();


            if (myTime <= 59 && !deathApplied && !victoryApplied)
            {
                bouncingText.Update();
                flyinghead.Update();
            }

            if (LevelWon())
            {

                Highscore.PutHighScore("", player.getScore());
                Highscore.SaveHighscore();

            }
            else
            {
                myTime += 1.0f;
                if (!bossThemeStarted)
                    controlMusic();
                player.Update(gameTime);   

                collision.Update(gameTime);
                                      
                levelEins.Update(gameTime, player);                                 // draws the level related to the xml file
                
                m_Background1.Update(gameTime);
                m_Background2.Update(gameTime);
                m_Background3.Update(gameTime);
                m_Background4.Update(gameTime);
                m_Background5.Update(gameTime);
                player.setXVelocity(0f);


                if (player.getActiv())
                {
                    if (b_touchState == true)
                    {
                        helper.updateInput(listOfTouchLocations);
                        helper.readInput(gameTime, listOfTouchLocations, player);
                    }
                    else
                    {

                        playerInput.Update(gameTime);
                    }
                }
            }
           base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            screenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, screenManager.Scale);
            // TODO: Add your drawing code here


            if ((myTime > 60.0f ) || deathApplied || victoryApplied)
            {

                //Levelkomponenten
                m_Background1.Draw(screenManager.SpriteBatch);
                m_Background2.Draw(screenManager.SpriteBatch);
                m_Background3.Draw(screenManager.SpriteBatch);
                m_Background4.Draw(screenManager.SpriteBatch);
                m_Background5.Draw(screenManager.SpriteBatch);

                player.Draw(ScreenManager.SpriteBatch);                                         //drawing the character
                levelEins.Draw(ScreenManager.SpriteBatch);                                      //draws the first level on the screen
                if (b_touchState == true)
                    m_buttons.Draw(screenManager.SpriteBatch);

                if(player.getCurrentPlayerWeapon() == screenManager.powerupSystem.laser)
                    screenManager.SpriteBatch.DrawString(screenManager.Font, "Ammo: Infinite", new Vector2(5, 5), Color.Red);
                else
                    screenManager.SpriteBatch.DrawString(screenManager.Font, "Ammo: " + player.getCurrentPlayerWeapon().getAmmo(), new Vector2(5, 5), Color.Red);

                screenManager.SpriteBatch.DrawString(screenManager.Font, "Points: " + player.getScore(), new Vector2(1500, 50), Color.Red);
                screenManager.SpriteBatch.DrawString(screenManager.Font, "Time: " + m_countdown, new Vector2(1500, 5), Color.Red);
            }
            else
            {
                if(!deathApplied)
                {
                    flyinghead.Draw(screenManager.SpriteBatch);
                    bouncingText.Draw(screenManager.SpriteBatch);
                }
            }
           
            screenManager.SpriteBatch.End();
            base.Draw(gameTime);
        }

        //behandelt das Siegszenario
        private bool LevelWon()
        {
            if (!helper.boss.m_alive)
            {
                
                if (!victoryApplied)
                {
                    calculateScore();
                    myTime = 0;
                    victoryApplied = true;
                    screenManager.audioFileSystem.s_levelOneTheme.Stop();
                    screenManager.audioFileSystem.s_endBossTheme.Stop();
                    player.m_Animation.setAnimationActive(false);
                    player.setActive(false);
                    player.setMoveFloor(false);
                }
                if (myTime > 30)
                {
                    
                    levelEins.Dispose();
                    player.Dispose();
                    m_Background1.Dispose();
                    m_Background2.Dispose();
                    m_Background3.Dispose();
                    m_Background4.Dispose();
                    screenManager.AddScreen(new LevelWonScreen(screenManager, helper, b_touchState), null);
                    ExitScreen();
                    return true;
                }


                
            }
            return false;
        }

        //behandelt todszenario
        private void gameOver()
        {
            if (player.f_Position.Y > screenManager.Viewport.Height)
                player.m_Health = 0;
            if (player.m_Health <= 0 || m_countdown <=0)
            {
                if (!deathApplied)
                {
                    myTime = 0;
                    deathApplied = true;
                    screenManager.audioFileSystem.marioDied.Play();
                    screenManager.audioFileSystem.s_levelOneTheme.Stop();
                    screenManager.audioFileSystem.s_endBossTheme.Stop();
                    player.m_Animation.setAnimationActive(false);
                    player.setActive(false);
                    player.setMoveFloor(false);
                }
                if (myTime > 30)
                {
                    levelEins.Dispose();
                    player.Dispose();
                    m_Background1.Dispose();
                    m_Background2.Dispose();
                    m_Background3.Dispose();
                    m_Background4.Dispose();
                    screenManager.AddScreen(new GameOverScreen(screenManager, helper, b_touchState), null);

                    ExitScreen();

                }              
            }
        }

        //Bonus score
        private void calculateScore()
        {
            if (myTime < 3600)
            {
                player.addScore(1000);
                return;
            }
            if (myTime < 5400)
            {
                player.addScore(500);
                return;
            }
            if (myTime < 7200)
            {
                player.addScore(250);
                return;
            }
        }

        private void controlMusic()
        {
            if (player.f_Position.X + 1000 > helper.boss.f_Position.X)
            {
                bossThemeStarted = true;
                screenManager.audioFileSystem.startEndbossTheme();
            }

        }
    }
}
                
                
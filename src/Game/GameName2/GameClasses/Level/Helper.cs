// In dieser Klasse sind helper-functions implementiert
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Storage;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using Microsoft.Xna.Framework.GamerServices;


namespace BloodyPlumber
{
    public class Helper : GameScreen
    {
        ScreenManager screenManager;       

        public Helper(ScreenManager manager)
        {
            screenManager = manager;
            
        }

        Vector2 f_scaling, f_tileScaling;

        //Enemy-Animationen
        private Texture2D m_turtleStrip;
        private Animation m_turtleAnimation;
        private Texture2D m_mushroomStrip;
        private Animation m_mushroomAnimation;
        private Texture2D m_flyingBomb;
        private Animation m_flyingBombAnimation;
        private TouchCollection touchCollection;
        public Boss boss;

       

        //Läd die Gegener für das Spiel
        public override void LoadContent()
        {
            m_turtleAnimation = new Animation();
            m_turtleStrip = screenManager.Game.Content.Load<Texture2D>("Enemies\\turtleAnimation.png");
            m_turtleAnimation.Initialize(m_turtleStrip, -1000, -1000, 1.0f, 0.0f, 65, 90, 4, 50, true, getTileScale());
            m_mushroomAnimation = new Animation();
            m_mushroomStrip = screenManager.Game.Content.Load<Texture2D>("Enemies\\mushroom_strip.png");
            m_mushroomAnimation.Initialize(m_mushroomStrip, -1000, -1000, 1.0f, 0.0f, 70, 70, 5, 50, true, getTileScale());
            m_flyingBombAnimation = new Animation();
            m_flyingBomb = screenManager.Game.Content.Load<Texture2D>("Enemies\\bomb.png");
            m_flyingBombAnimation.Initialize(m_flyingBomb, -1000, -1000, 1.0f, 0.0f, 180, 100, 4, 50, true, getTileScale());
        }

        //Skaliert die grafiken im Spiel
        public Vector2  setScale()
        {
            float tmpScale = UIConstants.screenWidth / UIConstants.screenWidth; 
            tmpScale += 0.5f;
            tmpScale = (int)tmpScale * UIConstants.m_scaleFactor;
            tmpScale = tmpScale / UIConstants.m_scaleFactor;
            f_scaling.X = tmpScale;

            tmpScale = UIConstants.screenHeight / UIConstants.screenHeight;  
            tmpScale += 0.5f;
            tmpScale = (int)tmpScale * UIConstants.m_scaleFactor;
            tmpScale = tmpScale / UIConstants.m_scaleFactor;
            f_scaling.Y = tmpScale;

            return f_scaling;
        }

        //Skaliert die leveltiles
        public Vector2 getTileScale()
        {
            float gridTileWidth = UIConstants.screenWidth / UIConstants.m_TileScale;
            f_tileScaling = new Vector2(gridTileWidth / 80, gridTileWidth / 80);
            return f_tileScaling;
        }

        //Diese Klasse läd das Level. Je nach ID werden unterschiedliche xmls geladen. Die ID wird im konstruktor des gameplayscreens mitgegeben
        public Level LoadLevel(Texture2D[] textures, Vector2 scale, Player player, int Levelid, Animation bloodAnimation, Animation mudPixel)
        {
            string path ="";
            switch (Levelid)
            {
                case 0:
                    {
                        path = "Background.xml";
                        break;
                    }
                case 1:
                    {
                        path = "Level1.xml";
                        break;
                    }
            }
            XmlReader reader = XmlReader.Create(path);

            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            Level mylevel = (Level)serializer.Deserialize(reader);
            mylevel.Initialize(textures, scale, (int)player.getPlayerSpeed(), player, bloodAnimation, mudPixel, getPowerups(player),screenManager);

            for(int i = 0 ; i < mylevel.m_listOfEnemies.Count; i++)
            {
                if (mylevel.m_listOfEnemies.ElementAt(i).m_endFlag)
                {
                    Enemy e = mylevel.m_listOfEnemies.ElementAt(i);
                    Crocodile croc = new Crocodile();
                    croc.Initialize(e.f_Position.X, e.f_Position.Y, e.f_xVelocity, e.f_yVelocity, e.f_Speed, 20, screenManager.imageFileSystem.crocAnimation, UIConstants.screenWidth, screenManager, "Mr. Croc");
                    mylevel.m_listOfEnemies.Insert(i + 1, croc);
                    mylevel.m_listOfEnemies.Remove(e);
                    boss = croc;
                }
                else
                {
                    if (mylevel.m_listOfEnemies.ElementAt(i).m_id == 1)
                        mylevel.m_listOfEnemies.ElementAt(i).Initialize(m_turtleAnimation);
                    if (mylevel.m_listOfEnemies.ElementAt(i).m_id == 2)
                        addFlyingEnemy(mylevel.m_listOfEnemies.ElementAt(i), mylevel, i);
                    if (mylevel.m_listOfEnemies.ElementAt(i).m_id == 3)
                        mylevel.m_listOfEnemies.ElementAt(i).Initialize(m_mushroomAnimation);
                }
            }
            return mylevel;
            
        }

        public List<IPowerUps> getPowerups(Player p)
        {
            List<IPowerUps> powerUps = new List<IPowerUps>();

            //Hier die möglichen Waffen und PowerUps erstellen und in die Liste einfügen
            powerUps.Add(screenManager.powerupSystem.ak47);
            powerUps.Add(screenManager.powerupSystem.laser);

            return powerUps;
        }

        //fügt den speziellen Typ flyingenemy dem spiel hinzu
        private void addFlyingEnemy(Enemy enemy, Level level, int index)
        {
            FlyingEnemy f = new FlyingEnemy();
            f.Initialize(m_flyingBombAnimation);
            f.m_id = enemy.m_id;
            f.m_Health = enemy.m_Health;
            f.m_gravity = enemy.m_gravity;
            f.m_endFlag = enemy.m_endFlag;
            f.m_alive = enemy.m_alive;
            f.f_Position = enemy.f_Position;
            f.f_Speed = enemy.f_Speed;
            f.f_xVelocity = enemy.f_xVelocity;

            level.m_listOfEnemies.RemoveAt(index);
            level.m_listOfEnemies.Insert(index, f);
        }

        #region TouchInput

        //aktualisiert deen input jeden tick
        public void updateInput(List<TouchLocation> listOfTouchLocations)
        {
            listOfTouchLocations.Clear();

            touchCollection = TouchPanel.GetState();
            foreach (var touchLocation in touchCollection)
            {
                if (touchLocation.State == TouchLocationState.Moved
                    || touchLocation.State == TouchLocationState.Pressed || touchLocation.State == TouchLocationState.Released)
                {
                    listOfTouchLocations.Add(touchLocation);
                }

            }

           
        }
        //liest den input und führt entsprechende aktion aus
        public void readInput(GameTime gameTime, List<TouchLocation> listOfTouchLocations,Player player)
        {
                if (listOfTouchLocations.Count == 0)
                {
                    player.setXVelocity(0f);
                    player.setAnimationLooping(false);
                    player.getAnimation().setCurrentFrame(0);
                    player.setMoveFloor(false);
                    player.setPlayerStartedJump(false);
                }
                else
                {
                    foreach (var touchLocation in listOfTouchLocations)
                    {

                        if (touchLocation.Position.X > UIConstants.rightButtonXleft && touchLocation.Position.X < UIConstants.rightButtonXright && touchLocation.State == TouchLocationState.Moved)
                        {

                            if (!player.getMoveFloor() && !player.getMovementCollisionRightSide() && player.f_Position.X + player.f_xVelocity <= UIConstants.playerMaxXPosition)
                            {
                                player.setXVelocity(player.getPlayerSpeed());
                                if (!player.playerIsJumping())
                                    player.setAnimationLooping(true);
                                player.setAnimationMirror(SpriteEffects.None);
                            }

                            if (!player.getMoveFloor() && !player.getMovementCollisionRightSide() && player.f_Position.X + player.f_xVelocity > UIConstants.playerMaxXPosition)
                            {

                                player.setXVelocity(0);
                                player.setMoveFloor(true);
                                if (!player.playerIsJumping())
                                    player.setAnimationLooping(true);
                                player.setAnimationMirror(SpriteEffects.None);
                            }
                            if (player.getMoveFloor() && player.getMovementCollisionRightSide() || player.getMovementCollisionRightSide())
                            {
                                player.setXVelocity((0f));
                                player.setAnimationLooping(false);
                                player.setAnimationMirror(SpriteEffects.None);
                                player.setMoveFloor(false);
                            }
                            if (player.getMoveFloor() && !player.getMovementCollisionRightSide() && player.f_Position.X + player.f_xVelocity <= UIConstants.playerMaxXPosition)
                            {

                                player.setXVelocity(0f);
                                if (!player.playerIsJumping())
                                    player.setAnimationLooping(true);
                                player.setAnimationMirror(SpriteEffects.None);
                            }

                            if (!player.getMovementCollisionRightSide() && player.f_Position.X + player.f_xVelocity > UIConstants.playerMaxXPosition)
                            {
                                player.setMoveFloor(true);
                                player.setXVelocity(0f);
                                if (!player.playerIsJumping())
                                    player.setAnimationLooping(true);
                                player.setAnimationMirror(SpriteEffects.None);
                            }
                        }
                        if (touchLocation.Position.X > UIConstants.leftButtonXleft && touchLocation.Position.X < UIConstants.rightButtonXleft && touchLocation.State == TouchLocationState.Moved)
                        {
                            if (!player.getMovementCollisionLeftSide())
                            {
                                player.setXVelocity(-player.getPlayerSpeed());
                                player.setMoveFloor(false);

                                //Animation anpassen
                                if (!player.playerIsJumping())
                                    player.setAnimationLooping(true);
                                player.setAnimationMirror(SpriteEffects.FlipHorizontally);
                            }

                            if (player.getMovementCollisionLeftSide())
                            {
                                player.setXVelocity(0f);
                                player.setMoveFloor(false);

                                //Animation anpassen
                                player.setAnimationLooping(false);
                                player.getAnimation().setCurrentFrame(0);
                                player.setAnimationMirror(SpriteEffects.FlipHorizontally);

                            }
                        }
                        if (touchLocation.Position.X > UIConstants.shootButtonxLeft && touchLocation.Position.X < UIConstants.jumpButtonXleft && touchLocation.State == TouchLocationState.Moved && player.canShoot())
                        {
                            player.shoot(gameTime);
                        }
                        if (player.playerIsJumping() == false)
                        {
                            if (touchLocation.Position.X > UIConstants.jumpButtonXleft && touchLocation.Position.X < UIConstants.jumpButtonXRight && touchLocation.State == TouchLocationState.Moved && player.getYPlayerVelocity() == 0)
                            {
                                player.playerJump(gameTime);
                            }

                            if (player.playerStartedJump())
                            {
                                player.setYVelocity(UIConstants.jumpVelocity);
                            }

                        }
                    }
                }
            
        }


        #endregion
        

        

    }
}

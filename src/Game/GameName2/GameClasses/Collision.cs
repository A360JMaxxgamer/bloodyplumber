//Diese Klasse berechnet alle Kollisionen zwischen allen Objekten
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;

namespace BloodyPlumber
{
    class Collision
    {
        private Level m_level;
        private Player m_player;
        private Animation m_bloodPixel;
        private bool m_isgameScreen;

        private List<SoundEffect> m_KillSounds;
        public void Initialize(Level level, Player player, Animation bloodPixel, bool isgameplay, List<SoundEffect> killSounds)
        {
            m_level = level;
            m_player = player;
            m_bloodPixel = bloodPixel;
            m_isgameScreen = isgameplay;
            m_KillSounds = killSounds;
        }

        public void Update(GameTime gametime)
        {

            if (m_isgameScreen)
            {
                checkBloodTiles();
                checkMudTiles();
                TilesCollision(gametime);
            }
            EnemyTilesCollision(gametime);
            if (m_isgameScreen)
            {
                projectilesCollideTiles(gametime);
                projectilesCollideEnemies(gametime);

                checkEnemyKillsPlayer(gametime);
                checkEnemieShots();
                checkCoins(gametime);
            }
            
        }





        #region Object vs Tiles
        #region Object vs Tiles
        public void TilesCollision(GameTime gameTime)
        {
            if (m_player.getActiv())
            {
                bool intersectBottom = false;
                bool leftIntersect = false;
                bool rightIntersect = false;
                bool topIntersect = false;
                Tile tmpTileBottom = new Tile();
                Tile tmpTileRight = new Tile();
                Tile tmpTileLeft = new Tile();
                Tile tmpTileTop = new Tile();
                //m_player.getAnimation().shiftRectangle((int)m_player.getXPlayerVelocity(),(int) m_player.getYPlayerVelocity());
                foreach (Tile tile in m_level.m_listOfTiles)
                {
                    if (tile.getActive() && tile.getNoRectangle() == false)
                    {
                        bool collision = (m_player.getAnimation().getBottom().Intersects(tile.getCollisionRectangle()));       //Temporäre Variabel die überprüft ob eine Kollision stattfindet
                        if (!leftIntersect)
                        {
                            leftIntersect = checkTilestoLeft(m_player, tile.getCollisionRectangle());
                            tmpTileLeft = tile;
                        }
                        if (!rightIntersect)
                        {
                            rightIntersect = checkTilestoRight(m_player, tile.getCollisionRectangle());
                            tmpTileRight = tile;
                        }
                        if (!topIntersect)
                        {
                            topIntersect = checkTilestoTop(m_player, tile.getCollisionRectangle());
                            tmpTileTop = tile;
                        }
                        if (checkTilestoBottom(m_player, tile.getCollisionRectangle()))
                        {
                            intersectBottom = true;
                            tmpTileBottom = tile;
                        }
                    }
                }
                if (topIntersect && !intersectBottom && m_player.playerStartedJump())
                {
                    m_player.setYVelocity(0);
                    m_player.setPlayerStartedJump(false);
                }

                if (intersectBottom && !m_player.playerStartedJump())
                {
                    m_player.setYPosition(tmpTileBottom.getPosition().Y - m_player.getAnimation().getFrameHeight() - 5);
                    m_player.setYVelocity(0);
                    m_player.setPlayerIsJumping(false);
                    if (tmpTileBottom.m_deadly)
                    {
                        m_player.setHealth(0);
                        createPuddle(m_player.f_Position, gameTime, 0);
                        m_player.setActive(false);
                        m_player.Dispose();
                    }
                    checkPlayerChest(tmpTileBottom);
                }

                if (!intersectBottom && !m_player.playerIsJumping())
                {

                    m_player.setYVelocity(10f);
                }

                if (topIntersect)
                {
                    m_player.setYPosition(tmpTileTop.getCollisionRectangle().Bottom);
                    m_player.setYVelocity(5f);
                    m_player.setPlayerStartedJump(false);

                }
                if (rightIntersect)
                {
                    m_player.setXPosition(tmpTileRight.getPosition().X - m_player.getAnimation().getFrameWidth());
                    m_player.setMoveFloor(false);
                    checkPlayerChest(tmpTileRight);
                }
                if (leftIntersect)
                {
                    m_player.setXPosition(tmpTileLeft.getPosition().X + tmpTileLeft.m_tileWidth);
                    checkPlayerChest(tmpTileLeft);
                }
            }
        }

        public bool checkTilestoTop(Object ob, Rectangle tile)
        {

            if (ob.getAnimation().getTop().Intersects(tile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkTilestoBottom(Object ob, Rectangle tile)
        {

            if (ob.getAnimation().getBottom().Intersects(tile))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Überprüft ob links und rechts vom Spieler ein tile ist in das er NICHT reinlaufen darf
        public bool checkTilestoLeft(Object ob, Rectangle tile)
        {

            if (ob.getAnimation().getLeft().Intersects(tile))
            {
                ob.setMovementCollisionLeftSide(true);
                return true;
            }
            else
            {
                ob.setMovementCollisionLeftSide(false);
                return false;
            }
        }

        public bool checkTilestoRight(Object ob, Rectangle tile)
        {

            if (ob.getAnimation().getRight().Intersects(tile))
            {
                ob.setMovementCollisionRightSide(true);
                return true;
            }
            else
            {
                ob.setMovementCollisionRightSide(false);
                return false;
            }
        }


        public void EnemyTilesCollision(GameTime gameTime)
        {
            for(int i = 0; i < m_level.m_listOfEnemies.Count(); i++)
            {
                if (m_level.m_listOfEnemies.ElementAt(i).m_active)
                {
                    bool intersectBottom = false;
                    bool leftIntersect = false;
                    bool rightIntersect = false;
                    bool topIntersect = false;

                    bool deadly = false;

                    foreach (Tile tile in m_level.m_listOfTiles)
                    {
                        if (tile.getActive() && tile.getNoRectangle() == false)
                        {
                            if (checkTilestoLeft(m_level.m_listOfEnemies.ElementAt(i), tile.getCollisionRectangle()) && !leftIntersect)
                            {
                                leftIntersect = true;
                            }
                            if (checkTilestoRight(m_level.m_listOfEnemies.ElementAt(i), tile.getCollisionRectangle()) && !rightIntersect)
                            {
                                rightIntersect = true;
                            }
                            
                            if (checkTilestoBottom(m_level.m_listOfEnemies.ElementAt(i), tile.getCollisionRectangle()) && !intersectBottom)
                            {
                                intersectBottom = true;

                                if(tile.m_deadly)
                                {
                                    createPuddle(m_level.m_listOfEnemies.ElementAt(i).f_Position, gameTime, 0);
                                    m_level.m_listOfEnemies.ElementAt(i).Dispose();
                                    m_level.m_listOfEnemies.RemoveAt(i);
                                    deadly = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!deadly)
                    {
                       m_level.m_listOfEnemies.ElementAt(i).updateIntersects(leftIntersect, rightIntersect, topIntersect, intersectBottom);
                    }


                }

            }
        }

        #endregion

        #region Projectiles
        //Prototyp
        public void projectilesCollideTiles(GameTime gameTime)
        {
            for(int i = 0 ; i < m_player.getProjectileList().Count; i++)
            {
                if (m_player.getProjectileList().ElementAt(i).getActivity())
                {
                    for(int j = 0; j < m_level.m_listOfTiles.Count();j++)
                        if (m_level.m_listOfTiles.ElementAt(j).getActive() && m_level.m_listOfTiles.ElementAt(j).getNoRectangle() == false)
                        {
                            if (m_player.getProjectileList().ElementAt(i).getProjectileAnimation().getRectangle().Intersects(m_level.m_listOfTiles.ElementAt(j).getCollisionRectangle()))
                            {
                                m_player.getProjectileList().ElementAt(i).deactivate();
                                if(m_level.m_listOfTiles.ElementAt(j).m_destroyable)
                                {
                                    createMud(m_level.m_listOfTiles.ElementAt(j).getPosition(), gameTime, m_player.getProjectileList().ElementAt(i).getSpeed());
                                    m_level.m_listOfTiles.ElementAt(j).Dispose();
                                    m_level.m_listOfTiles.RemoveAt(j);
                                    j++;
                                }
                            }
                        }
                }
            }
        }

        //Prototyp
        public void projectilesCollideEnemies(GameTime gameTime)
        {
            {
                for (int i = 0; i < m_player.getProjectileList().Count; i++)
                {
                    if(m_player.getProjectileList().ElementAt(i).getActivity())
                    {
                        foreach (Enemy enemy in m_level.m_listOfEnemies)
                        {
                            if (m_player.getProjectileList().ElementAt(i).getProjectileAnimation().getRectangle().Intersects(enemy.getAnimation().getRectangle()))
                            {
                                if(enemy.m_endFlag)
                                {
                                    Boss b = (Boss) enemy;
                                    if (b.isFighting())
                                        b.gotShot(gameTime);
                                    m_player.getProjectileList().ElementAt(i).deactivate();
                                }

                                if (!enemy.m_endFlag)
                                {
                                    enemy.gotShot();
                                    m_player.getProjectileList().ElementAt(i).deactivate();
                                }
                                if (enemy.m_Health == 0 && !enemy.m_endFlag)
                                {
                                    createPuddle(m_player.getProjectileList().ElementAt(i).getPosition(), gameTime, m_player.getProjectileList().ElementAt(i).getSpeed());
                                    playKillSound(gameTime);
                                    enemy.Dispose();
                                    m_level.m_listOfEnemies.Remove(enemy);
                                    m_player.addScore(UIConstants.killingPoints);
                                }
                                if (enemy.m_Health == 0 && enemy.m_endFlag )
                                {
                                    Boss b = (Boss)enemy;
                                    b.setVisible(false);
                                    b.setActive(false);
                                    createPuddle(m_player.getProjectileList().ElementAt(i).getPosition(), gameTime, m_player.getProjectileList().ElementAt(i).getSpeed());
                                    m_KillSounds.ElementAt(1).Play();
                                    m_player.addScore(UIConstants.killingPoints);
                                }
                           
                           
                            
                                break;
                            }
                        }
                    }

                    
                }
            }
        }

        public void checkEnemieShots()
        {
            Crocodile b = new Crocodile();
            Boolean foundBoss = false;
            foreach (Enemy e in m_level.m_listOfEnemies)
                if (e.m_endFlag)
                {
                    b = (Crocodile)e;
                    foundBoss = true;
                }
            if(foundBoss)
                foreach (Projectile p in b.getProjectiles())
                {
                    if(p.getProjectileAnimation().getRectangle().Intersects(m_player.getAnimation().getRectangle()))
                    {
                        m_player.m_Health --;
                    }
                }
        }
        #endregion

        #region Player vs Enemies


        public void checkEnemyKillsPlayer(GameTime gameTime)
        {
            if (m_player.getActiv())
            {
                foreach (Enemy enemy in m_level.m_listOfEnemies)
                {
                    if (m_player.getAnimation().getRectangle().Intersects(enemy.getAnimation().getRectangle()))
                    {
                        m_level.createBlood(m_player.f_Position, gameTime, 0);
                        m_level.createBlood(m_player.f_Position, gameTime, 0);
                        m_level.createBlood(m_player.f_Position, gameTime, 1);
                        m_level.createBlood(m_player.f_Position, gameTime, -1);
                        //m_player.m_Animation.setAnimationActive(false);
                        m_player.m_Health --;
                        //m_player.setActive(false);
                        //m_player.setMoveFloor(false);
                        break;
                    }
                }
            }

        }

        public void createPuddle(Vector2 position, GameTime gameTime, int projectileSpeed)
        {
            m_level.createBlood(position, gameTime, projectileSpeed);
        }

        public void createMud(Vector2 position, GameTime gameTime, int projectileSpeed)
        {
            m_level.createMud(position, gameTime, projectileSpeed);
    }
#endregion

        public void checkBloodTiles()
        {
            foreach(PuddleOfBlood puddle in m_level.m_puddleOfBloodList)
            {
                foreach(Blood blood in puddle.m_listOfBlood)
                {
                    bool intersectBottom = false;
                    bool leftIntersect = false;
                    bool rightIntersect = false;
                    bool topIntersect = false;

                    foreach (Tile tile in m_level.m_listOfTiles)
                    {
                        if (tile.getActive() && puddle.getActivity() && tile.getNoRectangle() == false)
                        {
                            if (checkTilestoLeft(blood, tile.getCollisionRectangle()) && !leftIntersect)
                            {
                                leftIntersect = true;
                                blood.setXVelocity(0);
                            }
                            if (checkTilestoRight(blood, tile.getCollisionRectangle()) && !rightIntersect)
                            {
                                rightIntersect = true;
                                blood.setXVelocity(0);
                            }
                            if (!intersectBottom)
                            {
                                blood.setYVelocity(0f);
                            }
                            if (checkTilestoBottom(blood, tile.getCollisionRectangle()) && !intersectBottom)
                            {
                                intersectBottom = true;
                                blood.setYVelocity(-5);
                                blood.setXVelocity(0);
                            }
                        }
                    }
                }
            }


        }

        public void checkMudTiles()
        {
            foreach (PuddleofMud puddle in m_level.m_puddleOfMudList)
            {
                foreach (Blood blood in puddle.m_listOfBlood)
                {
                    bool intersectBottom = false;
                    bool leftIntersect = false;
                    bool rightIntersect = false;
                    bool topIntersect = false;

                    foreach (Tile tile in m_level.m_listOfTiles)
                    {
                        if (tile.getActive() && puddle.getActivity())
                        {
                            if (checkTilestoLeft(blood, tile.getCollisionRectangle()) && !leftIntersect)
                            {
                                leftIntersect = true;
                                blood.setXVelocity(0);
                            }
                            if (checkTilestoRight(blood, tile.getCollisionRectangle()) && !rightIntersect)
                            {
                                rightIntersect = true;
                                blood.setXVelocity(0);
                            }
                            if (!intersectBottom)
                            {
                                blood.setYVelocity(0f);
                            }
                            if (checkTilestoBottom(blood, tile.getCollisionRectangle()) && !intersectBottom)
                            {
                                intersectBottom = true;
                                blood.setYVelocity(-5);
                                blood.setXVelocity(0);
                            }
                        }
                    }
                }
            }


        }

        public void checkCoins(GameTime gameTime)
        {
            if (m_player.getActiv())
            {
                foreach (Coin coin in m_level.m_listOfCoins)
                {
                    if (!coin.isCollectd() && coin.m_Animation.getRectangle().Intersects(m_player.getAnimation().getRectangle()))
                    {
                        coin.collected(gameTime);
                        m_player.addScore(coin.getPoints());
                    }
                }
            }
        }

        private  void checkPlayerChest(Tile tile)
        {
            if(m_player.getActiv())
            {
                if(tile.m_powerup)
                {
                    m_level.collectChest(tile.m_xData, tile.m_yData, m_player);
                    tile.Dispose();
                    m_level.m_listOfTiles.Remove(tile);
                }
            }
        }
        
        private void playKillSound(GameTime gameTime)
        {
            Random r = new Random();
            Random play = new Random();
            int playR = play.Next(0, 10);
            if (playR == 1 || playR == 5 || playR == 8 || playR == 10)
            {
                int i = r.Next(0, m_KillSounds.Count - 1);
                m_KillSounds.ElementAt(i).Play();
                m_player.speak((int)m_KillSounds.ElementAt(i).Duration.TotalMilliseconds+(int) gameTime.TotalGameTime.TotalMilliseconds);
            }
        }

    }
}



        #endregion
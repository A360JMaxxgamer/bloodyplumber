//In dieser Klasse sind Konstanten für das Spiel gesammelt
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace BloodyPlumber
{
    static class UIConstants
    {
        //General
        public const int screenWidth = 1920;
        public const int screenHeight = 1080;

        //Blood class
        public const int bloodFaktor = 30;

        //Puddle of Blood
        public const int m_bloodSpeedFaktor = 10;

        //Coin
        public const int m_initializePositionCoin = 2000;
        public const int m_coinMoveUp = 5000;

        //Enemy
        public const int m_initializePositionEnemy = 2400;
        public const int m_disposeEnemyPosition = -100;

        //Helper
        public const int m_scaleFactor = 10;
        public const int m_TileScale = 24;

        public const int rightButtonXright = 300;
        public const int rightButtonXleft = 150;
        public const int leftButtonXleft = 0;
        public const int jumpButtonXleft = 1770;
        public const int jumpButtonXRight = 1920;
        public const int shootButtonxLeft = 1620;

        public const int playerMaxXPosition = 890;

        public const int jumpVelocity = -30;

        //Level
        public const int tileDisposePosition = -200;

        //Player
        public const int timeBetweenShoot = 200;
        public const int shootSpeed = 30;
        public const int disoposeProjectileRight = 2500;
        public const int disoposeProjectileLeft = -200;

        //Tile
        public const int activeTilePosition = 5000;

        //Weapon
        //hier die richtigen werte für die waffenanimation einfügen und zum verschieben

        //
        public const TouchLocation[] test = null;

        //Points for killing
        public const int killingPoints = 50;


    }
}

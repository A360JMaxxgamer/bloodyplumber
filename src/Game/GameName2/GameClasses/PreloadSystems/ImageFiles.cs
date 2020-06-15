//Läd alle Texturen für das Spiel
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BloodyPlumber
{
    public class ImageFiles
    {
        public PlayerAnimation playerAnimation;
        public Animation akAnimation;
        public Animation laserAnimation;
        public Animation mudAnimation;
        public Animation bloodAnimation;
        public Animation mouth;
        public Animation crocAnimation;

        public Texture2D redPixel;
        public Texture2D blue_background;
        public Texture2D mountain1;
        public Texture2D mountain2;
        public Texture2D cloud1;
        public Texture2D cloud2;
        public Texture2D tree1;
        public Texture2D tree2;

        public Texture2D flyingHead;

        public Texture2D smokeParticle, DustParticle;

        public Texture2D left, right, shoot, jump;

        public Texture2D laserProjectile;

        public Texture2D akProjectile;

        public Texture2D crocProjectile;

        public Texture2D redCoin;

        public Texture2D level1_TileStrip;

        public Texture2D boogeyMan;

        public void Initialize()
        {
            playerAnimation = new PlayerAnimation();
            akAnimation = new Animation();
            laserAnimation = new Animation();
            mudAnimation = new Animation();
            bloodAnimation = new Animation();
            mouth = new Animation();
            crocAnimation = new Animation();
        }

        public void LoadContent(ScreenManager screenmanager)
        {
            loadTextures(screenmanager);
            loadAnimation(screenmanager);
        }

        public void loadTextures(ScreenManager screenmanager)
        {
            blue_background = screenmanager.Game.Content.Load<Texture2D>("Level_1\\level1_Background");
            mountain1 = screenmanager.Game.Content.Load<Texture2D>("Level_1\\level1_Mountains1");
            mountain2 = screenmanager.Game.Content.Load<Texture2D>("Level_1\\level1_Mountains2");
            cloud1 = screenmanager.Game.Content.Load<Texture2D>("Level_1\\level1_Clouds1");
            cloud2 = screenmanager.Game.Content.Load<Texture2D>("Level_1\\level1_Clouds2");
            tree1 = screenmanager.Game.Content.Load<Texture2D>("Level_1\\level1_Trees1");
            tree2 = screenmanager.Game.Content.Load<Texture2D>("Level_1\\level1_Trees2");

            redPixel = screenmanager.Game.Content.Load<Texture2D>("pixelRed");

            left = screenmanager.Game.Content.Load<Texture2D>("Buttons\\Button_ArrowLeft");
            right = screenmanager.Game.Content.Load<Texture2D>("Buttons\\ButtonArrowRight");
            shoot = screenmanager.Game.Content.Load<Texture2D>("Buttons\\Button_Shoot");
            jump = screenmanager.Game.Content.Load<Texture2D>("Buttons\\Button_Jump");

            smokeParticle = screenmanager.Game.Content.Load<Texture2D>("Smoke");
            DustParticle = screenmanager.Game.Content.Load<Texture2D>("Dust");
            laserProjectile = screenmanager.Game.Content.Load<Texture2D>("projektil");
            akProjectile = screenmanager.Game.Content.Load<Texture2D>("bullet");

            flyingHead = screenmanager.Game.Content.Load<Texture2D>("Mario_Head");

            redCoin = screenmanager.Game.Content.Load<Texture2D>("Coin");

            level1_TileStrip = screenmanager.Game.Content.Load<Texture2D>("Level_1\\grassTileSet");

            boogeyMan = screenmanager.Game.Content.Load<Texture2D>("Sensenmann");

            crocProjectile = screenmanager.Game.Content.Load<Texture2D>("orb");
        }

        public void loadAnimation(ScreenManager screenmanager)
        {
            Texture2D mudPixel = screenmanager.Game.Content.Load<Texture2D>("pixel");
            mudAnimation.Initialize(mudPixel, 1000, 1000, 0, 0.01f, 8, 8, 1, 1000, true, new Vector2(1, 1));

            Texture2D playerSpriteStrip = screenmanager.Game.Content.Load<Texture2D>("mario_Animation");
            playerAnimation.Initialize(playerSpriteStrip, -1000, -1000, 1, 0.0f, 108, 154, 8, 50, true, new Vector2(1,1));

            Texture2D pixel = screenmanager.Game.Content.Load<Texture2D>("pixelRed");
            bloodAnimation.Initialize(pixel, 1000, 1000, 0, 0.01f, 8, 8, 1, 1000, true, new Vector2(1, 1));

            Texture2D akStrip = screenmanager.Game.Content.Load<Texture2D>("AK_Aktuell");
            akAnimation.Initialize(akStrip, -1000, -1000, 1, 0.0f, 140, 58, 1, 1000, false,new Vector2(1,1));

            Texture2D mouthStrip = screenmanager.Game.Content.Load<Texture2D>("mouth");
            mouth.Initialize(mouthStrip, -1000, -1000, 1, 0.0f, 30, 15, 4, 30, true, new Vector2(1, 1));

            Texture2D laserStrip = screenmanager.Game.Content.Load<Texture2D>("laser");
            laserAnimation.Initialize(laserStrip, -1000, -1000, 1, 0.0f, 140, 58, 1, 1000, false, new Vector2(1, 1));

            Texture2D crocodileStrip = screenmanager.Game.Content.Load<Texture2D>("CrocAnimation");
            crocAnimation.Initialize(crocodileStrip,-1000, -1000, 1, 0.0f, 247, 175,8, 50, true, new Vector2(1,1));
        }
    }
}

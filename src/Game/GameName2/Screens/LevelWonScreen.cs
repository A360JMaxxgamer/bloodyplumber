using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using ParticleEmitter;

namespace BloodyPlumber
{
    class LevelWonScreen : GameScreen
    { 
        ScreenManager screenManager;
        Helper helper;
        List<TouchLocation> listOfTouchLocations;
        bool b_touchState;
        int myTick;

        private ParticleSystemSettings m_ermitterSettings;
        private ParticleEmitter.ParticleSystem m_ermitter;

        public LevelWonScreen(ScreenManager manager,Helper h,bool b)
        {
            screenManager = manager;
            helper = h;
            listOfTouchLocations = new List<TouchLocation>();
            b_touchState = b;
            myTick = 0;

            #region Ermitter
            
            m_ermitterSettings = new ParticleSystemSettings();
            m_ermitterSettings.IsBurst = false;
            m_ermitterSettings.color = Color.Red;
            m_ermitterSettings.SetLifeTimes(5f, 10.0f);
            m_ermitterSettings.SetScales(1.0f, 1.5f);
            m_ermitterSettings.ParticlesPerSecond = 1000.0f;
            m_ermitterSettings.InitialParticleCount = (int)(m_ermitterSettings.ParticlesPerSecond * m_ermitterSettings.MaximumLifeTime);
            m_ermitterSettings.SetDirectionAngles(0, 360);
            m_ermitterSettings.SetSpeeds(200, 800);
            m_ermitter = new ParticleEmitter.ParticleSystem(screenManager.Game, m_ermitterSettings, screenManager.imageFileSystem.redPixel);
            m_ermitter.OriginPosition = new Vector2(UIConstants.screenWidth / 2, UIConstants.screenHeight / 2);
            m_ermitterSettings.emitterOn = true;
            #endregion

        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {

            if (myTick == 25)
                screenManager.audioFileSystem.ninini.Play();
            myTick++;

            TouchPanel.EnabledGestures = GestureType.None;
            helper.updateInput(listOfTouchLocations);
            m_ermitter.Update(gameTime);
            if (myTick == 150)
            {
                BackgroundScreen bscreen = new BackgroundScreen(screenManager);
                screenManager.AddScreen(bscreen, null);
                screenManager.AddScreen(new MainMenuScreen(screenManager, bscreen,b_touchState), null);


                ExitScreen();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            screenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, screenManager.Scale);
            m_ermitter.Draw(screenManager.SpriteBatch);
            screenManager.SpriteBatch.DrawString(screenManager.Font, "Congrats \n Level finished", new Vector2(UIConstants.screenWidth / 2, UIConstants.screenHeight / 2), Color.White);

            screenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }


    }
}

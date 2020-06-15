// Diese Klasse zeigt den Hintergrund im Menü an
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BloodyPlumber
{
    class BackgroundScreen : GameScreen
    {
        Helper helper;
        Level level;
        Player p;

        Background m_Background1;
        Background m_Background2;
        Background m_Background3;
        Background m_Background4;

        Texture2D t_Background1;
        Texture2D t_Background2;
        Texture2D t_Background3;
        Texture2D t_Background4;
        Texture2D pixel;

        Collision collision;
        float m_myTime;

        private AudioFiles audioFileSystem;

        private Texture2D[] m_textures;

        List<Animation> m_enemyAnimationList;

        ScreenManager screenManager;

        Texture2D m_backgroundScreenTexture;

        SoundEffectInstance song;

        public BouncingText titleText;
        

        public BackgroundScreen(ScreenManager manager)
        {
            
            screenManager = manager;
            audioFileSystem = screenManager.audioFileSystem;
            helper = new Helper(screenManager);
            TransitionOnTime = TimeSpan.FromSeconds(0.0);
            TransitionOffTime = TimeSpan.FromSeconds(0.0);

            level = new Level();
            m_enemyAnimationList = new List<Animation>();

            //Level
            m_Background1 = new Background();
            m_Background2 = new Background();
            m_Background3 = new Background();
            m_Background4 = new Background();


            m_textures = new Texture2D[5];
            p = new Player();
            p.Initialize(0, 0, 0, 0, 0, 0, new Animation(), 1920);

            collision = new Collision();
            titleText = new BouncingText("Bloody Plumber", new Vector2(400, 100), 50, 150, 10, screenManager);
        }

        public override void LoadContent()
        {
            
        
                pixel = screenManager.Game.Content.Load<Texture2D>("pixelRed");
                t_Background1 = screenManager.imageFileSystem.blue_background;
                t_Background2 = screenManager.imageFileSystem.mountain1;               
                t_Background3 = screenManager.imageFileSystem.cloud1;                
                t_Background4 = screenManager.imageFileSystem.tree1;
            
            
            m_Background1.Initialize(t_Background1,t_Background1, Vector2.Zero, 0, p, false, false);
            m_Background2.Initialize(t_Background2, t_Background2, Vector2.Zero, 0, p, false, false);
            m_Background3.Initialize(t_Background3,t_Background3, Vector2.Zero, 2, p, true, false);
            m_Background4.Initialize(t_Background4, t_Background4,new Vector2(0, 320), 0, p, false,false);

            m_textures[0] = screenManager.Game.Content.Load<Texture2D>("Level_1\\grassTileSet");
            m_textures[1] = screenManager.Game.Content.Load<Texture2D>("Coin");

            helper.LoadContent();
            level = helper.LoadLevel(m_textures, helper.getTileScale(), p, 0, new Animation(), new Animation());
            collision.Initialize(level, p, new Animation(), false, null);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
           if (m_myTime == 0)
            {
                audioFileSystem.s_thisAintMario.Play();
                
                audioFileSystem.menuTheme.Play();
                
                
            }

            m_myTime += 1.0f;
            base.Update(gameTime, otherScreenHasFocus, false);
            level.Update(gameTime, p);
            m_Background1.Update(gameTime);
            m_Background2.Update(gameTime);
            m_Background3.Update(gameTime);
            m_Background4.Update(gameTime);
            collision.Update(gameTime);
            titleText.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch m_spriteBatch = ScreenManager.SpriteBatch;
            ScreenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, ScreenManager.Scale);
            if (m_myTime > 10)
            {
                m_Background1.Draw(screenManager.SpriteBatch);
                m_Background2.Draw(screenManager.SpriteBatch);
                m_Background3.Draw(screenManager.SpriteBatch);
                m_Background4.Draw(screenManager.SpriteBatch);

                level.Draw(screenManager.SpriteBatch);             
                titleText.Draw(m_spriteBatch);
            }
            m_spriteBatch.End();

        }
    }
}

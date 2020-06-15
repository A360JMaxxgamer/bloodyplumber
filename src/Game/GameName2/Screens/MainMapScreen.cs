using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System.Xml;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace BloodyPlumber
{
    class MainMapScreen : GameScreen
    {
        ScreenManager screenManager;
        Texture2D m_mainMap;
        List<Rectangle> listOfRectangles = new List<Rectangle>();
        bool touchState;
        Rectangle rect_level1;
        int selected_Level = 0;


        public MainMapScreen(ScreenManager manager,bool touch)
        {
            screenManager = manager;
            rect_level1 = new Rectangle(200, 150, 250, 100);
            EnabledGestures = GestureType.Tap;
            this.touchState = touch;

        }


        public override void LoadContent()
        {
            m_mainMap = screenManager.Game.Content.Load<Texture2D>("mainmap.png");

            listOfRectangles.Add(rect_level1);


        }

        public override void HandleInput(InputState input)
        {
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {

                    Point tapLocation = new Point((int)gesture.Position.X, (int)gesture.Position.Y);


                    for (int i = 0; i < listOfRectangles.Count; i++)
                    {
                        Rectangle temp = listOfRectangles[i];

                        if (temp.Contains(tapLocation) && temp == rect_level1)
                        {
                            selected_Level = 1;
                        }
                    }
                }
            }

            if (input.MouseGesture.HasFlag(MouseGestureType.LeftClick))
            {

                Point clickLocation = new Point((int)input.CurrentMousePosition.X, (int)input.CurrentMousePosition.Y);

                for (int i = 0; i < listOfRectangles.Count; i++)
                {
                    Rectangle temp = listOfRectangles[i];

                    if (temp.Contains(clickLocation) && temp == rect_level1)
                    {
                        selected_Level = 1;
                    }
                }
            }
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (selected_Level == 1)
            {
                //LoadingScreen.Load(screenManager, true, null, new GameplayScreen(screenManager, 1,touchState));                
                ExitScreen();
            }

              
            

        }

        public override void Draw(GameTime gameTime)
        {
            screenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, ScreenManager.Scale);

            screenManager.SpriteBatch.Draw(m_mainMap, Vector2.Zero, Color.White);


            screenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }




    }
}

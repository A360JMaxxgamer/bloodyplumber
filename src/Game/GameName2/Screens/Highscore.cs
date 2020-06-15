using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;
using System.Text;




namespace BloodyPlumber
{
    class Highscore : GameScreen
    {
        #region Attribute
        List<Rectangle> listOfRectangles = new List<Rectangle>();
        Rectangle rect1;
        bool end;
        Vector2 exitButtonPosition;
        BackgroundScreen bscreen;

        ScreenManager screenManager;

        bool touchState;

        
        const int highscorePlaces = 10;
        public static List<KeyValuePair<string, int>> highScore =
            new List<KeyValuePair<string, int>>(highscorePlaces)
        {
            new KeyValuePair<string, int>(" ",10),
            new KeyValuePair<string, int>(" ",9),
            new KeyValuePair<string, int>(" ",8),
            new KeyValuePair<string, int>(" ",7),
            new KeyValuePair<string, int>(" ",6),
            new KeyValuePair<string, int>(" ",5),
            new KeyValuePair<string, int>(" ",4),
            new KeyValuePair<string, int>(" ",3),
            new KeyValuePair<string, int>(" ",2),
            new KeyValuePair<string, int>(" ",1)
        };

        

        #endregion

        #region Initialzations

        public Highscore(ScreenManager manager,BackgroundScreen b, bool touch)
        {
            EnabledGestures = GestureType.Tap;
            screenManager = manager;
            bscreen = b;
            exitButtonPosition = new Vector2(900, 900);
            rect1 = new Rectangle((int)exitButtonPosition.X,(int)exitButtonPosition.Y, 100, 100);
            end = false;
            listOfRectangles.Add(rect1);
            touchState = touch;
            screenManager.Game.IsMouseVisible = true;
            
        }

        #endregion

        #region Loading

        /// <summary>
        /// Load screen resources.
        /// </summary>
        public override void LoadContent()
        {
            LoadHighscores();
            
            base.LoadContent();
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Handles user input as a part of screen logic update.
        /// </summary>
        /// <param name="gameTime">Game time information.</param>
        /// <param name="input">Input information.</param>
        public override void HandleInput(InputState input)
        {
            

            if (input == null)
            {
                throw new ArgumentNullException("Input loading failed");
            }

            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {

                    Point tapLocation = new Point((int)gesture.Position.X, (int)gesture.Position.Y);

                    for (int i = 0; i < listOfRectangles.Count; i++)
                    {
                        Rectangle temp = listOfRectangles[i];

                        if (temp.Contains(tapLocation) && temp == rect1)
                        {
                            end = true;
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

                    if (temp.Contains(clickLocation) && temp == rect1)
                    {
                        end = true;
                    }
                }
            }
        }

        /// <summary>
        /// Exit this screen.
        /// </summary>
        private void Exit()
        {
            bscreen.titleText.newText("Bloody Plumber");
            this.ExitScreen();
            ScreenManager.AddScreen(new MainMenuScreen(screenManager,bscreen,touchState), null);
        }


        #endregion

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (end)
            {
                screenManager.Game.IsMouseVisible = false;
                Exit();
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        #region Render

        /// <summary>
        /// Erstellt den text
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            screenManager.SpriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, screenManager.Scale);

            ScreenManager.SpriteBatch.DrawString(screenManager.Font, "Back", exitButtonPosition, Color.Red);
            
          

            // Überschrift
            string text = "High Scores";
            var textSize = screenManager.Font.MeasureString(text);
            var position = new Vector2(
                ScreenManager.GraphicsDevice.Viewport.Width / 2 - textSize.X / 2,
                340);

            ScreenManager.SpriteBatch.DrawString(screenManager.Font, text,
                position, Color.Red);

            //zeichnet die Highscoretabelle
            for (int i = 0; i < highScore.Count; i++)
            {
                ScreenManager.SpriteBatch.DrawString(screenManager.Font,
                    String.Format("{0,2}. {1}", i + 1, highScore[i].Key),
                    new Vector2(500, i * 40 + position.Y + 40), Color.Red);
                ScreenManager.SpriteBatch.DrawString(screenManager.Font,
                    highScore[i].Value.ToString(),
                    new Vector2(650, i * 40 + position.Y + 40),
                    Color.Red);
            }

            ScreenManager.SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

        #region Highscore loading/saving logic

        /// <summary>
        /// Überprüft ob die Punktzahl in den Highscores liegt
        /// </summary>
        /// <returns></returns>
        public static bool IsInHighscores(int score)
        {
            
            return score > highScore[highscorePlaces - 1].Value;
        }

        /// <summary>
        /// PFügte Punkte zum Highscore hinzu
        /// </summary>
        /// <param name="name">Name</param>
        public static void PutHighScore(string playerName, int score)
        {
            if (IsInHighscores(score))
            {
                highScore[highscorePlaces - 1] =
                    new KeyValuePair<string, int>(playerName, score);
                OrderGameScore();
            }
        }

        /// <summary>
        /// OSortieren der Tabelle
        /// </summary>
        private static void OrderGameScore()
        {
            highScore.Sort(CompareScores);
        }

        /// <summary>
        /// Vergleicht Punkte
        /// </summary>
        private static int CompareScores(KeyValuePair<string, int> score1,
            KeyValuePair<string, int> score2)
        {
            if (score1.Value < score2.Value)
            {
                return 1;
            }

            if (score1.Value == score2.Value)
            {
                return 0;
            }

            return -1;
        }

        /// <summary>
        /// Speichert den Highscore in einem Textfile 
        /// </summary>
        public static void SaveHighscore()
        {
      
            using (IsolatedStorageFile isf =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
             

                using (IsolatedStorageFileStream isfs = 
                    isf.CreateFile("highscores.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(isfs))
                    {
                        for (int i = 0; i < highScore.Count; i++)
                        {
                     
                            writer.WriteLine(highScore[i].Key);
                            writer.WriteLine(highScore[i].Value.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Läde den Highscore aus dem textfile  
        /// </summary>
        public static void LoadHighscores()
        {
            //Wo sind die daten gespeichert
            using (IsolatedStorageFile isf = 
                IsolatedStorageFile.GetUserStoreForApplication())
            {
            
                if (isf.FileExists("highscores.txt"))
                {
                    using (IsolatedStorageFileStream isfs = 
                        isf.OpenFile("highscores.txt", FileMode.Open))                        
                    {
             
                        using (StreamReader reader = new StreamReader(isfs))
                        {
                           
                            int i = 0;
                            while (!reader.EndOfStream)
                            {
                                string name = reader.ReadLine();
                                int score = int.Parse(reader.ReadLine());

                                highScore[i++] =  new KeyValuePair<string, int>(
                                    name, score);
                            }
                        }
                    }
                }
            }

            OrderGameScore();
            
        }

        #endregion
    }
}
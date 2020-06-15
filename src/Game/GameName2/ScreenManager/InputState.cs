#region Using Statements
using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

#endregion

namespace BloodyPlumber
{
    /// <summary>
    /// Diese Klasse liest Gamepad, Tastatur und Touchgesten aus
    /// </summary>
    public class InputState
    {
        #region Fields

        public const int MaxInputs = 4;
        public readonly KeyboardState[] CurrentKeyboardStates;
        public readonly GamePadState[] CurrentGamePadStates;
        public readonly KeyboardState[] LastKeyboardStates;
        public readonly GamePadState[] LastGamePadStates;
        public readonly bool[] GamePadWasConnected;
        public TouchCollection TouchState;
        public MouseState CurrentMouseState;
        public MouseState LastMouseState;
        public readonly List<GestureSample> Gestures = new List<GestureSample>();
        public int wait;

        #endregion

        #region Initialization


        
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            CurrentGamePadStates = new GamePadState[MaxInputs];

            LastKeyboardStates = new KeyboardState[MaxInputs];
            LastGamePadStates = new GamePadState[MaxInputs];

            GamePadWasConnected = new bool[MaxInputs];

            wait = 0;
        }


        #endregion

        #region Public Methods


        
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                LastKeyboardStates[i] = CurrentKeyboardStates[i];
                LastGamePadStates[i] = CurrentGamePadStates[i];

                CurrentKeyboardStates[i] = Keyboard.GetState((PlayerIndex)i);
                CurrentGamePadStates[i] = GamePad.GetState((PlayerIndex)i);

                
                if (CurrentGamePadStates[i].IsConnected)
                {
                    GamePadWasConnected[i] = true;
                }
            }


            TouchState = new TouchCollection(UIConstants.test);
                    

                LastMouseState = CurrentMouseState;
                CurrentMouseState = Mouse.GetState();
                Vector2 _mousePosition = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                Vector2 p = _mousePosition - ScreenManager.InputTranslate;
                p = Vector2.Transform(p, ScreenManager.InputScale);

                CurrentMouseState = new MouseState((int)p.X, (int)p.Y, CurrentMouseState.ScrollWheelValue, CurrentMouseState.LeftButton, CurrentMouseState.MiddleButton, CurrentMouseState.RightButton, CurrentMouseState.XButton1, CurrentMouseState.XButton2);

                UpdateMouseStates();

                Gestures.Clear();
                
                while (TouchPanel.IsGestureAvailable)
                {
                    

                    var g = TouchPanel.ReadGesture();
                    var p1 = Vector2.Transform(g.Position - ScreenManager.InputTranslate, ScreenManager.InputScale);
                    var p2 = Vector2.Transform(g.Position2 - ScreenManager.InputTranslate, ScreenManager.InputScale);
                    var p3 = Vector2.Transform(g.Delta - ScreenManager.InputTranslate, ScreenManager.InputScale);
                    var p4 = Vector2.Transform(g.Delta2 - ScreenManager.InputTranslate, ScreenManager.InputScale);
                    g = new GestureSample(g.GestureType, g.Timestamp, p1, p2, p3, p4);


                    Gestures.Add(g);
                }
        }

        bool dragging = false;
        bool dragComplete = false;
        bool leftMouseDown = false;
        int dragThreshold = 3;
        MouseGestureType mouseGestureType;
        Vector2 currentMousePosition = Vector2.Zero;
        Vector2 prevMousePosition = Vector2.Zero;
        Vector2 dragMouseStart = Vector2.Zero;
        Vector2 dragMouseEnd = Vector2.Zero;

        public MouseGestureType MouseGesture
        {
            get
            {
                return mouseGestureType;
            }
        }

        public Vector2 CurrentMousePosition
        {
            get
            {
                return currentMousePosition;
            }
        }

        public Vector2 PrevMousePosition
        {
            get
            {
                return prevMousePosition;
            }
        }

        public Vector2 MouseDelta
        {
            get
            {
                return prevMousePosition - currentMousePosition;
            }
        }

        public Vector2 MouseDragDelta
        {
            get
            {
                return dragMouseStart - dragMouseEnd;
            }
        }

        public Vector2 MouseDragStartPosition
        {
            get
            {
                return dragMouseStart;
            }
        }

        public Vector2 MouseDragEndPosition
        {
            get
            {
                return dragMouseEnd;
            }
        }

        public ScreenManager ScreenManager
        {
            get;
            set;
        }

        void UpdateMouseStates()
        {
            currentMousePosition.X = CurrentMouseState.X;
            currentMousePosition.Y = CurrentMouseState.Y;

            prevMousePosition.X = LastMouseState.X;
            prevMousePosition.Y = LastMouseState.Y;

            if (mouseGestureType.HasFlag(MouseGestureType.LeftClick))
                mouseGestureType = mouseGestureType ^ MouseGestureType.LeftClick;

            if (mouseGestureType.HasFlag(MouseGestureType.Move))
                mouseGestureType = mouseGestureType ^ MouseGestureType.Move;

            if (MouseDelta.Length() != 0)
                mouseGestureType = mouseGestureType | MouseGestureType.Move;

            
            if (CurrentMouseState.LeftButton == ButtonState.Released &&
                    dragging)
            {

                leftMouseDown = false;
                dragging = false;
                dragComplete = true;
                dragMouseEnd = currentMousePosition;
                mouseGestureType |= MouseGestureType.DragComplete;
                mouseGestureType = mouseGestureType ^ MouseGestureType.FreeDrag;
               

            }

           
            if (!leftMouseDown && CurrentMouseState.LeftButton == ButtonState.Pressed &&
                    !CurrentMouseState.Equals(LastMouseState))
            {
                
                leftMouseDown = true;
                dragComplete = false;
                dragMouseStart = currentMousePosition;
            }

            if (leftMouseDown && CurrentMouseState.LeftButton == ButtonState.Released &&
                    !CurrentMouseState.Equals(LastMouseState))
            {
                leftMouseDown = false;
                mouseGestureType |= MouseGestureType.LeftClick;
            }

           
            if (leftMouseDown && !dragging)
            {

                Vector2 delta = dragMouseStart - currentMousePosition;

                if (delta.Length() > dragThreshold)
                {
                    dragging = true;
                    dragMouseStart = currentMousePosition;
                    mouseGestureType = mouseGestureType | MouseGestureType.FreeDrag;
                    
                }
            }

            
        }

        
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) && LastKeyboardStates[i].IsKeyUp(key));
            }
            else
            {
                
                return (IsNewKeyPress(key, PlayerIndex.One, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Two, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Three, out playerIndex) ||
                    IsNewKeyPress(key, PlayerIndex.Four, out playerIndex));
            }
        }


        
        public bool IsNewButtonPress(Buttons button, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentGamePadStates[i].IsButtonDown(button) && LastGamePadStates[i].IsButtonUp(button));
            }
            else
            {
                
                return (IsNewButtonPress(button, PlayerIndex.One, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Two, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Three, out playerIndex) ||
                    IsNewButtonPress(button, PlayerIndex.Four, out playerIndex));
            }
        }


        
        public bool IsMenuSelect(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Space, controllingPlayer, out playerIndex) ||
                        IsNewKeyPress(Keys.Enter, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.A, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        
        public bool IsMenuCancel(PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.B, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex);
        }


       
        public bool IsMenuUp(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Up, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.DPadUp, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.LeftThumbstickUp, controllingPlayer, out playerIndex);
        }


        
        public bool IsMenuDown(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Down, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.DPadDown, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.LeftThumbstickDown, controllingPlayer, out playerIndex);
        }


        
        public bool IsPauseGame(PlayerIndex? controllingPlayer)
        {
            PlayerIndex playerIndex;

            return IsNewKeyPress(Keys.Escape, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.Back, controllingPlayer, out playerIndex) ||
                        IsNewButtonPress(Buttons.Start, controllingPlayer, out playerIndex);
        }


        #endregion
    }
}
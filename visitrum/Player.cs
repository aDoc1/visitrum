#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements the player.
    /// </summary>
    public class Player : DrawableGameComponent
    {
        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        protected TimeSpan elapsedTime = TimeSpan.Zero;
        protected PlayerIndex playerIndex;
        protected SpriteBatch sBatch;
        protected Color paddleColor;

        // Screen Area
        protected Rectangle screenBounds;
        protected const int PADDLEWIDTH = 230;
        protected const int PADDLEHEIGHT = 26;


        // Game Stuff
        protected int score;
        protected int lives;
        protected int level;
        protected int initLives;
        private const int INITIALSCORE = 0;

        
        //private const int INITIALLIVES = 3;

        public Player(Game game, ref Texture2D theTexture, PlayerIndex playerID, Rectangle rectangle)
            : base(game)
        {
            texture = theTexture;
            position = new Vector2();
            playerIndex = playerID;
            // Get the current spritebatch
            sBatch = (SpriteBatch)
                Game.Services.GetService(typeof(SpriteBatch));

            // Create the source rectangle.
            // This represents where is the sprite picture in surface
            spriteRectangle = rectangle;
            paddleColor = Color.Gray;

#if XBOX360
    // On the 360, we need take care about the tv "safe" area.
            screenBounds = new Rectangle((int)(Game.Window.ClientBounds.Width * 
                0.03f),(int)(Game.Window.ClientBounds.Height * 0.03f),
                Game.Window.ClientBounds.Width - 
                (int)(Game.Window.ClientBounds.Width * 0.03f),
                Game.Window.ClientBounds.Height - 
                (int)(Game.Window.ClientBounds.Height * 0.03f));
#else
            screenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                Game.Window.ClientBounds.Height);
#endif
        }

        /// <summary>
        /// Put the paddle in your start position in screen
        /// </summary>
        public void Reset(int initLives)
        {
#if XBOX360
            position.X = (screenBounds.Width - spriteRectangle.Width) / 2 + 10;
            position.Y = screenBounds.Height - spriteRectangle.Height;

#else
            position.X = (screenBounds.Width - spriteRectangle.Width) / 2;
            position.Y = screenBounds.Height - spriteRectangle.Height;
#endif

            score = INITIALSCORE;
            lives = initLives;
            level = 1;
            paddleColor = Color.Gray;
        }

        /// <summary>
        /// Total Points of the Player
        /// </summary>
        public int Score
        {
            get { return score; }
            set
            {
                if (value < 0)
                {
                    score = 0;
                }
                else
                {
                    score = value;
                }
            }
        }


        /// <summary>
        /// Remaining Lives
        /// </summary>
        public int Lives
        {
            get { return lives; }
            set { lives = value; }
        }

        /// <summary>
        /// Level Value
        /// </summary>
        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        
        public Color PaddleColor
        {
            get { return paddleColor; }
            set { paddleColor = value; }
        }


        /// <summary>
        /// Put the player in a starting position in the screen
        /// </summary>

         public void putInStartPosition()
        {
            position.X = (screenBounds.Width - PADDLEWIDTH) / 2;
            position.Y = screenBounds.Height - PADDLEHEIGHT;
        }
        

        /// <summary>
        /// Update the player information
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            HandleInput(playerIndex);

            base.Update(gameTime);
        }


        /// <summary>
        /// Handle player input
        /// </summary>
        protected void HandleInput(PlayerIndex thePlayerIndex)
        {
            GamePadState gamepadstatus = GamePad.GetState(thePlayerIndex);
            KeyboardState keyboard = Keyboard.GetState();
            
            // Yellow button
            if (gamepadstatus.Buttons.Y == ButtonState.Pressed || keyboard.IsKeyDown(Keys.Y)) 
            {
                paddleColor = Color.Yellow;
            }
            // Green button
            if (gamepadstatus.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.G))
            {
                paddleColor = Color.Green;
            }
            // Blue button
            if (gamepadstatus.Buttons.X == ButtonState.Pressed || keyboard.IsKeyDown(Keys.B))
            {
                paddleColor = Color.Blue;
            }
            // Red button
            if (gamepadstatus.Buttons.B == ButtonState.Pressed || keyboard.IsKeyDown(Keys.R))
            {
                paddleColor = Color.Red;
            }
            // White Button
            if (gamepadstatus.IsButtonDown(Buttons.LeftTrigger) ==  true || keyboard.IsKeyDown(Keys.Right))
            {
                paddleColor = Color.White;
            }
            // Black Button
            if (gamepadstatus.IsButtonDown(Buttons.RightTrigger) == true || keyboard.IsKeyDown(Keys.Left))
            {
                paddleColor = Color.Black;
            }
            // Orange
            if (gamepadstatus.Buttons.B == ButtonState.Pressed && gamepadstatus.Buttons.Y == ButtonState.Pressed || keyboard.IsKeyDown(Keys.O))
            {
                paddleColor = Color.Orange;
            }
            // Cyan
            if (gamepadstatus.Buttons.X == ButtonState.Pressed && gamepadstatus.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.C))
            {
                paddleColor = Color.Cyan;
            }
            //Purple
            if (gamepadstatus.Buttons.B == ButtonState.Pressed && gamepadstatus.Buttons.X == ButtonState.Pressed || keyboard.IsKeyDown(Keys.P))
            {
                paddleColor = Color.Purple;
            }
            //Brown
            if (gamepadstatus.Buttons.A == ButtonState.Pressed && gamepadstatus.Buttons.B == ButtonState.Pressed || keyboard.IsKeyDown(Keys.W))
            {
                paddleColor = Color.Chocolate;
            }
            //Yellow-Green
            if (gamepadstatus.Buttons.Y == ButtonState.Pressed && gamepadstatus.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.T))
            {
                paddleColor = Color.YellowGreen;
            }
            //Gray
            if (gamepadstatus.IsButtonDown(Buttons.RightTrigger) == true && gamepadstatus.IsButtonDown(Buttons.LeftTrigger) == true || keyboard.IsKeyDown(Keys.E))
            {
                paddleColor = Color.Gray;
            }
            //Dark Blue
            if (gamepadstatus.IsButtonDown(Buttons.RightTrigger) == true && gamepadstatus.Buttons.X == ButtonState.Pressed || keyboard.IsKeyDown(Keys.D))
            {
                paddleColor = Color.DarkBlue;
            }
            //Dark Red
            if (gamepadstatus.IsButtonDown(Buttons.RightTrigger) == true && gamepadstatus.Buttons.B == ButtonState.Pressed || keyboard.IsKeyDown(Keys.K))
            {
                paddleColor = Color.DarkRed;
            }
            //Light Green
            if (gamepadstatus.IsButtonDown(Buttons.LeftTrigger) == true && gamepadstatus.Buttons.A == ButtonState.Pressed || keyboard.IsKeyDown(Keys.L))
            {
                paddleColor = Color.Chartreuse;
            }
            //Pink
            if (gamepadstatus.Buttons.B == ButtonState.Pressed && gamepadstatus.IsButtonDown(Buttons.LeftTrigger) == true || keyboard.IsKeyDown(Keys.Space))
            {
                paddleColor = Color.Pink;
            }
        }

        /// <summary>
        /// Draw the paddle sprite
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // Get the current spritebatch
            sBatch = (SpriteBatch)
                Game.Services.GetService(typeof(SpriteBatch));

            // Draw the paddle
            sBatch.Draw(texture, position, spriteRectangle, paddleColor);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Get the bound rectangle of paddle position in screen
        /// </summary>
        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y,
                spriteRectangle.Width, spriteRectangle.Height);
        }
    }
}
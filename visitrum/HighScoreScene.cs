#region Using Statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Visitrum
{
    /// <summary>
    /// This is a game component thats represents the High Scores Scene
    /// </summary>
    public class HighScoreScene : GameScene
    {
        // Spritebatch
        protected SpriteBatch spriteBatch = null;
        // Gui Stuff
        protected Vector2 titlePosition;
        protected const int TITLEWIDTH = 360;
        protected string[] highScoreInfo = {"#","Player","Score","Level"};
        protected string highScoreTitle = "HIGH SCORES"; 
        //protected Vector2 highScoreInfoPosition;
        protected Vector2 numberPosition;
        protected Vector2 playerPosition;
        protected Vector2 scorePosition;
        protected Vector2 levelPosition;

        protected int hsPageNum;
        protected int linesPerPage;

        protected readonly SpriteFont titleFont;
        protected readonly SpriteFont infoFont;

        protected readonly Color fontColor;

        // Used for handle input
        protected KeyboardState oldKeyboardState;
        protected GamePadState oldGamePadState;
        protected bool tsReleased, tsUp, tsDown;

        public HighScoreScene(Game game, Texture2D textureBack, SpriteFont smallFont, SpriteFont largeFont)
            : base(game)
        {
            Components.Add(new ImageComponent(game, textureBack,
                ImageComponent.DrawMode.Stretch));

            titleFont = largeFont;
            infoFont = smallFont;
            fontColor = Color.White;

            hsPageNum = 0;
            int tempSize = ((int)Game.Window.ClientBounds.Height - (int)titleFont.MeasureString(highScoreTitle).Y) / (int)infoFont.MeasureString(highScoreInfo[0]).Y;
            linesPerPage = tempSize - 2;

            
            // Get the current spritebatch
            spriteBatch = (SpriteBatch)Game.Services.GetService(
                                            typeof(SpriteBatch));
        }

        public Game1 Game1 { get { return (Game1)Game; } }

        /// <summary>
        /// Show the start scene
        /// </summary>
        public override void Show()
        {
            // Load the High Scores table
           // Game1.LoadHighScores();

            hsPageNum = Game1.highScores.getHighScoreIndex() / linesPerPage;

            titlePosition.X = (Game.Window.ClientBounds.Width - titleFont.MeasureString(highScoreTitle).X) / 2;
            titlePosition.Y = Game.Window.ClientBounds.Height / 10 - titleFont.MeasureString(highScoreTitle).Y;

            numberPosition.X = infoFont.MeasureString(highScoreInfo[0]).X + 20;
            numberPosition.Y = titlePosition.Y + 40;

            playerPosition.X = (numberPosition.X + infoFont.MeasureString(highScoreInfo[1]).X) - 30;
            playerPosition.Y = titlePosition.Y + 40;

            //namePosition.X = (playerPosition.X + infoFont.MeasureString(highScoreInfo[2]).X) + 10;
            //namePosition.Y = titlePosition.Y + 40;

            scorePosition.X = (playerPosition.X + infoFont.MeasureString(highScoreInfo[2]).X) + titleFont.MeasureString(highScoreTitle).X + 50;
            scorePosition.Y = titlePosition.Y + 40;

            levelPosition.X = (scorePosition.X + infoFont.MeasureString(highScoreInfo[3]).X) + 90;
            levelPosition.Y = titlePosition.Y + 40;

            //highScoreInfoPosition.X = 10;
            //highScoreInfoPosition.Y = titlePosition.Y + titleFont.MeasureString(highScoreTitle).Y + 5;
            
            base.Show();
        }

        /// <summary>
        /// Hide the difficulty scene
        /// </summary>
        public override void Hide()
        {
            //MediaPlayer.Stop();
            base.Hide();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState();

            // Handle thumbstick
            if (oldGamePadState.ThumbSticks.Left.Y > gamepadState.ThumbSticks.Left.Y && !tsUp)
                tsDown = true;
            if (oldGamePadState.ThumbSticks.Left.Y < gamepadState.ThumbSticks.Left.Y && !tsDown)
                tsUp = true;
            if (gamepadState.ThumbSticks.Left.Y == 0)
                tsReleased = true;
            else
                tsReleased = false;

            bool down, up;
            // Handle the keyboard
            down = (oldKeyboardState.IsKeyDown(Keys.Down) &&
                (keyboardState.IsKeyUp(Keys.Down)));
            up = (oldKeyboardState.IsKeyDown(Keys.Up) &&
                (keyboardState.IsKeyUp(Keys.Up)));
            // Handle the D-Pad
            down |= ((oldGamePadState.DPad.Down == ButtonState.Pressed) &&
                    (gamepadState.DPad.Down == ButtonState.Released)) ||
                    (tsDown && tsReleased);
            up |= ((oldGamePadState.DPad.Up == ButtonState.Pressed) &&
                    (gamepadState.DPad.Up == ButtonState.Released)) ||
                    (tsUp && tsReleased);

            if (down)
            {
              
                if (hsPageNum < Game1.highScores.getHighScores().Count / linesPerPage)
                {   
                    hsPageNum++;
                }
            }
            if (up)
            {
                if (hsPageNum > 0)
                {
                    hsPageNum--;
                }

            }

            oldKeyboardState = keyboardState;
            oldGamePadState = gamepadState;
            if (tsReleased)
            {
                tsDown = false;
                tsUp = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            base.Draw(gameTime);

            // Draw the text shadow
            spriteBatch.DrawString(titleFont, highScoreTitle, new Vector2(titlePosition.X + 1,
                                    titlePosition.Y + 1), Color.Black);
            // Draw the text item
            spriteBatch.DrawString(titleFont, highScoreTitle,
                                    new Vector2(titlePosition.X, titlePosition.Y),
                                    fontColor);

            // Draw the text shadow
            //spriteBatch.DrawString(infoFont, highScoreInfo, new Vector2(highScoreInfoPosition.X + 1,
                                    //highScoreInfoPosition.Y + 1), Color.Black);
            // Draw the text item
            //spriteBatch.DrawString(infoFont, highScoreInfo,
                                    //new Vector2(highScoreInfoPosition.X, highScoreInfoPosition.Y),
                                    //fontColor);

            spriteBatch.DrawString(infoFont, highScoreInfo[0], new Vector2(numberPosition.X + 1,
                                    numberPosition.Y + 1), Color.Black);

            spriteBatch.DrawString(infoFont, highScoreInfo[0], new Vector2(numberPosition.X,
                                    numberPosition.Y), Color.White);

            spriteBatch.DrawString(infoFont, highScoreInfo[1], new Vector2(playerPosition.X + 1,
                                    playerPosition.Y + 1), Color.Black);

            spriteBatch.DrawString(infoFont, highScoreInfo[1], new Vector2(playerPosition.X,
                                    playerPosition.Y), Color.White);

            //spriteBatch.DrawString(infoFont, highScoreInfo[2], new Vector2(namePosition.X + 1,
            //                        namePosition.Y + 1), Color.Black);

            //spriteBatch.DrawString(infoFont, highScoreInfo[2], new Vector2(namePosition.X,
            //                        namePosition.Y), Color.Black);

            spriteBatch.DrawString(infoFont, highScoreInfo[2], new Vector2(scorePosition.X + 1,
                                    scorePosition.Y + 1), Color.Black);

            spriteBatch.DrawString(infoFont, highScoreInfo[2], new Vector2(scorePosition.X,
                                    scorePosition.Y), Color.White);

            spriteBatch.DrawString(infoFont, highScoreInfo[3], new Vector2(levelPosition.X + 1,
                                    levelPosition.Y + 1), Color.Black);

            spriteBatch.DrawString(infoFont, highScoreInfo[3], new Vector2(levelPosition.X,
                                    levelPosition.Y), Color.White);

            int startInx = (hsPageNum) * linesPerPage;

            if (Game1.highScores.getHighScores().Count % linesPerPage > 0 || Game1.highScores.getHighScores().Count == 0)
            {
                Game1.highScores.prepareForDisplay(linesPerPage - (Game1.highScores.getHighScores().Count % linesPerPage));
            }

            int rowNum = 0;
            for (int i = startInx; i < startInx + linesPerPage && i < Game1.highScores.getDisplayHighScores().Count; i++)
            {
                
                // Draw the text shadow
                spriteBatch.DrawString(infoFont, (i+1).ToString(),
                    new Vector2(numberPosition.X + 1, numberPosition.Y + (infoFont.MeasureString(highScoreInfo[0]).Y + 5) * (rowNum + 1) + 1), Color.Black);

                spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].PlayerName,
                    new Vector2(playerPosition.X + 1, playerPosition.Y + (infoFont.MeasureString(highScoreInfo[1]).Y + 5) * (rowNum + 1) + 1), Color.Black);


                spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].Score.ToString(),
                    new Vector2(scorePosition.X + 1, scorePosition.Y + (infoFont.MeasureString(highScoreInfo[2]).Y + 5) * (rowNum + 1) + 1), Color.Black);


                spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].Level.ToString(),
                    new Vector2(levelPosition.X + 1, levelPosition.Y + (infoFont.MeasureString(highScoreInfo[3]).Y + 5) * (rowNum + 1) + 1), Color.Black);


                if (i == Game1.highScores.getHighScoreIndex() && Game1.gameIsOver)
                {
                    spriteBatch.DrawString(infoFont, (i + 1).ToString(),
                        new Vector2(numberPosition.X, numberPosition.Y + (infoFont.MeasureString(highScoreInfo[0]).Y + 5) * (rowNum + 1)), Color.Yellow);

                    spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].PlayerName,
                        new Vector2(playerPosition.X, playerPosition.Y + (infoFont.MeasureString(highScoreInfo[1]).Y + 5) * (rowNum + 1)), Color.Yellow);

                    spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].Score.ToString(),
                        new Vector2(scorePosition.X, scorePosition.Y + (infoFont.MeasureString(highScoreInfo[2]).Y + 5) * (rowNum + 1)), Color.Yellow);

                    spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].Level.ToString(),
                        new Vector2(levelPosition.X, levelPosition.Y + (infoFont.MeasureString(highScoreInfo[3]).Y + 5) * (rowNum + 1)), Color.Yellow);
                }
                else
                {
                    spriteBatch.DrawString(infoFont, (i + 1).ToString(),
                        new Vector2(numberPosition.X, numberPosition.Y + (infoFont.MeasureString(highScoreInfo[0]).Y + 5) * (rowNum + 1)), Color.White);

                    spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].PlayerName,
                        new Vector2(playerPosition.X, playerPosition.Y + (infoFont.MeasureString(highScoreInfo[1]).Y + 5) * (rowNum + 1)), Color.White);

                    spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].Score.ToString(),
                        new Vector2(scorePosition.X, scorePosition.Y + (infoFont.MeasureString(highScoreInfo[2]).Y + 5) * (rowNum + 1)), Color.White);

                    spriteBatch.DrawString(infoFont, Game1.highScores.getDisplayHighScores()[i].Level.ToString(),
                        new Vector2(levelPosition.X, levelPosition.Y + (infoFont.MeasureString(highScoreInfo[3]).Y + 5) * (rowNum + 1)), Color.White);
                }

                rowNum++;
            }
        }

    }
}
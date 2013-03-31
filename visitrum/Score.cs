#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements the Game Score.
    /// </summary>
    public class Score : DrawableGameComponent
    {
        // Spritebatch
        protected SpriteBatch spriteBatch = null;

        // Score, lives, level Position 
        protected Vector2 position = new Vector2();

        // Values
        protected int value;
        protected int lives;
        protected int level;

        protected readonly SpriteFont font;
        protected readonly Color fontColor;

        public Score(Game game, SpriteFont font, Color fontColor)
            : base(game)
        {
            this.font = font;
            this.fontColor = fontColor;
            // Get the current spritebatch
            spriteBatch = (SpriteBatch)
                            Game.Services.GetService(typeof(SpriteBatch));
        }

        public void Reset()
        {
            value = 0;
            lives = 0;
            level = 0;
        }


        /// <summary>
        /// Points value
        /// </summary>
        public int Value
        {
            get { return value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Lives Value
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

        /// <summary>
        /// Position of score, lives, level in screen
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            string TextToDraw = string.Format("Score: {0}", value);

            // Draw the text shadow
            spriteBatch.DrawString(font, TextToDraw, new Vector2(position.X + 1,
                                    position.Y + 1), Color.Black);
            // Draw the text item
            spriteBatch.DrawString(font, TextToDraw,
                                    new Vector2(position.X, position.Y),
                                    fontColor);

            float height = font.MeasureString(TextToDraw).Y;

            TextToDraw = string.Format("Lives: {0}", lives);

            // Draw the text shadow
            spriteBatch.DrawString(font, TextToDraw,
                new Vector2(position.X + 1, position.Y + 1 + height),
                Color.Black);

            // Draw the text item
            spriteBatch.DrawString(font, TextToDraw,
                new Vector2(position.X, position.Y + 1 + height),
                fontColor);
    
            TextToDraw = string.Format("Level: {0}", level);
            float width = font.MeasureString(TextToDraw).X;
#if XBOX360
            //String
            spriteBatch.DrawString(font, TextToDraw, new Vector2(Game.Window.ClientBounds.Right - width - 25, position.Y + 1), Color.Black);

            //Shadow
            spriteBatch.DrawString(font, TextToDraw,
                new Vector2(Game.Window.ClientBounds.Right - width - 25, position.Y), fontColor);
#else
            // Draw the text shadow
            spriteBatch.DrawString(font, TextToDraw,
                new Vector2(Game.Window.ClientBounds.Right - width - 341, position.Y + 1), Color.Black);

            // Draw the text item
            spriteBatch.DrawString(font, TextToDraw,
                new Vector2(Game.Window.ClientBounds.Right - width - 340, position.Y), fontColor);
#endif

            base.Draw(gameTime);
        }
    }
}
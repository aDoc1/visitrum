#region Using Statements

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

#endregion

namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements level text elements.
    /// </summary>
    public class GameText : DrawableGameComponent
    {
        // Spritebatch
        protected SpriteBatch spriteBatch = null;
        //Background texture box texture
        protected Texture2D textBoxTexture;
        // Fonts
        protected readonly SpriteFont regularFont, selectedFont;
        // Colors
        protected Color regularColor = Color.White, selectedColor = Color.Red;
        // Text Position
        protected Vector2 position = new Vector2();
        // Items
        private readonly List<string> textItems;
        // Used for handle input
        protected KeyboardState oldKeyboardState;
        protected GamePadState oldGamePadState;
        // Size of menu in pixels
        protected int width, height;
        // For audio effects
        //protected AudioLibrary audio;

        //protected int Xspeed;
        protected int whichItem;
        protected bool showtext;

        protected Rectangle textbox;


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">the main game object</param>
        /// <param name="normalFont">Font to regular items</param>
        /// <param name="selectedFont">Font to selected item</param>
        public GameText(Game game, Texture2D texture, SpriteFont font)
            : base(game)
        {
            regularFont = font;
            textBoxTexture = texture;
            textItems = new List<string>();
            showtext = true;

            // Draw the background box
#if XBOX360
            textbox = new Rectangle((Game.Window.ClientBounds.Width - 640) / 2, (Game.Window.ClientBounds.Height - 480) / 2, 640, 480); 
#else
            textbox = new Rectangle((Game.Window.ClientBounds.Width - 320) / 2, (Game.Window.ClientBounds.Height - 240) / 2, 320, 240);
#endif
            // Get the current spritebatch
            spriteBatch = (SpriteBatch)
                Game.Services.GetService(typeof(SpriteBatch));

            // Get the audio library
            //audio = (AudioLibrary)
            //    Game.Services.GetService(typeof(AudioLibrary));

            // Used for input handling
            oldKeyboardState = Keyboard.GetState();
            oldGamePadState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Set the Menu Options
        /// </summary>
        /// <param name="items"></param>
        public void SetTextItems(string[] items)
        {
            textItems.Clear();
            textItems.AddRange(items);
        }

        /// <summary>
        /// Width of menu in pixels
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// Height of menu in pixels
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Regular item color
        /// </summary>
        public Color RegularColor
        {
            get { return regularColor; }
            set { regularColor = value; }
        }

        /// <summary>
        /// Position of component in screen
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //public int textSpeed
        //{
        //    get { return Xspeed; }
        //    set { Xspeed = value; }
        //}

        public int displayItem
        {
            get { return whichItem; }
            set { whichItem = value; }
        }

        public bool showText
        {
            get { return showtext; }
            set { showtext = value; }
        }

        private string parseText(string text)
        {
            string line = string.Empty;
            string returnString = string.Empty;
            string[] wordArray = text.Split(' ');

            foreach (string word in wordArray)
            {
                if (regularFont.MeasureString(line + word).Length() > textbox.Width)
                {
                    returnString = returnString + line + '\n';
                    line = string.Empty;
                }

                line = line + word + ' ';
            }

            return returnString + line;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Allows the game component to draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            if (!showtext)
                return;

            spriteBatch.Draw(textBoxTexture, textbox, new Color(255, 255, 255, 0));

            spriteBatch.DrawString(regularFont, parseText(textItems[whichItem]), new Vector2(textbox.X, textbox.Y), Color.Blue);

            ////float height = regularFont.MeasureString(textItems[whichItem]).Y;
            //position.Y = (Game.Window.ClientBounds.Height - height) / 2;
            //float width = regularFont.MeasureString(textItems[whichItem]).X;

            //position.X = (Game.Window.ClientBounds.Width - width) / 2;

            //// Draw the text shadow
            //spriteBatch.DrawString(regularFont, textItems[whichItem],
            //    new Vector2(position.X + 1, position.Y + 1), Color.Black);
            //// Draw the text item
            //spriteBatch.DrawString(regularFont, textItems[whichItem],
            //    new Vector2(position.X, position.Y), Color.Blue);
            //position.Y += regularFont.LineSpacing;
           
            base.Draw(gameTime);
        }
    }
}
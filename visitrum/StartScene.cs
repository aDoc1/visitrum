#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

#endregion

namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements the Game Start Scene.
    /// </summary>
    public class StartScene : GameScene
    {
        // Misc
        protected TextMenuComponent menu;
        protected readonly Texture2D elements;
        // Audio
        //protected AudioLibrary audio;
        // Spritebatch
        protected SpriteBatch spriteBatch = null;
        // Gui Stuff
        protected Rectangle titleRect = new Rectangle(10, 37, 350, 70);
        protected Vector2 titlePosition;
        protected TimeSpan elapsedTime = TimeSpan.Zero;
        protected const int TITLEWIDTH = 360;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="game">Main game object</param>
        /// <param name="smallFont">Font for the menu items</param>
        /// <param name="largeFont">Font for the menu selcted item</param>
        /// <param name="background">Texture for background image</param>
        /// <param name="elements">Texture with the foreground elements</param>
        public StartScene(Game game, SpriteFont smallFont, SpriteFont largeFont,
                            Texture2D background, Texture2D elements)
            : base(game)
        {
            this.elements = elements;
            Components.Add(new ImageComponent(game, background,
                                            ImageComponent.DrawMode.Center));

            // Create the Menu
            string[] items = { "Play Game","High Scores", "Help", "Quit" };
            menu = new TextMenuComponent(game, smallFont, largeFont);
            menu.SetMenuItems(items);
            Components.Add(menu);

            // Get the current spritebatch
            spriteBatch = (SpriteBatch)Game.Services.GetService(
                                            typeof(SpriteBatch));

            // Get the audio library
            //audio = (AudioLibrary)
            //    Game.Services.GetService(typeof(AudioLibrary));
        }

        /// <summary>
        /// Show the start scene
        /// </summary>
        public override void Show()
        {
            //audio.NewMeteor.Play();

            //rockPosition.X = -1 * rockRect.Width;
            //rockPosition.Y = 40;
            titlePosition.X = (Game.Window.ClientBounds.Width - TITLEWIDTH) / 2;
            titlePosition.Y = Game.Window.ClientBounds.Height / 4;
            // Put the menu centered in screen
            menu.Position = new Vector2((Game.Window.ClientBounds.Width -
                                          menu.Width) / 2, Game.Window.ClientBounds.Height/2);

            // These elements will be visible when the 'Rock Rain' title
            // is done.
            //menu.Visible = false;
            //menu.Enabled = false;

            base.Show();
        }

        /// <summary>
        /// Hide the start scene
        /// </summary>
        public override void Hide()
        {
            //MediaPlayer.Stop();
            base.Hide();
        }

        /// <summary>
        /// Gets the selected menu option
        /// </summary>
        public int SelectedMenuIndex
        {
            get { return menu.SelectedIndex; }
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
            base.Draw(gameTime);

            spriteBatch.Draw(elements, titlePosition, titleRect, Color.White);
        }
    }
}
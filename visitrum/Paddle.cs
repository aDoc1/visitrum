using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace Visitrum
{
    /// <summary>
    /// This is a game component that implements the player ship.
    /// </summary>
    public class Paddle : Microsoft.Xna.Framework.DrawableGameComponent
    {
        protected Texture2D texture;
        protected Rectangle spriteRectangle;
        protected Vector2 position;
        protected SpriteBatch sBatch;

        //Width and height of sprite in texture

        protected const int PADDLEWIDTH = 544;
        protected const int PADDLEHEIGHT = 121;

        //Screen area
        protected Rectangle screenBounds;


        public Paddle(Game game, ref Texture2D theTexture)
            : base(game)
        {
            texture = theTexture;
            position = new Vector2();

            // Get the current sprite batch
            sBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            //Create the source rectangle
            //This represents where the sprite picture is in the surface
            //spriteRectangle = new Rectangle(0, 0, PADDLEWIDTH, PADDLEHEIGHT);

#if XBOX360
            //Make sure the ship is within the TV screen area
            screenBounds = new Rectangle(
                (int)(Game.Window.ClientBounds.Width * 0.03f), 
                (int)(Game.Window.ClientBounds.Height * 0.03f),
                Game.Window.ClientBounds.Width - 
                (int)(Game.Window.ClientBounds.Width * 0.03f),
                Game.Window.ClientBounds.Height - 
                (int)(Game.Window.ClientBounds.Height * 0.03f));
#else
            screenBounds = new Rectangle(0, 0, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height);
#endif
        }


        /// <summary>
        /// Put the ship in a starting position in the screen
        /// </summary>

        public void putInStartPosition()
        {
            position.X = (screenBounds.Width - PADDLEWIDTH) / 2;
            position.Y = screenBounds.Height - PADDLEHEIGHT;
        }
        
        /// <summary>
        /// Update the ship position.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //Change the paddle's color
            GamePadState gamepadstatus = GamePad.GetState(PlayerIndex.One);

            base.Update(gameTime);
        }

        /// <summary>
        /// Draw the ship sprite
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            //Draw the ship
            sBatch.Draw(texture, position, null, Color.White);

            base.Draw(gameTime);
        }

        /// <summary>
        /// Get the bound rectangle of the ship position in the screen
        /// </summary>
        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, PADDLEWIDTH, PADDLEHEIGHT);
        }
    }
}